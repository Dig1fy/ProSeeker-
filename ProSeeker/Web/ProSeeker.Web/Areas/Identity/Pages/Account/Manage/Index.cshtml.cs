namespace ProSeeker.Web.Areas.Identity.Pages.Account.Manage
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using ProSeeker.Common;
    using ProSeeker.Data;
    using ProSeeker.Data.Common.Repositories;
    using ProSeeker.Data.Models;

    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IDeletableEntityRepository<Specialist_Details> specialistsRepository;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IDeletableEntityRepository<Specialist_Details> specialistsRepository)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.specialistsRepository = specialistsRepository;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Your user name")]
            public string Username { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Required]
            [StringLength(30, ErrorMessage = "Your first name should be between 1 and 30 characters long", MinimumLength = 1)]
            [RegularExpression(@"^[a-zA-Z-\s]*$", ErrorMessage = @"Your first name can only contain letters, dashes '-', spaces.")]
            [Display(Name = "Your first name*")]
            public string FirstName { get; set; }

            [Required]
            [StringLength(40, ErrorMessage = "Your last name should be between 1 and 40 characters long", MinimumLength = 1)]
            [RegularExpression(@"^[a-zA-Z-\s]*$", ErrorMessage = @"Your last name can only contain letters, dashes '-', spaces.")]
            [Display(Name = "Your last name*")]
            public string LastName { get; set; }

            [Required]
            [StringLength(30, ErrorMessage = "The city name should be between 3 and 30 characters long", MinimumLength = 3)]
            [Display(Name = "City name*")]
            public string City { get; set; }

            public bool IsSpecialist { get; set; }

            public SpecialistInputModel SpecialistDetails { get; set; }
        }

        public class SpecialistInputModel
        {
            [Display(Name = "Additional information about you and your professional experience")]
            public string AboutMe { get; set; }

            [Display(Name = "Your company name")]
            public string CompanyName { get; set; }

            [Display(Name = "Your professional activities")]
            public string WorkActivities { get; set; }

            [Display(Name = "Your professional website")]
            public string Website { get; set; }
        }

        private async Task LoadAsync(ApplicationUser currentUser)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            this.Input = new InputModel
            {
                Username = user.UserName,
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                City = user.City,
                IsSpecialist = user.IsSpecialist,
            };

            if (user.IsSpecialist)
            {
                this.Input.SpecialistDetails = this.specialistsRepository.All().Where(x => x.UserId == user.Id).Select(y => new SpecialistInputModel
                {
                    AboutMe = y.AboutMe,
                    CompanyName = y.CompanyName,
                    Website = y.Website,
                    WorkActivities = y.WorkActivities,
                }).FirstOrDefault();
            }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            await this.LoadAsync(user);
            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            if (!this.ModelState.IsValid)
            {
                await this.LoadAsync(user);
                return this.Page();
            }

            if (user.IsSpecialist)
            {
                var specDetails = this.specialistsRepository.All().FirstOrDefault(x => x.UserId == user.Id);

                if (specDetails.AboutMe != this.Input.SpecialistDetails.AboutMe)
                {
                    specDetails.AboutMe = this.Input.SpecialistDetails.AboutMe;
                }

                if (specDetails.CompanyName != this.Input.SpecialistDetails.CompanyName)
                {
                    specDetails.CompanyName = this.Input.SpecialistDetails.CompanyName;
                }

                if (specDetails.Website != this.Input.SpecialistDetails.Website)
                {
                    specDetails.Website = this.Input.SpecialistDetails.Website;
                }

                if (specDetails.WorkActivities != this.Input.SpecialistDetails.WorkActivities)
                {
                    specDetails.WorkActivities = this.Input.SpecialistDetails.WorkActivities;
                }
            }

            var phoneNumber = await this.userManager.GetPhoneNumberAsync(user);

            if (this.Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await this.userManager.SetPhoneNumberAsync(user, this.Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    this.StatusMessage = "Unexpected error when trying to set phone number.";
                    return this.RedirectToPage();
                }
            }

            var updateResult = await this.userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                this.StatusMessage = GlobalConstants.UpdateProfileErrorMessage;
            }

            await this.signInManager.RefreshSignInAsync(user);
            this.StatusMessage = "Your profile has been updated";
            return this.RedirectToPage();
        }
    }
}
