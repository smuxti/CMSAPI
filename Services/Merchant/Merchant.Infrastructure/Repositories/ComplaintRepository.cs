using AutoMapper;
using Merchants.Core.Entities;
using Merchants.Core.Interfaces;
using Merchants.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using Merchants.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StackExchange.Redis.Role;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Runtime.CompilerServices;
using System.Reflection.Metadata;
using System.Globalization;

namespace Merchants.Infrastructure.Repositories
{
    //public class ComplaintRepository
    //{
    //}


    public class ComplaintRepository : AsyncRepository<Complaint>, IComplaint
    {
        private readonly MerchantContext _merchantContext;
        private readonly ILogger<ComplaintRepository> _logger;


        public ComplaintRepository(MerchantContext merchantContext, ILogger<ComplaintRepository> logger) : base(merchantContext, logger)
        {
            _merchantContext = merchantContext;
            _logger = logger;
        }

        public Task<Complaint> CreateComplaintAsync(Complaint complaintRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<ComplaintHistoryView>> GetComlaintHistory(int ID)
        {
            var complaints = (from e in _merchantContext.Complaints
                              join d in _merchantContext.ComplaintDetails on e.ID equals d.ComplaintID
                              join c in _merchantContext.ComplaintCategories
                              on e.CategoryID equals c.ID
                              join t in _merchantContext.ComplaintTypes on e.TypeID equals t.ID
                              join ch in _merchantContext.Channels on e.ChannelID equals ch.ID
                              join com in _merchantContext.Complainers on e.ComplainerID equals com.ID
                              where e.isDeleted == false && e.ID == ID
                              select new ComplaintHistoryView
                              {


                                  ID = e.ID,
                                  EquipmentID = e.EquipmentID,
                                  ComplaintDate = e.ComplaintDate,
                                  TicketNo = d.TickentNo,
                                  Description = e.Description,
                                  CategoryID = e.CategoryID,
                                  Category = c.Category,
                                  Type = e.TypeID,
                                  ComplaintTypes = t.ComplaintTypes,
                                  ChannelID = e.ChannelID,
                                  Channel = ch.ChannelType,
                                  ComplainerID = e.ComplainerID,
                                  ComplainerName = com.Name,
                                  ComplainerEmail = com.Email,
                                  ComplainerCellNo = com.Mobile,
                                  CurrentStatus = d.CurrentStatus,
                                  ComplDtlDescription = d.Description,
                                  ComplDtlRemarks = d.Remarks,
                                  ManagementID = d.ManagementId,
                                  Level = d.Level,
                                  CreatedAt = d.CreatedAt,

                                  //Added Later
                                  CompDescription = e.Description,
                                  CompRemarks = e.Remarks,
                                  CreatedBy = d.CreatedBy ?? Guid.Empty


                              }).ToList();


            return complaints; 
        }

        public async Task<IList<ComplaintHistoryView>> GetComlaintMerchantHistory(int ID)
        {

            var complaints = (from e in _merchantContext.Complaints
                              join d in _merchantContext.ComplaintDetails on e.ID equals d.ComplaintID
                              //join c in _merchantContext.ComplaintCategories
                              //on e.CategoryID equals c.ID
                              join t in _merchantContext.ComplaintTypes on e.TypeID equals t.ID
                              join ch in _merchantContext.Channels on e.ChannelID equals ch.ID
                              join com in _merchantContext.Complainers on e.ComplainerID equals com.ID
                              join mgm in _merchantContext.Merchants on e.MerchantID equals mgm.ID
                              //join z in _merchantContext.ManagementHierarchies on mgm.Zone equals z.ID into zone
                              //from z in zone.DefaultIfEmpty()
                              //join a in _merchantContext.ManagementHierarchies on mgm.Area equals a.ID into area
                              //from a in area.DefaultIfEmpty()
                              where e.isDeleted == false && e.ID == ID
                              select new ComplaintHistoryView
                              {


                                  ID = e.ID,
                                  ComplaintDate = e.ComplaintDate,
                                  EquipmentID = e.EquipmentID,
                                  TicketNo = d.TickentNo,
                                  Description = e.Description,
                                  CategoryID = e.CategoryID,
                                  Type = e.TypeID,
                                  ComplaintTypes = t.ComplaintTypes,
                                  ChannelID = e.ChannelID,
                                  Channel = ch.ChannelType,
                                  ComplainerID = e.ComplainerID,
                                  ComplainerName = com.Name,
                                  ComplainerEmail = com.Email,
                                  ComplainerCellNo = com.Mobile,
                                  CurrentStatus = d.CurrentStatus,
                                  ComplDtlDescription = d.Description,
                                  ComplDtlRemarks = d.Remarks,
                                  ManagementID = d.ManagementId,
                                  Level = d.Level,
                                  POCName = mgm.MerchantName,
                                  CreatedAt = d.CreatedAt,

                                  //Added Later
                                  CompDescription = e.Description,
                                  CompRemarks = e.Remarks,
                                  CreatedBy = d.CreatedBy ?? Guid.Empty,

                              }).ToList();


            return complaints; ;
        }


        public Task<Complaint> GetComplaintByIdAsync(int ID)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<ComplainView>> GetComplaintsAsync()
        {
            var complaints = (from e in _merchantContext.Complaints
                              join d in _merchantContext.ComplaintDetails on e.ID equals d.ComplaintID
                              join c in _merchantContext.ComplaintCategories
                              on e.CategoryID equals c.ID
                              join m in _merchantContext.Merchants on e.MerchantID equals m.ID into mj
                              from m in mj.DefaultIfEmpty()
                              join t in _merchantContext.ComplaintTypes on e.TypeID equals t.ID
                              join ch in _merchantContext.Channels on e.ChannelID equals ch.ID
                              join com in _merchantContext.Complainers on e.ComplainerID equals com.ID
                              where e.isDeleted == false
                              select new ComplainView
                              {


                                  ID = e.ID,
                                  EquipmentID = e.EquipmentID,
                                  ComplaintDate = e.ComplaintDate,
                                  TicketNo = e.TicketNo,
                                  ComplainerID = e.ComplainerID,
                                  Name = com.Name,
                                  ComplainerCell=com.Mobile,
                                  Remarks = e.Remarks,
                                  Description = e.Description,
                                  TypeID = e.TypeID,
                                  ComplaintTypes = t.ComplaintTypes,
                                  CategoryID = e.CategoryID,
                                  Category = c.Category,
                                  CategoryAltname = c.AltName,
                                  MerchantID = e.MerchantID.Value,
                                  ChannelID = e.ChannelID,
                                  Mangementid = d.ManagementId,
                                  ChannlerType = ch.ChannelType,
                                  ResolvedDate = e.ResolvedDate,
                                  SatisfactionScore = e.SatisfactionScore,
                                  CreatedAt=Convert.ToDateTime(d.CreatedAt),
                                  OTP = e.OTP,
                                  MerchantName = m != null ? m.MerchantName ?? "N/A" : "N/A",
                                  MerchantCity = m != null ? m.City ?? "N/A" : "N/A"


                              }).ToList();


            return complaints;
        }



        public async Task<IList<ComplainView>> GetComplaintByManagmentIdAsync(int ManagmentId)
        {
            var complaints = (from e in _merchantContext.Complaints
                              join d in _merchantContext.ComplaintDetails on e.ID equals d.ComplaintID
                              join c in _merchantContext.ComplaintCategories
                              on e.CategoryID equals c.ID
                              join m in _merchantContext.Merchants on e.MerchantID equals m.ID
                              join t in _merchantContext.ComplaintTypes on e.TypeID equals t.ID
                              join ch in _merchantContext.Channels on e.ChannelID equals ch.ID
                              join com in _merchantContext.Complainers on e.ComplainerID equals com.ID
                              where e.isDeleted == false && e.ManagementId == ManagmentId
                              select new ComplainView
                              {


                                  //ID = e.ID,
                                  //ComplaintDate = e.ComplaintDate,
                                  //TicketNo = e.TicketNo,
                                  //ComplainerCell=com.Mobile,
                                  //ComplainerID = e.ComplainerID,
                                  //Name = com.Name,
                                  //Remarks = e.Remarks,
                                  //Description = e.Description,
                                  //TypeID = e.TypeID,
                                  //ComplaintTypes = t.ComplaintTypes,
                                  //CategoryID = e.CategoryID,
                                  //Category = c.Category,
                                  //MerchantID = e.MerchantID.Value,
                                  //MerchantName = e.MerchantName,
                                  //ChannelID = e.ChannelID,
                                  //ChannlerType = ch.ChannelType,
                                  //ResolvedDate = e.ResolvedDate,
                                  //SatisfactionScore = e.SatisfactionScore,
                                  //OTP = e.OTP

                                  ID = e.ID,
                                  EquipmentID = e.EquipmentID,
                                  ComplaintDate = e.ComplaintDate,
                                  TicketNo = e.TicketNo,
                                  ComplainerID = e.ComplainerID,
                                  Name = com.Name,
                                  ComplainerCell = com.Mobile,
                                  Remarks = e.Remarks,
                                  Description = e.Description,
                                  TypeID = e.TypeID,
                                  ComplaintTypes = t.ComplaintTypes,
                                  CategoryID = e.CategoryID,
                                  Category = c.Category,
                                  MerchantID = e.MerchantID.Value,
                                  ChannelID = e.ChannelID,
                                  Mangementid = d.ManagementId,
                                  ChannlerType = ch.ChannelType,
                                  ResolvedDate = e.ResolvedDate,
                                  SatisfactionScore = e.SatisfactionScore,
                                  CreatedAt = Convert.ToDateTime(d.CreatedAt),
                                  OTP = e.OTP,
                                  MerchantName = m.MerchantName,
                                  MerchantCity = m.City


                              }).ToList();


            return complaints;
        }

        public async Task<List<Complaint>> GetYearlyComplaints(string year)
        {
            int selectedYear = int.Parse(year);

            DateTime today = DateTime.Now;

            DateTime startOfYear = new DateTime(selectedYear, 1, 1);
            DateTime endOfYear = new DateTime(selectedYear, 12, 31);

            var yearlyComplaints = await _dbContext.Complaints
                .Where(d =>
                    d.ComplaintDate >= startOfYear &&
                    d.ComplaintDate <= endOfYear)
                .AsNoTracking()
                .ToListAsync();

            return yearlyComplaints;
        }

        public async Task<List<Complaint>> GetWeeklyComplaints(string year, string? month, string? week)
        {
            try
            {

                if (string.IsNullOrEmpty(year) || string.IsNullOrEmpty(month) || string.IsNullOrEmpty(week))
                {
                    throw new ArgumentException("Year, month, and week parameters are required.");
                }

                int yearParsed = int.Parse(year);
                int monthParsed = int.Parse(month);
                int weekParsed = int.Parse(week);

                monthParsed++;

                //DateTime firstDayOfMonth = new DateTime(yearParsed, monthParsed, 1);
                //DateTime weekStart = firstDayOfMonth.AddDays((weekParsed - 1) * 7);
                //DateTime weekEnd = weekStart.AddDays(6);

                DateTime today = DateTime.Today;

                DateTime weekStart = today;
                while (weekStart.DayOfWeek != DayOfWeek.Saturday)
                {
                    weekStart = weekStart.AddDays(-1);
                }

                DateTime weekEnd = weekStart.AddDays(6);

                //DateTime weekStart = firstSaturday.AddDays((weekParsed - 1) * 7);
                //DateTime weekEnd = weekStart.AddDays(6);

                var weeklyComplaints = await _dbContext.Complaints
                    .Where(d => d.ComplaintDate >= weekStart && d.ComplaintDate <= weekEnd)
                    .AsNoTracking()
                    .ToListAsync();

                return weeklyComplaints;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<List<ComplaintDetails>> GetWeeklyComplaintDetails(string year, string? month, string? week)
        {
            try
            {
                if (string.IsNullOrEmpty(year) || string.IsNullOrEmpty(month) || string.IsNullOrEmpty(week))
                {
                    throw new ArgumentException("Year, month, and week parameters are required.");
                }

                int yearParsed = int.Parse(year);
                int monthParsed = int.Parse(month);
                int weekParsed = int.Parse(week);

                monthParsed++;

                //DateTime firstDayOfMonth = new DateTime(yearParsed, monthParsed, 1);
                //DateTime weekStart = firstDayOfMonth.AddDays((weekParsed - 1) * 7);
                //DateTime weekEnd = weekStart.AddDays(6);

                //DateTime firstDayOfMonth = new DateTime(yearParsed, monthParsed, 1);

                //DateTime firstSaturday = firstDayOfMonth;
                //while (firstSaturday.DayOfWeek != DayOfWeek.Saturday)
                //{
                //    firstSaturday = firstSaturday.AddDays(1);
                //}

                //DateTime weekStart = firstSaturday.AddDays((weekParsed - 1) * 7);
                //DateTime weekEnd = weekStart.AddDays(6);

                DateTime today = DateTime.Today;

                DateTime weekStart = today;
                while (weekStart.DayOfWeek != DayOfWeek.Saturday)
                {
                    weekStart = weekStart.AddDays(-1);
                }

                DateTime weekEnd = weekStart.AddDays(6);

                //var weeklyComplaints = await _dbContext.ComplaintDetails
                //    .Where(d => d.CreatedAt >= weekStart && d.CreatedAt <= weekEnd)
                //    .AsNoTracking()
                //    .ToListAsync();

                var weeklyComplaints = await _dbContext.ComplaintDetails
                    .Where(d => d.CreatedAt >= weekStart && d.CreatedAt <= weekEnd) // Filter by date range
                    .AsNoTracking()
                    .GroupBy(d => d.ComplaintID)
                    .Select(group => group.OrderByDescending(d => d.CreatedAt).FirstOrDefault()) // Take the latest record for each ComplaintId
                    .ToListAsync();

                return weeklyComplaints;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<List<Complaint>> GetMonthlyComplaints(string year, string month)
        {
            if (string.IsNullOrEmpty(year) || string.IsNullOrEmpty(month))
            {
                throw new ArgumentException("Year and month parameters are required.");
            }

            int yearParsed = int.Parse(year);
            int monthParsed = int.Parse(month);
            monthParsed++;

            DateTime monthStart = new DateTime(yearParsed, monthParsed, 1);
            DateTime monthEnd = monthStart.AddMonths(1).AddDays(-1);

            var monthlyComplaints = await _dbContext.Complaints
                .Where(d => d.ComplaintDate >= monthStart && d.ComplaintDate <= monthEnd)
                .AsNoTracking()
                .ToListAsync();

            return monthlyComplaints;
        }

        public async Task<int> GetNoOfComplainToday()
        {
            //DateTime today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            //var result = await _dbContext.Complaints.Where(s => s.ComplaintDate.Value.Date == today)
            //    .GroupBy(g => g.ComplaintDate).Select(s => new ComplainCount { Key = s.Key, Count = s.Count() }).FirstOrDefaultAsync();
            DateTime startOfToday = DateTime.Today;
            DateTime endOfToday = DateTime.Today.AddDays(1).AddTicks(-1);

            var result = await _dbContext.Complaints.Where(x => x.ComplaintDate >= startOfToday && x.ComplaintDate <= endOfToday).ToListAsync();
            return result.Count();
        }

        public async Task<int> GetNoOfPendingComplainToday()
        {
            //DateTime today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            DateTime startOfToday = DateTime.Today;
            DateTime endOfToday = DateTime.Today.AddDays(1).AddTicks(-1);
            //var result = await (from e in _dbContext.Complaints
            //                    join c in _dbContext.ComplaintDetails on e.ID equals c.ComplaintID
            //                    where e.ComplaintDate.Value >= startOfToday
            //                       && e.ComplaintDate.Value <= endOfToday
            //                       && (c.CurrentStatus == "New" || c.CurrentStatus == "Processing")
            //                    select e
            //       ).ToListAsync();

            //return result.Count();
            var result = await (
                from e in _dbContext.Complaints
                where e.ComplaintDate >= startOfToday
                      && e.ComplaintDate <= endOfToday
                      && _dbContext.ComplaintDetails
                          .Any(c => c.ComplaintID == e.ID
                                    && (c.CurrentStatus == "New" || c.CurrentStatus == "Processing"))
                select e
                ).CountAsync();

            return result;
        }

        public async Task<int> GetTotalClosedComplain()
        {
            var query = from e in _dbContext.Complaints
                        join c in _dbContext.ComplaintDetails on e.ID equals c.ComplaintID
                        where e.ComplaintDate.HasValue &&
                              (c.CurrentStatus == "Closed")
                        select e;

            // Use Distinct() to avoid duplicate complaints if multiple complaint details match.
            return await query.Distinct().CountAsync();
        }

        public Task<short> GetAveragResponseTime()
        {
            throw new NotImplementedException();
        }
        public async Task<ComplainCount> GetPendingComplainToday()
        {
            DateTime today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);


            //var escalations = await (from e in _merchantContext.Complaints
            //                         join c in _merchantContext.ComplaintDetails
            //                         on e.ID  equals c.ComplaintID
            //                         where (e.ComplaintDate == today && c.CurrentStatus == "Inprocess" && e.isDeleted == false)
            //                         select new EscalationView
            //                         {
            //                             ID = e.ID,
            //                             CategoryID = e.CategoryID,
            //                             Category = c.Category,
            //                             Type = e.Type.Value,
            //                             ComplaintTypes = t.ComplaintTypes,
            //                             Level = e.Level,
            //                             ManagementID = e.ManagementID.Value,
            //                             ResponseType = e.ResponseType,
            //                             ResponseTime = e.ResponseTime,
            //                             Email = e.Email,
            //                         }).ToListAsync();

            var result = await _dbContext.Complaints.Where(s => s.ComplaintDate == today && s.Status == "Inprocess")
                .GroupBy(g => g.ComplaintDate).Select(s => new ComplainCount { Key = s.Key, Count = s.Count() }).FirstOrDefaultAsync();
            return result;
        }
    }
}
