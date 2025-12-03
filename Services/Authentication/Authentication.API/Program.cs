using Authentication.Application.Extensions;
using Authentication.Infrastructure.Data;
using Authentication.Infrastructure.Extensions;
using AuthenticationManager;
using AuthenticationManager.Extensions;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
//using StackExchange.Redis;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(o => o.JsonSerializerOptions.PropertyNamingPolicy = null);
builder.Services.AddApiVersioning();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddAuthServices(builder.Configuration);
builder.Services.AddAutoMapper(typeof(Program));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "User.API", Version = "v1" });
});
builder.Services.AddHealthChecks().Services.AddDbContext<AuthenticationContext>();
builder.Services.AddSingleton<JwtTokenHandler>();// AuthenticationManager Class Library
builder.Services.AddTransient<Helper>();
//builder.Services.AddTransient<RouteIdAuthorizeAttribute>();
//builder.Services.AddSingleton<IConnectionMultiplexer>(options =>
//  ConnectionMultiplexer.Connect(("127.0.0.1:6379")));
//Add support to logging with SERILOG
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AuthenticationContext>();
    db.Database.Migrate();
    var logger = scope.ServiceProvider.GetService<ILogger<AuthenticationContextSeed>>();
    AuthenticationContextSeed.Seeder(db, logger).Wait();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Merchant.API v1");
    });
}
//Add support to logging request with SERILOG
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.Run();
