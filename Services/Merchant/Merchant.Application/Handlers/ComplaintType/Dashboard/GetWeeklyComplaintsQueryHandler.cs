using MediatR;
using Merchants.Application.Queries;
using Merchants.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Handlers.Dashboard
{
    public class GetWeeklyComplaintsQueryHandler : IRequestHandler<GetWeeklyComplaintsCountQuery, List<int>>
    {
        private readonly IComplaint _repository;
        private readonly ILogger<GetWeeklyComplaintsQueryHandler> _logger;

        public GetWeeklyComplaintsQueryHandler(IComplaint repository, ILogger<GetWeeklyComplaintsQueryHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<List<int>> Handle(GetWeeklyComplaintsCountQuery request, CancellationToken cancellationToken)
        {
            var complaints = await _repository.GetWeeklyComplaints(request.year, request?.month, request?.week);

            //Dummy List
            List<int> complaintsByDay = Enumerable.Repeat(0, 7).ToList();

            foreach (var complaint in complaints)
            {
                // Sat 0, Sun 1, Mon 2, Tue 3 etc.
                int adjustedDayOfWeek = ((int)complaint.ComplaintDate.Value.DayOfWeek + 1) % 7;

                complaintsByDay[adjustedDayOfWeek]++;
            }

            return complaintsByDay;
        }
    }
}
