using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Authentication.Application.Commands;
using Authentication.Core.Entities;
using Microsoft.Extensions.Logging;

namespace Authentication.Infrastructure.Data
{
    public class AuthenticationContextSeed
    {
        public static async Task Seeder(AuthenticationContext context, ILogger<AuthenticationContextSeed> logger)
        {
            if (!context.Users.Any())
            {
                await context.Database.EnsureCreatedAsync();
                await context.Users.AddRangeAsync(GetSeedData());
                await context.SaveChangesAsync();

            }
            if (!context.UserTypes.Any())
            {
                await context.UserTypes.AddRangeAsync(GetUserTypesData());
                await context.SaveChangesAsync();
                logger.LogInformation($"User Seeded.{typeof(AuthenticationContext).Name}");
            }
            if (!context.Routes.Any())
            {
                await context.Routes.AddRangeAsync(GetSeedDataRoutes());
                await context.SaveChangesAsync();
                logger.LogInformation($"User Seeded.{typeof(AuthenticationContext).Name}");
            }
        }

        private static IEnumerable<UserType> GetUserTypesData()
        {
            return new List<UserType>
            {
                new (){
                    TypeName="Administrator",
                    TypeCode=1
                },
                new (){
                    TypeName="Integrator",
                    TypeCode=2
                },
                new (){
                    TypeName="Merchant",
                    TypeCode=3
                },
                new (){
                    TypeName="MerchantUser",
                    TypeCode=4
                },
                new (){
                    TypeName="MerchantSupervisor",
                    TypeCode=5
                },
                new (){
                    TypeName="MerchantAccountant",
                    TypeCode=6
                },
                new (){
                    TypeName="Customer",
                    TypeCode=7
                },
                new (){
                    TypeName="CustomerUser",
                    TypeCode=8
                },
                new (){
                    TypeName="Supervisor",
                    TypeCode=9
                },
                new (){
                    TypeName="FinanceManager",
                    TypeCode=10
                },
            };
        }

        private static IEnumerable<User> GetSeedData()
        {
            return new List<User> {
                new (){
                    FirstName="Administrator",
                    MiddleName ="",
                    LastName="",
                    Email="Admin@makglobalps.com",
                    MobileNumber="923212109730",
                    PhoneNumber="9232123535",
                    Username="admin",
                    UserTypeCode=1,
                    PasswordHash="gsia3GzAj0JyM9vFkudo2mOIA5GvEl89Mw2b6ORm8MA=",
                    SecurityKey="MGPSDefaultGeneratedSecurityKey",
                    //PasswordHash="1ziNsVQfTX80l0Jy5eZqBcbcqLm/ePt8FEeoGOusfAk=",//abcd.1234
                    //SecurityKey="053e80b6a5844a9da9fab6a919767833",
                    TenantCode="1Link"

                },
                new (){
                    FirstName="1Link",
                    MiddleName ="",
                    LastName="",
                    Email="info@1link.com.pk",
                    MobileNumber="923212109730",
                    PhoneNumber="9232123535",
                    Username="1link",
                    UserTypeCode=2,
                    PasswordHash="BYcHFkIuGsDB+pGDseYrFnX5MWcOVhDZG+2H5TXVfxY=",
                    SecurityKey="MGPSDefaultGeneratedSecurityKey",
                    //PasswordHash="V8SrssVxLWIqHpEVYjg7ifAFm1aXS5FXRdDZlz3t2Uk=", //1234
                    //SecurityKey="6e665299a7d149bfb9fdaebf70393671",
                    TenantCode="1Link"
                },
                new (){
                    FirstName="MGPS",
                    MiddleName ="",
                    LastName="Merchant",
                    Email="ehsan@makglobalps.com",
                    MobileNumber="+923212109731",
                    PhoneNumber="+923212109731",
                    Username="MGPS",
                    UserTypeCode=3,
                    MerchantId=Guid.Parse("71E1E50D-B917-4550-7ECB-08DCC1CDBE0A"),
                    PasswordHash="BYcHFkIuGsDB+pGDseYrFnX5MWcOVhDZG+2H5TXVfxY=",
                    SecurityKey="MGPSDefaultGeneratedSecurityKey",
                    //PasswordHash="lumPyQlmN+fHx81vNAuqVyhpGLwxsoTwuvvUdDV5UP8=", //1234
                    //SecurityKey="6cd99eccfd774c7fa2e65c3e8b7b8b5d",
                    TenantCode="1Link"
                },
                new (){
                    FirstName="MGPS",
                    MiddleName ="",
                    LastName="User",
                    Email="ehsan@makglobalps.com",
                    MobileNumber="+923212109731",
                    PhoneNumber="+923212109731",
                    Username="MGPSUser",
                    UserTypeCode=4,
                    MerchantId=Guid.Parse("71E1E50D-B917-4550-7ECB-08DCC1CDBE0A"),
                    PasswordHash="BYcHFkIuGsDB+pGDseYrFnX5MWcOVhDZG+2H5TXVfxY=",
                    SecurityKey="MGPSDefaultGeneratedSecurityKey",
                    //PasswordHash="lumPyQlmN+fHx81vNAuqVyhpGLwxsoTwuvvUdDV5UP8=", //1234
                    //SecurityKey="6cd99eccfd774c7fa2e65c3e8b7b8b5d",
                    TenantCode="1Link"
                },
            };
        }
        private static IEnumerable<Routes> GetSeedDataRoutes()
        {
            return new List<Routes> {
                new (){
                    RouteId=1,
                    RouteName ="Authenticate/AddUser"
                },
                new (){
                   RouteId=2,
                   RouteName ="Merchant/GetMerchants"
                },
                 new (){
                   RouteId=3,
                   RouteName ="Merchants/AddMerchant"
                },
                  new (){
                   RouteId=4,
                   RouteName ="Merchant/UpdateMerchant"
                },
                   new (){
                   RouteId=5,
                   RouteName ="Merchant/DeleteMerchant"
                },
                    new (){
                   RouteId=6,
                   RouteName ="Merchant/PostMerchant"
                },
                     new (){
                   RouteId=7,
                   RouteName ="Terminal/GetTerminals"
                },
                      new (){
                   RouteId=8,
                   RouteName ="Terminal/AddTerminal"
                },
                       new (){
                   RouteId=9,
                   RouteName ="Terminal/ValidateTerminal"
                },
                        new (){
                   RouteId=10,
                   RouteName ="Payment/AddPayment"
                },
                         new (){
                   RouteId=11,
                   RouteName ="Payment/GenerateDQRC"
                },
                          new (){
                   RouteId=12,
                   RouteName ="Payment/NotifyMerchant"
                },
                           new (){
                   RouteId=13,
                   RouteName ="Payment/PaymentNotification"
                },
                            new (){
                   RouteId=14,
                   RouteName ="Payment/RTPIdRequestAlias"
                },
                 new (){
                   RouteId=15,
                   RouteName ="Payment/RTPIdRequestIban"
                },
                  new (){
                   RouteId=16,
                   RouteName ="Payment/RTPIdRequestBiller"
                },
                  new (){
                   RouteId=17,
                   RouteName ="Payment/RTPIdCancellation"
                },
                  new(){RouteId=18,RouteName="Merchant/GetBank"},
                  new(){RouteId=19,RouteName="Merchant/GetBanks"},
                  new(){RouteId=20,RouteName="Terminal/UpdateTerminal"},
                  new(){RouteId=21,RouteName="Terminal/DeleteTerminal"}
            };
        }
    }
}
