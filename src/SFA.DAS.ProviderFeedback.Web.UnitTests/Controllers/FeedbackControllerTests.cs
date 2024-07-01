using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderFeedback.Web.Controllers;
using SFA.DAS.ProviderFeedback.Web.Infrastructure.Authorization;
using SFA.DAS.ProviderFeedback.Web.ViewModels;
using System.Security.Claims;
using SFA.DAS.ProviderFeedback.Application.Queries.GetProviderFeedbackAnnual;
using SFA.DAS.ProviderFeedback.Domain.GetProviderFeedbackAnnual;

namespace SFA.DAS.ProviderFeedbackWeb.UnitTests.Controllers
{
    [TestFixture]
    public class FeedbackControllerTests
    {
        private Mock<IOptions<SFA.DAS.ProviderFeedback.Domain.Configuration.ProviderFeedbackWeb>> _configMock;

        [SetUp]
        public void SetUp()
        {
            var config = new SFA.DAS.ProviderFeedback.Domain.Configuration.ProviderFeedbackWeb
            {
                ShowReviewNotice = true,
                ReviewNoticeDate = "Any text goes here"
            };
            _configMock = new Mock<IOptions<SFA.DAS.ProviderFeedback.Domain.Configuration.ProviderFeedbackWeb>>();
            _configMock.Setup(ap => ap.Value).Returns(config);
        }

        [Test]
        public async Task Index_ReturnsCorrectViewModel()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<FeedbackController>>();

            var providerId = 1234;

            var queryResult = new GetProviderFeedbackAnnualResult
            {
                Ukprn = providerId,
                ApprenticeFeedback = new FeedbackAnnual.ApprenticeFeedbackAnnual()
                {
                    AnnualApprenticeFeedbackDetails = new List<FeedbackAnnual.ApprenticeFeedbackAnnualSummary>()
                },
                EmployerFeedback = new FeedbackAnnual.EmployerFeedbackAnnual()
                {
                    AnnualEmployerFeedbackDetails = new List<FeedbackAnnual.EmployerFeedbackAnnualSummary>()
                }
            };
            mediatorMock.Setup(x => x.Send(It.IsAny<GetProviderFeedbackAnnualQuery>(), default)).ReturnsAsync(queryResult);

            var controller = new FeedbackController(mediatorMock.Object, loggerMock.Object, _configMock.Object);

            var claim = new Claim(ProviderClaims.ProviderUkprn, providerId.ToString());
            var claimsPrinciple = new ClaimsPrincipal(new[] { new ClaimsIdentity(new[] { claim }) });
            controller.ControllerContext = new ControllerContext() { HttpContext = new DefaultHttpContext() { User = claimsPrinciple } };

            var result =
                await controller.Index() as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Model, Is.InstanceOf<ProviderFeedbackAnnualViewModel>());
            var viewModel = (ProviderFeedbackAnnualViewModel)result.Model;

            Assert.That(viewModel.UKPRN, Is.EqualTo(providerId));
            Assert.That(viewModel.ShowReviewNotice, Is.EqualTo(true));
            Assert.That(viewModel.ReviewNoticeDate, Is.EqualTo(_configMock.Object.Value.ReviewNoticeDate));
        }
    }
}
