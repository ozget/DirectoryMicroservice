using EventBus.Base.Abstraction;
using EventBus.Base.SubManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace EventBus.Base.Events
{
    public abstract class BaseEventBus : IEventBus
    {
        public readonly IServiceProvider _serviceProvider;
        public readonly IEventBusSubscriptionManager _subManager;

        private EventBusConfig _eventBusConfig;

        public BaseEventBus(EventBusConfig config,IServiceProvider serviceProvider)
        {
            _eventBusConfig = config;
            _serviceProvider = serviceProvider;
            _subManager = new InMemoryEventBusSubscriptionManager(ProcessEventName);
        }
        //integrationEvent verildiginde name kısmından kırpma yapabilmeyi saglıyor
        public virtual string ProcessEventName(string eventName)
        {
            if (_eventBusConfig.DeleteEventPrefix)
                eventName = eventName.TrimStart(_eventBusConfig.EventNamePrefix.ToArray());

            if (_eventBusConfig.DeleteEventSuffix)
                eventName = eventName.TrimEnd(_eventBusConfig.EventNameSuffix.ToArray());

            return eventName;
        }

        //service.EventName 
        public virtual string GetSubName(string eventName)=> $"{_eventBusConfig.SubscriberClientAppName}.{ProcessEventName(eventName)}";

        public virtual void Dispose() => _eventBusConfig = null;

      
        public async Task<bool> ProcessEvent(string eventName,string message)
        {
            eventName = ProcessEventName(eventName);// başından veya sonundan kırpma queueName elde edilir

            var processed = false;

            if (_subManager.HasSubscriptionsForEvent(eventName))// gelen event daha önce dinlenilmişmi
            {
                var subscriptions = _subManager.GetHandlersForEvent(eventName);
                using(var scope = _serviceProvider.CreateScope())
                {
                    foreach (var subscription in subscriptions)
                    {
                        var handler = _serviceProvider.GetService(subscription.HandlerType);
                        if (handler==null) continue;

                        var eventType = _subManager.GetEventTypeByName($"{_eventBusConfig.EventNamePrefix}{eventName}{_eventBusConfig.EventNameSuffix}");
                        var integrationEvent = JsonConvert.DeserializeObject(message, eventType);

                        var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                        await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
                    }
                    
                }
                processed = true;
            }
            return processed;
        }

        public abstract void Publish(IntegrationEvent integrationEvent);

        public abstract void Subscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>;

        public abstract void UnSubscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>;

    }
}
