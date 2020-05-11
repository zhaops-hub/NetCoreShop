namespace CoreShopUms.Conf
{
    public class AppSettings
    {
        public Deploy Deploy { get; set; }

        public RabbitMq RabbitMq { get; set; }

        public Consul Consul { get; set; }

        public Cap Cap { get; set; }

        public string DefaultConnection { get; set; }
    }

    public class Deploy
    {
        public string HostName { get; set; }

        public int Port { get; set; }

        public string NodeId { get; set; }

        public string NodeName { get; set; }
    }

    public class RabbitMq
    {
        public string HostName { get; set; }
        public string UserName { get; set; }

        public string PassWord { get; set; }
    }

    public class Consul
    {
        public string HostName { get; set; }


        public int Port { get; set; }
    }

    public class Cap
    {
        public string ConnectionString { get; set; }

        public string TableNamePrefix { get; set; }
    }
}