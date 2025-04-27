using CompanyEmployees.ActionFilters;
using CompanyEmployees.Extensions;
using Contracts;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using NLog;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

// Configure NLog
LogManager.Setup().LoadConfigurationFromFile(Path.Combine(Directory.GetCurrentDirectory(), "nlog.development.config"));
var logger = LogManager.GetCurrentClassLogger();

try
{
    logger.Info("Application Starting up...");

    // 🔹 Register services BEFORE calling builder.Build()
    builder.Services.ConfigureCors();
    builder.Services.ConfigureIISIntegration();
    builder.Services.ConfigureLoggerService();
    builder.Services.ConfigureRepositoryManager();
    builder.Services.ConfigureServiceManager();
    builder.Services.ConfigureSqlContext(builder.Configuration);
    builder.Services.AddAutoMapper(typeof(Program));
    builder.Services.AddScoped<ValidationFilterAttribute>();

    //builder.Services.Configure<ApiBehaviorOptions>(options =>
    //{
    //    options.SuppressModelStateInvalidFilter = true;
    //});

    // 🔹 Configure Controllers
    builder.Services.AddControllers(config => {
        config.RespectBrowserAcceptHeader = true;
        config.ReturnHttpNotAcceptable = true;
        config.InputFormatters.Insert(0,GetJsonPatchInputFormatter());
    }).AddXmlDataContractSerializerFormatters()
    .AddApplicationPart(typeof(CompanyEmployees.Presentation.AssemblyReference).Assembly);

    // 🔹 Custom API response configuration
    builder.Services.Configure<ApiBehaviorOptions>(options => {
        options.SuppressModelStateInvalidFilter = true;
    });
     
    NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter() =>
        new ServiceCollection().AddLogging().AddMvc().AddNewtonsoftJson()
        .Services.BuildServiceProvider()
        .GetRequiredService<IOptions<MvcOptions>>().Value.InputFormatters
        .OfType<NewtonsoftJsonPatchInputFormatter>().First(); 

    var app = builder.Build(); // ✅ Only build once

    // 🔹 Get the logger from the service provider
    var serviceLogger = app.Services.GetRequiredService<ILoggerManager>();
    app.ConfigureExceptionHandler(serviceLogger);

    if (app.Environment.IsProduction())
        app.UseHsts();

    // Configure the HTTP request pipeline
    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.All
    });
    app.UseCors("CordPolicy");
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Application failed to start");
    throw;
}
finally
{
    LogManager.Shutdown(); // ✅ Corrected shutdown method
}
