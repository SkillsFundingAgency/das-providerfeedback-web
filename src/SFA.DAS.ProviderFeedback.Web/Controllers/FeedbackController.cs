using Microsoft.AspNetCore.Mvc;
using MediatR;
using SFA.DAS.ProviderFeedback.Web.Infrastructure;
using SFA.DAS.ProviderFeedback.Web.ViewModels;
using System.Text.Json;
using SFA.DAS.ProviderFeedback.Application.Queries.GetProviderFeedback;
using SFA.DAS.ProviderFeedback.Domain.Configuration;
using Microsoft.Extensions.Options;


namespace SFA.DAS.ProviderFeedback.Web.Controllers;

public class FeedbackController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<FeedbackController> _logger;
    private readonly IOptions<ProviderFeedbackWeb> _config;

    public FeedbackController(IMediator mediator,
        ILogger<FeedbackController> logger,
        IOptions<ProviderFeedbackWeb> config
    )
    {
        _mediator = mediator;
        _logger = logger;
        _config = config;
    }

    [HttpGet]
    [Route("{providerId}", Name = RouteNames.ServiceStartDefault)]
    public async Task<IActionResult> Index(int providerId)
    {
        ProviderFeedbackViewModel model = new ProviderFeedbackViewModel(

            await _mediator.Send(new GetProviderFeedbackQuery
            {
                ProviderId = providerId
            })
        );

        model.ShowReviewNotice = _config.Value.ShowReviewNotice;
        model.ReviewNoticeDate = _config.Value.ReviewNoticeDate;

        return View(model);
    }
}

