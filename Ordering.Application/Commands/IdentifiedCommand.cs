using MediatR;
using System;

namespace Ordering.Application.Commands
{
    public class IdentifiedCommand : IRequest<bool>
    {
        public string Type { get; }

        public IRequest<bool> Command { get; }
        public String Id { get; }
        public IdentifiedCommand(IRequest<bool> command, String id, string type)
        {
            Command = command;
            Id = id;
            Type = type;
        }
    }
}
