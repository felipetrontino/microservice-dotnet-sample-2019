using Framework.Core.Bus;
using MediatR;

namespace Library.Domain.CommandSide.Commands
{
    public class CheckDueCommand : BusMessage, IRequest<bool>
    {
    }
}