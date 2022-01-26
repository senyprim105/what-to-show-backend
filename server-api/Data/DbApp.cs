using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using server_api.Data.Models;
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace server_api.Data
{
    public class DbApp :IdentityDbContext<User>
    {
        public DbSet<FileApp> Files {get;set;}
        public DbSet<Genre> Genres {get;set;}
        public DbSet<Movie> Movies {get;set;}
        public DbSet<Person> Persons {get;set;}
        public DbSet<Review> Reviews {get;set;}
        public DbSet<UserData> UserDatas {get;set;}
        
        public DbApp(DbContextOptions<DbApp>options) :base(options)
        {

        }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new FileAppConfiguration());
            modelBuilder.ApplyConfiguration(new GenreConfiguration());
            modelBuilder.ApplyConfiguration(new MovieConfiguration());
            modelBuilder.ApplyConfiguration(new PersonConfiguration());
            modelBuilder.ApplyConfiguration(new ReviewConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}
