using AutoMapper;
using MediatR;
using Merchants.Application.Queries;
using Merchants.Application.Responses;
using Merchants.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Handlers.Complaint
{
    public class GetAverageResponseTimeHandler : IRequestHandler<GetAverageResponseTime, Response>
    {
        private readonly IComplaintDetails _complaintDetails;
        private readonly IComplaint _complaint;
        private readonly ILogger<GetAverageResponseTimeHandler> _logger;
        private readonly IEscalation _escalation;
        private readonly IMapper _mapper;


        public GetAverageResponseTimeHandler(IComplaint repo, IMapper mapper, ILogger<GetAverageResponseTimeHandler> logger, IEscalation escalation, IComplaintDetails complaintDetails)
        {
            _mapper = mapper;
            _complaint = repo;
            _logger = logger;
            _escalation = escalation;
            _complaintDetails = complaintDetails;
        }
        public async Task<Response> Handle(GetAverageResponseTime request, CancellationToken cancellationToken)
        {
            Response response = new Response();
            try
            {
                var complaint = await _complaint.GetAllAsync();
                var inactiveComplaints = complaint.Where(x => x.Status.ToLower() == "inactive").TakeLast(100).ToList();

                foreach (var item in inactiveComplaints)
                {
                    var complaintDetails = await _complaintDetails.GetComplaintDetailByComplaintId(item.ID);
                    bool isClosed = complaintDetails.Any(c => c.CurrentStatus == "Closed" && c.isDeleted == false); 

                    if (isClosed)
                    {
                        var newComplaints = complaintDetails.FirstOrDefault(c => c.CurrentStatus == "New");
                        var closedComplaints = complaintDetails.FirstOrDefault(c => c.CurrentStatus == "Closed");

                        if (newComplaints != null && closedComplaints != null)
                        {
                            TimeSpan responseTime = closedComplaints?.CreatedAt - newComplaints?.CreatedAt ?? TimeSpan.Zero;

                            response.isSuccess = true;
                            response.ResponseCode = 1;
                            response.ResponseDescription = "Average Time";
                            response.Data = FormatTimeSpan(responseTime);

                            return response;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }

                response.isSuccess = false;
                response.ResponseCode = 0;
                response.ResponseDescription = "Average Time";
                response.Data = null;

                return response;

                //var merchantEntity = await _escalation.GetAllAsync();
                //if (merchantEntity != null && merchantEntity.Any())
                //{

                //    var responseTimesInMinutes = merchantEntity.Select(m =>
                //    {
                //        return m.ResponseType switch
                //        {
                //            "Minutes" => m.ResponseTime,
                //            "Hours" => m.ResponseTime * 60,
                //            "Days" => m.ResponseTime * 24 * 60,
                //            "Weeks" => m.ResponseTime * 7 * 24 * 60,
                //            _ => 0 
                //        };
                //    }).ToList();

                //    if (responseTimesInMinutes.Any())
                //    {
                //        double averageMinutes = responseTimesInMinutes.Average();

                //        string formattedAverage;
                //        if (averageMinutes >= 10080) 
                //        {
                //            formattedAverage = $"{averageMinutes / 10080:F1} Weeks";
                //        }
                //        else if (averageMinutes >= 1440) 
                //        {
                //            formattedAverage = $"{averageMinutes / 1440:F1} Days";
                //        }
                //        else if (averageMinutes >= 60) 
                //        {
                //            formattedAverage = $"{averageMinutes / 60:F1} Hours";
                //        }
                //        else
                //        {
                //            formattedAverage = $"{averageMinutes:F1} Minutes";
                //        }

                //        //Console.WriteLine($"Average Response Time: {formattedAverage}");
                //        response.isSuccess = true;
                //        response.ResponseCode = 1;
                //        response.ResponseDescription = "Average Time";
                //        response.Data = formattedAverage;


                //        return response;
                //    }
                //    else
                //    {
                //        _logger.LogError($" failed ");
                //        response.isSuccess = false;
                //        response.ResponseCode = 0;
                //        response.ResponseDescription = "Something went wrong...";
                //        response.Data = null;
                //        return response;
                //        //Console.WriteLine("No valid response times available to calculate average.");
                //    }
                //}
                //else
                //{
                //    _logger.LogError($"No Data Found ...failed ");
                //    response.isSuccess = false;
                //    response.ResponseCode = 0;
                //    response.ResponseDescription = "Something went wrong...";
                //    response.Data = null;
                //    return response;
                //}


            }
            catch (Exception ex)
            {
                _logger.LogError($"Complaint History failed {ex.Message}.");
                response.isSuccess = false;
                response.ResponseCode = 0;
                response.ResponseDescription = ex.Message;
                response.Data = null;
                return response;
            }

        }

        public string FormatTimeSpan(TimeSpan timeSpan)
        {
            if (timeSpan.TotalDays >= 1)
                return $"{timeSpan.TotalDays:F1} days";
            if (timeSpan.TotalHours >= 1)
                return $"{timeSpan.TotalHours:F1} Hrs";
            return $"{timeSpan.TotalMinutes:F1} minutes";
        }
    }
}
