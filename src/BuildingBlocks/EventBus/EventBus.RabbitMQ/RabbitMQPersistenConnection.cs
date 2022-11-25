using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace EventBus.RabbitMQ
{
    public class RabbitMQPersistenConnection : IDisposable
    {
        private IConnection connection;
        public readonly IConnectionFactory _connectionFactory;
        private readonly int retryCount;
        private object lock_object = new object();
        private bool _disposed;
        public RabbitMQPersistenConnection(IConnectionFactory connectionFactory, int retryCount=5)
        {
            _connectionFactory = connectionFactory;
            this.retryCount = retryCount;
        }
        public bool IsConnected => connection != null && connection.IsOpen;
        public IModel CreateModel()
        {
            return connection.CreateModel();
        }
        public void Dispose()
        {
            _disposed = true;
          connection.Dispose();
        }
        public bool TryConnect()
        {   // her connection create edilince gelmesi icin , kilitliyoruz
            // bir önceki objenin bitmesini bekleyecek
            lock (lock_object)
            {
                var policy = Policy.Handle<SocketException>()
                    .Or<BrokerUnreachableException>()
                    .WaitAndRetry(retryCount, retyAttempt => TimeSpan.FromSeconds(Math.Pow(2, retyAttempt)), (Exception, TimeSpan) =>
                    {

                    });

                //connection oluşturulması
                policy.Execute(() =>
                {
                    connection = _connectionFactory.CreateConnection();
                });

                if (IsConnected)// baglantı yapıldıysa
                {
                    // baglantının sürekli baglı kaldıgını dinlemek için
                    connection.ConnectionShutdown += Connection_ConnectionShutdown;
                    connection.CallbackException += Connection_CallbackException;
                    connection.ConnectionBlocked += Connection_ConnectionBlocked;
                    return true;
                }
                return false;
            }
        }

        private void Connection_ConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            if (_disposed) return;
            TryConnect();
        }

        private void Connection_CallbackException(object sender, CallbackExceptionEventArgs e)
        {
            if (_disposed) return;
            TryConnect();
        }

        private void Connection_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            if (_disposed) return;
            TryConnect();
        }
    }
}
