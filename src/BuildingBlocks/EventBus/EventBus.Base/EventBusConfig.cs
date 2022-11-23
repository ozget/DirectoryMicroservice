using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Base
{
    public class EventBusConfig
    {
        public int ConnectionRetryCount { get; set; } = 5;// rabbitMQ baglanırken en fazla 5 kere deneme
        public string DefaultTopicName { get; set; } = "DirectoryEventBus"; // herhangi bir topicName verilmemisse  buraya düşecek
        public string EventBusConnectionString { get; set; } = String.Empty;
        public string SubscriberClientAppName { get; set; } = String.Empty; //hangi servis queue yaracak

        public string EventNamePrefix { get; set; } = String.Empty;
        public string EventNameSuffix { get; set; } = "IntegrationEvent";
        public EventBusType EventBusType { get; set; } = EventBusType.RabbitMQ;

        public object Connection { get; set; } // dışarıdan kütüphane eklememek icin kullanıldı

        public bool DeleteEventPrefix => !String.IsNullOrEmpty(EventNamePrefix);
        public bool DeleteEventSuffix => !String.IsNullOrEmpty(EventNameSuffix);

    }
    public enum EventBusType
    {
        RabbitMQ=0,
        AzureServiceBus=1
    }
}
