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

    public class GetAllComplaintbyQueryHandler : IRequestHandler<GetAllComplaintsQuery, Response>
    {
        private readonly IComplaint _terminalRepository;
        private readonly ILogger<GetAllComplaintbyQueryHandler> _logger;
        private readonly IManagementHierarchy _managementHierarchy;
        private readonly IMerchant _merchant;
        private readonly IMapper _mapper;


        public GetAllComplaintbyQueryHandler(IComplaint terminalRepository, IMapper mapper, ILogger<GetAllComplaintbyQueryHandler> logger,IManagementHierarchy managementHierarchy,IMerchant merchant)
        {
            _mapper = mapper;

            _terminalRepository = terminalRepository;
            _logger = logger;
            _managementHierarchy = managementHierarchy;
            _merchant = merchant;
        }

        public async Task<Response> Handle(GetAllComplaintsQuery request, CancellationToken cancellationToken)
        {
            Response response = new Response();
            try
            {
                //var merchantEntity = _mapper.Map<Merchants.Core.Entities.ComplaintCategory>(request);

                var complaint = await _terminalRepository.GetComplaintsAsync(); //.GetAllAsync(x => x.isDeleted != true);

            

                //var complaint = await _terminalRepository.GetAllAsync();
                if (complaint.Count() == 0)
                {

                    response.isSuccess = false;
                    response.ResponseCode = 0;
                    response.ResponseDescription = " Record not Found.";
                    //response.Data = _mapper.Map<List<MerchantResponse>>(tenants); 
                    response.Data = complaint;

                    _logger.LogInformation($"Complaint {complaint} not Found.");
                }
                else
                {
                    var latestComplaints = complaint
                   .GroupBy(c => c.ID)  // Grouping by ID
                   .Select(g => g.OrderByDescending(c => c.CreatedAt).First()) // Selecting the latest record
                   .ToList();

                    foreach (var item in latestComplaints)
                    {
                        var poc = await _managementHierarchy.GetById(item.Mangementid.Value);
                        item.POCName = poc?.POCName ?? "N/A"; 
                        if (poc != null)
                        {
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
                        }
                        var merc = await _merchant.GetMerchantByID(item.MerchantID.Value);
                        item.MerchantName = merc.MerchantName;

                    }


                    response.isSuccess = true;
                    response.ResponseCode = 1;
                    response.ResponseDescription = "GetAll Complaint  Successfully.";
                    //response.Data = _mapper.Map<List<MerchantResponse>>(tenants); 
                    response.Data = latestComplaints;

                    _logger.LogInformation($"Complaint {complaint}  successfully.");
                }
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
