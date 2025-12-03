using AutoMapper;
using Merchants.Application.Commands.Escalation;
using Merchants.Core.Common;
using Merchants.Core.Entities;
using Merchants.Core.Interfaces;
using Merchants.Infrastructure.Data;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using static StackExchange.Redis.Role;

namespace Merchants.Infrastructure.Repositories
{

    public class EscalationService :  AsyncRepository<Escalation>, IEscalation
    {

        private readonly MerchantContext _merchantContext;
        private readonly ILogger<EscalationService> _logger;
        private readonly IMapper _mapper;

        public EscalationService(MerchantContext merchantContext, ILogger<EscalationService> logger, IMapper mapper)
            : base(merchantContext, logger)
        {
            _merchantContext = merchantContext;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Escalation>> AddEsalationsAs(IEnumerable<Escalation> escalations)
        {
           await _merchantContext.AddRangeAsync(escalations);

            // Saving changes to the database
            await _merchantContext.SaveChangesAsync();

            // Returning the added escalations (with the updated state, such as Ids if auto-generated)
            return escalations;
        }

        public async Task<Escalation> AddEscalationAsync(Escalation escalation)
        {
            //throw new NotImplementedException();
            var _escObj =  await _merchantContext.AddAsync(escalation);
            await _merchantContext.SaveChangesAsync();

            return _escObj.Entity;

        }

        public async Task<IEnumerable<Escalation>> GetEscalationAsyn()
        {
            return await _merchantContext.Escalations.Where(s=> s.isDeleted == false).ToListAsync();
        }

        public async Task<IList<EscalationView>> GetEscalationByCategory(int CategoryID, int ComplainTypeID)
        {
            //throw new NotImplementedException();
            //var escalations = await _merchantContext.Escalations.Where(x => x.CategoryID == CategoryID && x.Type == ComplainTypeID).ToListAsync();

            try
            {
                var escalations = await (from e in _merchantContext.Escalations
                                         join c in _merchantContext.ComplaintCategories
                                         on e.CategoryID equals c.ID
                                         join t in _merchantContext.ComplaintTypes on e.Type equals t.ID
                                         where (e.CategoryID == CategoryID && e.Status == "Active") && e.Type == ComplainTypeID && e.Status == "Active" && e.isDeleted == false 
                                         select new EscalationView
                                         {
                                             ID = e.ID,
                                             CategoryID = e.CategoryID,
                                             Category = c.Category,
                                             Type = e.Type.Value,
                                             ComplaintTypes = t.ComplaintTypes,
                                             Level = e.Level,
                                             ManagementID = e.ManagementID.Value,
                                             ResponseType = e.ResponseType,
                                             ResponseTime = e.ResponseTime,
                                             Email = e.Email,
                                         }).ToListAsync();


                return escalations;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<IList<EscalationView>> GetEscalationByMerchant(int LevelID)
        {
            //throw new NotImplementedException();
            //var escalations = await _merchantContext.Escalations.Where(x => x.CategoryID == CategoryID && x.Type == ComplainTypeID).ToListAsync();

            try
            {
                var escalations = await (from e in _merchantContext.Escalations
                                         join t in _merchantContext.ComplaintTypes on e.Type equals t.ID
                                         where (e.Email == "Merchant" && e.Level >= LevelID && e.Status == "Active") && e.isDeleted == false
                                         select new EscalationView
                                         {
                                             ID = e.ID,
                                             CategoryID = e.CategoryID,
                                             Type = e.Type.Value,
                                             ComplaintTypes = t.ComplaintTypes,
                                             Level = e.Level,
                                             ManagementID = e.ManagementID.Value,
                                             ResponseType = e.ResponseType,
                                             ResponseTime = e.ResponseTime,
                                             Email = e.Email,
                                         }).ToListAsync();


                return escalations;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public async Task<Escalation> GetEscalationByID(int ID)
        {
            try
            {
                var escalation = await _merchantContext.Escalations.AsNoTracking().Where(x => x.ID == ID).FirstOrDefaultAsync();
                return escalation;
            }
            catch(Exception ex)
            {
                var error = ex.Message;
            }
            return new Escalation();

        }
        public async Task<IEnumerable<Escalation>> GetEscalationByManagementID(int ID)
        {
            try
            {
                var escalation = await _merchantContext.Escalations.AsNoTracking().Where(x => x.ManagementID == ID).ToListAsync();
                return escalation;
            }
            catch(Exception ex)
            {
                var error = ex.Message;
            }
            return null;

        }

    }

}
