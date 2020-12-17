namespace ProSeeker.Data.Models.PrivateChat
{
    using System.ComponentModel.DataAnnotations;

    public class UserConversation
    {
        [Required]
        public string ConversationId { get; set; }

        public Conversation Conversation { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}
