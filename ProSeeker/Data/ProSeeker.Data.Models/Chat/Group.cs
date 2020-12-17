namespace ProSeeker.Data.Models.Chat
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ProSeeker.Data.Common.Models;

    public class Group : BaseDeletableModel<string>
    {
        public Group()
        {
            this.Id = Guid.NewGuid().ToString();
            this.UsersGroups = new HashSet<UserGroup>();
            this.ChatMessages = new HashSet<ChatMessage>();
        }

        [MaxLength(70)]
        public string Name { get; set; }

        public ICollection<UserGroup> UsersGroups { get; set; }

        public ICollection<ChatMessage> ChatMessages { get; set; }
    }
}
