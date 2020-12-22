namespace ProSeeker.Common
{
    public static class GlobalConstants
    {
        public const string SystemName = "ProSeeker";

        public const string AdministratorRoleName = "Administrator";

        public const string SpecialistRoleName = "Specialist";

        public const string RegularUserRoleName = "User";

        public const string DefaultProfileImagePath = "/images/ProfileImage/defaultUser.png";

        public const string ErrorAccessDenied = "403";

        public const string ErrorNotFound = "404";

        public const string ErrorHandlerAction = "Error";

        public const string SendMessagePrivateChatMethod = "SendMessage";

        public const string HomePageRedirect = "/";

        // Paging
        public const int ItemsPerPage = 8;

        public const int SpecialistsPerPage = 12;

        // Ad Sorting options
        public const string ByDateDescending = "CreatedOn";

        public const string ByUpVotesDescending = "UpVotesCount";

        public const string ByDownVotesDescending = "DownVotesCount";

        public const string ByOpinionsDescending = "OpinionsCount";

        public const string ByRatingDesc = "AverageRating";
    }
}
