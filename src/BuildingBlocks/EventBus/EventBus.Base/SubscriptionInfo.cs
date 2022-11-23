using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Base
{
    // bize gelen verilerin iceride tutulması için kullanılır
    public class SubscriptionInfo
    {
        // integrationEventin Tipi tutulur buradan handler metoduna ulaşılır
        public Type HandlerType { get; }

        public SubscriptionInfo(Type handleType)
        {
            HandlerType = handleType ?? throw new ArgumentOutOfRangeException(nameof(handleType));
        }

        public static SubscriptionInfo Typed(Type handlerType)
        {
            return new SubscriptionInfo(handlerType);
        }
    }
}
