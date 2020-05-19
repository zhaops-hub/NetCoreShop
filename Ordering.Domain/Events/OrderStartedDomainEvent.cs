using MediatR;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using System;

namespace Ordering.Domain.Events
{
    public class OrderStartedDomainEvent : INotification
    {
        public Order Order { get; }

        public OrderStartedDomainEvent(Order order )
        {
            Order = order;
        }
    }
}
