namespace SFA.DAS.ProviderFeedback.Application.Commands.Queries.GetProviderFeedback
{
    public static class EmployerFeedbackAttributes
    {
        public static string AdaptingToMyNeeds => "Adapting to my needs";
        public static string TrainingFacilities => "Training facilities";
        public static string ReportingOnApprentices => "Reporting on progress of apprentices";
        public static string Communication => "Communication with employers";
        public static string ProvidingTheRightTraining => "Providing the right training at the right time";
        public static string GettingApprenticesStarted => "Getting new apprentices started";
        public static string ImprovingApprenticeSkills => "Improving apprentice skills";
        public static string WorkingWithAapprentices => "Working with small numbers of apprentices";
        public static string InitialAssessment => "Initial assessment of apprentices";

        public static List<string> AllFeedbackAttributes => new List<string>
        {
            AdaptingToMyNeeds,
            Communication,
            ImprovingApprenticeSkills,
            InitialAssessment,
            GettingApprenticesStarted,
            ProvidingTheRightTraining,
            ReportingOnApprentices,
            TrainingFacilities,
            WorkingWithAapprentices
        };
    }
}
