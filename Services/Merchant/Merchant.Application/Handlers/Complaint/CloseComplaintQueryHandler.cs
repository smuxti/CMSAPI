using MediatR;
using Merchants.Application.Queries;
using Merchants.Application.Responses;
using Merchants.Core.Entities;
using Merchants.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Handlers.Complaint
{
    public class CloseComplaintQueryHandler : IRequestHandler<CloseComplaintQuery, Response>
    {
        private readonly ILogger<CloseComplaintQueryHandler> _logger;
        private readonly IComplaint _complaint;
        private readonly IComplaintDetails _complaintDetails;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly INotificationRepo _notification;

        public CloseComplaintQueryHandler(ILogger<CloseComplaintQueryHandler> logger, IComplaint complaint, IComplaintDetails complaintDetails, IHttpContextAccessor httpContextAccessor, INotificationRepo notification)
        {
            _logger = logger;
            _complaint = complaint;
            _complaintDetails = complaintDetails;
            _httpContextAccessor = httpContextAccessor;
            _notification = notification;
        }

        public async Task<Response> Handle(CloseComplaintQuery request, CancellationToken cancellationToken)
        {
            Response baseResponse = new Response();
            try
            {
                var complaint = await _complaint.GetById(request.Id);
                var complaintDetails = (await _complaintDetails.GetComplaintDetailByComplaintId(request.Id)).OrderByDescending(x => x.CreatedAt).FirstOrDefault();
                if (complaint == null || complaintDetails == null)
                {
                    baseResponse.isSuccess = false;
                    baseResponse.ResponseCode = 0;
                    baseResponse.ResponseDescription = "No Complaint Found!";
                    baseResponse.Data = null;
                    return baseResponse;
                }
                if (complaint.OTP == request.Otp)
                {
                    complaint.Status = "InActive";
                    await _complaint.UpdateAsync(complaint);

                    Core.Entities.ComplaintDetails cDetails = new Core.Entities.ComplaintDetails();
                    //cDetails.TickentNo = await _complaintRepository.GetNextTicketNo();
                    cDetails.CurrentStatus = "Closed";
                    cDetails.Status = "InActive";
                    cDetails.ManagementId = complaintDetails.ManagementId;
                    cDetails.Level = complaintDetails.Level;
                    cDetails.ComplaintID = complaintDetails.ComplaintID;
                    cDetails.Description = complaintDetails.Description;
                    cDetails.EscalationId = complaintDetails.EscalationId;
                    cDetails.Remarks = request.Remarks;
                    cDetails.TickentNo = complaintDetails.TickentNo;
                    cDetails.CreatedBy = Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("UserID")?.Value);

                    await _complaintDetails.AddAsync(cDetails);
                    await _notification.NotificationToManagement(cDetails.ManagementId.ToString(), $"Complain Has Been Marked Closed :{cDetails.TickentNo}");

                    //complaintDetails.CurrentStatus = "Complete";
                    //await _complaintDetails.UpdateAsync(complaintDetails);

                    baseResponse.isSuccess = true;
                    baseResponse.ResponseCode = 1;
                    baseResponse.ResponseDescription = "Complaint Closed Successfully..";
                    baseResponse.Data = complaint.TicketNo;
                    return baseResponse;
                }
                baseResponse.isSuccess = false;
                baseResponse.ResponseCode = 0;
                baseResponse.ResponseDescription = "Incorrect Otp..";
                baseResponse.Data = null;
                return baseResponse;
            }
            catch (Exception ex)
            {
                baseResponse.isSuccess = false;
                baseResponse.ResponseCode = 0;
                baseResponse.ResponseDescription = ex.Message;
                baseResponse.Data = null;
                return baseResponse;
                throw;
            }
        }
    }
}
