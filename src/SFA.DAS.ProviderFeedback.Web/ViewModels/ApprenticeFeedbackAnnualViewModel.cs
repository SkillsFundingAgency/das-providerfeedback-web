using static SFA.DAS.ProviderFeedback.Domain.GetProviderFeedbackAnnual.FeedbackAnnual;
using ProviderRating = SFA.DAS.ProviderFeedback.Domain.GetProviderFeedbackAnnual.FeedbackAnnual.ProviderRating;

namespace SFA.DAS.ProviderFeedback.Web.ViewModels
{
    public class ApprenticeFeedbackAnnualViewModel
    {
        public string NoFeedBackText { get; set; }
        public List<ApprenticeFeedbackAnnualSummary> AnnualApprenticeFeedbackDetails { get; set; }
        public ApprenticeFeedbackAnnualViewModel(ApprenticeFeedbackAnnual apprenticeFeedback)
        {
            NoFeedBackText = GetFeedbackRatingText(true, 0);

            // Determine the current academic year
            int currentYear = DateTime.Now.Year;
            if (DateTime.Now.Month < 8) // Academic year starts in August
            {
                currentYear--;
            }

            // Generate the list of expected TimePeriod values for the last five years including special cases
            var expectedPeriods = new List<string>();
            for (int i = 0; i < 5; i++)
            {
                expectedPeriods.Add($"AY{(currentYear - i).ToString().Substring(2)}{(currentYear - i + 1).ToString().Substring(2)}");
            }

            // Include AY2425 if current date is on or after 1 August
            if (DateTime.Now >= new DateTime(currentYear + 1, 8, 1))
            {
                expectedPeriods.Add($"AY{(currentYear + 2).ToString().Substring(2)}{(currentYear + 3).ToString().Substring(2)}");
            }

            if (apprenticeFeedback == null || !apprenticeFeedback.AnnualApprenticeFeedbackDetails.Any())
            {
                if (AnnualApprenticeFeedbackDetails == null)
                {
                    AnnualApprenticeFeedbackDetails = new List<ApprenticeFeedbackAnnualSummary>();
                }

                foreach (var period in expectedPeriods)
                {
                    AnnualApprenticeFeedbackDetails.Add(new ApprenticeFeedbackAnnualSummary
                    {
                        TimePeriod = period,
                        TotalFeedbackRating = 0,
                        DisplayYear = FormatDisplayYear(period),
                        DisplayPeriod = FormatDisplayPeriod(period),
                    });
                }

                return;
            }

            // Check for missing periods
            var existingPeriods = apprenticeFeedback.AnnualApprenticeFeedbackDetails
                .Select(x => x.TimePeriod)
                .ToHashSet();

            var missingPeriods = expectedPeriods
                .Where(period => !existingPeriods.Contains(period))
                .ToList();

            AnnualApprenticeFeedbackDetails = apprenticeFeedback.AnnualApprenticeFeedbackDetails
                .Select(summary => new ApprenticeFeedbackAnnualSummary
                {
                    TotalFeedbackRating = summary.TotalFeedbackRating,
                    TotalFeedbackResponses = summary.TotalApprenticeResponses,
                    TimePeriod = summary.TimePeriod,
                    DisplayYear = FormatDisplayYear(summary.TimePeriod),
                    DisplayPeriod = FormatDisplayPeriod(summary.TimePeriod),
                    TotalFeedbackRatingText = GetFeedbackRatingText(false, summary.TotalApprenticeResponses),
                    TotalFeedbackRatingTextProviderDetail = GetFeedbackRatingText(true, summary.TotalApprenticeResponses),
                    TotalFeedbackText = (ProviderRating)summary.TotalFeedbackRating,
                    FeedbackAttributeSummary = GenerateAttributeSummary(summary.FeedbackAttributes).ToList()
                })
                .ToList();

            // Add default records for missing periods
            foreach (var period in missingPeriods)
            {
                AnnualApprenticeFeedbackDetails.Add(new ApprenticeFeedbackAnnualSummary
                {
                    TimePeriod = period,
                    TotalFeedbackRating = 0,
                    DisplayYear = FormatDisplayYear(period),
                    DisplayPeriod = FormatDisplayPeriod(period),
                });
            }

            AnnualApprenticeFeedbackDetails = AnnualApprenticeFeedbackDetails
                .OrderBy(x => x.TimePeriod != "All")
                .ThenByDescending(x => ParseTimePeriod(x.TimePeriod))
                .ToList();

        }

        private int ParseTimePeriod(string timePeriod)
        {
            if (timePeriod == "All")
            {
                return int.MinValue;
            }

            return int.Parse(timePeriod.Substring(2, 2) + timePeriod.Substring(4, 2));
        }


        private string FormatDisplayYear(string timePeriod)
        {
            if (timePeriod.Length == 6 && (timePeriod.StartsWith("AY")))
            {
                string startYear = "20" + timePeriod.Substring(2, 2);
                string endYear = "20" + timePeriod.Substring(4, 2);
                return $"{startYear} to {endYear}";
            }
            return timePeriod;
        }

        private string FormatDisplayPeriod(string timePeriod)
        {
            if (timePeriod.Length != 6 || !timePeriod.StartsWith("AY"))
            {
                return "Overall rating";
            }

            string startYear = "20" + timePeriod.Substring(2, 2);
            string endYear = "20" + timePeriod.Substring(4, 2);

            string startDate = $"1 August {startYear}";

            DateTime currentDate = DateTime.Now;

            string endDate;
            if (currentDate <= new DateTime(int.Parse(endYear), 7, 31))
            {
                endDate = currentDate.ToString("d MMMM yyyy");
            }
            else
            {
                endDate = $"31 July {endYear}";
            }

            return $"{startDate} to {endDate}";
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
            public string DisplayYear { get; set; }
            public string DisplayPeriod { get; set; }
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
    }
}
