using static SFA.DAS.ProviderFeedback.Domain.GetProviderFeedbackAnnual.FeedbackAnnual;
using ProviderRating = SFA.DAS.ProviderFeedback.Domain.GetProviderFeedbackAnnual.FeedbackAnnual.ProviderRating;

namespace SFA.DAS.ProviderFeedback.Web.ViewModels
{
    public class EmployerFeedbackAnnualViewModel
    {
        public List<EmployerFeedbackAnnualSummary> AnnualApprenticeFeedbackDetails { get; set; }

        public EmployerFeedbackAnnualViewModel(EmployerFeedbackAnnual employerFeedback)
        {
            if (employerFeedback == null || !employerFeedback.AnnualEmployerFeedbackDetails.Any())
            {
                return;
            }

            AnnualApprenticeFeedbackDetails = employerFeedback.AnnualEmployerFeedbackDetails
                .Select(summary => new EmployerFeedbackAnnualSummary
                {
                    TotalFeedbackRating = summary.TotalFeedbackRating,
                    TimePeriod = summary.TimePeriod,
                    TimePeriodDisplay = FormatTimePeriod(summary.TimePeriod),
                    TotalFeedbackResponses = summary.TotalEmployerResponses,
                    TotalFeedbackRatingText = GetFeedbackRatingText(false, summary.TotalEmployerResponses),
                    TotalFeedbackRatingTextProviderDetail = GetFeedbackRatingText(true, summary.TotalEmployerResponses),
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
                    return !isProviderDetail ? "Not yet reviewed (employer reviews)" : "Not yet reviewed";
                case 1:
                    return !isProviderDetail ? "(1 employer review)" : "(1 review)";
            }

            var returnText = TotalFeedbackResponses > 50 && !isProviderDetail ? "(50+ employer reviews)"
                : $"({TotalFeedbackResponses} employer reviews)";

            return isProviderDetail ? returnText.Replace("employer ", "") : returnText;
        }

        private List<EmployerFeedbackAnnualDetailViewModel> GenerateAttributeSummary(List<EmployerFeedbackAnnualAttributeDetail> source)
        {
            List<EmployerFeedbackAnnualDetailViewModel> AttributeSummary = new List<EmployerFeedbackAnnualDetailViewModel>();

            foreach (var entry in source)
            {
                int totalCount = entry.Strength + entry.Weakness;

                AttributeSummary.Add(
                    new EmployerFeedbackAnnualDetailViewModel
                    {
                        AttributeName = entry.AttributeName,
                        StrengthCount = entry.Strength,
                        WeaknessCount = entry.Weakness,
                        TotalCount = totalCount,
                        StrengthPerc = Math.Round((double)entry.Strength / totalCount * 100, 0),
                        WeaknessPerc = Math.Round((double)entry.Weakness / totalCount * 100, 0)
                    });
            }

            return AttributeSummary.OrderBy(o => o.AttributeName).ToList();
        }

        public class EmployerFeedbackAnnualSummary
        {
            public int TotalFeedbackResponses { get; set; }
            public int TotalFeedbackRating { get; set; }
            public string TotalFeedbackRatingText { get; set; }
            public string TotalFeedbackRatingTextProviderDetail { get; set; }
            public ProviderRating TotalFeedbackText { get; set; }
            public string TimePeriod { get; set; }
            public string TimePeriodDisplay { get; set; }
            public List<EmployerFeedbackAnnualDetailViewModel> FeedbackAttributeSummary { get; set; }
        }

        public class EmployerFeedbackAnnualDetailViewModel
        {
            public string AttributeName { get; set; }
            public int StrengthCount { get; set; }
            public int WeaknessCount { get; set; }
            public int TotalCount { get; set; }
            public double StrengthPerc { get; set; }
            public double WeaknessPerc { get; set; }
        }

        public class EmployerFeedBackAnnualDetail
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


