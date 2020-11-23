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
    using Microsoft.AspNetCore.Mvc.Rendering;
    using ProSeeker.Common;
    using ProSeeker.Data.Common.Repositories;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Data.Cloud;
    using ProSeeker.Web.ViewModels.Services;

    public partial class IndexModel : PageModel
    {
        // Profile
        private const string UnableToLoadUserByIdErrorMessage = "Unable to load user with ID ";
        private const string InvalidProfilePictureMessage = "Invalid profile picture type. We support jpg, jpeg, png files only.";
        private const string InvalidPhoneNumber = "Unexpected error when trying to set phone number.";
        private const string SuccessfullyUpdatedProfile = "Your profile has been updated";
        private const string UpdateProfileErrorMessage = "Ouch! Unexpected error occured when updating your profile!";

        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IDeletableEntityRepository<Specialist_Details> specialistsRepository;
        private readonly IDeletableEntityRepository<Service> servicesRepository;
        private readonly ICloudinaryApplicationService cloudinaryApplicationService;
        private readonly IRepository<City> citiesRepository;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IDeletableEntityRepository<Specialist_Details> specialistsRepository,
            IDeletableEntityRepository<Service> servicesRepository,
            ICloudinaryApplicationService cloudinaryApplicationService,
            IRepository<City> citiesRepository)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.specialistsRepository = specialistsRepository;
            this.servicesRepository = servicesRepository;
            this.cloudinaryApplicationService = cloudinaryApplicationService;
            this.citiesRepository = citiesRepository;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<SelectListItem> AllCities => this.citiesRepository.All()
            .OrderBy(n => n.Name)
            .Select(c => new SelectListItem() { Text = c.Name, Value = c.Id.ToString() })
            .ToList();

        public class InputModel
        {
            public string ProfilePictureUrl { get; set; }

            [Display(Name = "Your user name")]
            public string Username { get; set; }

            [Phone]
            [Display(Name = "Телефон за връзка")]
            public string PhoneNumber { get; set; }

            [Required]
            [StringLength(16, ErrorMessage = "Името Ви трябва да бъде между 1 и 16 символа.", MinimumLength = 1)]
            [RegularExpression(@"^[а-яА-Я]*?[- .]{0,2}[а-яА-Я]*?[- .]{0,2}[а-яА-Я]*$", ErrorMessage = @"Невалидно първо име. Примери за валидно име:'Георги', 'инж. Иван', 'г-н Тодор'")]
            [Display(Name = "Име*")]
            public string FirstName { get; set; }

            [Required]
            [StringLength(25, ErrorMessage = "Фамилията Ви трябва да бъде между 1 и 25 символа.", MinimumLength = 1)]
            [RegularExpression(@"^[а-яА-Я]*?[- .]{0,2}[а-яА-Я]*$", ErrorMessage = @"Невалидна фамилия. Примери за валидна фамилия: 'Тодоров', 'Петрова-Алексиева'")]
            [Display(Name = "Фамилия*")]
            public string LastName { get; set; }

            //[Required]
            //[StringLength(30, ErrorMessage = "Градът трябва да бъде между 3 и 30 символа.", MinimumLength = 3)]
            //[RegularExpression(@"^[а-яА-Я]*?[- .]{0,2}[а-яА-Я]*$", ErrorMessage = "Невалиден град. Примери за валидно име на град: 'Стара Загора', 'Димитровград'")]
            //[Display(Name = "Град*")]
            public City City { get; set; }

            public bool IsSpecialist { get; set; }

            public SpecialistInputModel SpecialistDetails { get; set; }
        }

        public class SpecialistInputModel
        {
            [Display(Name = "Допълнителна информация за Вас")]
            public string AboutMe { get; set; }

            [StringLength(60, ErrorMessage = "Съдържанието трябва да бъде между 1 и 60 символа.", MinimumLength = 1)]
            [Display(Name = "Фирма /търговско наименование/")]
            public string CompanyName { get; set; }

            [Display(Name = "Описание на професионалния Ви опит")]
            public string Experience { get; set; }

            [Display(Name = "Специализация и квалификация")]
            public string Qualification { get; set; }

            [StringLength(75, ErrorMessage = "Съдържанието трябва да бъде между 5 и 75 символа.", MinimumLength = 5)]
            [Display(Name = "Професионален Уеб Сайт")]
            public string Website { get; set; }

            [Display(Name = "Услугите, които предлагате")]
            public IEnumerable<Service> Services { get; set; }
        }

        private async Task LoadAsync(ApplicationUser currentUser)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            user.City = this.citiesRepository.All().Where(x => x.Id == user.CityId).FirstOrDefault();

            this.Input = new InputModel
            {
                Username = user.UserName,
                PhoneNumber = user.PhoneNumber,
                FirstName = GlobalMethods.UpperFirstLetterOfEachWord(user.FirstName),
                LastName = GlobalMethods.UpperFirstLetterOfEachWord(user.LastName),
                City = user.City,
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
                return this.NotFound($"{UnableToLoadUserByIdErrorMessage}'{this.userManager.GetUserId(this.User)}'.");
            }

            await this.LoadAsync(user);
            return this.Page();
        }

        // Input cannot bind services in razor pages.... 8 hours debugging and finally -> services need to be passed explicitly.
        public async Task<IActionResult> OnPostAsync(IFormFile imageFile, IEnumerable<ServiceInputModel> services)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            user.City = this.citiesRepository.All().Where(x => x.Id == user.CityId).FirstOrDefault();

            if (user == null)
            {
                return this.NotFound($"{UnableToLoadUserByIdErrorMessage}'{this.userManager.GetUserId(this.User)}'.");
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

            if (user.City.Name != this.Input.City.Name)
            {
                user.City = this.citiesRepository.All().FirstOrDefault(c => c.Id == int.Parse(this.Input.City.Name));
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

                foreach (var service in services)
                {
                    var ns = new Service
                    {
                        Name = service.Name,
                        Description = service.Description,
                        SpecialistDetailsId = user.Id,
                    };

                    await this.servicesRepository.AddAsync(ns);
                    specDetails.Services.Add(ns);
                    await this.servicesRepository.SaveChangesAsync();
                }
            }

            var phoneNumber = await this.userManager.GetPhoneNumberAsync(user);

            if (this.Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await this.userManager.SetPhoneNumberAsync(user, this.Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    this.StatusMessage = InvalidPhoneNumber;
                    return this.RedirectToPage();
                }
            }

            var profileImageName = $"{user.Id}profilePicture";

            if (imageFile != null)
            {
                if (!this.cloudinaryApplicationService.IsFileValid(imageFile))
                {
                    this.StatusMessage = InvalidProfilePictureMessage;

                    return this.RedirectToPage();
                }

                var imageUrl = await this.cloudinaryApplicationService.UploadImageAsync(imageFile, profileImageName);
                user.ProfilePicture = imageUrl;
            }

            var updateResult = await this.userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                this.StatusMessage = UpdateProfileErrorMessage;
            }

            await this.signInManager.RefreshSignInAsync(user);
            this.StatusMessage = SuccessfullyUpdatedProfile;
            return this.RedirectToPage();
        }

        public async Task<IActionResult> OnPostDelete(int id)
        {
            var service = await this.servicesRepository.GetByIdWithDeletedAsync(id);
            this.servicesRepository.Delete(service);
            await this.servicesRepository.SaveChangesAsync();
            return this.RedirectToPage();
        }
    }
}
