
using MediatR;
using Merchants.Application.Queries;
using Merchants.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Handlers
{
    public class GetYearlyComplaintQueryHandler : IRequestHandler<GetYearlyComplaintsCountQuery, List<int>>
    {
        private readonly IComplaint _repository;
        private readonly ILogger<GetYearlyComplaintQueryHandler> _logger;

        public GetYearlyComplaintQueryHandler(IComplaint repository, ILogger<GetYearlyComplaintQueryHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<List<int>> Handle(GetYearlyComplaintsCountQuery request, CancellationToken cancellationToken)
        {
            var complaints = await _repository.GetYearlyComplaints(request.Year);

            List<int> complaintsByMonth = Enumerable.Repeat(0, 12).ToList();

            foreach (var complaint in complaints)
            {
                int month = complaint.ComplaintDate.Value.Month;

                complaintsByMonth[month - 1]++;
            }

            return complaintsByMonth;
        }
    }
}
