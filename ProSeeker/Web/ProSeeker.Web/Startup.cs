namespace ProSeeker.Web
{
    using System.Reflection;
    using CloudinaryDotNet;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using ProSeeker.Data;
    using ProSeeker.Data.Common;
    using ProSeeker.Data.Common.Repositories;
    using ProSeeker.Data.Models;
    using ProSeeker.Data.Repositories;
    using ProSeeker.Data.Seeding;
    using ProSeeker.Services.Data;
    using ProSeeker.Services.Data.CategoriesService;
    using ProSeeker.Services.Data.Cloud;
    using ProSeeker.Services.Data.Home;
    using ProSeeker.Services.Data.ServicesService;
    using ProSeeker.Services.Data.UsersService;
    using ProSeeker.Services.Mapping;
    using ProSeeker.Services.Messaging;
    using ProSeeker.Web.ViewModels;

    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<CookiePolicyOptions>(
                options =>
                    {
                        options.CheckConsentNeeded = context => true;
                        options.MinimumSameSitePolicy = SameSiteMode.None;
                    });

            // TODO: Add Privacy page and add implement facebook/google APIs
            services.AddAuthentication();
            //.AddFacebook(facebookOptions =>
            //{
            //    facebookOptions.AppId = this.configuration["Authentication:Facebook:AppId"];
            //    facebookOptions.AppSecret = this.configuration["Authentication:Facebook:AppSecret"];
            //})
            //.AddGoogle(googleOptions =>
            //{
            //    googleOptions.ClientId = this.configuration["Authentication:Google:ClientId"];
            //    googleOptions.ClientSecret = this.configuration["Authentication:Google:ClientSecret"];
            //});

            // Cloudinary Authentication
            var clodAccount = new CloudinaryDotNet.Account(
                this.configuration["Cloudinary:CloudName"],
                this.configuration["Cloudinary:ApiKey"],
                this.configuration["Cloudinary:Secret"]);
            var cloudinary = new Cloudinary(clodAccount);
            services.AddSingleton(cloudinary);

            services.AddControllersWithViews(
                options =>
                    {
                        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                    }).AddRazorRuntimeCompilation();

            // CSRF
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()); 
            });
            services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-CSRF-TOKEN";
            });

            services.AddRazorPages();

            services.AddSingleton(this.configuration);

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Register all application services (Dependency container)
            services.AddTransient<IEmailSender>(x => new SendGridEmailSender("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"));
            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<IHomeService, HomeService>();
            services.AddTransient<ICategoriesService, CategoriesService>();
            services.AddTransient<ICloudinaryApplicationService, CloudinaryApplicationService>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IServicesService, ServicesService>();
            //services.AddTransient<IApplicationUsersController, ApplicationUsersController>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            // Seed data on application startup
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
                new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(
                endpoints =>
                    {
                        endpoints.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapControllerRoute("JobCategories", "jobcategories/{name:minlength(2)}", new { controller = "JobCategories", action = "ByName" });
                        endpoints.MapRazorPages();
                    });
        }
    }
}
