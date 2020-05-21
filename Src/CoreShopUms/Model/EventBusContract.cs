using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreShopUms.Model
{
    public class EventBusContract
    {

        public EventBusContract(string id, string msg)
        {
            Id = id;
            Msg = msg;
        }

        public string Id { get; private set; }

        public string Msg { get; private set; }
    }
}
