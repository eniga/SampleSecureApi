using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SampleSecureApi.Areas.Identity.Data;
using SampleSecureApi.Data;

[assembly: HostingStartup(typeof(SampleSecureApi.Areas.Identity.IdentityHostingStartup))]
namespace SampleSecureApi.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<SampleSecureApiContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("SampleSecureApiContextConnection")));

                services.AddDefaultIdentity<SampleSecureApiUser>(options => options.SignIn.RequireConfirmedAccount = false)
                    .AddEntityFrameworkStores<SampleSecureApiContext>();
            });
        }
    }
}