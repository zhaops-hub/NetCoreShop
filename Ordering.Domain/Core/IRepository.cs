using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Domain.Core
{
    public interface IRepository<T> where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
