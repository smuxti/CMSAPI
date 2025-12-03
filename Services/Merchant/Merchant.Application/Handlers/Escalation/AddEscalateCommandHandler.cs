using AutoMapper;
using MediatR;
using Merchants.Application.Commands.ComplaintType;
using Merchants.Application.Commands.Escalation;
using Merchants.Application.Responses;
using Merchants.Core.Entities;
using Merchants.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using EmailManager.MailService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EmailManager.MailService.Mail;
using Microsoft.Extensions.Configuration;

namespace Merchants.Application.Handlers.Escalation
{
    public class AddEscalateCommandHandler : IRequestHandler<AddEscalateCommand, Response>
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IEscalation _escalation;
        private readonly IComplaintCategory _compCateg;
        private readonly IManagementHierarchy _mngHr;
        private readonly IComplaintDetails _compDtl;
        private readonly Mail _mailService;
        private readonly HttpClient _httpClient;
        private readonly IComplaint _complaint;
        private readonly IMerchant _merchant;
        private readonly IComplainer _complainer;
        private readonly IConfiguration _configuration;
        private readonly INotificationRepo _notificationRepo;

        public AddEscalateCommandHandler(IComplaintDetails compDtl, IEscalation escalation, IComplaintCategory compCateg, IManagementHierarchy mngHr, IMapper mapper,
            ILogger<AddEscalationCommandHandler> logger, IHttpContextAccessor httpContextAccessor, Mail mailService, HttpClient httpClient, IComplaint complaint, IMerchant merchant, IComplainer complainer, IConfiguration configuration,INotificationRepo notificationRepo)
        {
            _compDtl = compDtl;
            _escalation = escalation;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _compCateg = compCateg;
            _mngHr = mngHr;
            _mailService = mailService;
            _mailService = mailService;
            _httpClient = httpClient;
            _complaint = complaint;
            _merchant = merchant;
            _complainer = complainer;
            _configuration = configuration;
            _notificationRepo = notificationRepo;

            //_mail = mail;
            //_configuration = configuration;
            ////_baseUrl = _configuration["Urls:ActivationUrl"];
            //_redisCacheService = redisCacheService;
        }

