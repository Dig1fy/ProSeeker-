namespace ProSeeker.Web.Areas.Identity.Pages.Account.Manage
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using ProSeeker.Common;
    using ProSeeker.Data.Common.Repositories;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Data.Cloud;

    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IDeletableEntityRepository<Specialist_Details> specialistsRepository;
        private readonly ICloudinaryApplicationService cloudinaryApplicationService;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IDeletableEntityRepository<Specialist_Details> specialistsRepository,
            ICloudinaryApplicationService cloudinaryApplicationService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.specialistsRepository = specialistsRepository;
            this.cloudinaryApplicationService = cloudinaryApplicationService;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string ProfilePictureUrl { get; set; }

            [Display(Name = "Your user name")]
            public string Username { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Required]
            [StringLength(16, ErrorMessage = "Your first name should be between 1 and 16 characters long", MinimumLength = 1)]
            [RegularExpression(@"^[a-zA-Z-\s]*$", ErrorMessage = @"Your first name can only contain letters, dashes '-', spaces.")]
            [Display(Name = "Your first name*")]
            public string FirstName { get; set; }

            [Required]
            [StringLength(25, ErrorMessage = "Your last name should be between 1 and 25 characters long", MinimumLength = 1)]
            [RegularExpression(@"^[a-zA-Z-\s]*$", ErrorMessage = @"Your last name can only contain letters, dashes '-', spaces.")]
            [Display(Name = "Your last name*")]
            public string LastName { get; set; }

            [Required]
            [StringLength(30, ErrorMessage = "The city name should be between 3 and 30 characters long", MinimumLength = 3)]
            [RegularExpression(@"^[a-zA-Z]*$", ErrorMessage = "Name should consist of letters only")]
            [Display(Name = "City name*")]
            public string City { get; set; }

            public bool IsSpecialist { get; set; }

            public SpecialistInputModel SpecialistDetails { get; set; }
        }

        public class SpecialistInputModel
        {
            [StringLength(250, ErrorMessage = "'About me' text content should be between 15 and 250 symbols long", MinimumLength = 15)]
            [Display(Name = "Additional information about you and your professional experience")]
            public string AboutMe { get; set; }

            [StringLength(60, ErrorMessage = "'Company name' should be between 1 and 60 symbols long", MinimumLength = 1)]
            [Display(Name = "Your company name")]
            public string CompanyName { get; set; }

            [StringLength(1000, ErrorMessage = "Your 'experience' text content should be between 15 and 1000 symbols long", MinimumLength = 15)]
            [Display(Name = "Your professional experience")]
            public string Experience { get; set; }

            [StringLength(1000, ErrorMessage = "'Your qualifications' text content should be between 15 and 1000 symbols long", MinimumLength = 15)]
            [Display(Name = "Your professional qualifications and specialisations")]
            public string Qualification { get; set; }

            [StringLength(75, ErrorMessage = "'Website' text content should be between 5 and 75 symbols long", MinimumLength = 5)]
            [Display(Name = "Your professional website")]
            public string Website { get; set; }

            [Display(Name = "The services you provide")]
            public ICollection<Service> Services { get; set; }
        }

        private async Task LoadAsync(ApplicationUser currentUser)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            this.Input = new InputModel
            {
                Username = user.UserName,
                PhoneNumber = user.PhoneNumber,
                FirstName = GlobalMethods.UpperFirstLetterOfEachWord(user.FirstName),
                LastName = GlobalMethods.UpperFirstLetterOfEachWord(user.LastName),
                City = GlobalMethods.UpperFirstLetterOfEachWord(user.City),
                IsSpecialist = user.IsSpecialist,
                ProfilePictureUrl = user.ProfilePicture,
            };

            if (user.IsSpecialist)
            {
                this.Input.SpecialistDetails = this.specialistsRepository.All().Where(x => x.UserId == user.Id).Select(y => new SpecialistInputModel
                {
                    AboutMe = y.AboutMe,
                    CompanyName = y.CompanyName,
                    Website = y.Website,
                    Experience = y.Experience,
                    Qualification = y.Qualification,
                    Services = y.Services,
                }).FirstOrDefault();
            }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"{GlobalConstants.UnableToLoadUserByIdErrorMessage}'{this.userManager.GetUserId(this.User)}'.");
            }

            await this.LoadAsync(user);
            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile imageFile)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (user == null)
            {
                return this.NotFound($"{GlobalConstants.UnableToLoadUserByIdErrorMessage}'{this.userManager.GetUserId(this.User)}'.");
            }

            if (!this.ModelState.IsValid)
            {
                await this.LoadAsync(user);
                return this.Page();
            }

            if (user.FirstName != this.Input.FirstName)
            {
                user.FirstName = GlobalMethods.UpperFirstLetterOfEachWord(this.Input.FirstName);
            }

            if (user.LastName != this.Input.LastName)
            {
                user.LastName = GlobalMethods.UpperFirstLetterOfEachWord(this.Input.LastName);
            }

            if (user.City != this.Input.City)
            {
                user.City = GlobalMethods.UpperFirstLetterOfEachWord(this.Input.City);
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
                    specDetails.CompanyName = GlobalMethods.UpperFirstLetterOfEachWord(this.Input.SpecialistDetails.CompanyName);
                }

                if (specDetails.Website != this.Input.SpecialistDetails.Website)
                {
                    specDetails.Website = this.Input.SpecialistDetails.Website;
                }

                if (specDetails.Experience != this.Input.SpecialistDetails.Experience)
                {
                    specDetails.Experience = this.Input.SpecialistDetails.Experience;
                }

                if (specDetails.Qualification != this.Input.SpecialistDetails.Qualification)
                {
                    specDetails.Qualification = this.Input.SpecialistDetails.Qualification;
                }

                // TODO: Implement Specialist SERVICES

            }

            var phoneNumber = await this.userManager.GetPhoneNumberAsync(user);

            if (this.Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await this.userManager.SetPhoneNumberAsync(user, this.Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    this.StatusMessage = GlobalConstants.InvalidPhoneNumber;
                    return this.RedirectToPage();
                }
            }

            var profileImageName = $"{user.Id}profilePicture";

            if (imageFile != null)
            {
                if (!this.cloudinaryApplicationService.IsFileValid(imageFile))
                {
                    this.StatusMessage = GlobalConstants.InvalidProfilePictureMessage;
                    return this.RedirectToPage();
                }

                var imageUrl = await this.cloudinaryApplicationService.UploadImageAsync(imageFile, profileImageName);
                user.ProfilePicture = imageUrl;
            }

            var updateResult = await this.userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                this.StatusMessage = GlobalConstants.UpdateProfileErrorMessage;
            }

            await this.signInManager.RefreshSignInAsync(user);
            this.StatusMessage = GlobalConstants.SuccessfullyUpdatedProfile;
            return this.RedirectToPage();
        }
    }
}
