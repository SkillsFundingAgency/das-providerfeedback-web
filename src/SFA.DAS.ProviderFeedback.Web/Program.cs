using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ProviderFeedback.Web.AppStart;
using SFA.DAS.Provider.Shared.UI.Startup;
using SFA.DAS.ProviderFeedback.Application.RegistrationExtensions;
using SFA.DAS.Validation.Mvc.Extensions;
using SFA.DAS.ProviderFeedback.Domain.Configuration;
using Microsoft.AspNetCore.Authorization;
using SFA.DAS.ProviderFeedback.Web.Infrastructure.Authorization;
using SFA.DAS.DfESignIn.Auth.Enums;
using SFA.DAS.DfESignIn.Auth.Interfaces;
using SFA.DAS.ProviderFeedback.Domain.Configuration;
using SFA.DAS.DfESignIn.Auth.AppStart;

var builder = WebApplication.CreateBuilder(args);

var isIntegrationTest = builder.Environment.EnvironmentName.Equals("IntegrationTest", StringComparison.CurrentCultureIgnoreCase);
var rootConfiguration = builder.Configuration.LoadConfiguration(isIntegrationTest);

builder.Services.AddOptions();

builder.Services.Configure<ProviderFeedback>(rootConfiguration.GetSection("ProviderFeedback"));
builder.Services.Configure<ProviderFeedbackWeb>(rootConfiguration.GetSection("ProviderFeedbackWeb"));

builder.Services.AddConfigurationOptions(rootConfiguration);

builder.Services.AddSingleton<IAuthorizationHandler, ProviderAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, TrainingProviderAllRolesAuthorizationHandler>();

builder.Services.AddLogging();

builder.Services.AddServiceRegistration();

builder.Services.AddProviderUiServiceRegistration(rootConfiguration);

builder.Services.AddMediatRHandlers();

builder.Services.AddAuthorizationServicePolicies();

builder.Services.AddAndConfigureDfESignInAuthentication(
    rootConfiguration,
    "SFA.DAS.ProviderApprenticeshipService",
    typeof(CustomServiceRole),
    ClientName.ProviderRoatp,
    "/signout",
    "");


builder.Services.AddHealthChecks();

builder.Services.AddProviderUiServiceRegistration(rootConfiguration);


builder.Services.Configure<RouteOptions>(options =>
{

}).AddMvc(options =>
{
    options.AddValidation();
    if (!isIntegrationTest)
    {
        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
    }

});

//builder.Services.AddDataProtection(rootConfiguration);

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddApplicationInsightsTelemetry();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

}

app.UseContentSecurityPolicy();

app.UseHealthChecks("/ping");

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.UseStaticFiles();

app.UseEndpoints(endpointBuilder =>
{
    endpointBuilder.MapControllerRoute(
        name: "default",
        pattern: "{controller=FeedbackController}/{action=Index}/");
});

app.Run();
