using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Merchants.Application.Commands.Complaint;
using Merchants.Application.Queries;
using Merchants.Application.Responses;
using Merchants.Core.Entities;
using Merchants.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace Merchants.Application.Handlers.Complaint
{
    public class GetComplaintByManagmentIdQueryHandler : IRequestHandler<GetComplaintByManagmentIDQuery, Response>
    {
        private readonly IComplaint _repo;
        private readonly ILogger<GetComplaintByManagmentIdQueryHandler> _logger;
        private readonly IManagementHierarchy _managementHierarchy;
        private readonly IMerchant _merchant;
        private readonly IMapper _mapper;

        public GetComplaintByManagmentIdQueryHandler(IComplaint repo, IMapper mapper, ILogger<GetComplaintByManagmentIdQueryHandler> logger, IManagementHierarchy managementHierarchy, IMerchant merchant)
        {
            _mapper = mapper;
            _repo = repo;
            _logger = logger;
            _managementHierarchy = managementHierarchy;
            _merchant = merchant;
        }

        public async Task<Response> Handle(GetComplaintByManagmentIDQuery request, CancellationToken cancellationToken)
        {
            Response response = new Response();
            try
            {
                var complaint = await _repo.GetComplaintByManagmentIdAsync(request.ManagementID);


                if (complaint == null)
                {

                    response.isSuccess = false;
                    response.ResponseCode = 0;
                    response.ResponseDescription = "Record not Found.";
                    response.Data = null;

                    _logger.LogInformation($"Complaint {complaint} not Found.");
                    return response;
                }
                var latestComplaints = complaint
                  .GroupBy(c => c.ID)  // Grouping by ID
                  .Select(g => g.OrderByDescending(c => c.CreatedAt).First()) // Selecting the latest record
                  .ToList();
                foreach (var item in latestComplaints)
                {
                    //var poc = await _managementHierarchy?.GetById(request.ManagementID);
                    //item.POCName = poc?.POCName ?? "N/A";

                    var poc = await _managementHierarchy?.GetById(item.Mangementid.Value);

                    if (poc.ManagementType >= 0)
                        item.POCName = poc?.POCName ?? "N/A";
                    else if (poc.ManagementType == -1)
                    {
                        var mer = await _merchant.GetMerchantByID(item.MerchantID.Value);
                        item.POCName = mer?.MerchantName ?? "N/A";
                    }
                    else if (poc.ManagementType == -2)
                    {
                        var mer = await _merchant.GetMerchantByID(item.MerchantID.Value);
                        var area = await _managementHierarchy.GetById(mer.Area);
                        item.POCName = area?.POCName ?? "N/A";
                    }
                    else if (poc.ManagementType == -3)
                    {
                        var mer = await _merchant.GetMerchantByID(item.MerchantID.Value);
                        var zone = await _managementHierarchy.GetById(mer.Zone);
                        item.POCName = zone?.POCName ?? "N/A";
                    }

                    var merc = await _merchant?.GetMerchantByID(item.MerchantID.Value);
                    item.MerchantName = merc?.MerchantName ?? "N/A";
                }



                response.isSuccess = true;
                response.ResponseCode = 1;
                response.ResponseDescription = "Get Complaint Successful.";
                response.Data = latestComplaints;

                _logger.LogInformation($"Complaint {complaint} found successfully.");
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Complaint addition failed {ex.Message}.");
                response.isSuccess = false;
                response.ResponseCode = 0;
                response.ResponseDescription = ex.Message;
                response.Data = null;
                return response;
            }


        }

    }
}
