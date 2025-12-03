using AutoMapper;
using MediatR;
using Merchants.Application.Commands.Channel;
using Merchants.Application.Commands.Complaint;
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
using System.Transactions;

namespace Merchants.Application.Handlers.ComplaintRegister
{
    //internal class AddRegisterComplaintHandler
    //{
    //}

    //public class AddRegisterComplaintHandler : IRequestHandler<AddChannelCommand, Response>
    //{
    //    private readonly ILogger _logger;
    //    private readonly IHttpContextAccessor _httpContextAccessor;
    //    private readonly IMapper _mapper;
    //    private readonly IComplaint _ComplaintRepository;
    //    private readonly IComplainer _complainer;
    //    private readonly IComplaintDetails _complaintDetails;

    //    //private readonly Mail _mail;
    //    //private readonly IConfiguration _configuration;
    //    //private readonly string _baseUrl;
    //    //private readonly IRedisCacheService _redisCacheService;


    //    public AddRegisterComplaintHandler(IComplaint ComplaintRepository, IComplainer complainerRepository, IComplaintDetails ComplaintDetailsRepository,
    //        IMapper mapper,
    //        ILogger<AddRegisterComplaintHandler> logger, IHttpContextAccessor httpContextAccessor )
    //    {
    //        _ComplaintRepository = ComplaintRepository;
    //        _complainer = complainerRepository;
    //        _complaintDetails = ComplaintDetailsRepository;
    //        //_ChannelRepository = merchantRepository;
    //        _mapper = mapper;
    //        _logger = logger;
    //        _httpContextAccessor = httpContextAccessor;
    //        //_mail = mail;
    //        //_configuration = configuration;
    //        ////_baseUrl = _configuration["Urls:ActivationUrl"];
    //        //_redisCacheService = redisCacheService;
    //    }

  

        public class AddFullComplaintCommandHandler : IRequestHandler<AddFullComplaintCommand, Response>
        {
            private readonly IMediator _mediator;
            private readonly ILogger<AddFullComplaintCommandHandler> _logger;

            public AddFullComplaintCommandHandler(IMediator mediator, ILogger<AddFullComplaintCommandHandler> logger)
            {
                _mediator = mediator;
                _logger = logger;
            }

            public async Task<Response> Handle(AddFullComplaintCommand request, CancellationToken cancellationToken)
            {
                var response = new Response();
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        // Execute the Complaint Command
                        //var complaintResponse = await _mediator.Send(request.ComplaintCommand, cancellationToken);
                        //if (!complaintResponse.isSuccess) throw new Exception(complaintResponse.ResponseDescription);

                        //// Set the generated ComplaintId for Complainer and ComplaintDetail
                        //request.ComplainerCommand.ID = complaintResponse.Data.Id;
                        //request.ComplaintDetailCommand.ID = complaintResponse.Data.Id;

                        // Execute the Complainer Command
                        var complainerResponse = await _mediator.Send(request.AddComplainer, cancellationToken);
                        if (!complainerResponse.isSuccess) throw new Exception(complainerResponse.ResponseDescription);

                        // Execute the Complaint Detail Command
                        //var complaintDetailResponse = await _mediator.Send(request.ComplaintDetailCommand, cancellationToken);
                        //if (!complaintDetailResponse.isSuccess) throw new Exception(complaintDetailResponse.ResponseDescription);

                        transaction.Complete();

                        response.isSuccess = true;
                        response.ResponseCode = 1;
                        response.ResponseDescription = "Full complaint added successfully.";
                        //response.Data = complaintResponse.Data;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Adding full complaint failed: {ex.Message}");
                        response.isSuccess = false;
                        response.ResponseCode = 0;
                        response.ResponseDescription = ex.Message;
                    }
                }
                return response;
            }
        }


    
}
