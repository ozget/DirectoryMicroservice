using EventBus.Base;
using EventBus.Base.Events;
using Newtonsoft.Json;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.RabbitMQ
{
    public class EventBusRabbitMQ:BaseEventBus
    {
        RabbitMQPersistenConnection _persistenConnection;
        private readonly IConnectionFactory _connectionFactory;
        private readonly IModel _consumerChannel;
        public EventBusRabbitMQ(EventBusConfig config,IServiceProvider serviceProvider):base(config,serviceProvider)
        {
            if (config.Connection != null)
            {
                var connJson = JsonConvert.SerializeObject(_eventBusConfig.Connection, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                _connectionFactory = JsonConvert.DeserializeObject<ConnectionFactory>(connJson);
            }
            else
                _connectionFactory = new ConnectionFactory();

            _persistenConnection = new RabbitMQPersistenConnection(_connectionFactory,config.ConnectionRetryCount);

            _consumerChannel = CreateConsumerChannel();
            _subManager.OnEventRemoved += SubsManager_OnEventRemoved;
        }

       
        public override void Publish(IntegrationEvent integrationEvent)
        {
            if (!_persistenConnection.IsConnected)
                _persistenConnection.TryConnect();

            //hatalar oldugunda
            var policy = Policy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(_eventBusConfig.ConnectionRetryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                    {
                        //log
                    });

            var eventName = integrationEvent.GetType().Name;
            eventName = ProcessEventName(eventName);

            //exchange olmaması durumunda
            _consumerChannel.ExchangeDeclare(exchange: _eventBusConfig.DefaultTopicName, type: "direct");

            var message = JsonConvert.SerializeObject(integrationEvent);
            var body = Encoding.UTF8.GetBytes(message);//rabbitMQ bizden byte Array olarak istedigi icin dönüşüm yaptık

            policy.Execute(() =>
            {
                var properties = _consumerChannel.CreateBasicProperties();
                properties.DeliveryMode = 2;

                //queue nun create edilip edilmemesi bilgisini alıyoruz Declare calıştırıp create edilip edilmedigini ögreniyoruz
                _consumerChannel.QueueDeclare(queue: GetSubName(eventName),
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                //data mızı karşı tarafa gönderiyoruz, rabittMq tarafına ulaşacak
                _consumerChannel.BasicPublish(
                    exchange: _eventBusConfig.DefaultTopicName,// nereye gönderilecek 
                    routingKey: eventName,
                    mandatory: true,
                    basicProperties: properties,
                    body: body);
            });
        }

        public override void Subscribe<T, TH>()
        {
            var eventName = typeof(T).Name;
            eventName = ProcessEventName(eventName);

            if (!_subManager.HasSubscriptionsForEvent(eventName))
            {//subscribe edilemiyor ise

                if (!_persistenConnection.IsConnected)//connection bilgisine bakıyoruz
                    _persistenConnection.TryConnect();

                //consum ettigimiz queue nun önceden oluşturulup oluşturulmadı bilgisini ede ediyoruz
                _consumerChannel.QueueDeclare(queue: GetSubName(eventName),//queue oluşturulması
                                                durable: true,
                                                exclusive: false,
                                                autoDelete: false,
                                                arguments: null);

                _consumerChannel.QueueBind(queue: GetSubName(eventName), //oluşturdugumuz queue ve exchange birbirine bind ediyoruz
                    exchange: _eventBusConfig.DefaultTopicName,
                    routingKey: eventName);

            }
            _subManager.AddSubscription<T, TH>();
            StartBasicConsume(eventName);  //queue dinlemeye başladık
        }

        public override void UnSubscribe<T, TH>()
        {
            _subManager.RemoveSubscription<T, TH>();

        }

        private IModel CreateConsumerChannel()
        {
            if (!_persistenConnection.IsConnected)//persisten baglı degilse baglanması icin yazıyoruz
                _persistenConnection.TryConnect();

            var channel = _persistenConnection.CreateModel();
            channel.ExchangeDeclare(exchange: _eventBusConfig.DefaultTopicName, type: "direct");

            return channel;

        }
        private void StartBasicConsume(string eventName)
        {
            if (_consumerChannel != null)
            {
                var consumer = new  EventingBasicConsumer(_consumerChannel); 
                consumer.Received += Consumer_Received;

                _consumerChannel.BasicConsume(
                    queue: GetSubName(eventName),
                    autoAck: false,
                    consumer: consumer);
            }
        }

        private async void Consumer_Received(object sender,BasicDeliverEventArgs eventArgs)
        {
            var eventName = eventArgs.RoutingKey;
            eventName = ProcessEventName(eventName);
            var message = Encoding.UTF8.GetString(eventArgs.Body.Span);

            try
            {
                await ProcessEvent(eventName, message);
            }
            catch (Exception)
            {

                throw;
            }
            _consumerChannel.BasicAck(eventArgs.DeliveryTag, multiple: false);
        }

        private void SubsManager_OnEventRemoved(object sender, string eventName)
        {
            eventName = ProcessEventName(eventName);

            if (!_persistenConnection.IsConnected)//connection bilgisine bakıyoruz
                _persistenConnection.TryConnect();

            //önceden bind edilen queue unbind ediyoruz, dinlemekten vazgeciyoruz
            _consumerChannel.QueueUnbind(queue: eventName,
                exchange: _eventBusConfig.DefaultTopicName,
                routingKey: eventName);
            
            if (_subManager.IsEmpty)
                _consumerChannel.Close();

        }
    }
}
