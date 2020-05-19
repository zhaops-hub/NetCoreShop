using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Infrastructure.Idempotency;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Commands
{
    public class IdentifiedCommandHandler : IRequestHandler<IdentifiedCommand, bool>
    {
        private readonly IMediator _mediator;
        private readonly IRequestManager _requestManager;
        private readonly ILogger<IdentifiedCommandHandler> _logger;

        public IdentifiedCommandHandler(
            IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler> logger)
        {
            _mediator = mediator;
            _requestManager = requestManager;
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }


        protected virtual bool CreateResultForDuplicateRequest()
        {
            return default(bool);
        }

        public async Task<bool> Handle(IdentifiedCommand request, CancellationToken cancellationToken)
        {
            var alreadyExists = await _requestManager.ExistAsync(request.Id);

            if (alreadyExists)
            {
                return CreateResultForDuplicateRequest();
            }
            else
            {
                await _requestManager.CreateRequestForCommandAsync(request.Id, request.Type);
                var command = request.Command;
                var result = await _mediator.Send(command, cancellationToken);

                return result;
            }
        }


    }
}
