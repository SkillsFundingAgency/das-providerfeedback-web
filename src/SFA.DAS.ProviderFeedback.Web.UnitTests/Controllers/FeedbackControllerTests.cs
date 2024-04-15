using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderFeedback.Web.Controllers;
using SFA.DAS.ProviderFeedback.Web.ViewModels;
using SFA.DAS.ProviderFeedback.Application.Queries.GetProviderFeedback;
using static SFA.DAS.ProviderFeedback.Domain.GetProviderFeedback.Feedback;

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

            var queryResult = new GetProviderFeedbackResult 
            { 
                Ukprn = providerId, 
                ApprenticeFeedback = new ApprenticeFeedback()
                { 
                    FeedbackAttributes = new List<ApprenticeFeedbackAttributeDetail>()
                }, 
                EmployerFeedback = new EmployerFeedback()
                { 
                    FeedbackAttributes = new List<EmployerFeedbackAttributeDetail>()
                }
            };
            mediatorMock.Setup(x => x.Send(It.IsAny<GetProviderFeedbackQuery>(), default)).ReturnsAsync(queryResult);

            var controller = new FeedbackController(mediatorMock.Object, loggerMock.Object, _configMock.Object);

            
            var result =
                await controller.Index(providerId) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Model, Is.InstanceOf<ProviderFeedbackViewModel>());
            var viewModel = (ProviderFeedbackViewModel)result.Model;
            
            Assert.That(viewModel.UKPRN, Is.EqualTo(providerId));
            Assert.That(viewModel.ShowReviewNotice, Is.EqualTo(true));
            Assert.That(viewModel.ReviewNoticeDate, Is.EqualTo(_configMock.Object.Value.ReviewNoticeDate));
        }
    }
}
