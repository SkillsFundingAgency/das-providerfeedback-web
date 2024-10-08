using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SFA.DAS.ProviderFeedback.Application.Queries.GetProviderFeedbackAnnual;
using SFA.DAS.ProviderFeedback.Domain.Configuration;
using SFA.DAS.ProviderFeedback.Web.Infrastructure;
using SFA.DAS.ProviderFeedback.Web.Infrastructure.Authorization;
using SFA.DAS.ProviderFeedback.Web.ViewModels;

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

        ProviderFeedbackAnnualViewModel model = new ProviderFeedbackAnnualViewModel(

            await _mediator.Send(new GetProviderFeedbackAnnualQuery
            {
                ProviderId = int.Parse(ukprn)
            })
        );

        model.ShowReviewNotice = _config.Value.ShowReviewNotice;
        model.ReviewNoticeDate = _config.Value.ReviewNoticeDate;

        return View(model);
    }

}

