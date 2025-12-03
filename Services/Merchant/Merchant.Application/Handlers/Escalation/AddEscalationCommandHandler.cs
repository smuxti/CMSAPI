using AutoMapper;
using MediatR;
using Merchants.Application.Commands.ComplaintType;
using Merchants.Application.Commands.Escalation;
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

namespace Merchants.Application.Handlers.Escalation
{
    //internal class AddEscalationCommandHandler
    //{
    //}

    public class AddEscalationCommandHandler : IRequestHandler<AddEscalationCommand, Response>
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IEscalation _escalation;
        private readonly IComplaintCategory _compCateg;
        private readonly IManagementHierarchy _mngHr;
        //private readonly Mail _mail;
        //private readonly IConfiguration _configuration;
        //private readonly string _baseUrl;
        //private readonly IRedisCacheService _redisCacheService;


        public AddEscalationCommandHandler(IEscalation escalation,IComplaintCategory compCateg,IManagementHierarchy mngHr, IMapper mapper,
            ILogger<AddEscalationCommandHandler> logger, IHttpContextAccessor httpContextAccessor)
        {
            _escalation = escalation;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _compCateg = compCateg;
            _mngHr = mngHr;
            //_mail = mail;
            //_configuration = configuration;
            ////_baseUrl = _configuration["Urls:ActivationUrl"];
            //_redisCacheService = redisCacheService;
        }

        public async Task<Response> Handle(AddEscalationCommand request, CancellationToken cancellationToken)
        {
            Response response = new Response();
            try
            {


                var merchantEntity = _mapper.Map<IEnumerable<Merchants.Core.Entities.Escalation>>(request.Escalations);
                if (merchantEntity.GroupBy(x => x.Type).Count() > 1)
                {
                    _logger.LogInformation($"Multiple Complain Types are not allowed.");


                    response.Data = new
                    {
                        Message = "Multiple Complain Types are not allowed.",
                    };
                    return response;
                }
                //bool chk = merchantEntity.Zip(merchantEntity.Skip(1), (first, second) => first.Level < second.Level).All(valid => valid);
                //if (!chk)
                //{
                //    _logger.LogInformation($"Level of each record should be less than the next record's Level");


                //    response.Data = new
                //    {
                //        Message = "Level of each record should be less than the next record's Level",
                //    };
                //    return response;
                //}
                foreach(var fld in merchantEntity)
                {
                    var categ = await _compCateg.GetAllAsync(x => x.ID == fld.CategoryID);
                    if(categ.Count == 0) 
                    {
                        _logger.LogInformation($"Invalid category");
                        response.Data = new
                        {
                            Message = "Invalid category",
                        };
                        return response;
                    }
                    var managmentH = await _mngHr.GetAllAsync(x => x.ID == fld.ManagementID);
                    if(managmentH.Count == 0) 
                    {
                        _logger.LogInformation($"Invalid Management ID");
                        response.Data = new
                        {
                            Message = "Invalid Managment ID",
                        };
                        return response;

                    }
                    else
                    {
                        if (managmentH.Select(s=> s.POCEmail == fld.Email).Count() == 0)
                        {
                            _logger.LogInformation($"Invalid Email ID");
                            response.Data = new
                            {
                                Message = "Invalid Email ID",
                            };
                            return response;

                        }
                        if (managmentH.Select(s => s.POCNumber == fld.ContactNumber).Count() == 0)
                        {
                            _logger.LogInformation($"Invalid Contact Number");
                            response.Data = new
                            {
                                Message = "Invalid Contact Number",
                            };
                            return response;

                        }

                    }
                    var dupEsc = await _escalation.GetAllAsync(x => x.CategoryID == fld.CategoryID && x.ManagementID == fld.ManagementID && x.Level == fld.Level && x.Type == fld.Type && x.isDeleted == false && x.Status == "Active");
                    if(dupEsc.Count() > 0) 
                    {
                        _logger.LogInformation($"Duplicate record.");
                        response.Data = new
                        {
                            Message = "Duplicate record.",
                        };

                        response.isSuccess = false;
                        response.ResponseCode = 0;
                        response.Data = null;
                        return response;
                    }

                    //var escalation = _mapper.Map<Merchants.Core.Entities.Escalation>(fld);
                    //escalation.CreatedBy = fld.CreatedBy;
                    //var obj = await _escalation.AddEscalationAsync(escalation);
                }

                //merchantEntity.CreatedBy = new Guid("00000000-0000-0000-0000-000000000001");  
                //merchantEntity.Status = "1";

                foreach(var item in merchantEntity)
                {
                    item.CreatedBy = Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("UserID")?.Value);
                }
                var obj = await _escalation.AddEsalationsAs(merchantEntity);
                var Escalation = "";
                int EscalationId ;

                //var existingComplainer = await _escalation.GetAllAsync(x => x.ManagementID == merchantEntity.ManagementID && x.CategoryID == merchantEntity.CategoryID);


                //var deletedComplainer = existingComplainer.FirstOrDefault(x => x.isDeleted == true);

                //if (existingComplainer.Any() && deletedComplainer == null)
                //{
                //    var firstComplainer = existingComplainer.First();
                //    Escalation = firstComplainer.Email ?? string.Empty; 
                //    EscalationId = firstComplainer.MatrixID;
                //    _logger.LogInformation($"Escalation already exists with ID: {EscalationId}. Using existing complainer.");


                //    response.Data = new
                //    {
                //        Message = "Escalation already exists.",
                //        EscalationDetails = Escalation
                //    };  
                //}
                //else
                //{
                //    response.ResponseDescription = "Escalation Created Successfully.";
                //    var generatedMerchant = await _escalation.AddAsync(merchantEntity);
                //    EscalationId = generatedMerchant.MatrixID;
                //    Escalation = generatedMerchant.Email ?? string.Empty;
                //    response.Data = generatedMerchant;
                //    _logger.LogInformation($"New Escalation added with ID: {EscalationId}.");
                //}





                response.isSuccess = true;
                response.ResponseCode = 1;
                response.Data = obj;
             
                //response.Data = generatedMerchant;

                //_logger.LogInformation($"Escalation {merchantEntity} added successfully.");


                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Escalation addition failed {ex.Message}.");
                response.isSuccess = false;
                response.ResponseCode = 0;
                response.ResponseDescription = ex.Message;
                response.Data = null;
                return response;
            }
        }

    }
}
