﻿namespace ProSeeker.Web.Areas.Identity.Pages.Account
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.Extensions.Logging;
    using ProSeeker.Common;
    using ProSeeker.Data.Common.Repositories;
    using ProSeeker.Data.Models;

    [AllowAnonymous]
    public class RegisterUserModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<RegisterUserModel> logger;
        private readonly IEmailSender emailSender;
        private readonly IRepository<City> citiesRepository;

        public RegisterUserModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterUserModel> logger,
            IEmailSender emailSender,
            IRepository<City> citiesRepository)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.emailSender = emailSender;
            this.citiesRepository = citiesRepository;
        }

        [BindProperty]
        public RegisterInputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public IList<SelectListItem> AllCities => this.citiesRepository.All()
            .OrderBy(n => n.Name)
            .Select(c => new SelectListItem() { Text = c.Name, Value = c.Id.ToString() })
            .ToList();

        public class RegisterInputModel
        {
            [Required(ErrorMessage = "Моля, попълнете полето 'потребителско име/имейл'!")]
            [EmailAddress(ErrorMessage ="Невалиден имейл.")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Моля, попълнете полето 'Парола'!")]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Парола")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Потвърждаване на паролата")]
            [Compare("Password", ErrorMessage = "Двете пароли не съответстват.")]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = "Моля, попълнете полето 'Име'!")]
            [StringLength(20, MinimumLength = 1)]
            [RegularExpression(@"^[а-яА-Я]*?[- .]{0,2}[а-яА-Я]*?[- .]{0,2}[а-яА-Я]*$", ErrorMessage = @"Невалидно първо име. Примери за валидно име:'Георги', 'инж. Иван', 'бай Иван'")]
            [Display(Name = "Име*")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Моля, попълнете полето 'Фамилия'!")]
            [StringLength(25, MinimumLength = 1)]
            [RegularExpression(@"^[а-яА-Я]*?[- .]{0,2}[а-яА-Я]*$", ErrorMessage = @"Невалидна фамилия. Примери за валидна фамилия: 'Тодоров', 'Петрова-Алексиева'")]
            [Display(Name = "Фамилия*")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "Моля, попълнете полето 'Град'!")]
            public int CityId { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            this.ReturnUrl = returnUrl;
            this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? this.Url.Content("~/");
            this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (this.ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = this.Input.Email,
                    Email = this.Input.Email,
                    FirstName = GlobalMethods.UpperFirstLetterOfEachWord(this.Input.FirstName),
                    LastName = GlobalMethods.UpperFirstLetterOfEachWord(this.Input.LastName),
                    CityId = this.Input.CityId,
                    IsSpecialist = false,
                    IsOnline = false,
                    ProfilePicture = GlobalConstants.DefaultProfileImagePath,
                };

                var result = await this.userManager.CreateAsync(user, this.Input.Password);
                if (result.Succeeded)
                {
                    this.logger.LogInformation("User created a new account with password.");

                    var code = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = this.Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: this.Request.Scheme);

                    await this.emailSender.SendEmailAsync(
                        this.Input.Email,
                        "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (this.userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return this.RedirectToPage("RegisterConfirmation", new { email = this.Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await this.userManager.AddToRoleAsync(user, GlobalConstants.RegularUserRoleName);
                        await this.signInManager.SignInAsync(user, isPersistent: false);
                        return this.LocalRedirect(returnUrl);
                    }
                }

                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return this.Page();
        }
    }
}
