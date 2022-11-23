using EventBus.Base.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Base.Abstraction
{
    // remde tutulan eventler daha sonradan vt eklenebilir değişiklikleri yönetebilmek icin kullandık
    public interface IEventBusSubscriptionManager
    {
        bool IsEmpty { get; } //Subscription olup olmadıgına bakıcak, bir event dinleniyormu
        event EventHandler<string> OnEventRemoved; // event silindiginde bir event oluşturacagız ve dışarıdan gelen unsubscription tetiklenecek
        void AddSubscription<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>;

        void RemoveSubscription<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>;
        bool HasSubscription<T>() where T : IntegrationEvent; // dışarıdan event gönderildiginde onu dinleyip dinlemedigimizi öğrendigimiz metod
        bool HasSubscriptionsForEvent(string eventName);//üstekiyle aynı işi yapıyor

        Type GetEventTypeByName(string eventName); // event isminden kendisine ulaşabilmek için

        void Clear();
        IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : IntegrationEvent; // bize gönderilen eventin tüm subscriptionları geriye dönen bir metod
        IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName);

        string GetEventKey<T>();


    }
}
