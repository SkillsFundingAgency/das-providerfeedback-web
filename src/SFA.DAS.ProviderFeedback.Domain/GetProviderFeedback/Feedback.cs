using System.ComponentModel;

namespace SFA.DAS.ProviderFeedback.Domain.GetProviderFeedback
{
    public class Feedback
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

        public class ProviderFeedbackModel
        {
            public int ProviderId { get; set; }
            public EmployerFeedback EmployerFeedback { get; set; }
            public ApprenticeFeedback ApprenticeFeedback { get; set; }
        }
        
        public class EmployerFeedback
        {
            public int TotalEmployerResponses { get; set; }
            public int TotalFeedbackRating { get; set; }
            public List<EmployerFeedbackAttributeDetail> FeedbackAttributes { get; set; }
        }

        public class ApprenticeFeedback
        {
            public int TotalApprenticeResponses { get; set; }
            public int TotalFeedbackRating { get; set; }
            public List<ApprenticeFeedbackAttributeDetail> FeedbackAttributes { get; set; }
        }

        public class EmployerFeedbackAttributeDetail
        {
            public string AttributeName { get; set; }
            public int Strength { get; set; }
            public int Weakness { get; set; }
            public int TotalVotes { get; set; }
            public int Rating { get; set; }
        }

        public class ApprenticeFeedbackAttributeDetail
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
