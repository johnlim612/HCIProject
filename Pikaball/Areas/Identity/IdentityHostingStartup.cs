﻿using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pikaball.Areas.Identity.Data;
using Pikaball.Data;

[assembly: HostingStartup(typeof(Pikaball.Areas.Identity.IdentityHostingStartup))]
namespace Pikaball.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {

                //This part removes the password requirements such as 
                //capital letter, or special characters
                //
                //Makes it so that unique email address are required for each user.
                //It also turns off the account confirmation requirement
                services.AddDefaultIdentity<PikaballUser>(options => {
                    options.User.RequireUniqueEmail = true;
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;

                })
                    .AddEntityFrameworkStores<PokemonDBContext>();
            });
        }
    }
}