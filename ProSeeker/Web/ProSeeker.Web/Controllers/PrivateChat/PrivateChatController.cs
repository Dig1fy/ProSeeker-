namespace ProSeeker.Web.Controllers.PrivateChat
{
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Data.PrivateChat;
    using ProSeeker.Services.Data.UsersService;
    using ProSeeker.Web.ViewModels.PrivateChat;
    using ProSeeker.Web.ViewModels.Users;
    using System.Threading.Tasks;

    public class PrivateChatController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPrivateChatService privateChatService;
        private readonly IUsersService usersService;

        public PrivateChatController(
            UserManager<ApplicationUser> userManager,
            IPrivateChatService privateChatService,
            IUsersService usersService)
        {
            this.userManager = userManager;
            this.privateChatService = privateChatService;
            this.usersService = usersService;
        }

        public async Task<IActionResult> Index(string username, string group)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            var model = new PrivateChatViewModel
            {
                Sender = await this.userManager.GetUserAsync(this.HttpContext.User),
                Receiver = await this.usersService.GetUserByUsernameAsync<ApplicationUser>(username),
                ChatMessages = await this.privateChatService.ExtractAllMessages(group),
                GroupName = group,
                Emojis = this.privateChatService.GetAllEmojis(),
                AllChatThemes = this.privateChatService.GetAllThemes(),
                ChatThemeViewModel = this.privateChatService.GetGroupTheme(group),
                AllStickers = this.privateChatService.GetAllStickers(),
            };
        }
    }
}
