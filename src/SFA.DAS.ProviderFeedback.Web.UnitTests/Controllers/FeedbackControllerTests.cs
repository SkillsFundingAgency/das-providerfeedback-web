using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderFeedback.Web.Controllers;
using SFA.DAS.ProviderFeedback.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderFeedbackWeb.UnitTests.Controllers
{
    [TestFixture] 
    public class FeedbackControllerTests
    {
        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public async Task Index_ReturnsCorrectViewModel()
        {
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<FeedbackController>>();

            var controller = new FeedbackController(mediatorMock.Object, loggerMock.Object);

            var result =
                await controller.Index(1234) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Model, Is.InstanceOf<ProviderFeedbackViewModel>());
            var viewModel = (ProviderFeedbackViewModel)result.Model;
            
            //Assert.That(viewModel.ProviderId, Is.EqualTo(providerId));
        }
    }
}
