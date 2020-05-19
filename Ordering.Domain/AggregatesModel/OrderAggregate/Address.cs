using Ordering.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Domain.AggregatesModel.OrderAggregate
{
    public class Address : ValueObject
    {
        public String County { get; private set; }
        public String City { get; private set; }
        public String Province { get; private set; }
        public String ZipCode { get; private set; }

        public Address() { }

        public Address(string county, string city, string province, string zipcode)
        {
            County = county;
            City = city;
            Province = province;
            ZipCode = zipcode;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return County;
            yield return City;
            yield return Province;
            yield return ZipCode;
        }
    }
}
