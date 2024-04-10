using Microsoft.AspNetCore.Mvc;
using MediatR;
using SFA.DAS.ProviderFeedback.Web.Infrastructure;

namespace SFA.DAS.ProviderFeedback.Web.Controllers;

public class Dummy : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<Dummy> _logger;

    public Dummy(IMediator mediator,
        ILogger<Dummy> logger
    )
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    [Route("dummy")]
    public IActionResult Index()
    {
        return View();
    }
}

