namespace ProSeeker.Web.Areas.Identity.Pages.Account
{
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
        private readonly ApplicationDbContext db;

        public RegisterSpecialistModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterSpecialistModel> logger,
            IEmailSender emailSender,
            ApplicationDbContext db)
        {

            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.emailSender = emailSender;
            this.db = db;
        }

        [BindProperty]
        public RegisterSpecialistInputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<SelectListItem> AllCategories => this.db.JobCategories
          .Select(c => new SelectListItem() { Text = c.Name, Value = c.Id.ToString() })
          .ToList();

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class RegisterSpecialistInputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [StringLength(30, ErrorMessage = "Your first name should be between 1 and 30 characters long", MinimumLength = 1)]
            [RegularExpression(@"^[a-zA-Z-\s]*$", ErrorMessage = @"Your first name can only contain letters, dashes '-', spaces.")]
            [Display(Name = "Your first name")]
            public string FirstName { get; set; }

            [Required]
            [StringLength(40, ErrorMessage = "Your last name should be between 1 and 40 characters long", MinimumLength = 1)]
            [RegularExpression(@"^[a-zA-Z-\s]*$", ErrorMessage = @"Your last name can only contain letters, dashes '-', spaces.")]
            [Display(Name = "Your last name")]
            public string LastName { get; set; }

            [Required]
            [StringLength(30, ErrorMessage = "The city name should be between 3 and 30 characters long", MinimumLength = 3)]
            [Display(Name = "City name")]
            public string City { get; set; }

            [Display(Name = "Your company name")]
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
                    FirstName = this.Input.FirstName,
                    LastName = this.Input.LastName,
                    IsSpecialist = true,
                    IsOnline = false,
                    City = this.Input.City,
                    SpecialistDetails = new Specialist_Details { JobCategoryId = int.Parse(this.Input.JobCategoryId), CompanyName = this.Input.CompanyName },
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
