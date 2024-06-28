using static SFA.DAS.ProviderFeedback.Domain.GetProviderFeedbackAnnual.FeedbackAnnual;
using ProviderRating = SFA.DAS.ProviderFeedback.Domain.GetProviderFeedbackAnnual.FeedbackAnnual.ProviderRating;

namespace SFA.DAS.ProviderFeedback.Web.ViewModels
{
    public class ApprenticeFeedbackAnnualViewModel
    {
        public List<ApprenticeFeedbackAnnualSummary> AnnualApprenticeFeedbackDetails { get; set; }
        public ApprenticeFeedbackAnnualViewModel(ApprenticeFeedbackAnnual apprenticeFeedback)
        {
            if (apprenticeFeedback == null || !apprenticeFeedback.AnnualApprenticeFeedbackDetails.Any())
            {
                return;
            }


            AnnualApprenticeFeedbackDetails = apprenticeFeedback.AnnualApprenticeFeedbackDetails
                .Select(summary => new ApprenticeFeedbackAnnualSummary
                {
                    TotalFeedbackRating = summary.TotalFeedbackRating,
                    TotalFeedbackResponses = summary.TotalApprenticeResponses,
                    TimePeriod = summary.TimePeriod,
                    TimePeriodDisplay = FormatTimePeriod(summary.TimePeriod),
                    TotalFeedbackRatingText = GetFeedbackRatingText(false, summary.TotalApprenticeResponses),
                    TotalFeedbackRatingTextProviderDetail = GetFeedbackRatingText(true, summary.TotalApprenticeResponses),
                    TotalFeedbackText = (ProviderRating)summary.TotalFeedbackRating,
                    FeedbackAttributeSummary = GenerateAttributeSummary(summary.FeedbackAttributes).ToList()
                })
                .ToList();
        }

        private string FormatTimePeriod(string timePeriod)
        {
            if (timePeriod.Length == 6 && (timePeriod.StartsWith("AY")))
            {
                string startYear = "20" + timePeriod.Substring(2, 2);
                string endYear = "20" + timePeriod.Substring(4, 2);
                return $"{startYear} to {endYear}";
            }
            return timePeriod; 
        }

        private string GetFeedbackRatingText(bool isProviderDetail, int TotalFeedbackResponses)
        {
            switch (TotalFeedbackResponses)
            {
                case 0:
                    return !isProviderDetail ? "Not yet reviewed (apprentice reviews)" : "Not yet reviewed";
                case 1:
                    return !isProviderDetail ? "(1 apprentice review)" : "(1 review)";
            }

            var returnText = TotalFeedbackResponses > 50 && !isProviderDetail ? "(50+ apprentice reviews)"
                : $"({TotalFeedbackResponses} apprentice reviews)";

            return isProviderDetail ? returnText.Replace("apprentice ", "") : returnText;
        }

        private List<ApprenticeFeedbackAnnualDetailViewModel> GenerateAttributeSummary(List<ApprenticeFeedbackAnnualAttributeDetail> source)
        {
            var attributeSummary = new List<ApprenticeFeedbackAnnualDetailViewModel>();

            foreach (var entry in source)
            {
                int totalCount = entry.Agree + entry.Disagree;

                attributeSummary.Add(
                    new ApprenticeFeedbackAnnualDetailViewModel
                    {
                        Name = entry.Name,
                        Category = entry.Category,
                        AgreeCount = entry.Agree,
                        DisagreeCount = entry.Disagree,
                        TotalCount = totalCount,
                        AgreePerc = Math.Round((double)entry.Agree / totalCount * 100, 0),
                        DisagreePerc = Math.Round((double)entry.Disagree / totalCount * 100, 0)
                    });
            }

            return attributeSummary.OrderBy(o => o.Name).ToList();
        }

        public class ApprenticeFeedbackAnnualSummary
        {
            public int TotalFeedbackResponses { get; set; }
            public int TotalFeedbackRating { get; set; }
            public string TotalFeedbackRatingText { get; set; }
            public string TotalFeedbackRatingTextProviderDetail { get; set; }
            public ProviderRating TotalFeedbackText { get; set; }
            public string TimePeriod { get; set; }
            public string TimePeriodDisplay { get; set; }
            public List<ApprenticeFeedbackAnnualDetailViewModel> FeedbackAttributeSummary { get; set; }
        }

        public class ApprenticeFeedbackAnnualDetailViewModel
        {
            public string Name { get; set; }
            public string Category { get; set; }
            public int AgreeCount { get; set; }
            public int DisagreeCount { get; set; }
            public int TotalCount { get; set; }
            public double AgreePerc { get; set; }
            public double DisagreePerc { get; set; }
        }

        public class ApprenticeFeedbackAnnualDetail
        {
            public ProviderRating Rating { get; set; }
            public decimal RatingPercentage { get; set; }
            public int RatingCount { get; set; }
            public string RatingText => GetRatingText();

            private string GetRatingText()
            {
                return RatingCount == 1 ? "1 review" : $"{RatingCount} reviews";
            }
        }
    }
}
