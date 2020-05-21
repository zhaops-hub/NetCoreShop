using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Domain.Conf
{
    public class AppSettings
    {
        public string DeployHostName { get; set; }
        public string DeployPort { get; set; }
        public string DeployNodeId { get; set; }
        public string DeployNodeName { get; set; }
        public string ConsulHostName { get; set; }
        public string ConsulPort { get; set; }
        public string RabbitMqHostName { get; set; }
        public string RabbitMqUserName { get; set; }
        public string RabbitMqPassWord { get; set; }
        public string DefaultConnection { get; set; }
        public string CapConnectionString { get; set; }
        public string CapTableNamePrefix { get; set; }
    }
}
