﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Idempotency
{
    public class ClientRequest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime Time { get; set; }
    }
}
