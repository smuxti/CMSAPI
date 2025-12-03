
using MediatR;
using Merchants.Application.Queries;
using Merchants.Application.Responses;
using Merchants.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Merchants.Application.Handlers.Dashboard
{
    public class GetTopCategoriesCountQueryHandler : IRequestHandler<GetTopCategoriesQuery, Response>
    {
        private readonly IConfiguration _configuration;
        private readonly IComplaint _repository;
        private readonly IComplaintCategory _categoryRepo;
        private readonly IHttpClientFactory _httpClient;
        private readonly ILogger<GetTopCategoriesCountQueryHandler> _logger;

        public GetTopCategoriesCountQueryHandler(IConfiguration configuration, IComplaint repository, ILogger<GetTopCategoriesCountQueryHandler> logger, IHttpClientFactory httpClient, IComplaintCategory categoryRepo)
        {
            _repository = repository;
            _logger = logger;
            _httpClient = httpClient;
            _configuration = configuration;
            _categoryRepo = categoryRepo;
        }

        public async Task<Response> Handle(GetTopCategoriesQuery request, CancellationToken cancellationToken)
        {
            Response response = new();

            var complaints = await _repository.GetMonthlyComplaints(request.year, request.month);
            var categories = await _categoryRepo.GetAllAsync();

            var complaintCountsByCategory = complaints?
                .GroupBy(complaint => complaint?.CategoryID)
                .Select(group => new
                {
                    CategoryID = group?.Key,
                    CategoryCount = group?.Count()
                })
                .ToList();

            var processedData = complaintCountsByCategory.Select(pc =>
            {
                var category = categories.FirstOrDefault(m => m?.ID == pc.CategoryID);

                return new
                {
                    CategoryName = category?.Category ?? "Unknown",
                    CategoryCount = pc?.CategoryCount
                };
            }).ToList().AsReadOnly();

            response.isSuccess = true;
            response.ResponseDescription = "Data Acquired";
            response.ResponseCode = 1;
            response.Data = processedData;

            return response;
        }
    }
}

