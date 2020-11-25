namespace ProSeeker.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore.Internal;
    using ProSeeker.Data.Models;

    public class CategoriesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var homeTemplate = "https://localhost:44319//images/CategoryImages/Home/{0}.png";
            var vehicleTemplate = "https://localhost:44319//images/CategoryImages/Vehicle/{0}.png";
            var othersTemplate = "https://localhost:44319//images/CategoryImages/Others/{0}.png";

            if (dbContext.BaseJobCategories.Any() && dbContext.JobCategories.Any())
            {
                return;
            }

            var baseCategories = new List<BaseJobCategory>
            {
                new BaseJobCategory
            {
                CategoryName = "За дома",
                Description = "Някои от най-ценните специалисти за новия Ви дом!",
                JobCategories = new List<JobCategory>
                {
                    new JobCategory
                    {
                        Name = "Интериорен дизайнер",
                        Description = "Тя: 'Трябва да изглежда съвършено!'",
                        PictureUrl = string.Format(homeTemplate, "интериорен-дизайнер"),
                    },
                    new JobCategory
                    {
                        Name = "Архитект",
                        Description = "Когато в спалнята има място и за дрехите Ви - безценно!",
                        PictureUrl = string.Format(homeTemplate, "архитект"),
                    },
                    new JobCategory
                    {
                        Name = "Геодезист",
                        Description = "Всичко свързано със заснемането и проучването на вашия поземлен имот",
                        PictureUrl = string.Format(homeTemplate, "геодезист"),
                    },
                    new JobCategory
                    {
                        Name = "Урбанист",
                        Description = "Умни хора - планират огромни територии. Ще се справят безупречно с вашия имот!",
                        PictureUrl = string.Format(homeTemplate, "Урбанист"),
                    },
                    new JobCategory
                    {
                        Name = "Консултант-недвижими имоти (брокер)",
                        Description = "Понякога са нужни...",
                        PictureUrl = string.Format(homeTemplate, "брокер"),
                    },
                },
            },
                new BaseJobCategory
                {
                    CategoryName = "За МПС-то",
                    Description = "Хората със златни ръце!",
                    JobCategories = new List<JobCategory>
                    {
                        new JobCategory
                        {
                            Name = "Автомонтьор",
                            Description = "За здравето на вашите любими машини",
                            PictureUrl = string.Format(vehicleTemplate, "автомонтьор"),
                        },
                        new JobCategory
                        {
                            Name = "Автобояджия",
                            Description = "Друго си е да е лъскаво!",
                            PictureUrl = string.Format(vehicleTemplate, "автобояджия"),
                        },
                        new JobCategory
                        {
                            Name = "Тенекиджия",
                            Description = "Чукне тук, чукне там...",
                            PictureUrl = string.Format(vehicleTemplate, "тенекиджия"),
                        },
                        new JobCategory
                        {
                            Name = "Газаджия",
                            Description = "Вече е безопасно да возиш бомба в колата",
                            PictureUrl = string.Format(vehicleTemplate, "газаджия"),
                        },
                    },
                },

                new BaseJobCategory
                {
                    CategoryName = "Други",
                    Description = "Без тях не може...",
                    JobCategories = new List<JobCategory>
                    {
                        new JobCategory
                        {
                            Name = "Адвокат",
                            Description = "Нека правото бъде вас!",
                            PictureUrl = string.Format(othersTemplate, "aдвокат"),
                        },
                        new JobCategory
                        {
                            Name = "Зъболекар",
                            Description = "Важно е да е добър, за да го виждаш по-рядко!",
                            PictureUrl = string.Format(othersTemplate, "зъболекар"),
                        },
                        new JobCategory
                        {
                            Name = "Психолог",
                            Description = "За да бъдем по-добри хора!",
                            PictureUrl = string.Format(othersTemplate, "психолог"),
                        },
                        new JobCategory
                        {
                            Name = "Фотограф",
                            Description = "С добрия фотограф, килограмите нямат значение!",
                            PictureUrl = string.Format(othersTemplate, "фотограф"),
                        },
                        new JobCategory
                        {
                            Name = "Счетоводител",
                            Description = "Всяка цифра е важна, особено ако ще ти излиза от джоба!",
                            PictureUrl = string.Format(othersTemplate, "счетоводител"),
                        },
                        new JobCategory
                        {
                            Name = "Фризьор",
                            Description = "Не е задължително, но трябва!",
                            PictureUrl = string.Format(othersTemplate, "фризьор"),
                        },
                    },
                },
            };

            foreach (var category in baseCategories)
            {
                await dbContext.BaseJobCategories.AddAsync(category);
            }
        }
    }
}
