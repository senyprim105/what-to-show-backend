
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using server_api.Data.Models;
using server_api.Data.Models.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace server_api.Data
{
    public class Seed
    {
        public static async Task SeedFromJsonAsync(IApplicationBuilder aplicationBuilder, string fileName, IWebHostEnvironment web, User user = null)
        {

            if (user == null)
            {
                var userManager = aplicationBuilder.ApplicationServices.GetRequiredService<UserManager<User>>();
                var roleManager = aplicationBuilder.ApplicationServices.GetRequiredService<RoleManager<IdentityRole>>();
                user = await Repository.CreateUser("user", new string[] { "user" }, "user@mail", "user", userManager, roleManager);
            }

        }
    }
}
