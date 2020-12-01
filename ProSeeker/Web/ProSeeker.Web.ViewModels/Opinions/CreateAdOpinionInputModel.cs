namespace ProSeeker.Web.ViewModels.Opinions
{
    public class CreateAdOpinionInputModel
    {
        public string AdId { get; set; }

        public int ParentId { get; set; }

        public string Content { get; set; }
    }
}
