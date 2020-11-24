namespace ProSeeker.Web.Areas.Identity.Pages.Account
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.Extensions.Logging;
    using ProSeeker.Common;
    using ProSeeker.Data;
    using ProSeeker.Data.Common.Repositories;
    using ProSeeker.Data.Models;

    public class RegisterSpecialistModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<RegisterSpecialistModel> logger;
        private readonly IEmailSender emailSender;
        private readonly IDeletableEntityRepository<JobCategory> categoriesRepository;
        private readonly IRepository<City> citiesRepository;

        public RegisterSpecialistModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterSpecialistModel> logger,
            IEmailSender emailSender,
            IDeletableEntityRepository<JobCategory> categoriesRepository,
            IRepository<City> citiesRepository)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.emailSender = emailSender;
            this.categoriesRepository = categoriesRepository;
            this.citiesRepository = citiesRepository;
        }

        [BindProperty]
        public RegisterSpecialistInputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<SelectListItem> AllCategories => this.categoriesRepository.All()
          .Select(c => new SelectListItem() { Text = c.Name, Value = c.Id.ToString() })
          .ToList();

        public IList<SelectListItem> AllCities => this.citiesRepository.All()
            .OrderBy(n => n.Name)
            .Select(c => new SelectListItem() { Text = c.Name, Value = c.Id.ToString() })
            .ToList();

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class RegisterSpecialistInputModel
        {
            [Required(ErrorMessage = "Моля, попълнете полето 'потребителско име/имейл'!")]
            [EmailAddress(ErrorMessage = "Невалиден мейл.")]
            [Display(Name = "Потребителско име /имейл/")]
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

            [Display(Name = "Град")]
            [Required(ErrorMessage = "Моля, попълнете полето 'Град'!")]
            public int CityId { get; set; }

            [Display(Name = "Компания")]
            public string CompanyName { get; set; }

            public string JobCategoryId { get; set; }

            public List<SelectListItem> AllCategories { get; set; }
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
                    IsSpecialist = true,
                    IsOnline = false,
                    CityId = this.Input.CityId,
                    SpecialistDetails = new Specialist_Details
                    {
                        JobCategoryId = int.Parse(this.Input.JobCategoryId),
                        CompanyName = GlobalMethods.UpperFirstLetterOfEachWord(this.Input.CompanyName),
                    },
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
                        await this.userManager.AddToRoleAsync(user, GlobalConstants.SpecialistRoleName);
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
