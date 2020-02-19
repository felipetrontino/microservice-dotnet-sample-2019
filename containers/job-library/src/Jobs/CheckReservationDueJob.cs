using Framework.Core.Job.Quartz;
using Library.Domain.CommandSide.Commands;
using MediatR;
using System.Threading.Tasks;

namespace Library.Job.Jobs
{
    public class CheckReservationDueJob : BaseJob
    {
        private readonly IMediator _mediator;

        public CheckReservationDueJob(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected override Task Process()
        {
            var message = new CheckDueCommand();
            return _mediator.Send(message);
        }
    }
}