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

            //// Include AY2324 if current date is on or before 31 July
            //if (DateTime.Now <= new DateTime(currentYear + 1, 7, 31))
            //{
            //    expectedPeriods.Add($"AY{(currentYear + 1).ToString().Substring(2)}{(currentYear + 2).ToString().Substring(2)}");
            //}

            // Include AY2425 if current date is on or after 1 August
            if (DateTime.Now >= new DateTime(currentYear + 1, 8, 1))
            {
                expectedPeriods.Add($"AY{(currentYear + 2).ToString().Substring(2)}{(currentYear + 3).ToString().Substring(2)}");
            }

            // Check for missing periods
            var existingPeriods = employerFeedback.AnnualEmployerFeedbackDetails
                .Select(x => x.TimePeriod)
                .ToHashSet();

            var missingPeriods = expectedPeriods
                .Where(period => !existingPeriods.Contains(period))
                .ToList();


            AnnualApprenticeFeedbackDetails = employerFeedback.AnnualEmployerFeedbackDetails
                .Select(summary => new EmployerFeedbackAnnualSummary
                {
                    TotalFeedbackRating = summary.TotalFeedbackRating,
                    TimePeriod = summary.TimePeriod,
                    DisplayYear = FormatDisplayYear(summary.TimePeriod),
                    DisplayPeriod = FormatDisplayPeriod(summary.TimePeriod),
                    TotalFeedbackResponses = summary.TotalEmployerResponses,
                    TotalFeedbackRatingText = GetFeedbackRatingText(false, summary.TotalEmployerResponses),
                    TotalFeedbackRatingTextProviderDetail = GetFeedbackRatingText(true, summary.TotalEmployerResponses),
                    TotalFeedbackText = (ProviderRating)summary.TotalFeedbackRating,
                    FeedbackAttributeSummary = GenerateAttributeSummary(summary.FeedbackAttributes).ToList()
                })
                .ToList();

            // Add default records for missing periods
            foreach (var period in missingPeriods)
            {
                AnnualApprenticeFeedbackDetails.Add(new EmployerFeedbackAnnualSummary
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
            // Handle 'All' period separately
            if (timePeriod == "All")
            {
                return int.MinValue;
            }

            // Parse the numeric part of the time period for sorting
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
            if (timePeriod.Length == 6 && timePeriod.StartsWith("AY"))
            {
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
            public string DisplayYear { get; set; }
            public string DisplayPeriod { get; set; }
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


