using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Merchants.Application.Commands.Complaint;
using Merchants.Application.Exceptions;
using Merchants.Application.Responses;
using Merchants.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Quartz.Logging;

namespace Merchants.Application.Handlers.Complaint
{
    public class UpdateComplaintStatusCommandHandler : IRequestHandler<UpdateComplaintStatusCommand, Response>
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IComplaint _repo;
        private readonly IComplaintDetails _complaintDetails;
        private readonly IUserRepository _userRepository;
        private readonly IManagementHierarchy _hierarchyRepository;
        private readonly INotificationRepo _notification;

        //private readonly IRedisCacheService _redisCacheService;

        public UpdateComplaintStatusCommandHandler(IComplaint repo, IMapper mapper,
            ILogger<UpdateComplaintStatusCommandHandler> logger, IHttpContextAccessor httpContextAccessor, IComplaintDetails complaintDetails,
            IUserRepository userRepository, IManagementHierarchy hierarchyRepository, INotificationRepo notification)
        {
            _repo = repo;
            _mapper = mapper;
            _logger = logger;
            _complaintDetails = complaintDetails;
            _userRepository = userRepository;
            _hierarchyRepository = hierarchyRepository;
            _notification = notification;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Response> Handle(UpdateComplaintStatusCommand request, CancellationToken cancellationToken)
        {
            Response response = new Response();
            try
            {
                var complaint = await _repo.GetById(request.ComplaintId);
                if (complaint == null)
                {
                    _logger.LogError($"Complaint Not Found.");
                    throw new MerchantNotFoundException(nameof(complaint), request.ComplaintId);
                }

                var complaintDetail = await _complaintDetails.GetComplaintDetailByComplaintId(request.ComplaintId);
                if (complaintDetail == null)
                {
                    _logger.LogError($"complaint Detail Not Found.");
                    throw new MerchantNotFoundException(nameof(complaint), request.ComplaintId);
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

                var user = await _userRepository.GetUserById((Guid)request.UserId);

                Core.Entities.ComplaintDetails cDetails = new Core.Entities.ComplaintDetails();

                if (request.Completed)
                {
                    cDetails.CurrentStatus = "Complete";
                    await _notification.NotificationToManagement("0", $"Complain Has Been Marked Closed :{cDetails.TickentNo},{complaint.ID}");
                }
                else
                    cDetails.CurrentStatus = "Processing";

                cDetails.Status = "Active";
                cDetails.ManagementId = user.ManagementId;
                cDetails.Remarks = request.Remarks;
                cDetails.Level = latestComplaintDetail.Level;
                cDetails.ComplaintID = latestComplaintDetail.ComplaintID;
                cDetails.Description = latestComplaintDetail.Description;
                cDetails.EscalationId = latestComplaintDetail.EscalationId;
                cDetails.EscalationTime = latestComplaintDetail.EscalationTime;
                var userId = Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("UserID")?.Value);
                cDetails.CreatedBy = Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("UserID")?.Value);

                var result = await _complaintDetails.AddAsync(cDetails);

                _logger.LogError($"Complaint Started");
                response.isSuccess = true;
                response.ResponseCode = 1;
                response.ResponseDescription = "Complaint Updated.";
                response.Data = cDetails;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Complaint updation failed {ex.Message}.");
                response.isSuccess = false;
                response.ResponseCode = 0;
                response.ResponseDescription = "Complaint Update Failed.";
                response.Data = null;
                return response;
            }

        }
    }
}
