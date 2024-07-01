using SFA.DAS.ProviderFeedback.Application.Queries.GetProviderFeedbackAnnual;

namespace SFA.DAS.ProviderFeedback.Web.ViewModels
{
    public class ProviderFeedbackAnnualViewModel
    {
        public bool ShowReviewNotice { get; set; }
        public string ReviewNoticeDate { get; set; }
        public int UKPRN { get; set; }
        public EmployerFeedbackAnnualViewModel EmployerFeedback { get; set; }
        public ApprenticeFeedbackAnnualViewModel ApprenticeFeedback { get; set; }

        public ProviderFeedbackAnnualViewModel(GetProviderFeedbackAnnualResult feedback)
        { 
            UKPRN = feedback.Ukprn;
            EmployerFeedback = new EmployerFeedbackAnnualViewModel( feedback.EmployerFeedback);
            ApprenticeFeedback = new ApprenticeFeedbackAnnualViewModel(feedback.ApprenticeFeedback);
        }
    }
}
