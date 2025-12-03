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
using Merchants.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Quartz.Logging;

namespace Merchants.Application.Handlers.Complaint
{
    public class GetComplaintHistoryHandler : IRequestHandler<GetComplaintHistoryQuery, Response>
    {
        private readonly IUserRepository _user;
        private readonly IComplaint _complaint;
        private readonly ILogger<GetComplaintHistoryHandler> _logger;
        private readonly IManagementHierarchy _managementHierarchy;
        private readonly IMapper _mapper;
        private readonly IComplaintCategory _category;
        private readonly IComplaintDetails _details;
        private readonly IMerchant _merchant;

        public GetComplaintHistoryHandler(IComplaintCategory category, IComplaint repo, IMapper mapper, ILogger<GetComplaintHistoryHandler> logger, IManagementHierarchy managementHierarchy, IComplaintDetails details, IMerchant merchant, IUserRepository user)
        {
            _mapper = mapper;
            _complaint = repo;
            _logger = logger;
            _managementHierarchy = managementHierarchy;
            _category = category;
            _details = details;
            _merchant = merchant;
            _user = user;
        }

        public async Task<Response> Handle(GetComplaintHistoryQuery request, CancellationToken cancellationToken)
        {
            Response response = new Response();
            try
            {
                //var merchantEntity = _mapper.Map<Merchants.Core.Entities.ComplaintCategory>(request);
                var co = await _complaint.GetById(request.ComplainID);
                var details = await _details.GetComplaintDetailByComplaintId(request.ComplainID);

                if (co.MerchantID != null && co.MerchantID != 0)
                {
                    var comp = await _complaint.GetComlaintMerchantHistory(request.ComplainID);

                    foreach (var item in comp)
                    {
                        var poc = await _managementHierarchy.GetManagementHierarchyByID(item.ManagementID.Value);
                        item.POCName = poc?.POCName ?? "N/A";

                        if (poc != null)
                        {
                            if (poc.ManagementType >= 0)
                                item.POCName = $"{poc?.Name} ({poc?.POCName ?? "N/A"})";
                            else if (poc.ManagementType == -1)
                            {
                                var mer = await _merchant.GetMerchantByID(co.MerchantID.Value);
                                item.POCName = $"{mer?.MerchantName ?? "N/A"}" ;
                            }
                            else if (poc.ManagementType == -2)
                            {
                                var mer = await _merchant.GetMerchantByID(co.MerchantID.Value);
                                var area = await _managementHierarchy.GetById(mer.Area);
                                item.POCName = $"{area?.POCName ?? "N/A"} ({mer?.MerchantName})" ;
                            }
                            else if (poc.ManagementType == -3)
                            {
                                var mer = await _merchant.GetMerchantByID(co.MerchantID.Value);
                                var zone = await _managementHierarchy.GetById(mer.Zone);
                                item.POCName = $"{zone?.POCName ?? "N/A"} ({mer?.MerchantName})" ;
                            }
                        }
                        var category = await _category.GetComplaintCategoryByID(item.CategoryID);
                        item.Category = category.Category;

                        var latestDetailMerchant = details
                            .OrderByDescending(d => d.CreatedAt)
                            .FirstOrDefault();

                        item.LatestStatus = latestDetailMerchant.CurrentStatus;
                    }

                    response.isSuccess = true;
                    response.ResponseCode = 1;
                    response.ResponseDescription = "Complaint History  Successfully.";
                    //response.Data = _mapper.Map<List<MerchantResponse>>(tenants); 
                    response.Data = comp;
                    return response;

                }

                var complaint = await _complaint.GetComlaintHistory(request.ComplainID); //.GetAllAsync(x => x.isDeleted != true);
                var latestDetail = details
                            .OrderByDescending(d => d.CreatedAt)
                            .FirstOrDefault();



                if (complaint.Count == 0)
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
                    foreach (var item in complaint)
                    {
                        item.LatestStatus = latestDetail.CurrentStatus;
                        var poc = await _managementHierarchy.GetManagementHierarchyByID(item.ManagementID.Value);
                        item.POCName = $"{poc?.Name} ({poc?.POCName ?? "N/A"})";
                    }

                    response.isSuccess = true;
                    response.ResponseCode = 1;
                    response.ResponseDescription = "Complaint History  Successfully.";
                    //response.Data = _mapper.Map<List<MerchantResponse>>(tenants); 
                    response.Data = complaint;

                    _logger.LogInformation($"Complaint {complaint}  successfully.");
                }
                return response;

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
    }
}
