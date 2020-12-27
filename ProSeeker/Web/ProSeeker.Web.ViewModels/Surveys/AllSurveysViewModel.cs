namespace ProSeeker.Web.ViewModels.Surveys
{
    using System.Collections.Generic;

    using ProSeeker.Web.ViewModels.Quizzes;

    public class AllSurveysViewModel
    {
        public IEnumerable<SurveyViewModel> Surveys { get; set; }
    }
}
