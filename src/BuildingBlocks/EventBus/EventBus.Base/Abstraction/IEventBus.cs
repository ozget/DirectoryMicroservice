using EventBus.Base.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Base.Abstraction
{
    //servislerimizin hangi eventi subscribe, subscript
    public interface IEventBus
    {
        void Publish(IntegrationEvent integrationEvent); //servis bir event fırlatacagı zaman kullanacagız

        //IntegrationEvent,IIntegrationEventHandler gelecek subscribe edilecek rabbitMq verilecek kanallar oluşturulup dinlemeye başlacak
        void Subscribe<T, TH>() where T : IntegrationEvent where TH: IIntegrationEventHandler<T>;
        void UnSubscribe<T, TH>() where T : IntegrationEvent where TH: IIntegrationEventHandler<T>;

    }
}
