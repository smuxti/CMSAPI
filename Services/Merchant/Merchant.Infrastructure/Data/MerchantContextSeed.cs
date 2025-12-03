using Azure.Core;
using Merchants.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Text.Json;

namespace Merchants.Infrastructure.Data
{
    public class MerchantContextSeed
    {
        public static async Task Seeder(MerchantContext context, ILogger<MerchantContextSeed> logger)
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
                logger.LogInformation($"User Seeded.{typeof(MerchantContext).Name}");
            }
            if (!context.ComplaintTypes.Any())
            {
                await context.ComplaintTypes.AddRangeAsync(GetComplaintTypesData());
                await context.SaveChangesAsync();
                logger.LogInformation($"Complaint Seeded.{typeof(MerchantContext).Name}");
            }
            if (!context.ManagementHierarchies.Any())
            {
                await context.ManagementHierarchies.AddRangeAsync(GetMerchantRelatedHierarchiesData());
                await context.SaveChangesAsync();
                logger.LogInformation($"Complaint Seeded.{typeof(MerchantContext).Name}");
            }
            if (!context.Channels.Any())
            {
                await context.Channels.AddRangeAsync(GetSeedDataChannels());
                await context.SaveChangesAsync();
                logger.LogInformation($"User Seeded.{typeof(MerchantContext).Name}");
            }
            if (!context.Routes.Any())
            {
                await context.Routes.AddRangeAsync(GetSeedDataRoutes());
                await context.SaveChangesAsync();
                logger.LogInformation($"Routes Seeded.{typeof(MerchantContext).Name}");
            }
            if (!context.RoleRoutes.Any())
            {
                await context.RoleRoutes.AddRangeAsync(GetSeedDataRoleRoutes());
                await context.SaveChangesAsync();
                logger.LogInformation($"Role Routes Seeded.{typeof(MerchantContext).Name}");
            }

