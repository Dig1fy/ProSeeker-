namespace ProSeeker.Web.ViewModels.Opinions
{
    public class CreateSpecialistOpinionInputModel
    {
        public string SpecialistId { get; set; }

        public int ParentId { get; set; }

        public string Content { get; set; }
    }
}
