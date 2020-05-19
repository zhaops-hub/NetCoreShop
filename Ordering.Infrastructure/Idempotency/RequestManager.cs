using Ordering.Domain.Exceptions;
using Ordering.Infrastructure;
using System;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Idempotency
{
    public class RequestManager : IRequestManager
    {
        private readonly OrderingContext _context;

        public RequestManager(OrderingContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


        public async Task CreateRequestForCommandAsync(string id, string name)
        {
            var exists = await ExistAsync(id);

            var request = exists ?
                throw new OrderingDomainException($"Request with {id} already exists") :
                new ClientRequest()
                {
                    Id = id,
                    Name = name,
                    Time = DateTime.UtcNow
                };

            _context.Add(request);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistAsync(string id)
        {
            var request = await _context.
                FindAsync<ClientRequest>(id);

            return request != null;
        }
    }
}
