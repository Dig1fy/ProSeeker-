namespace ProSeeker.Common
{
    public static class GlobalConstants
    {
        public const string SystemName = "ProSeeker";

        public const string AdministratorRoleName = "Administrator";

        public const string SpecialistRoleName = "Specialist";

        public const string RegularUserRoleName = "User";

        public const string DefaultProfileImagePath = "/images/ProfileImage/defaultUser.png";

        public const string DefaultCategoryPictureUrl = "/images/defaultCategory.png";

        public const string ErrorAccessDenied = "403";

        public const string ErrorNotFound = "404";

        public const string ErrorHandlerAction = "Error";

        public const string SendMessagePrivateChatMethod = "SendMessage";

        public const string HomePageRedirect = "/";

        public const string InvalidProfilePictureMessage = "Невалиден формат на снимка. Поддържаме jpg, jpeg и png файлове.";

        public const string AcceptedOfferSubject = "Приета оферта";

        public const string ApplicationName = "ProSeeker";

        public const string ApplicationEmail = "q2kforeveralon3@gmail.com";

        // Paging
        public const int ItemsPerPage = 8;

        public const int SpecialistsPerPage = 4;

        // Ad Sorting options
        public const string ByDateDescending = "CreatedOn";

        public const string ByUpVotesDescending = "UpVotesCount";

        public const string ByDownVotesDescending = "DownVotesCount";

        public const string ByOpinionsDescending = "OpinionsCount";

        public const string ByRatingDesc = "AverageRating";
    }
}
