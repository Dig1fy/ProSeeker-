﻿namespace ProSeeker.Common
{
    public static class GlobalConstants
    {
        public const string SystemName = "ProSeeker";

        public const string AdministratorRoleName = "Administrator";

        public const string SpecialistRoleName = "Specialist";

        public const string RegularUserRoleName = "User";

        public const string DefaultProfileImagePath = "/images/ProfileImage/defaultUser.png";

        // Paging
        public const int ItemsPerPage = 6;

        public const int SpecialistsPerPage = 3;

        // Ad Sorting options
        public const string ByDateDescending = "CreatedOn";

        public const string ByUpVotesDescending = "UpVotesCount";

        public const string ByDownVotesDescending = "DownVotesCount";

        public const string ByOpinionsDescending = "OpinionsCount";

        public const string ByRatingDesc = "AverageRating";
    }
}
