using MediatR;
using Merchants.Application.Commands.Escalation;
using Merchants.Application.Queries;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.BackgroundJobs
{
    public class GetAllChannelQueryJob : IJob
    {
        private readonly IMediator _mediator;
        private readonly ILogger<GetAllChannelQueryJob> _logger;

        public GetAllChannelQueryJob(IMediator mediator, ILogger<GetAllChannelQueryJob> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Starting The Job");

            //GetAllChannelQuery query = new GetAllChannelQuery(); 
            AddEscalateCommand AddEscal = new AddEscalateCommand();
            Console.WriteLine("Running GetAllChannelbyQueryHandler at: " + DateTime.Now);
            await _mediator.Send(AddEscal);

            _logger.LogInformation("Get All Channel query job done");
        }
    }
}
