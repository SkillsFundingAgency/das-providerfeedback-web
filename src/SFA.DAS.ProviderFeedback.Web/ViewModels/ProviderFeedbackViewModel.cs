

using SFA.DAS.ProviderFeedback.Application.Queries.GetProviderFeedback;

namespace SFA.DAS.ProviderFeedback.Web.ViewModels
{
    public class ProviderFeedbackViewModel
    {
        public bool ShowReviewNotice { get; set; }
        public string ReviewNoticeDate { get; set; }
        public int UKPRN { get; set; }
        public EmployerFeedbackViewModel EmployerFeedback { get; set; }
        public ApprenticeFeedbackViewModel ApprenticeFeedback { get; set; }

        public ProviderFeedbackViewModel(GetProviderFeedbackResult feedback)
        { 
            UKPRN = feedback.Ukprn;
            EmployerFeedback = new EmployerFeedbackViewModel( feedback.EmployerFeedback);
            ApprenticeFeedback = new ApprenticeFeedbackViewModel(feedback.ApprenticeFeedback);
        }
    }
}
