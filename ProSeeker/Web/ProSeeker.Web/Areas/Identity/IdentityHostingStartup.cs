using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(ProSeeker.Web.Areas.Identity.IdentityHostingStartup))]

namespace ProSeeker.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}
