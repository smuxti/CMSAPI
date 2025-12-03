using MediatR;
using Merchants.Application.Queries;
using Merchants.Core.Entities;
using Merchants.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Handlers.Dashboard
{
    public class GetComplaintStatusCountQueryHandler : IRequestHandler<GetComplaintStatusCountQuery, List<int>>
    {
        private readonly IComplaint _repository;
        private readonly IComplaintDetails _complaintDetail;
        private readonly ILogger<GetComplaintStatusCountQueryHandler> _logger;

        public GetComplaintStatusCountQueryHandler(IComplaint repository, ILogger<GetComplaintStatusCountQueryHandler> logger, IComplaintDetails complaintDetail)
        {
            _repository = repository;
            _logger = logger;
            _complaintDetail = complaintDetail;
        }

        public async Task<List<int>> Handle(GetComplaintStatusCountQuery request, CancellationToken cancellationToken)
        {
            var complaints = await _repository.GetWeeklyComplaintDetails(request.year, request.month, request.week);

            List<int> complaintsByStatus = Enumerable.Repeat(0, 5).ToList();

            foreach (var complaint in complaints)
            {
                string? status = complaint.CurrentStatus;
                if (status != null)
                {
                    switch (status.ToLower())
                    {
                        case "complete" or "completed":
                            complaintsByStatus[0]++;
                            break;
                        case "processing" or "pending" or "process":
                            complaintsByStatus[1]++; 
                            break;
                        case "new" or "active":
                            complaintsByStatus[2]++;
                            break;
                        case "cancel" or "cancelled" or "closed":
                            complaintsByStatus[3]++; 
                            break;
                        default:
                            complaintsByStatus[4]++; //Unknown
                            break;
                    }
                }
            }
            return complaintsByStatus;
        }
    }
}
