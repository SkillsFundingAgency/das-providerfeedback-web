using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderFeedback.Application.Queries.GetProviderFeedbackAnnual;
using SFA.DAS.ProviderFeedback.Domain.ApiResponse;
using SFA.DAS.ProviderFeedback.Domain.GetProviderFeedbackAnnual;
using SFA.DAS.ProviderFeedback.Domain.Interfaces;
using System.Net;
using SFA.DAS.ProviderFeedback.Domain.Exceptions;
using static SFA.DAS.ProviderFeedback.Domain.GetProviderFeedbackAnnual.FeedbackAnnual;

namespace SFA.DAS.ProviderFeedback.Application.UnitTests.Commands
{
    [TestFixture]
    public class GetProviderFeedbackAnnualHandlerTest
    {
        private Mock<IApiClient> _mockApiClient;
        private GetProviderFeedbackAnnualQueryHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mockApiClient = new Mock<IApiClient>();
            _handler = new GetProviderFeedbackAnnualQueryHandler(_mockApiClient.Object);
        }

        [Test]
        public async Task Handle_ShouldReturnFeedbackResult_WhenApiClientReturnsSuccessfulResponse()
        {
            var providerId = 123456;
            var command = new GetProviderFeedbackAnnualQuery { ProviderId = providerId };

            var expectedResponse = CreateMockGetProviderFeedbackAnnualResponse(123456);

            _mockApiClient
                .Setup(api => api.Get<GetProviderFeedbackAnnualResponse>(It.IsAny<GetProviderFeedbackAnnualRequest>()))
                .ReturnsAsync(new ApiResponse<GetProviderFeedbackAnnualResponse>(expectedResponse, HttpStatusCode.OK, string.Empty));

            var result = await _handler.Handle(command, CancellationToken.None);

            _mockApiClient.Verify(x => x.Get<GetProviderFeedbackAnnualResponse>(It.IsAny<GetProviderFeedbackAnnualRequest>()), Times.Once);
            Assert.AreEqual(expectedResponse.ProviderFeedback.ApprenticeFeedback, result.ApprenticeFeedback);
            Assert.AreEqual(expectedResponse.ProviderFeedback.EmployerFeedback, result.EmployerFeedback);
            Assert.AreEqual(expectedResponse.ProviderFeedback.IsEmployerProvider, result.IsEmployerProvider);
            Assert.AreEqual(expectedResponse.ProviderId, result.Ukprn);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenApiClientReturnsErrorResponse()
        {
            var providerId = 123456;
            var command = new GetProviderFeedbackAnnualQuery { ProviderId = providerId };

            _mockApiClient
                .Setup(api => api.Get<GetProviderFeedbackAnnualResponse>(It.IsAny<GetProviderFeedbackAnnualRequest>()))
                .ReturnsAsync(new ApiResponse<GetProviderFeedbackAnnualResponse>(null, HttpStatusCode.BadRequest, "Error"));

            Assert.ThrowsAsync<ApiResponseException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public async Task Handle_ShouldCallApiClientWithCorrectParameters()
        {
            var providerId = 123456;
            var command = new GetProviderFeedbackAnnualQuery { ProviderId = providerId };

            var expectedResponse = CreateMockGetProviderFeedbackAnnualResponse(123456);

            _mockApiClient
                .Setup(api => api.Get<GetProviderFeedbackAnnualResponse>(It.IsAny<GetProviderFeedbackAnnualRequest>()))
                .ReturnsAsync(new ApiResponse<GetProviderFeedbackAnnualResponse>(expectedResponse, HttpStatusCode.OK, string.Empty));

            await _handler.Handle(command, CancellationToken.None);

            _mockApiClient.Verify(x => x.Get<GetProviderFeedbackAnnualResponse>(It.Is<GetProviderFeedbackAnnualRequest>(r => r.ProviderId == providerId)), Times.Once);
        }
        public GetProviderFeedbackAnnualResponse CreateMockGetProviderFeedbackAnnualResponse(int providerId)
        {
            return new GetProviderFeedbackAnnualResponse
            {
                ProviderId = providerId,
                ProviderFeedback = new ProviderFeedbackAnnualModel
                {
                    IsEmployerProvider = true,
                    ApprenticeFeedback = new ApprenticeFeedbackAnnual
                    {
                        AnnualApprenticeFeedbackDetails = new List<ApprenticeFeedbackAnnualSummary>
                {
                    new ApprenticeFeedbackAnnualSummary
                    {
                        TotalApprenticeResponses = 10,
                        TotalFeedbackRating = 3,
                        TimePeriod = "2023",
                        FeedbackAttributes = new List<ApprenticeFeedbackAnnualAttributeDetail>
                        {
                            new ApprenticeFeedbackAnnualAttributeDetail
                            {
                                Name = "Attribute 1",
                                Category = "Category A",
                                Agree = 6,
                                Disagree = 4,
                                TotalVotes = 10,
                                Rating = 3
                            },
                            new ApprenticeFeedbackAnnualAttributeDetail
                            {
                                Name = "Attribute 2",
                                Category = "Category B",
                                Agree = 7,
                                Disagree = 3,
                                TotalVotes = 10,
                                Rating = 4
                            }
                        }
                    },
                    new ApprenticeFeedbackAnnualSummary
                    {
                        TotalApprenticeResponses = 15,
                        TotalFeedbackRating = 4,
                        TimePeriod = "2022",
                        FeedbackAttributes = new List<ApprenticeFeedbackAnnualAttributeDetail>
                        {
                            new ApprenticeFeedbackAnnualAttributeDetail
                            {
                                Name = "Attribute 1",
                                Category = "Category A",
                                Agree = 10,
                                Disagree = 5,
                                TotalVotes = 15,
                                Rating = 4
                            },
                            new ApprenticeFeedbackAnnualAttributeDetail
                            {
                                Name = "Attribute 3",
                                Category = "Category C",
                                Agree = 9,
                                Disagree = 6,
                                TotalVotes = 15,
                                Rating = 3
                            }
                        }
                    }
                }
                    },
                    EmployerFeedback = new EmployerFeedbackAnnual
                    {
                        AnnualEmployerFeedbackDetails = new List<EmployerFeedbackAnnualSummary>
                {
                    new EmployerFeedbackAnnualSummary
                    {
                        TotalEmployerResponses = 8,
                        TotalFeedbackRating = 2,
                        TimePeriod = "2023",
                        FeedbackAttributes = new List<EmployerFeedbackAnnualAttributeDetail>
                        {
                            new EmployerFeedbackAnnualAttributeDetail
                            {
                                AttributeName = "Attribute A",
                                Strength = 3,
                                Weakness = 5,
                                TotalVotes = 8,
                                Rating = 2
                            },
                            new EmployerFeedbackAnnualAttributeDetail
                            {
                                AttributeName = "Attribute B",
                                Strength = 2,
                                Weakness = 6,
                                TotalVotes = 8,
                                Rating = 1
                            }
                        }
                    },
                    new EmployerFeedbackAnnualSummary
                    {
                        TotalEmployerResponses = 12,
                        TotalFeedbackRating = 3,
                        TimePeriod = "2022",
                        FeedbackAttributes = new List<EmployerFeedbackAnnualAttributeDetail>
                        {
                            new EmployerFeedbackAnnualAttributeDetail
                            {
                                AttributeName = "Attribute A",
                                Strength = 5,
                                Weakness = 7,
                                TotalVotes = 12,
                                Rating = 3
                            },
                            new EmployerFeedbackAnnualAttributeDetail
                            {
                                AttributeName = "Attribute C",
                                Strength = 6,
                                Weakness = 6,
                                TotalVotes = 12,
                                Rating = 3
                            }
                        }
                    }
                }
                    }
                }
            };
        }

    }
}