        public async Task<Response> Handle(AddEscalateCommand request, CancellationToken cancellationToken)
        {
            string link = _configuration.GetValue<string>("CmsPortal");
            Response response = new Response();
            try
            {
                var result = await _compDtl.GetAllAsync(x => (x.CurrentStatus.Equals("New")) && DateTime.Now > x.EscalationTime);

                var complaintDetails = result
                            .GroupBy(x => x.ComplaintID)
                            .Select(g => g.OrderByDescending(x => x.CreatedAt).FirstOrDefault())
                            .ToList();


                foreach (var row in complaintDetails)
                {
                    var complaintlink = $"{link}/Complaint/Details?Id={row.ComplaintID}";
                    var com = await _complaint.GetById(row.ComplaintID);
                    var cat = await _compCateg.GetComplaintCategoryByID(com.CategoryID);
                    if (com.Status == "InActive")
                    {
                        continue;
                    }
                    var complainer = await _complainer.GetCmplainerByID(com.ComplainerID);
                    if (com.MerchantID > 0 && cat.Type != 4)
                    {
                        var merc = await _merchant.GetById(com.MerchantID.Value);
                        //var z = merc.Zone;
                        _logger.LogInformation($"complaint = {row.ComplaintID} :: ComplaintDetail = {row.ID} :: Merch = {merc.ID}");
                        if (merc.Area != 0 && row.Level == 1)
                        {

                            _logger.LogInformation($"Getting Details for lvl2 = {row.Level}");

                            var mg = (await _mngHr.GetAllAsync(x => x.ManagementType == -2)).FirstOrDefault().ID;
                            var es = (await _escalation.GetAllAsync(x => x.ManagementID == mg && x.CategoryID == com.CategoryID && x.Type == com.TypeID)).FirstOrDefault();
                            
                            if (es is null)
                            {
                                es = (await _escalation.GetAllAsync(x => x.CategoryID == com.CategoryID && x.Level == 2 && x.Type == com.TypeID)).FirstOrDefault();
                            }

                            var hir = await _mngHr.GetManagementHierarchyByID(merc.Area);
                            _logger.LogInformation($"Email for this level = {hir.POCEmail}");

                            Core.Entities.ComplaintDetails cDetails = new Core.Entities.ComplaintDetails();
                            cDetails.CurrentStatus = "New";
                            cDetails.Status = "Active";
                            cDetails.ManagementId = es?.ManagementID ?? hir.ID;
                            cDetails.Level = es?.Level ?? 2;
                            cDetails.ComplaintID = row.ComplaintID;
                            cDetails.Description = row.Description;
                            cDetails.EscalationId = es.ID;
                            cDetails.TickentNo = row.TickentNo;
                            cDetails.Remarks = row.Remarks;

                            if (es.ResponseType == "Days")
                            {
                                cDetails.EscalationTime = DateTime.Now.AddDays(es.ResponseTime);
                            }
                            if (es.ResponseType == "Hours")
                            {
                                cDetails.EscalationTime = DateTime.Now.AddHours(es.ResponseTime);
                            }
                            if (es.ResponseType == "Minutes")
                            {
                                cDetails.EscalationTime = DateTime.Now.AddMinutes(es.ResponseTime);
                            }
                            if (es.ResponseType == "Weeks")
                            {
                                cDetails.EscalationTime = DateTime.Now.AddDays(es.ResponseTime * 7);
                            }
                            row.CurrentStatus = "Escalate";
                            await _compDtl.UpdateAsync(row);
                            await _compDtl.AddAsync(cDetails);

                            var masterComplaint = await _complaint.GetById(row.ComplaintID);
                            masterComplaint.ManagementId = hir.ID;

                            await _complaint.UpdateAsync(masterComplaint);

                            Email newmail = new Email();
                            newmail.to = hir.POCEmail;
                            newmail.cc = hir.OtherEmail;
                            newmail.subject = $"Complaint Registration!! Ticket : {com.TicketNo}";
                            newmail.body = await GenerateComplaintBody(complainer.Name, cDetails.Description, complainer.Email, complainer.Mobile, complaintlink);
                            newmail.isHtml = true;
                            //$@"
                            //            <html>
                            //                <body>
                            //                    <h2>Complaint Registration</h2>
                            //                    <p>A complaint has been registered with the following details:</p>
                            //                    <p><strong>Complainer :</strong> {complainer.Name}</p>
                            //                    <p><strong>Complainer Email:</strong> {complainer.Email}</p>
                            //                    <p><strong>Complaint ID:</strong> {row.ComplaintID}</p>
                            //                    <p><strong>Description:</strong> {cDetails.Description}</p>
                            //                    <p>Please take the necessary action.</p>
                            //                    <br>
                            //                    <p>Best Regards,</p>
                            //                    <p>Your Support Team</p>
                            //                </body>
                            //            </html>";

                            await _mailService.PublishEmailToQueueAsync(newmail);
                            _logger.LogInformation($" Email Sent To Queue");

                            //await _notificationRepo.NotificationToManagement(hir.ID.ToString(), $"New Complain Has Been Registered Ticket :{cDetails.TickentNo},{com.ID}");


                        }

                        else if (merc.Zone > 0)
                        {
                            if (row.Level < 2)
                            {
                                _logger.LogInformation($"Getting Details for lvl3 = {row.Level}");

                                var mg = (await _mngHr.GetAllAsync(x => x.ManagementType == -3));
                                var i = mg.FirstOrDefault();
                                var es = (await _escalation.GetAllAsync(x => x.ManagementID == i.ID && x.CategoryID == com.CategoryID && x.Type == com.TypeID)).FirstOrDefault();
                                //var es = (await _escalation.GetAllAsync(x => x.Email == "Merchant" && x.Level == 3)).FirstOrDefault();
                                if (es is null)
                                {
                                    es = (await _escalation.GetAllAsync(x => x.CategoryID == com.CategoryID && x.Type == com.TypeID && x.Level > row.Level)).FirstOrDefault();
                                }

                                if (es is null) { continue; }
                                _logger.LogInformation($"Escalation ID:{es?.ID}");
                                _logger.LogInformation($"Merch Zone = {merc.Zone}");
                                var hir = await _mngHr.GetManagementHierarchyByID(merc.Zone);
                                _logger.LogInformation($"Email for this level = {hir.POCEmail}");

                                Core.Entities.ComplaintDetails cDetails = new Core.Entities.ComplaintDetails();
                                cDetails.CurrentStatus = "New";
                                cDetails.Status = "Active";
                                cDetails.ManagementId = es?.ManagementID ?? hir.ID;
                                cDetails.Level = es?.Level ?? 2;
                                cDetails.ComplaintID = row.ComplaintID;
                                cDetails.Description = row.Description;
                                cDetails.EscalationId = es.ID;
                                cDetails.Remarks = row.Remarks;
                                cDetails.TickentNo = row.TickentNo;

                                if (es.ResponseType == "Days")
                                {
                                    cDetails.EscalationTime = DateTime.Now.AddDays(es.ResponseTime);
                                }
                                if (es.ResponseType == "Hours")
                                {
                                    cDetails.EscalationTime = DateTime.Now.AddHours(es.ResponseTime);
                                }
                                if (es.ResponseType == "Minutes")
                                {
                                    cDetails.EscalationTime = DateTime.Now.AddMinutes(es.ResponseTime);
                                }
                                if (es.ResponseType == "Weeks")
                                {
                                    cDetails.EscalationTime = DateTime.Now.AddDays(es.ResponseTime * 7);
                                }
                                row.CurrentStatus = "Escalate";
                                await _compDtl.UpdateAsync(row);
                                await _compDtl.AddAsync(cDetails);

                                var masterComplaint = await _complaint.GetById(row.ComplaintID);
                                masterComplaint.ManagementId = hir.ID;

                                await _complaint.UpdateAsync(masterComplaint);

                                Email newmail = new Email();
                                newmail.to = hir.POCEmail;
                                newmail.cc = hir.OtherEmail;
                                newmail.subject = $"Complaint Registration!! Ticket : {com.TicketNo}";
                                newmail.body = await GenerateComplaintBody(complainer.Name, cDetails.Description, complainer.Email, complainer.Mobile, complaintlink);
                                newmail.isHtml = true;
                                await _mailService.PublishEmailToQueueAsync(newmail);
                                _logger.LogInformation($" Email Sent To Queue");
                                //await _notificationRepo.NotificationToManagement(hir.ID.ToString(), $"New Complain Has Been Registered Ticket :{cDetails.TickentNo},{com.ID}");

                            }
                            else
                            {
                                _logger.LogInformation("inside zone else escalate block");

                                var escalation = await _escalation.GetById(row.EscalationId);
                                var compMobile = string.Empty;
                                var ticket = string.Empty;
                                if (escalation != null)
                                {
                                    var escalationNext = (await _escalation.GetAllAsync(x => x.CategoryID == com.CategoryID && x.Type == com.TypeID && x.Level > escalation.Level)).FirstOrDefault();

                                    if (escalationNext != null)
                                    {

                                        Core.Entities.ComplaintDetails cDetails = new Core.Entities.ComplaintDetails();
                                        cDetails.CurrentStatus = "New";
                                        cDetails.Status = "Active";
                                        cDetails.ManagementId = escalationNext.ManagementID;
                                        cDetails.Level = escalationNext.Level;
                                        cDetails.ComplaintID = row.ComplaintID;
                                        cDetails.Description = row.Description;
                                        cDetails.EscalationId = escalationNext.ID;
                                        cDetails.TickentNo = row.TickentNo;
                                        compMobile = escalationNext.ContactNumber;
                                        ticket = row.TickentNo;

                                        if (escalationNext.ResponseType == "Days")
                                        {
                                            cDetails.EscalationTime = DateTime.Now.AddDays(escalationNext.ResponseTime);
                                        }
                                        if (escalationNext.ResponseType == "Hours")
                                        {
                                            cDetails.EscalationTime = DateTime.Now.AddHours(escalationNext.ResponseTime);
                                        }
                                        if (escalationNext.ResponseType == "Minutes")
                                        {
                                            cDetails.EscalationTime = DateTime.Now.AddMinutes(escalationNext.ResponseTime);
                                        }
                                        if (escalationNext.ResponseType == "Weeks")
                                        {
                                            cDetails.EscalationTime = DateTime.Now.AddDays(escalationNext.ResponseTime * 7);
                                        }
                                        string message = "A complain has been asssign to you Ticket# " + ticket;

                                        await _compDtl.AddAsync(cDetails);
                                        //var olddtl = await _compDtl.GetById(row.ID);
                                        row.CurrentStatus = "Escalate";
                                        await _compDtl.UpdateAsync(row);

                                        var masterComplaint = await _complaint.GetById(row.ComplaintID);
                                        masterComplaint.ManagementId = escalationNext.ManagementID;

                                        await _complaint.UpdateAsync(masterComplaint);

                                        Email newmail = new Email();
                                        newmail.to = escalationNext.Email;
                                        newmail.subject = $"Complaint Registration!! Ticket : {com.TicketNo}";
                                        newmail.body = await GenerateComplaintBody(complainer.Name, cDetails.Description, complainer.Email, complainer.Mobile, complaintlink);
                                        newmail.isHtml = true;
                                        await _mailService.PublishEmailToQueueAsync(newmail);
                                        _logger.LogInformation($" Email Sent To Queue");
                                        //var result = await _mailService.SendEmailAsync(escalationNext.Email, "Compalint Registerd", $"A Complained with Id {row.ComplaintID} Is Registered To You");
                                        //await _notificationRepo.NotificationToManagement(escalationNext.ManagementID.ToString(), $"New Complain Has Been Registered Ticket : {cDetails.TickentNo} , {com.ID}");

                                    }
                                    //else
                                    //{
                                    //    row.CurrentStatus = "Escalate";
                                    //    await _compDtl.UpdateAsync(row);
                                    //}
                                }

                            }
                        }
                        else
                        {
                            _logger.LogInformation("inside when zone is null else escalate block");

                            var escalation = await _escalation.GetById(row.EscalationId);
                            var compMobile = string.Empty;
                            var ticket = string.Empty;
                            if (escalation != null)
                            {
                                var escalationNext = (await _escalation.GetAllAsync(x => x.CategoryID == com.CategoryID && x.Type == com.TypeID && x.Level > escalation.Level && x.Level != 2 && x.Level != 3)).FirstOrDefault();

                                if (escalationNext != null)
                                {

                                    Core.Entities.ComplaintDetails cDetails = new Core.Entities.ComplaintDetails();
                                    cDetails.CurrentStatus = "New";
                                    cDetails.Status = "Active";
                                    cDetails.ManagementId = escalationNext.ManagementID;
                                    cDetails.Level = escalationNext.Level;
                                    cDetails.ComplaintID = row.ComplaintID;
                                    cDetails.Description = row.Description;
                                    cDetails.EscalationId = escalationNext.ID;
                                    cDetails.Remarks = row.Remarks;
                                    cDetails.TickentNo = row.TickentNo;
                                    compMobile = escalationNext.ContactNumber;
                                    ticket = row.TickentNo;

                                    if (escalationNext.ResponseType == "Days")
                                    {
                                        cDetails.EscalationTime = DateTime.Now.AddDays(escalationNext.ResponseTime);
                                    }
                                    if (escalationNext.ResponseType == "Hours")
                                    {
                                        cDetails.EscalationTime = DateTime.Now.AddHours(escalationNext.ResponseTime);
                                    }
                                    if (escalationNext.ResponseType == "Minutes")
                                    {
                                        cDetails.EscalationTime = DateTime.Now.AddMinutes(escalationNext.ResponseTime);
                                    }
                                    if (escalationNext.ResponseType == "Weeks")
                                    {
                                        cDetails.EscalationTime = DateTime.Now.AddDays(escalationNext.ResponseTime * 7);
                                    }
                                    string message = "A complain has been asssign to you Ticket# " + ticket;

                                    await _compDtl.AddAsync(cDetails);
                                    //var olddtl = await _compDtl.GetById(row.ID);
                                    row.CurrentStatus = "Escalate";
                                    await _compDtl.UpdateAsync(row);

                                    var masterComplaint = await _complaint.GetById(row.ComplaintID);
                                    masterComplaint.ManagementId = escalationNext.ManagementID;

                                    await _complaint.UpdateAsync(masterComplaint);

                                    Email newmail = new Email();
                                    newmail.to = escalationNext.Email;
                                    newmail.subject = $"Complaint Registration!! Ticket : {com.TicketNo}";
                                    newmail.body = await GenerateComplaintBody(complainer.Name, cDetails.Description, complainer.Email, complainer.Mobile, complaintlink);
                                    newmail.isHtml = true;
                                    await _mailService.PublishEmailToQueueAsync(newmail);
                                    _logger.LogInformation($" Email Sent To Queue");
                                    //var result = await _mailService.SendEmailAsync(escalationNext.Email, "Compalint Registerd", $"A Complained with Id {row.ComplaintID} Is Registered To You");
                                    //await _notificationRepo.NotificationToManagement(escalationNext.ManagementID.ToString(), $"New Complain Has Been Registered Ticket : {cDetails.TickentNo} , {com.ID}");

                                }
                                //else
                                //{
                                //    row.CurrentStatus = "Escalate";
                                //    await _compDtl.UpdateAsync(row);
                                //}
                            }
                        }



                    }
                    else
                    {
                        var escID = row.EscalationId;
                        var compMobile = string.Empty;
                        var esc = await _escalation.GetById(escID);
                        string ticket = string.Empty;
                        if (esc != null)
                        {
                            var escalation = await _escalation.GetAllAsync(x => x.CategoryID == esc.CategoryID && x.Type == esc.Type && x.Level > esc.Level);
                            var escalationNext = escalation.FirstOrDefault();
                            if (escalationNext != null)
                            {

                                Core.Entities.ComplaintDetails cDetails = new Core.Entities.ComplaintDetails();
                                cDetails.CurrentStatus = "New";
                                cDetails.Status = "Active";
                                cDetails.ManagementId = escalationNext.ManagementID;
                                cDetails.Level = escalationNext.Level;
                                cDetails.ComplaintID = row.ComplaintID;
                                cDetails.Description = row.Description;
                                cDetails.EscalationId = escalationNext.ID;
                                cDetails.Remarks = row.Remarks;
                                cDetails.TickentNo = row.TickentNo;
                                compMobile = escalationNext.ContactNumber;
                                ticket = row.TickentNo;

                                if (escalationNext.ResponseType == "Days")
                                {
                                    cDetails.EscalationTime = DateTime.Now.AddDays(escalationNext.ResponseTime);
                                }
                                if (escalationNext.ResponseType == "Hours")
                                {
                                    cDetails.EscalationTime = DateTime.Now.AddHours(escalationNext.ResponseTime);
                                }
                                if (escalationNext.ResponseType == "Minutes")
                                {
                                    cDetails.EscalationTime = DateTime.Now.AddMinutes(escalationNext.ResponseTime);
                                }
                                if (escalationNext.ResponseType == "Weeks")
                                {
                                    cDetails.EscalationTime = DateTime.Now.AddDays(escalationNext.ResponseTime * 7);
                                }
                                string message = "A complain has been asssign to you Ticket# " + ticket;

                                await _compDtl.AddAsync(cDetails);
                                var olddtl = await _compDtl.GetById(row.ID);
                                olddtl.CurrentStatus = "Escalate";
                                await _compDtl.UpdateAsync(olddtl);

                                var masterComplaint = await _complaint.GetById(row.ComplaintID);
                                masterComplaint.ManagementId = escalationNext.ManagementID;

                                await _complaint.UpdateAsync(masterComplaint);

                                Email newmail = new Email();
                                newmail.to = escalationNext.Email;
                                newmail.subject = $"Complaint Registration!! Ticket : {com.TicketNo}";
                                newmail.body = await GenerateComplaintBody(complainer.Name, cDetails.Description, complainer.Email, complainer.Mobile, complaintlink);
                                newmail.isHtml = true;

                                await _mailService.PublishEmailToQueueAsync(newmail);
                                _logger.LogInformation($" Email Sent To Queue");
                                //var result = await _mailService.SendEmailAsync(escalationNext.Email, "Compalint Registerd", $"A Complained with Id {row.ComplaintID} Is Registered To You");
                                //await _notificationRepo.NotificationToManagement(escalationNext.ManagementID.ToString(), $"New Complain Has Been Registered Ticket : {cDetails.TickentNo} , {com.ID}");

                            }

                            //row.CurrentStatus = "Escalate";
                            //await _compDtl.UpdateAsync(row);
                        }
                    }
                }
                response.isSuccess = true;
                response.ResponseCode = 1;
                response.Data = null;
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
        public async Task<string> SendSMS(string Receiver, string Msg)
        {
            string smsURL = "http://aplapi.makglobalps.com/BESendSMS.php?recipient=" + Receiver + "&Message=" + Msg;
            var response = await _httpClient.GetStringAsync(smsURL);
            return response;

        }

        public async Task<string> GenerateComplaintBody(string complainerName, string description, string email, string mobile, string link)
        {
            // Construct the correct path for the email template
            //string templatePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "./Views/SendEmailTemplate.cshtml"));
            string templatePath = Path.Combine(AppContext.BaseDirectory, "wwwroot", "SendEmailTemplate.cshtml");

            if (!System.IO.File.Exists(templatePath))
            {
                throw new FileNotFoundException("Email template not found", templatePath);
            }

            // Read the template file
            string emailBody = await System.IO.File.ReadAllTextAsync(templatePath);

            // Replace placeholders with actual values
            emailBody = emailBody.Replace("{complainerName}", complainerName)
                                 .Replace("{Description}", description).Replace("{complainerEmail}", email).Replace("{compMobile}", mobile).Replace("{complaintLink}", link);

            return emailBody;
        }

    }
}
