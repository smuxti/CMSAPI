using AutoMapper;
using MediatR;
using Merchants.Application.Commands.Complaint;
using Merchants.Application.Responses;
using Merchants.Core.Interfaces;
using Merchants.Core.Entities;
using Microsoft.Extensions.Logging;
using EmailManager.MailService;
using System;
using System.Formats.Asn1;
using Quartz.Impl.Triggers;
using Microsoft.AspNetCore.Http;
using static EmailManager.MailService.Mail;
using Microsoft.Extensions.Configuration;
using Merchants.Infrastructure.Data;

namespace Merchants.Application.Handlers.Complaint
{
    public class AddFullComplaintCommandHandler : IRequestHandler<AddFullComplaintCommand, Response>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AddFullComplaintCommandHandler> _logger;
        private readonly Mail _mailService;
        private readonly IComplaint _complaintRepository;
        private readonly IComplainer _complainerRepository;
        private readonly IComplaintDetails _complaintDetailRepository;
        private readonly IMapper _mapper;
        private readonly IComplaintCategory _complaintCategory;
        private readonly IManagementHierarchy _managementHierarchy;
        private readonly IEscalation _escalation;
        private readonly IChannel _Channel;
        private readonly IMerchant _merchant;
        private readonly IComplaintType _ComplaintType;
        private readonly HttpClient _httpClient;
        private readonly IZones _merchantLocation;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly INotificationRepo _notification;
        private readonly IManagementHierarchy _hierarchy;

        public AddFullComplaintCommandHandler(
            IMediator mediator,
            IComplaint complaintRepository,
            IComplainer complainerRepository,
            IComplaintDetails complaintDetailRepository,
            IMapper mapper,
            IComplaintCategory complaintCategory,
            IManagementHierarchy managementRepository,
            IEscalation escalationRepository,
            IMerchant merchantRepository,
            IChannel Channel,
            IComplaintType complaintType,
            ILogger<AddFullComplaintCommandHandler> logger, Mail mailService, HttpClient httpClient, IZones merchantLocation,
            IConfiguration configuration, IHttpContextAccessor httpContextAccessor, INotificationRepo notification, IManagementHierarchy hierarchy)
        {
            _complaintRepository = complaintRepository;
            _complainerRepository = complainerRepository;
            _complaintDetailRepository = complaintDetailRepository;
            _mediator = mediator;
            _logger = logger;
            _mailService = mailService;
            _mapper = mapper;
            _complaintCategory = complaintCategory;
            _managementHierarchy = managementRepository;
            _escalation = escalationRepository;
            _merchant = merchantRepository;
            _Channel = Channel;
            _ComplaintType = complaintType;
            _httpClient = httpClient;
            _merchantLocation = merchantLocation;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _notification = notification;
            _hierarchy = hierarchy;
        }

        public async Task<Response> Handle(AddFullComplaintCommand request, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("UserID")?.Value);
            string link = _configuration.GetValue<string>("CmsPortal");
            int complainerid = 0;
            string otpMessage = "";
            string compMobile = "";
            string complainerEmail = string.Empty;
            string complainerName = string.Empty;
            string TicketNo = string.Empty;
            int compalinerId = 0;

            List<string>? ticketNumbers = new();

