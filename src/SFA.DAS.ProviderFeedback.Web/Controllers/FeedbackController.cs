using Microsoft.AspNetCore.Mvc;
using MediatR;
using SFA.DAS.ProviderFeedback.Web.Infrastructure;
using SFA.DAS.ProviderFeedback.Web.ViewModels;
using System.Text.Json;
using static SFA.DAS.ProviderFeedback.Domain.Feedback.Feedback;

namespace SFA.DAS.ProviderFeedback.Web.Controllers;

public class FeedbackController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<FeedbackController> _logger;

    public FeedbackController(IMediator mediator,
        ILogger<FeedbackController> logger
    )
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    [Route("", Name =RouteNames.ServiceStartDefault)]
    public IActionResult Index()
    {
        ProviderFeedbackViewModel model = new ProviderFeedbackViewModel
        {
            EmployerFeedback = GetEmployerFeedback(),
            ApprenticeFeedback = GetApprenticeFeedback(),
        };

        return View(model);
    }

    private EmployerFeedbackViewModel GetEmployerFeedback ()
    {
        string fileName = "EmployerFeedback.json";
        string jsonString = System.IO.File.ReadAllText(fileName);
        // Deserialize using the same WeatherForecast class
        EmployerFeedback fb =  JsonSerializer.Deserialize<EmployerFeedback>(jsonString);
        return new EmployerFeedbackViewModel(fb);
    }

    private ApprenticeFeedbackViewModel GetApprenticeFeedback()
    {
        string fileName = "ApprenticeFeedback.json";
        string jsonString = System.IO.File.ReadAllText(fileName);
        // Deserialize using the same WeatherForecast class
        ApprenticeFeedback fb = JsonSerializer.Deserialize<ApprenticeFeedback>(jsonString);
        return new ApprenticeFeedbackViewModel(fb);
    }


}

