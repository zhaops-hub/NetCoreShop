using DotNetCore.CAP;
using Microsoft.Extensions.Options;
using Ordering.Application.Model;
using Ordering.Domain.Conf;

namespace Ordering.Service.EventBus
{
    public class EventBusHandler : ICapSubscribe
    {
        [CapSubscribe(EventBusSettings.AddUserEvent)]
        public void AddUserReceivedMessage(EventBusContract msg)
        {
            throw new System.Exception("111");
        }
    }
}
