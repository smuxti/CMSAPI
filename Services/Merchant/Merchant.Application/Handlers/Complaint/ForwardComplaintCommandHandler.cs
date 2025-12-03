using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Merchants.Application.Commands.Complaint;
using Merchants.Application.Exceptions;
using Merchants.Application.Handlers.ComplaintDetails;
using Merchants.Application.Responses;
using Merchants.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Merchants.Application.Commands;

namespace Merchants.Application.Handlers.Complaint
{
    public class ForwardComplaintCommandHandler : IRequestHandler<ForwardComplaintCommand, Response>
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IComplaint _repo;
        private readonly IComplaintDetails _complaintDetails;
        private readonly IUserRepository _userRepository;
        private readonly IManagementHierarchy _hierarchyRepository;
        //private readonly IRedisCacheService _redisCacheService;

        public ForwardComplaintCommandHandler(IComplaint repo, IMapper mapper,
            ILogger<ForwardComplaintCommandHandler> logger, IHttpContextAccessor httpContextAccessor, IComplaintDetails complaintDetails,
            IUserRepository userRepository, IManagementHierarchy hierarchyRepository)
        {
            _repo = repo;
            _mapper = mapper;
            _logger = logger;
            _complaintDetails = complaintDetails;
            _userRepository = userRepository;
            _hierarchyRepository = hierarchyRepository;
        }
        public async Task<Response> Handle(ForwardComplaintCommand request, CancellationToken cancellationToken)
        {
            Response response = new Response();
            try
            {
                var complaint = await _repo.GetById(request.Id);
                if (complaint == null)
                {
                    _logger.LogError($"Complaint Not Found.");
                    throw new MerchantNotFoundException(nameof(complaint), request.Id);
                }

                var complaintDetail = await _complaintDetails.GetComplaintDetailByComplaintId(request.Id);
                if (complaintDetail == null)
                {
                    _logger.LogError($"complaint Detail Not Found.");
                    throw new MerchantNotFoundException(nameof(complaint), request.Id);
                }

                var latestComplaintDetail = complaintDetail
                    .OrderByDescending(c => c.CreatedAt)
                    .FirstOrDefault();

                if (latestComplaintDetail.CurrentStatus == "Complete" || latestComplaintDetail.CurrentStatus == "Completed")
                {
                    _logger.LogError($"Complaint already marked as complete.");
                    response.isSuccess = false;
                    response.ResponseCode = 0;
                    response.ResponseDescription = "Complaint already marked as Complete.";
                    response.Data = null;
                    return response;
                }

                string previousStatus = latestComplaintDetail.CurrentStatus;
                latestComplaintDetail.Status = "InActive";
                latestComplaintDetail.CurrentStatus = "Forwarded";
                //latestComplaintDetail.UpdatedBy = Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("UserID")?.Value);

                var updateDetail = await _complaintDetails.UpdateAsync(latestComplaintDetail);

                Core.Entities.ComplaintDetails cDetails = new Core.Entities.ComplaintDetails();
                cDetails.TickentNo = latestComplaintDetail.TickentNo;
                cDetails.ComplaintID = latestComplaintDetail.ComplaintID;
                cDetails.EscalationId = latestComplaintDetail.EscalationId;
                cDetails.EscalationTime = latestComplaintDetail.EscalationTime;
                cDetails.Description = latestComplaintDetail.Description;

                cDetails.CurrentStatus = previousStatus;
                cDetails.Status = "Active";

                cDetails.Level = request.Level;
                cDetails.ManagementId = request.HierarchyId;

                cDetails.ID = 0;
                //cDetails.CreatedBy = Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("UserID")?.Value);

                var result = await _complaintDetails.AddAsync(cDetails);

                _logger.LogError($"Complaint Forwarded");
                response.isSuccess = true;
                response.ResponseCode = 1;
                response.ResponseDescription = "Complaint Forwarded.";
                response.Data = cDetails;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Complaint Forward failed {ex.Message}.");
                response.isSuccess = false;
                response.ResponseCode = 0;
                response.ResponseDescription = "Complaint Forward Failed.";
                response.Data = null;
                return response;
            }

        }
    }
}
