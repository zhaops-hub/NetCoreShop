using Ordering.Domain.Core;
using Ordering.Domain.Exceptions;

namespace Ordering.Domain.AggregatesModel.OrderAggregate
{
    public class OrderItem : Entity
    {
        private string _productName;
        private string _pictureUrl;
        private decimal _unitPrice;
        private decimal _discount;
        private int _units;

        public int OrderId { get; private set; }

        public int ProductId { get; private set; }


        protected OrderItem() { }


        public OrderItem(int orderId, int productId, string productName, decimal unitPrice, decimal discount, string PictureUrl, int units = 1)
        {
            if (units <= 0)
            {
                throw new OrderingDomainException("Invalid number of units");
            }

            if ((unitPrice * units) < discount)
            {
                throw new OrderingDomainException("The total of order item is lower than applied discount");
            }

            OrderId = orderId;
            ProductId = productId;
            _productName = productName;
            _unitPrice = unitPrice;
            _discount = discount;
            _units = units;
            _pictureUrl = PictureUrl;
        }

        public string GetPictureUri() => _pictureUrl;


        public decimal GetCurrentDiscount()
        {
            return _discount;
        }

        public int GetUnits()
        {
            return _units;
        }

        public decimal GetUnitPrice()
        {
            return _unitPrice;
        }

        public string GetOrderItemProductName() => _productName;

        public void SetNewDiscount(decimal discount)
        {
            if (discount < 0)
            {
                throw new OrderingDomainException("Discount is not valid");
            }

            _discount = discount;
        }


        public void AddUnits(int units)
        {
            if (units < 0)
            {
                throw new OrderingDomainException("Invalid units");
            }

            _units += units;
        }
    }
}
