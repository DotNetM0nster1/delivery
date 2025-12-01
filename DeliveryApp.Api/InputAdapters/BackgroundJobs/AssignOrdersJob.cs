using DeliveryApp.Core.Application.UseCases.Commands.OrderCommands.AssignOrder;
using MediatR;
using Quartz;

namespace DeliveryApp.Api.InputAdapters.BackgroundJobs
{
    [DisallowConcurrentExecution]
    public class AssignOrdersJob : IJob
    {
        private readonly IMediator _mediator;

        public AssignOrdersJob(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var assignOrdersCommand = new AssignOrdersCommand();
            await _mediator.Send(assignOrdersCommand);
        }
    }
}