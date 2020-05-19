using Ordering.Domain.Core;
using Ordering.Domain.Events;
using Ordering.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ordering.Domain.AggregatesModel.OrderAggregate
{
    public class Order : Entity, IAggregateRoot
    {
        private DateTime _orderDate;


        public Address Address { get; private set; }

        public int? GetBuyerId => _buyerId;
        private int? _buyerId;


        public OrderStatus OrderStatus { get; private set; }
        private int? _orderStatusId;

        private int? _paymentMethodId;


        private string _description;

        private bool _isDraft;

        private readonly List<OrderItem> _orderItems;
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

        public static Order NewDraft()
        {
            var order = new Order();
            order._isDraft = true;
            return order;
        }

        protected Order()
        {
            _orderItems = new List<OrderItem>();
            _isDraft = false;
        }


        public Order(string userId, string userName, Address address, int cardTypeId, string cardNumber, string cardSecurityNumber,
               string cardHolderName, DateTime cardExpiration, int? buyerId = null, int? paymentMethodId = null) : this()
        {
            _buyerId = buyerId;
            _paymentMethodId = paymentMethodId;
            _orderStatusId = OrderStatus.Submitted.Id;
            _orderDate = DateTime.UtcNow;
            Address = address;


            AddOrderStartedDomainEvent(userId, userName, cardTypeId, cardNumber,
                                       cardSecurityNumber, cardHolderName, cardExpiration);
        }



        public void AddOrderItem(int productId, string productName, decimal unitPrice, decimal discount, string pictureUrl, int units = 1)
        {
            var existingOrderForProduct = _orderItems.Where(o => o.ProductId == productId)
                .SingleOrDefault();

            if (existingOrderForProduct != null)
            {
                //if previous line exist modify it with higher discount  and units..

                if (discount > existingOrderForProduct.GetCurrentDiscount())
                {
                    existingOrderForProduct.SetNewDiscount(discount);
                }

                existingOrderForProduct.AddUnits(units);
            }
            else
            {
                //add validated new order item

                var orderItem = new OrderItem(Id, productId, productName, unitPrice, discount, pictureUrl, units);
                _orderItems.Add(orderItem);
            }
        }


        public void SetPaymentId(int id)
        {
            _paymentMethodId = id;
        }

        public void SetBuyerId(int id)
        {
            _buyerId = id;
        }

        public void SetCancelledStatusWhenStockIsRejected(IEnumerable<int> orderStockRejectedItems)
        {
            if (_orderStatusId == OrderStatus.AwaitingValidation.Id)
            {
                _orderStatusId = OrderStatus.Cancelled.Id;

                var itemsStockRejectedProductNames = OrderItems
                    .Where(c => orderStockRejectedItems.Contains(c.ProductId))
                    .Select(c => c.GetOrderItemProductName());

                var itemsStockRejectedDescription = string.Join(", ", itemsStockRejectedProductNames);
                _description = $"The product items don't have stock: ({itemsStockRejectedDescription}).";
            }
        }



        private void AddOrderStartedDomainEvent(string userId, string userName, int cardTypeId, string cardNumber,
               string cardSecurityNumber, string cardHolderName, DateTime cardExpiration)
        {
            var orderStartedDomainEvent = new OrderStartedDomainEvent(this);

            this.AddDomainEvent(orderStartedDomainEvent);
        }

        private void StatusChangeException(OrderStatus orderStatusToChange)
        {
            throw new OrderingDomainException($"Is not possible to change the order status from {OrderStatus.Name} to {orderStatusToChange.Name}.");
        }


        public decimal GetTotal()
        {
            return _orderItems.Sum(o => o.GetUnits() * o.GetUnitPrice());
        }
    }
}
