using System.ComponentModel;

namespace SFA.DAS.ProviderFeedback.Domain.GetProviderFeedbackAnnual
{
    public class FeedbackAnnual
    {
        public enum ProviderRating
        {
            [Description("Not yet reviewed")]
            NotYetReviewed = 0,
            [Description("Very poor")]
            VeryPoor = 1,
            [Description("Poor")]
            Poor = 2,
            [Description("Good")]
            Good = 3,
            [Description("Excellent")]
            Excellent = 4
        }

        public class ProviderFeedbackAnnualModel
        {
            public int ProviderId { get; set; }
            public bool IsEmployerProvider { get; set; }
            public EmployerFeedbackAnnual EmployerFeedback { get; set; }
            public ApprenticeFeedbackAnnual ApprenticeFeedback { get; set; }
        }
        
        public class EmployerFeedbackAnnual
        {
            public List<EmployerFeedbackAnnualSummary> AnnualEmployerFeedbackDetails { get; set; }
        }

        public class EmployerFeedbackAnnualSummary
        {
            public int TotalEmployerResponses { get; set; }
            public int TotalFeedbackRating { get; set; }
            public string TimePeriod { get; set; }
            public List<EmployerFeedbackAnnualAttributeDetail> FeedbackAttributes { get; set; }
        }

        public class ApprenticeFeedbackAnnual
        {
            public IEnumerable<ApprenticeFeedbackAnnualSummary> AnnualApprenticeFeedbackDetails { get; set; }
        }

        public class ApprenticeFeedbackAnnualSummary
        {
            public int TotalApprenticeResponses { get; set; }
            public int TotalFeedbackRating { get; set; }
            public string TimePeriod { get; set; }
            public List<ApprenticeFeedbackAnnualAttributeDetail> FeedbackAttributes { get; set; }
        }

        public class EmployerFeedbackAnnualAttributeDetail
        {
            public string AttributeName { get; set; }
            public int Strength { get; set; }
            public int Weakness { get; set; }
            public int TotalVotes { get; set; }
            public int Rating { get; set; }
        }

        public class ApprenticeFeedbackAnnualAttributeDetail
        {
            public string Name { get; set; }
            public string Category { get; set; }
            public int Agree { get; set; }
            public int Disagree { get; set; }
            public int TotalVotes { get; set; }
            public int Rating { get; set; }
        }
    }
}
