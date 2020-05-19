using MediatR;
using Ordering.Domain.Events;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.DomainEventHandlers.Order
{
    public class OrderStartedDomainEventHandler : INotificationHandler<OrderStartedDomainEvent>
    {
        public Task Handle(OrderStartedDomainEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
