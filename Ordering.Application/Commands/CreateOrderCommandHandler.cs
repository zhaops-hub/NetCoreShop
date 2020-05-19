using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using Ordering.Infrastructure.Idempotency;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Commands
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;

        public CreateOrderCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }


        public async Task<bool> Handle(CreateOrderCommand message, CancellationToken cancellationToken)
        {
            var address = new Address(message.Country, message.City, "", message.ZipCode);
            var order = new Order(message.UserId, message.UserName, address, message.CardTypeId, message.CardNumber, message.CardSecurityNumber, message.CardHolderName, message.CardExpiration);
 
            _orderRepository.Add(order);

            return await _orderRepository.UnitOfWork
               .SaveEntitiesAsync(cancellationToken);
        }
    }


    // 幂等性处理
    public class CreateOrderIdentifiedCommandHandler : IdentifiedCommandHandler
    {
        public CreateOrderIdentifiedCommandHandler(
            IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler> logger)
            : base(mediator, requestManager, logger)
        {

        }

        protected override bool CreateResultForDuplicateRequest()
        {
            return true;
        }
    }
}
