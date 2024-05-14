using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderFeedback.Application.Queries.GetProviderFeedback;
using SFA.DAS.ProviderFeedback.Domain.ApiResponse;
using SFA.DAS.ProviderFeedback.Domain.GetProviderFeedback;
using SFA.DAS.ProviderFeedback.Domain.Interfaces;
using System.Net;

namespace SFA.DAS.ProviderFeedback.Application.UnitTests.Commands
{
    [TestFixture]
    public class GetProviderFeedbackHandlerTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task GetProviderFeedbackHandler()
        {
            // Arrange
            var mockApiClient = new Mock<IApiClient>();
            var command = new GetProviderFeedbackQuery();
            var handler = new GetProviderFeedbackQueryHandler(mockApiClient.Object);

            var request = new GetProviderFeedbackRequest(123456789);

            var expectedResponse = new GetProviderFeedbackResponse
            {
                ProviderFeedback = new Feedback.ProviderFeedbackModel
                {
                    ProviderId = request.ProviderId,
                    EmployerFeedback = new Feedback.EmployerFeedback()
                    {
                        TotalEmployerResponses = 2,
                        TotalFeedbackRating = 4,
                        FeedbackAttributes = new List<Feedback.EmployerFeedbackAttributeDetail>()
                        {
                            new Feedback.EmployerFeedbackAttributeDetail
                            {
                                AttributeName = "Attribute One",
                                Rating = 5,
                                Strength = 2,
                                TotalVotes = 2,
                                Weakness = 0,
                            },
                            new Feedback.EmployerFeedbackAttributeDetail
                            {
                                AttributeName = "Attribute Two",
                                Rating = 3,
                                Strength = 2,
                                TotalVotes = 2,
                                Weakness = 0,
                            }
                        }
                    },
                    ApprenticeFeedback = new Feedback.ApprenticeFeedback
                    {
                        TotalApprenticeResponses = 5,
                        TotalFeedbackRating = 3,
                        FeedbackAttributes = new List<Feedback.ApprenticeFeedbackAttributeDetail>
                        {
                            new Feedback.ApprenticeFeedbackAttributeDetail
                            {
                                Name = "Apprentice Attribute 1",
                                Agree = 4,
                                Disagree =1,
                                Category = "Category A",
                                Rating = 3,
                                TotalVotes = 5
                            },
                            new Feedback.ApprenticeFeedbackAttributeDetail
                            {
                                Name = "Apprentice Attribute 2",
                                Agree = 3,
                                Disagree =2,
                                Category = "Category B",
                                Rating = 3,
                                TotalVotes = 5
                            },

                        }
                    }
                }
            };

            mockApiClient
                .Setup(api => api.Get<GetProviderFeedbackResponse>(It.IsAny<GetProviderFeedbackRequest>()))
                .ReturnsAsync(new ApiResponse<GetProviderFeedbackResponse>(expectedResponse, HttpStatusCode.OK, string.Empty));

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert

            mockApiClient.Verify(x => x.Get<GetProviderFeedbackResponse>(It.IsAny<GetProviderFeedbackRequest>()), Times.Once);
            Assert.AreEqual(expectedResponse.ProviderFeedback.ApprenticeFeedback.TotalApprenticeResponses, result.ApprenticeFeedback.TotalApprenticeResponses);
            Assert.AreEqual(expectedResponse.ProviderFeedback.ApprenticeFeedback.TotalFeedbackRating, result.ApprenticeFeedback.TotalFeedbackRating);
            Assert.AreEqual(expectedResponse.ProviderFeedback.ApprenticeFeedback.FeedbackAttributes.Count, result.ApprenticeFeedback.FeedbackAttributes.Count);
            Assert.AreEqual(expectedResponse.ProviderFeedback.EmployerFeedback.TotalEmployerResponses, result.EmployerFeedback.TotalEmployerResponses);
            Assert.AreEqual(expectedResponse.ProviderFeedback.EmployerFeedback.TotalFeedbackRating, result.EmployerFeedback.TotalFeedbackRating);
            Assert.AreEqual(expectedResponse.ProviderFeedback.EmployerFeedback.FeedbackAttributes.Count, result.EmployerFeedback.FeedbackAttributes.Count);
        }
    }
}
