using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreShopUms.Conf
{
    public class AppSettings
    {
        public RabbitMq RabbitMq { get; set; }

        public Cap Cap { get; set; }

    }


    public class RabbitMq
    {
        public string HostName { get; set; }
        public string UserName { get; set; }

        public string PassWord { get; set; }
    }

    public class Cap
    {
        public string ConnectionString { get; set; }

        public string TableNamePrefix { get; set; }
    }
}
