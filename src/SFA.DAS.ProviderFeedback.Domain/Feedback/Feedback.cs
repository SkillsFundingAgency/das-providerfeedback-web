using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderFeedback.Domain.Feedback
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

        public class EmployerFeedback
        {
            public int ReviewCount { get; set; }
            public int Stars { get; set; }
            public List<EmployerFeedbackAttributeDetail> ProviderAttribute { get; set; }
        }

        public class ApprenticeFeedback
        {
            public int ReviewCount { get; set; }
            public int Stars { get; set; }
            public List<ApprenticeFeedbackAttributeDetail> ProviderAttribute { get; set; }
        }

        public class EmployerFeedbackAttributeDetail
        {
            public string Name { get; set; }
            public int Strength { get; set; }
            public int Weakness { get; set; }
        }

        public class ApprenticeFeedbackAttributeDetail
        {
            public string Name { get; set; }
            public string Category { get; set; }
            public int Agree { get; set; }
            public int Disagree { get; set; }
        }
    }
}
