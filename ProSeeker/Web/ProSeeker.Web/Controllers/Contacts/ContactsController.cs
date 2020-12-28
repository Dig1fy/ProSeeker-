namespace ProSeeker.Web.Controllers.Contacts
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using ProSeeker.Common;
    using ProSeeker.Services.Messaging;
    using ProSeeker.Web.ViewModels.EmailsSender;

    public class ContactsController : BaseController
    {
        private readonly IEmailSender emailSender;

        public ContactsController(IEmailSender emailSender)
        {
            this.emailSender = emailSender;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(EmailFromContactsInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            try
            {
                var receiver = GlobalConstants.ApplicationEmail;
                var updatedContent = GlobalMethods.GetUpdatedContentFromContactsForm(input.Content, input.FromEmail, input.FromName);
                await this.emailSender.SendEmailAsync(receiver, input.FromName, receiver, input.Subject, updatedContent);
            }
            catch (Exception)
            {
                return this.CustomCommonError();
            }

            return this.Redirect("/");
        }
    }
}