            try
            {
                Response baseResponse = new Response();

                var existingcomplainer = await _complainerRepository.GetComplainerByEmail(request.AddComplainer.Mobile);
                if (existingcomplainer == null)
                {
                    Core.Entities.Complainer complainer = new Core.Entities.Complainer();
                    complainer.Email = request.AddComplainer.Email;
                    complainer.Mobile = request.AddComplainer.Mobile;
                    complainer.Name = request.AddComplainer.Name;
                    complainer.CreatedBy = userId;
                    complainer.Status = "Active";
                    var r = await _complainerRepository.AddAsync(complainer);
                    complainerid = r.ID;
                    compMobile = complainer.Mobile;
                    complainerEmail = complainer.Email;
                    complainerName = complainer.Name;
                    compalinerId = r.ID;
                }
                else
                {
                    complainerid = existingcomplainer.ID;
                    compMobile = existingcomplainer.Mobile;
                    complainerName = request.AddComplainer.Name;
                    complainerEmail = existingcomplainer.Email;
                    compalinerId = existingcomplainer.ID;
                }
                foreach (var item in request.AddComplaint)
                {
                    Core.Entities.Complaint com = new Core.Entities.Complaint();
                    //com.TickentNo = await _complaintRepository.GetNextTicketNo();
                    com.ChannelID = item.ChannelID;
                    com.EquipmentID = item?.EquipmentID ?? null;
                    com.CategoryID = item.CategoryID;
                    com.TypeID = item.TypeID;
                    com.MerchantID = item.MerchantID;
                    com.MerchantName = item.MerchantName;
                    com.Description = item.Description;
                    com.Remarks = item.Remarks;
                    com.ComplainerID = complainerid;
                    com.ComplaintDate = DateTime.Now;
                    com.CreatedBy = userId;
                    com.TicketNo = await GenerateComplaintReference(item.ChannelID, complainerid);
                    com.Attachment = item.Attachment;
                    TicketNo = com.TicketNo;
                    ticketNumbers?.Add(TicketNo);

                    if (item.MerchantID > 0 && item.MerchantID != null)
                    {
                        var merExistis = await _merchant.GetMerchantByID(item.MerchantID.Value);
                        if (merExistis == null)
                        {
                            baseResponse.isSuccess = false;
                            baseResponse.ResponseDescription = "Invalid Merchant.";
                            baseResponse.ResponseCode = 0;
                            baseResponse.Data = null;
                            return baseResponse;
                        }
                        var hir = await _hierarchy.GetManagementHierarchyByParentId(merExistis.Zone);
                        var cat = await _complaintCategory.GetById(item.CategoryID);
                        EscalationView escalation = new EscalationView();
                        if (cat.Type == 4)
                        {
                            escalation = (await _escalation.GetEscalationByCategory(item.CategoryID, item.TypeID)).Where(x => x.Level == 1).FirstOrDefault();

                        }
                        else
                        {
                            escalation = (await _escalation.GetEscalationByCategory(item.CategoryID, item.TypeID)).Where(x => x.Level == 2).FirstOrDefault();

                        }
                        //var AreaDetail = await _merchantLocation.GetZoneByID(merExistis.Area);
                        if (escalation == null)
                        {
                            baseResponse.isSuccess = false;
                            baseResponse.ResponseDescription = "Escalation not found.";
                            baseResponse.ResponseCode = 0;
                            baseResponse.Data = null;
                            return baseResponse;
                        }
                        if (cat.Type == 4)
                        {
                            if (hir.Count() > 1)
                            {
                                var random = new Random();
                                var randomIndex = random.Next(hir.Count());
                                var randomId = hir.ElementAt(randomIndex).ID;
                                com.ManagementId = randomId;
                            }
                            else
                            {
                                com.ManagementId = hir.FirstOrDefault().ID;
                            }
                        }
                        else
                        {
                            com.ManagementId = escalation.ManagementID;
                        }

                        var AddedComplain = await _complaintRepository.AddAsync(com);
                        if (AddedComplain == null)
                        {
                            baseResponse.isSuccess = false;
                            baseResponse.ResponseDescription = "Complaint Addition Failed.";
                            baseResponse.ResponseCode = 0;
                            baseResponse.Data = null;
                            return baseResponse;
                        }

                        com.OTP = GenerateOTP();
                        otpMessage = $"Dear Customer, Your complaint {TicketNo} has been successfully registered. To close your complaint, please use the PIN code: {com.OTP}";

                        Core.Entities.ComplaintDetails cDetails = new Core.Entities.ComplaintDetails();
                        //cDetails.TickentNo = await _complaintRepository.GetNextTicketNo();
                        cDetails.CurrentStatus = "New";
                        cDetails.Status = "Active";
                        cDetails.ManagementId = com.ManagementId;
                        cDetails.Level = escalation.Level;
                        cDetails.ComplaintID = AddedComplain.ID;
                        cDetails.Description = AddedComplain.Description;
                        cDetails.EscalationId = escalation.ID;
                        cDetails.Remarks = com.Remarks;
                        cDetails.TickentNo = com.TicketNo;
                        cDetails.CreatedBy = userId;
                        if (escalation.ResponseType == "Days")
                        {
                            cDetails.EscalationTime = DateTime.Now.AddDays(escalation.ResponseTime);
                        }
                        if (escalation.ResponseType == "Hours")
                        {
                            cDetails.EscalationTime = DateTime.Now.AddHours(escalation.ResponseTime);
                        }
                        if (escalation.ResponseType == "Minutes")
                        {
                            cDetails.EscalationTime = DateTime.Now.AddMinutes(escalation.ResponseTime);
                        }
                        if (escalation.ResponseType == "Weeks")
                        {
                            cDetails.EscalationTime = DateTime.Now.AddDays(escalation.ResponseTime * 7);
                        }
                        var resp = await SendSMS(compMobile, otpMessage);

                        string complaintLink = $"{link}/Complaint/Details?Id={AddedComplain.ID}";

                        if (cat.Type == 4)
                        {
                            var cdetails1 = await _complaintDetailRepository.AddAsync(cDetails);

                            foreach (var to in hir)
                            {
                                Email newmail1 = new Email();
                                newmail1.to = to.POCEmail;
                                newmail1.cc = to.OtherEmail;
                                newmail1.subject = $"Complaint Registration!! Ticket : {TicketNo}";
                                //mobile number and complain direct url
                                newmail1.body = await GenerateComplaintBody(complainerName, cDetails.Description, complainerEmail, compMobile, complaintLink);
                                await _mailService.PublishEmailToQueueAsync(newmail1);
                            }
                        }
                        else
                        {
                            var rmemail = await _managementHierarchy.GetManagementHierarchyByID(merExistis.Zone);
                            var cdetails = await _complaintDetailRepository.AddAsync(cDetails);
                            Email newmail = new Email();
                            newmail.to = rmemail.POCEmail;
                            newmail.cc = rmemail.OtherEmail;
                            newmail.subject = $"Complaint Registration!! Ticket : {TicketNo}";
                            //mobile number and complain direct url
                            newmail.body = await GenerateComplaintBody(complainerName, cDetails.Description, complainerEmail, compMobile, complaintLink);
                            await _mailService.PublishEmailToQueueAsync(newmail);
                        }




                        Email mailComplainer = new Email();
                        mailComplainer.to = complainerEmail;
                        mailComplainer.subject = $"Complaint Registration!! Ticket {TicketNo}";
                        mailComplainer.body = await GenerateComplaintEmailBody(complainerName, cDetails.Description, com.OTP, TicketNo);
                        //await _mailService.PublishEmailToQueueAsync(newmail);
                        await _mailService.PublishEmailToQueueAsync(mailComplainer);
                        _logger.LogInformation($" Email Sent To Queue");



                    }
                    else
                    {
                        //var escalation12 = (await _escalation.GetEscalationByCategory(item.CategoryID, item.TypeID)).Where(x => x.Level >= 1).FirstOrDefault();
                        var escalations = await _escalation.GetEscalationByCategory(item.CategoryID, item.TypeID);
                        var escalation = escalations
                            .OrderBy(x => x.Level == 1 ? 0 : 1) // Prioritize Level == 1
                            .ThenBy(x => x.Level) // If no Level 1 exists, pick the next lowest level
                            .FirstOrDefault();

                        if (escalation == null)
                        {
                            baseResponse.isSuccess = false;
                            baseResponse.ResponseDescription = "Escalation not found.";
                            baseResponse.ResponseCode = 0;
                            baseResponse.Data = null;
                            return baseResponse;
                        }

                        com.ManagementId = escalation.ManagementID;
                        var AddedComplain = await _complaintRepository.AddAsync(com);
                        if (AddedComplain == null)
                        {
                            baseResponse.isSuccess = false;
                            baseResponse.ResponseDescription = "Complaint Addition Failed.";
                            baseResponse.ResponseCode = 0;
                            baseResponse.Data = null;
                            return baseResponse;
                        }

                        com.OTP = GenerateOTP();
                        otpMessage = $"Dear Customer, Your complaint {TicketNo} has been successfully registered. To close your complaint, please use the PIN code: {com.OTP}";

                        Core.Entities.ComplaintDetails cDetails = new Core.Entities.ComplaintDetails();
                        //cDetails.TickentNo = await _complaintRepository.GetNextTicketNo();
                        cDetails.CurrentStatus = "New";
                        cDetails.Status = "Active";
                        cDetails.ManagementId = escalation.ManagementID;
                        cDetails.Level = escalation.Level;
                        cDetails.ComplaintID = AddedComplain.ID;
                        cDetails.Description = AddedComplain.Description;
                        cDetails.EscalationId = escalation.ID;
                        cDetails.Remarks = com.Remarks;
                        cDetails.TickentNo = com.TicketNo;
                        //cDetails.CreatedBy = userId;



                        if (escalation.ResponseType == "Days")
                        {
                            cDetails.EscalationTime = DateTime.Now.AddDays(escalation.ResponseTime);
                        }
                        if (escalation.ResponseType == "Hours")
                        {
                            cDetails.EscalationTime = DateTime.Now.AddHours(escalation.ResponseTime);
                        }
                        if (escalation.ResponseType == "Minutes")
                        {
                            cDetails.EscalationTime = DateTime.Now.AddMinutes(escalation.ResponseTime);
                        }
                        if (escalation.ResponseType == "Weeks")
                        {
                            cDetails.EscalationTime = DateTime.Now.AddDays(escalation.ResponseTime * 7);
                        }
                        var resp = await SendSMS(compMobile, otpMessage);
                        string complaintLink = $"{link}/Complaint/Details?Id={AddedComplain.ID}";
                        var cdetails = await _complaintDetailRepository.AddAsync(cDetails);
                        _logger.LogInformation($"Escalation Email {escalation.Email}");
                        //await _notification.NotificationToManagement(com.ManagementId.ToString(), $"New Complain has been registerd Ticket : {TicketNo},{AddedComplain.ID}");
                        Email newmail = new Email();
                        newmail.to = escalation.Email;
                        newmail.subject = $"Complaint Registration!! Ticket {TicketNo}";
                        newmail.body = await GenerateComplaintBody(complainerName, cdetails.Description, complainerEmail, compMobile, complaintLink);
                        newmail.isHtml = true;

                        Email mailComplainer = new Email();
                        mailComplainer.to = complainerEmail;
                        mailComplainer.subject = $"Complaint Registration!! Ticket {TicketNo}";
                        mailComplainer.isHtml = true;

                        mailComplainer.body = await GenerateComplaintEmailBody(complainerName, cdetails.Description, com.OTP, TicketNo);


                        await _mailService.PublishEmailToQueueAsync(newmail);
                        await _mailService.PublishEmailToQueueAsync(mailComplainer);
                        _logger.LogInformation($" Email Sent To Queue");
                    }
                }

                baseResponse.isSuccess = true;
                baseResponse.ResponseDescription = "Complaint Added Successfully";
                baseResponse.ResponseCode = 1;
                baseResponse.Data = ticketNumbers;
                return baseResponse;

            }
            catch (Exception ex)
            {
                Response baseResponse = new Response();
                baseResponse.isSuccess = false;
                baseResponse.ResponseDescription = ex.Message;
                baseResponse.ResponseCode = 0;
                baseResponse.Data = ticketNumbers;
                return baseResponse;
                throw;
            }
        }
        private async Task<string> GenerateComplaintReference(int channelId, int complainerId)
        {
            var chan = await _Channel.GetCHANELByID(channelId);
            string channelPrefix = string.Empty;
            Random random = new Random();
            int randomNumber = random.Next(1000, 9999);
            channelPrefix = chan.ChannelType.ToString().Substring(0, 2);

            if (chan.ChannelType == "Email")
            {
                channelPrefix = "EM";
            }
            if (chan.ChannelType == "UAN")
            {
                channelPrefix = "OP";
            }
            if (chan.ChannelType == "WhatsApp")
            {
                channelPrefix = "WA";
            }

            string year = DateTime.Now.Year.ToString();
            string formattedComplainerId = complainerId.ToString("D4");


            return $"{year}{channelPrefix}{formattedComplainerId}-{randomNumber}";
        }
        public string GenerateOTP()
        {
            Random random = new Random();
            int otp = random.Next(1000, 10000);
            return otp.ToString();
        }
        public async Task<string> SendSMS(string Receiver, string Msg)
        {
            var ur = _configuration.GetValue<string>("SMS:url");
            string smsURL = ur + Receiver + "&Message=" + Msg;
            var response = await _httpClient.GetStringAsync(smsURL);
            return response;

        }

        public async Task<string> GenerateComplaintEmailBody(string complainerName, string description, string otp, string TicketNo)
        {
            _logger.LogInformation("inside Generate Complaint To Complainer");
            // Construct the correct path for the email template
            //string templatePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "./Views/SendEmailToComplainerTemplate.cshtml"));
            string templatePath = Path.Combine(AppContext.BaseDirectory, "wwwroot", "SendEmailToComplainerTemplate.cshtml");

            if (!System.IO.File.Exists(templatePath))
            {
                throw new FileNotFoundException("Email template not found", templatePath);
            }
            _logger.LogInformation("Template found");
            // Read the template file
            string emailBody = await System.IO.File.ReadAllTextAsync(templatePath);

            // Replace placeholders with actual values
            emailBody = emailBody.Replace("{complainerName}", complainerName)
                                 .Replace("{Description}", description).Replace("{otp}", otp).Replace("{ticketNo}", TicketNo);

            return emailBody;
        }
        public async Task<string> GenerateComplaintBody(string complainerName, string description, string email, string mobile, string link)
        {
            _logger.LogInformation($"inside Generate Complaint To Management {email}: Link : {link}");

            // Construct the correct path for the email template
            //string templatePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "./Views/SendEmailTemplate.cshtml"));
            string templatePath = Path.Combine(AppContext.BaseDirectory, "wwwroot", "SendEmailTemplate.cshtml");

            if (!System.IO.File.Exists(templatePath))
            {
                throw new FileNotFoundException("Email template not found", templatePath);
            }
            _logger.LogInformation("Template found");

            // Read the template file
            string emailBody = await System.IO.File.ReadAllTextAsync(templatePath);

            // Replace placeholders with actual values
            emailBody = emailBody.Replace("{complainerName}", complainerName)
                                 .Replace("{Description}", description).Replace("{complainerEmail}", email).Replace("{compMobile}", mobile).Replace("{complaintLink}", link);

            return emailBody;
        }

    }





}





