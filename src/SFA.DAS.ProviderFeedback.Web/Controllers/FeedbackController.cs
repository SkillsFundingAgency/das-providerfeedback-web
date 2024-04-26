using Microsoft.AspNetCore.Mvc;
using MediatR;
using SFA.DAS.ProviderFeedback.Web.Infrastructure;
using SFA.DAS.ProviderFeedback.Web.ViewModels;
using SFA.DAS.ProviderFeedback.Application.Queries.GetProviderFeedback;
using SFA.DAS.ProviderFeedback.Domain.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using SFA.DAS.ProviderFeedback.Web.Infrastructure.Authorization;

namespace SFA.DAS.ProviderFeedback.Web.Controllers;
[Authorize(Policy = nameof(PolicyNames.HasProviderAccount))]
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
    [Route("", Name = RouteNames.ServiceStartDefault)]
    public async Task<IActionResult> Index()
    {
        var ukprn = HttpContext.User.FindFirst(c => c.Type.Equals(ProviderClaims.ProviderUkprn)).Value;

        ProviderFeedbackViewModel model = new ProviderFeedbackViewModel(

            await _mediator.Send(new GetProviderFeedbackQuery
            {
                ProviderId = int.Parse(ukprn)
            })
        );

        model.ShowReviewNotice = _config.Value.ShowReviewNotice;
        model.ReviewNoticeDate = _config.Value.ReviewNoticeDate;

        return View(model);
    }

}

