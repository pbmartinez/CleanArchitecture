using Infrastructure.DependencyInjectionExtensions;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Text.Json.Serialization;
using Infrastructure.Domain.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using System.Configuration;
using Microsoft.IdentityModel.Logging;
using WebApi.WellKnownNames;
using Serilog;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using WebApi.Helpers;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Hellang.Middleware.ProblemDetails;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddControllers(setupAction =>
{
    //Return Not Acceptable Status Code when api is requested in a format that it does not support
    setupAction.ReturnHttpNotAcceptable = true;
})
.AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// CORS Configuration
var allowedHosts = builder.Configuration[AppSettings.AllowedHosts].Split(',');

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowedHosts", builder =>
    {
        builder.WithOrigins(allowedHosts.ToArray());
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        builder.SetIsOriginAllowedToAllowWildcardSubdomains();
    });
});

//Api Docs Configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Application Core Services Configuration
builder.Services.AddAutoMapperWithProfiles();
builder.Services.AddEntitiesServicesAndRepositories();
builder.Services.AddCustomApplicationServices();

//Unit of Work Implementation Configuration
builder.Services.AddDbContext<UnitOfWorkContainer>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString(AppSettings.DefaultConnectionString), sqlServerOptions =>
   {
       sqlServerOptions.CommandTimeout(30);
       sqlServerOptions.EnableRetryOnFailure(3);
   }));


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddMicrosoftIdentityWebApi(builder.Configuration.GetSection(AppSettings.AzureAd));

builder.Services.AddHealthChecks().AddDbContextCheck<UnitOfWorkContainer>(failureStatus: HealthStatus.Degraded);

builder.Services.AddProblemDetails(options =>
{
    options.Map<Exception>((httpContext, exce) =>
    {
        return new ProblemDetails()
        {
            Instance = httpContext.Request.Path,
            Detail = exce.Message,
            Status = httpContext.Response.StatusCode,
            Title = "Internal Server Error",
            Type = $"https://httpstatuses.io/{httpContext.Response.StatusCode}"
        };
    });
});

IdentityModelEventSource.ShowPII = true;

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseProblemDetails();

app.MapHealthChecks("/health/live", new HealthCheckOptions()
{
    Predicate = _ => false,
    ResponseWriter = HealthCheckExtensions.WriteJsonResponse
});
app.MapHealthChecks("/health/ready", new HealthCheckOptions()
{
    ResponseWriter = HealthCheckExtensions.WriteJsonResponse
});



app.UseHttpsRedirection();

app.UseCors("AllowedHosts");


app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();