            await CheckForOtherCategory(context);
        }

        private static async Task CheckForOtherCategory(MerchantContext context)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var existingCategory = await context.ComplaintCategories.FirstOrDefaultAsync(c => c.ID == -999 && c.isDeleted == false);
                    if (existingCategory != null)
                    {
                        return;
                    }

                    await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.tblComplaintCategory ON");

                    var seedData = GetSeedDataCategories();
                    await context.ComplaintCategories.AddRangeAsync(seedData);
                    await context.SaveChangesAsync();

                    await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.tblComplaintCategory OFF");

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();

                    Console.WriteLine($"An error occurred while seeding categories: {ex.Message}");
                    throw;
                }
            }
        }

        private static IEnumerable<Channel> GetSeedDataChannels()
        {
            return new List<Channel>
            {
                new()
                {
                    ChannelType = "UAN",
                    Remarks = null,
                    Status = "Active"
                },
                new()
                {
                    ChannelType = "Email",
                    Remarks = null,
                    Status = "Active"
                },
                new()
                {
                    ChannelType = "Whatsapp",
                    Remarks = null,
                    Status = "Active"
                },
                new()
                {
                    ChannelType = "Portal",
                    Remarks = null,
                    Status = "Active"
                }
            };
        }

        private static IEnumerable<ComplaintCategory> GetSeedDataCategories()
        {
            return new List<ComplaintCategory>
            {
                new()
                {
                    ID = -999,
                    Category = "Others",
                    Type = 2
                }
            };
        }

        private static IEnumerable<ManagementHierarchy> GetMerchantRelatedHierarchiesData()
        {
            return new List<ManagementHierarchy>
            {
                new()
                {
                    POCName = "Dealer",
                    Name = "Dealer",
                    POCEmail = "Dealer@Makglobalps.com",
                    POCNumber = "03000000000",
                    OtherContact = "03000000000",
                    OtherEmail = "Dealer@Makglobalps.com",
                    Address = "407, Parsa Tower, Sharah-e-Faisal, Karachi.",
                    ManagementType = -1
                },
                new()
                {
                    POCName = "TSM",
                    Name = "TSM",
                    POCEmail = "TSM@Makglobalps.com",
                    POCNumber = "03000000000",
                    OtherContact = "03000000000",
                    OtherEmail = "TSM@Makglobalps.com",
                    Address = "407, Parsa Tower, Sharah-e-Faisal, Karachi.",
                    ManagementType = -2
                },
                new()
                {
                    POCName = "Region",
                    Name = "Region",
                    POCEmail = "Region@Makglobalps.com",
                    POCNumber = "03000000000",
                    OtherContact = "03000000000",
                    OtherEmail = "Region@Makglobalps.com",
                    Address = "407, Parsa Tower, Sharah-e-Faisal, Karachi.",
                    ManagementType = -3
                },
                new()
                {
                    POCName = "Engineer",
                    Name = "Engineer",
                    POCEmail = "Engineer@Makglobalps.com",
                    POCNumber = "03000000000",
                    OtherContact = "03000000000",
                    OtherEmail = "Engineer@Makglobalps.com",
                    Address = "407, Parsa Tower, Sharah-e-Faisal, Karachi.",
                    ManagementType = -4
                },
            };
        }

        private static IEnumerable<UserType> GetUserTypesData()
        {
            return new List<UserType>
            {
                new (){
                    TypeName="Administrator",
                    Status="Active",
                    TypeCode=1,
                    Locked = true
                },
                new (){
                    TypeName="Marketing",
                    Status="Active",
                    TypeCode=5,
                    Locked = true
                },
                new (){
                    TypeName="Manager",
                    Status="Active",
                    TypeCode=2,
                    Locked = true
                },
                new (){
                    TypeName="Dealer",
                    Status="Active",
                    TypeCode=3,
                    Locked = true
                },
                new (){
                    TypeName="User",
                    Status="Active",
                    TypeCode=4,
                    Locked = true
                },
                 new (){
                    TypeName="Agent",
                    Status="Active",
                    TypeCode=6,
                    Locked = true
                },new (){
                    TypeName="Territory Salesman",
                    Status="Active",
                    TypeCode=7,
                    Locked = true
                },new (){
                    TypeName="Regional Manager",
                    Status="Active",
                    TypeCode=8,
                    Locked = true
                },new (){
                    TypeName="Engineer",
                    Status="Active",
                    TypeCode=9,
                    Locked = true
                },
            };
        }
        private static IEnumerable<ComplaintType> GetComplaintTypesData()
        {
            return new List<ComplaintType>
            {
                new (){
                    ComplaintTypes="Internal"
                },
                new (){
                    ComplaintTypes="External"
                },

            };
        }

        private static IEnumerable<User> GetSeedData()
        {
            return new List<User> {
                new (){
                    Username = "admin",
                    PasswordHash = "JjxXlHdhCHir53xRhtGZJEjFUH2OEVyrina1m8Tu3K8=",
                    SecurityKey = "MGPSDefaultGeneratedSecurityKey",
                    Email = "ehsan@makglobalps.com",
                    //RoleId = 1,
                    UserTypeCode = 1,
                    MerchantId = 1,
                    ManagementId = 0
                },
            };
        }

        private static IEnumerable<Routes> GetSeedDataRoutes()
        {
            return new List<Routes>
            {
                //Dashboard
                new() { RouteId = 1, RoutePath = "Dashboards/Home", ModuleName = "Dashboards", RouteName = "Homepage" },

                //Complaints
                new() { RouteId = 2, RoutePath = "Complaints/Index", ModuleName = "Complaints", RouteName = "Index" },
                new() { RouteId = 3, RoutePath = "Complaints/Create", ModuleName = "Complaints", RouteName = "Create" },
                new() { RouteId = 4, RoutePath = "Complaints/Delete", ModuleName = "Complaints", RouteName = "Delete" },
                new() { RouteId = 5, RoutePath = "Complaints/Details", ModuleName = "Complaints", RouteName = "Details" },
                new() { RouteId = 6, RoutePath = "Complaints/GetStatus", ModuleName = "Complaints", RouteName = "Complaint-Status" },
                new() { RouteId = 7, RoutePath = "Complaints/StartComplaint", ModuleName = "Complaints", RouteName = "Start-Complaint" },
                new() { RouteId = 8, RoutePath = "Complaints/UpdateComplaint", ModuleName = "Complaints", RouteName = "Update-Complaint" },
                new() { RouteId = 9, RoutePath = "Complaints/ForwardComplaint", ModuleName = "Complaints", RouteName = "Forward-Complaint" },
                new() { RouteId = 10, RoutePath = "Complaints/GetComplaintHistory", ModuleName = "Complaints", RouteName = "Complaint-History" },

                //Completed Complaints
                new() { RouteId = 11, RoutePath = "CompletedComplaints/Index", ModuleName = "Completed Complaints", RouteName = "Index" },
                new() { RouteId = 12, RoutePath = "CompletedComplaints/CloseComplaint", ModuleName = "Completed Complaints", RouteName = "Close-Complaint" },
                new() { RouteId = 13, RoutePath = "CompletedComplaints/ForceCloseComplaint", ModuleName = "Completed Complaints", RouteName = "Force-Close-Complaint" },

                //Channel
                new() { RouteId = 14, RoutePath = "Channel/Index", ModuleName = "Channel", RouteName = "Index" },
                new() { RouteId = 15, RoutePath = "Channel/Create", ModuleName = "Channel", RouteName = "Create" },
                new() { RouteId = 16, RoutePath = "Channel/Edit", ModuleName = "Channel", RouteName = "Edit" },
                new() { RouteId = 17, RoutePath = "Channel/Delete", ModuleName = "Channel", RouteName = "Delete" },

                //Category
                new() { RouteId = 18, RoutePath = "Category/Index", ModuleName = "Category", RouteName = "Index" },
                new() { RouteId = 19, RoutePath = "Category/Create", ModuleName = "Category", RouteName = "Create" },
                new() { RouteId = 20, RoutePath = "Category/Edit", ModuleName = "Category", RouteName = "Edit" },
                new() { RouteId = 21, RoutePath = "Category/Delete", ModuleName = "Category", RouteName = "Delete" },

                //Escalation
                new() { RouteId = 22, RoutePath = "Escalation/Index", ModuleName = "Escalation", RouteName = "Index" },
                new() { RouteId = 23, RoutePath = "Escalation/Create", ModuleName = "Escalation", RouteName = "Create" },
                new() { RouteId = 24, RoutePath = "Escalation/Details", ModuleName = "Escalation", RouteName = "Details" },
                new() { RouteId = 25, RoutePath = "Escalation/Delete", ModuleName = "Escalation", RouteName = "Delete" },

                //Hierarchy
                new() { RouteId = 26, RoutePath = "Hierarchy/Index", ModuleName = "Hierarchy", RouteName = "Index" },
                new() { RouteId = 27, RoutePath = "Hierarchy/Create", ModuleName = "Hierarchy", RouteName = "Create" },
                new() { RouteId = 28, RoutePath = "Hierarchy/Details", ModuleName = "Hierarchy", RouteName = "Details" },
                new() { RouteId = 29, RoutePath = "Hierarchy/Edit", ModuleName = "Hierarchy", RouteName = "Edit" },
                new() { RouteId = 30, RoutePath = "Hierarchy/Delete", ModuleName = "Hierarchy", RouteName = "Delete" },

                //Dealer
                new() { RouteId = 31, RoutePath = "Dealer/Index", ModuleName = "Dealer", RouteName = "Index" },
                new() { RouteId = 32, RoutePath = "Dealer/Create", ModuleName = "Dealer", RouteName = "Create" },
                new() { RouteId = 33, RoutePath = "Dealer/Details", ModuleName = "Dealer", RouteName = "Details" },
                new() { RouteId = 34, RoutePath = "Dealer/Edit", ModuleName = "Dealer", RouteName = "Edit" },
                new() { RouteId = 35, RoutePath = "Dealer/Delete", ModuleName = "Dealer", RouteName = "Delete" },

                //Territory
                new() { RouteId = 36, RoutePath = "Territory/Index", ModuleName = "Territory", RouteName = "Index" },
                new() { RouteId = 37, RoutePath = "Territory/Create", ModuleName = "Territory", RouteName = "Create" },
                new() { RouteId = 38, RoutePath = "Territory/Details", ModuleName = "Territory", RouteName = "Details" },
                new() { RouteId = 39, RoutePath = "Territory/Edit", ModuleName = "Territory", RouteName = "Edit" },
                new() { RouteId = 40, RoutePath = "Territory/Delete", ModuleName = "Territory", RouteName = "Delete" },

                //Region
                new() { RouteId = 41, RoutePath = "Region/Index", ModuleName = "Region", RouteName = "Index" },
                new() { RouteId = 42, RoutePath = "Region/Create", ModuleName = "Region", RouteName = "Create" },
                new() { RouteId = 43, RoutePath = "Region/Details", ModuleName = "Region", RouteName = "Details" },
                new() { RouteId = 44, RoutePath = "Region/Edit", ModuleName = "Region", RouteName = "Edit" },
                new() { RouteId = 45, RoutePath = "Region/Delete", ModuleName = "Region", RouteName = "Delete" },

                //Engineer
                new() { RouteId = 46, RoutePath = "Engineer/Index", ModuleName = "Engineer", RouteName = "Index" },
                new() { RouteId = 47, RoutePath = "Engineer/Create", ModuleName = "Engineer", RouteName = "Create" },
                new() { RouteId = 48, RoutePath = "Engineer/Details", ModuleName = "Engineer", RouteName = "Details" },
                new() { RouteId = 49, RoutePath = "Engineer/Edit", ModuleName = "Engineer", RouteName = "Edit" },
                new() { RouteId = 50, RoutePath = "Engineer/Delete", ModuleName = "Engineer", RouteName = "Delete" },

                //Reports
                new() { RouteId = 51, RoutePath = "Reports/ComplaintReport", ModuleName = "Reports", RouteName = "Index" },
                new() { RouteId = 52, RoutePath = "Reports/Details", ModuleName = "Reports", RouteName = "Details" },

                //User
                new() { RouteId = 53, RoutePath = "User/Index", ModuleName = "User", RouteName = "Index" },
                new() { RouteId = 54, RoutePath = "User/Create", ModuleName = "User", RouteName = "Create" },
                new() { RouteId = 55, RoutePath = "User/Edit", ModuleName = "User", RouteName = "Edit" },
                new() { RouteId = 56, RoutePath = "User/Delete", ModuleName = "User", RouteName = "Delete" },
                new() { RouteId = 57, RoutePath = "User/Details", ModuleName = "User", RouteName = "Details" },

                //Role
                new() { RouteId = 58, RoutePath = "Role/Index", ModuleName = "Role", RouteName = "Index" },
                new() { RouteId = 59, RoutePath = "Role/Edit", ModuleName = "Role", RouteName = "Edit" },
                new() { RouteId = 60, RoutePath = "Role/Create", ModuleName = "Role", RouteName = "Create" },
                new() { RouteId = 61, RoutePath = "Role/Delete", ModuleName = "Role", RouteName = "Delete" },

                //Route
                new() { RouteId = 62, RoutePath = "Route/Index", ModuleName = "Route", RouteName = "Index" },
                new() { RouteId = 63, RoutePath = "Route/Create", ModuleName = "Route", RouteName = "Create" },
                new() { RouteId = 64, RoutePath = "Route/Edit", ModuleName = "Route", RouteName = "Edit" },
                new() { RouteId = 65, RoutePath = "Route/Delete", ModuleName = "Route", RouteName = "Delete" },
                
                //Equipment
                new() { RouteId = 66, RoutePath = "Equipment/Index", ModuleName = "Equipment", RouteName = "Index" },
                new() { RouteId = 67, RoutePath = "Equipment/Create", ModuleName = "Equipment", RouteName = "Create" },
                new() { RouteId = 68, RoutePath = "Equipment/Edit", ModuleName = "Equipment", RouteName = "Edit" },
                new() { RouteId = 69, RoutePath = "Equipment/Delete", ModuleName = "Equipment", RouteName = "Delete" },
            };
        }

        private static IEnumerable<RoleRouts> GetSeedDataRoleRoutes()
        {
            return new List<RoleRouts>
            {   
                /////////////////
                //ADMINISTRATOR//
                /////////////////
                
                // Dashboard
                new() { RouteId = 1, RoutePathId = 1, RoleTypeId = 1 },

                // Complaints
                new() { RouteId = 2, RoutePathId = 2, RoleTypeId = 1 },
                new() { RouteId = 3, RoutePathId = 3, RoleTypeId = 1 },
                new() { RouteId = 4, RoutePathId = 4, RoleTypeId = 1 },
                new() { RouteId = 5, RoutePathId = 5, RoleTypeId = 1 },
                new() { RouteId = 6, RoutePathId = 6, RoleTypeId = 1 },
                new() { RouteId = 7, RoutePathId = 7, RoleTypeId = 1 },
                new() { RouteId = 8, RoutePathId = 8, RoleTypeId = 1 },
                new() { RouteId = 9, RoutePathId = 9, RoleTypeId = 1 },
                new() { RouteId = 10, RoutePathId = 10, RoleTypeId = 1 },

                // Completed Complaints
                new() { RouteId = 11, RoutePathId = 11, RoleTypeId = 1 },
                new() { RouteId = 12, RoutePathId = 12, RoleTypeId = 1 },
                new() { RouteId = 13, RoutePathId = 13, RoleTypeId = 1 },

                // Channel
                new() { RouteId = 14, RoutePathId = 14, RoleTypeId = 1 },
                new() { RouteId = 15, RoutePathId = 15, RoleTypeId = 1 },
                new() { RouteId = 16, RoutePathId = 16, RoleTypeId = 1 },
                new() { RouteId = 17, RoutePathId = 17, RoleTypeId = 1 },

                // Category
                new() { RouteId = 18, RoutePathId = 18, RoleTypeId = 1 },
                new() { RouteId = 19, RoutePathId = 19, RoleTypeId = 1 },
                new() { RouteId = 20, RoutePathId = 20, RoleTypeId = 1 },
                new() { RouteId = 21, RoutePathId = 21, RoleTypeId = 1 },

                // Escalation
                new() { RouteId = 22, RoutePathId = 22, RoleTypeId = 1 },
                new() { RouteId = 23, RoutePathId = 23, RoleTypeId = 1 },
                new() { RouteId = 24, RoutePathId = 24, RoleTypeId = 1 },
                new() { RouteId = 25, RoutePathId = 25, RoleTypeId = 1 },

                // Hierarchy
                new() { RouteId = 26, RoutePathId = 26, RoleTypeId = 1 },
                new() { RouteId = 27, RoutePathId = 27, RoleTypeId = 1 },
                new() { RouteId = 28, RoutePathId = 28, RoleTypeId = 1 },
                new() { RouteId = 29, RoutePathId = 29, RoleTypeId = 1 },
                new() { RouteId = 30, RoutePathId = 30, RoleTypeId = 1 },

                // Dealer
                new() { RouteId = 31, RoutePathId = 31, RoleTypeId = 1 },
                new() { RouteId = 32, RoutePathId = 32, RoleTypeId = 1 },
                new() { RouteId = 33, RoutePathId = 33, RoleTypeId = 1 },
                new() { RouteId = 34, RoutePathId = 34, RoleTypeId = 1 },
                new() { RouteId = 35, RoutePathId = 35, RoleTypeId = 1 },

                // Territory
                new() { RouteId = 36, RoutePathId = 36, RoleTypeId = 1 },
                new() { RouteId = 37, RoutePathId = 37, RoleTypeId = 1 },
                new() { RouteId = 38, RoutePathId = 38, RoleTypeId = 1 },
                new() { RouteId = 39, RoutePathId = 39, RoleTypeId = 1 },
                new() { RouteId = 40, RoutePathId = 40, RoleTypeId = 1 },

                // Region
                new() { RouteId = 41, RoutePathId = 41, RoleTypeId = 1 },
                new() { RouteId = 42, RoutePathId = 42, RoleTypeId = 1 },
                new() { RouteId = 43, RoutePathId = 43, RoleTypeId = 1 },
                new() { RouteId = 44, RoutePathId = 44, RoleTypeId = 1 },
                new() { RouteId = 45, RoutePathId = 45, RoleTypeId = 1 },

                // Engineer
                new() { RouteId = 46, RoutePathId = 46, RoleTypeId = 1 },
                new() { RouteId = 47, RoutePathId = 47, RoleTypeId = 1 },
                new() { RouteId = 48, RoutePathId = 48, RoleTypeId = 1 },
                new() { RouteId = 49, RoutePathId = 49, RoleTypeId = 1 },
                new() { RouteId = 50, RoutePathId = 50, RoleTypeId = 1 },

                // Reports
                new() { RouteId = 51, RoutePathId = 51, RoleTypeId = 1 },
                new() { RouteId = 52, RoutePathId = 52, RoleTypeId = 1 },

                // User
                new() { RouteId = 53, RoutePathId = 53, RoleTypeId = 1 },
                new() { RouteId = 54, RoutePathId = 54, RoleTypeId = 1 },
                new() { RouteId = 55, RoutePathId = 55, RoleTypeId = 1 },
                new() { RouteId = 56, RoutePathId = 56, RoleTypeId = 1 },
                new() { RouteId = 57, RoutePathId = 57, RoleTypeId = 1 },

                // Role
                new() { RouteId = 58, RoutePathId = 58, RoleTypeId = 1 },
                new() { RouteId = 59, RoutePathId = 59, RoleTypeId = 1 },
                new() { RouteId = 60, RoutePathId = 60, RoleTypeId = 1 },
                new() { RouteId = 61, RoutePathId = 61, RoleTypeId = 1 },

                // Route
                new() { RouteId = 62, RoutePathId = 62, RoleTypeId = 1 },
                new() { RouteId = 63, RoutePathId = 63, RoleTypeId = 1 },
                new() { RouteId = 64, RoutePathId = 64, RoleTypeId = 1 },
                new() { RouteId = 65, RoutePathId = 65, RoleTypeId = 1 },
                
                // Equipment
                new() { RouteId = 66, RoutePathId = 66, RoleTypeId = 1 },
                new() { RouteId = 67, RoutePathId = 67, RoleTypeId = 1 },
                new() { RouteId = 68, RoutePathId = 68, RoleTypeId = 1 },
                new() { RouteId = 69, RoutePathId = 69, RoleTypeId = 1 },

                //////////////
                ///MARKETING//
                //////////////
                
                // Dashboard
                new() { RouteId = 1, RoutePathId = 1, RoleTypeId = 5 },

                // Complaints
                new() { RouteId = 2, RoutePathId = 2, RoleTypeId = 5 },
                new() { RouteId = 3, RoutePathId = 3, RoleTypeId = 5 },
                new() { RouteId = 4, RoutePathId = 4, RoleTypeId = 5 },
                new() { RouteId = 5, RoutePathId = 5, RoleTypeId = 5 },
                new() { RouteId = 6, RoutePathId = 6, RoleTypeId = 5 },
                new() { RouteId = 7, RoutePathId = 7, RoleTypeId = 5 },
                new() { RouteId = 8, RoutePathId = 8, RoleTypeId = 5 },
                new() { RouteId = 9, RoutePathId = 9, RoleTypeId = 5 },
                new() { RouteId = 10, RoutePathId = 10, RoleTypeId = 5 },

                // Completed Complaints
                new() { RouteId = 11, RoutePathId = 11, RoleTypeId = 5 },
                new() { RouteId = 12, RoutePathId = 12, RoleTypeId = 5 },
                new() { RouteId = 13, RoutePathId = 13, RoleTypeId = 5 },

                // Channel
                new() { RouteId = 14, RoutePathId = 14, RoleTypeId = 5 },
                new() { RouteId = 15, RoutePathId = 15, RoleTypeId = 5 },
                new() { RouteId = 16, RoutePathId = 16, RoleTypeId = 5 },
                new() { RouteId = 17, RoutePathId = 17, RoleTypeId = 5 },

                // Category
                new() { RouteId = 18, RoutePathId = 18, RoleTypeId = 5 },
                new() { RouteId = 19, RoutePathId = 19, RoleTypeId = 5 },
                new() { RouteId = 20, RoutePathId = 20, RoleTypeId = 5 },
                new() { RouteId = 21, RoutePathId = 21, RoleTypeId = 5 },

                // Escalation
                new() { RouteId = 22, RoutePathId = 22, RoleTypeId = 5 },
                new() { RouteId = 23, RoutePathId = 23, RoleTypeId = 5 },
                new() { RouteId = 24, RoutePathId = 24, RoleTypeId = 5 },
                new() { RouteId = 25, RoutePathId = 25, RoleTypeId = 5 },

                // Hierarchy
                new() { RouteId = 26, RoutePathId = 26, RoleTypeId = 5 },
                new() { RouteId = 27, RoutePathId = 27, RoleTypeId = 5 },
                new() { RouteId = 28, RoutePathId = 28, RoleTypeId = 5 },
                new() { RouteId = 29, RoutePathId = 29, RoleTypeId = 5 },
                new() { RouteId = 30, RoutePathId = 30, RoleTypeId = 5 },

                // Dealer
                new() { RouteId = 31, RoutePathId = 31, RoleTypeId = 5 },
                new() { RouteId = 32, RoutePathId = 32, RoleTypeId = 5 },
                new() { RouteId = 33, RoutePathId = 33, RoleTypeId = 5 },
                new() { RouteId = 34, RoutePathId = 34, RoleTypeId = 5 },
                new() { RouteId = 35, RoutePathId = 35, RoleTypeId = 5 },

                // Territory
                new() { RouteId = 36, RoutePathId = 36, RoleTypeId = 5 },
                new() { RouteId = 37, RoutePathId = 37, RoleTypeId = 5 },
                new() { RouteId = 38, RoutePathId = 38, RoleTypeId = 5 },
                new() { RouteId = 39, RoutePathId = 39, RoleTypeId = 5 },
                new() { RouteId = 40, RoutePathId = 40, RoleTypeId = 5 },

                // Region
                new() { RouteId = 41, RoutePathId = 41, RoleTypeId = 5 },
                new() { RouteId = 42, RoutePathId = 42, RoleTypeId = 5 },
                new() { RouteId = 43, RoutePathId = 43, RoleTypeId = 5 },
                new() { RouteId = 44, RoutePathId = 44, RoleTypeId = 5 },
                new() { RouteId = 45, RoutePathId = 45, RoleTypeId = 5 },

                // Engineer
                new() { RouteId = 46, RoutePathId = 46, RoleTypeId = 5 },
                new() { RouteId = 47, RoutePathId = 47, RoleTypeId = 5 },
                new() { RouteId = 48, RoutePathId = 48, RoleTypeId = 5 },
                new() { RouteId = 49, RoutePathId = 49, RoleTypeId = 5 },
                new() { RouteId = 50, RoutePathId = 50, RoleTypeId = 5 },

                // Reports
                new() { RouteId = 51, RoutePathId = 51, RoleTypeId = 5 },
                new() { RouteId = 52, RoutePathId = 52, RoleTypeId = 5 },

                // User
                new() { RouteId = 53, RoutePathId = 53, RoleTypeId = 5 },
                new() { RouteId = 54, RoutePathId = 54, RoleTypeId = 5 },
                new() { RouteId = 55, RoutePathId = 55, RoleTypeId = 5 },
                new() { RouteId = 56, RoutePathId = 56, RoleTypeId = 5 },
                new() { RouteId = 57, RoutePathId = 57, RoleTypeId = 5 },

                // Role
                new() { RouteId = 58, RoutePathId = 58, RoleTypeId = 5 },
                new() { RouteId = 59, RoutePathId = 59, RoleTypeId = 5 },
                new() { RouteId = 60, RoutePathId = 60, RoleTypeId = 5 },
                new() { RouteId = 61, RoutePathId = 61, RoleTypeId = 5 },

                // Route
                new() { RouteId = 62, RoutePathId = 62, RoleTypeId = 5 },
                new() { RouteId = 63, RoutePathId = 63, RoleTypeId = 5 },
                new() { RouteId = 64, RoutePathId = 64, RoleTypeId = 5 },
                new() { RouteId = 65, RoutePathId = 65, RoleTypeId = 5 },

                // Equipment
                new() { RouteId = 66, RoutePathId = 66, RoleTypeId = 5 },
                new() { RouteId = 67, RoutePathId = 67, RoleTypeId = 5 },
                new() { RouteId = 68, RoutePathId = 68, RoleTypeId = 5 },
                new() { RouteId = 69, RoutePathId = 69, RoleTypeId = 5 },

                ///////////
                //MANAGER//
                ///////////
                
                // Dashboard
                new() { RouteId = 1, RoutePathId = 1, RoleTypeId = 2 },

                // Complaints
                new() { RouteId = 2, RoutePathId = 2, RoleTypeId = 2 },
                new() { RouteId = 3, RoutePathId = 3, RoleTypeId = 2 },
                new() { RouteId = 4, RoutePathId = 4, RoleTypeId = 2 },
                new() { RouteId = 5, RoutePathId = 5, RoleTypeId = 2 },
                new() { RouteId = 6, RoutePathId = 6, RoleTypeId = 2 },
                new() { RouteId = 7, RoutePathId = 7, RoleTypeId = 2 },
                new() { RouteId = 8, RoutePathId = 8, RoleTypeId = 2 },
                new() { RouteId = 9, RoutePathId = 9, RoleTypeId = 2 },
                new() { RouteId = 10, RoutePathId = 10, RoleTypeId = 2 },

                // Completed Complaints
                new() { RouteId = 11, RoutePathId = 11, RoleTypeId = 2 },
                new() { RouteId = 12, RoutePathId = 12, RoleTypeId = 2 },
                new() { RouteId = 13, RoutePathId = 13, RoleTypeId = 2 },

                // Channel
                new() { RouteId = 14, RoutePathId = 14, RoleTypeId = 2 },
                new() { RouteId = 15, RoutePathId = 15, RoleTypeId = 2 },
                new() { RouteId = 16, RoutePathId = 16, RoleTypeId = 2 },
                new() { RouteId = 17, RoutePathId = 17, RoleTypeId = 2 },

                // Category
                new() { RouteId = 18, RoutePathId = 18, RoleTypeId = 2 },
                new() { RouteId = 19, RoutePathId = 19, RoleTypeId = 2 },
                new() { RouteId = 20, RoutePathId = 20, RoleTypeId = 2 },
                new() { RouteId = 21, RoutePathId = 21, RoleTypeId = 2 },

                // Escalation
                new() { RouteId = 22, RoutePathId = 22, RoleTypeId = 2 },
                new() { RouteId = 23, RoutePathId = 23, RoleTypeId = 2 },
                new() { RouteId = 24, RoutePathId = 24, RoleTypeId = 2 },
                new() { RouteId = 25, RoutePathId = 25, RoleTypeId = 2 },

                // Hierarchy
                new() { RouteId = 26, RoutePathId = 26, RoleTypeId = 2 },
                new() { RouteId = 27, RoutePathId = 27, RoleTypeId = 2 },
                new() { RouteId = 28, RoutePathId = 28, RoleTypeId = 2 },
                new() { RouteId = 29, RoutePathId = 29, RoleTypeId = 2 },
                new() { RouteId = 30, RoutePathId = 30, RoleTypeId = 2 },

                // Dealer
                new() { RouteId = 31, RoutePathId = 31, RoleTypeId = 2 },
                new() { RouteId = 32, RoutePathId = 32, RoleTypeId = 2 },
                new() { RouteId = 33, RoutePathId = 33, RoleTypeId = 2 },
                new() { RouteId = 34, RoutePathId = 34, RoleTypeId = 2 },
                new() { RouteId = 35, RoutePathId = 35, RoleTypeId = 2 },

                // Territory
                new() { RouteId = 36, RoutePathId = 36, RoleTypeId = 2 },
                new() { RouteId = 37, RoutePathId = 37, RoleTypeId = 2 },
                new() { RouteId = 38, RoutePathId = 38, RoleTypeId = 2 },
                new() { RouteId = 39, RoutePathId = 39, RoleTypeId = 2 },
                new() { RouteId = 40, RoutePathId = 40, RoleTypeId = 2 },

                // Region
                new() { RouteId = 41, RoutePathId = 41, RoleTypeId = 2 },
                new() { RouteId = 42, RoutePathId = 42, RoleTypeId = 2 },
                new() { RouteId = 43, RoutePathId = 43, RoleTypeId = 2 },
                new() { RouteId = 44, RoutePathId = 44, RoleTypeId = 2 },
                new() { RouteId = 45, RoutePathId = 45, RoleTypeId = 2 },

                // Engineer
                new() { RouteId = 46, RoutePathId = 46, RoleTypeId = 2 },
                new() { RouteId = 47, RoutePathId = 47, RoleTypeId = 2 },
                new() { RouteId = 48, RoutePathId = 48, RoleTypeId = 2 },
                new() { RouteId = 49, RoutePathId = 49, RoleTypeId = 2 },
                new() { RouteId = 50, RoutePathId = 50, RoleTypeId = 2 },

                // Reports
                new() { RouteId = 51, RoutePathId = 51, RoleTypeId = 2 },
                new() { RouteId = 52, RoutePathId = 52, RoleTypeId = 2 },

                // User
                new() { RouteId = 53, RoutePathId = 53, RoleTypeId = 2 },
                new() { RouteId = 54, RoutePathId = 54, RoleTypeId = 2 },
                new() { RouteId = 55, RoutePathId = 55, RoleTypeId = 2 },
                new() { RouteId = 56, RoutePathId = 56, RoleTypeId = 2 },
                new() { RouteId = 57, RoutePathId = 57, RoleTypeId = 2 },

                // Role
                new() { RouteId = 58, RoutePathId = 58, RoleTypeId = 2 },
                new() { RouteId = 59, RoutePathId = 59, RoleTypeId = 2 },
                new() { RouteId = 60, RoutePathId = 60, RoleTypeId = 2 },
                new() { RouteId = 61, RoutePathId = 61, RoleTypeId = 2 },

                // Route
                new() { RouteId = 62, RoutePathId = 62, RoleTypeId = 2 },
                new() { RouteId = 63, RoutePathId = 63, RoleTypeId = 2 },
                new() { RouteId = 64, RoutePathId = 64, RoleTypeId = 2 },
                new() { RouteId = 65, RoutePathId = 65, RoleTypeId = 2 },

                // Equipment
                new() { RouteId = 66, RoutePathId = 66, RoleTypeId = 2 },
                new() { RouteId = 67, RoutePathId = 67, RoleTypeId = 2 },
                new() { RouteId = 68, RoutePathId = 68, RoleTypeId = 2 },
                new() { RouteId = 69, RoutePathId = 69, RoleTypeId = 2 },
            };
        }
    }
}
