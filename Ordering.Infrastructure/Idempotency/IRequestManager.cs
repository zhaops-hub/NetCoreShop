using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Idempotency
{
    public interface IRequestManager
    {
        Task<bool> ExistAsync(string id);

        Task CreateRequestForCommandAsync(string id, string name);
    }
}
