
using server_api.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace server_api
{
    public class GenreConfiguration : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder.ToTable("Genres");
            builder.HasKey(it => it.Id);
            builder.HasIndex(it=>it.Name).IsUnique();
            builder.Property(it => it.Name).HasMaxLength(50);
            builder.Property(it => it.Name).HasMaxLength(250);
            builder.HasMany(g => g.Movies).WithMany(m => m.Genres);
        }
    }
    public class MovieConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.ToTable("Movies");
            builder.HasKey(it => it.Id);
            builder.Property(it => it.Name).HasMaxLength(250);
            builder.Property(it => it.Description).HasMaxLength(2000);
            builder.Property(it => it.ReleasedDate).HasColumnType("datetime2");
            builder.HasOne(it => it.Director).WithMany().IsRequired(false).HasForeignKey("DirectorId").OnDelete(DeleteBehavior.SetNull);
            builder.Property(it => it.BackgroundColor).HasMaxLength(6);
            builder.HasOne(it => it.BackgroundImage).WithMany(file => file.WithBackgroundImage).HasForeignKey(m => m.BackgroundImageId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(it => it.Poster).WithMany(file => file.WithPoster).HasForeignKey(m => m.PosterId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(it => it.Image).WithMany(file => file.WithImage).HasForeignKey(m => m.ImageId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(it => it.Preview).WithMany(file => file.WithPreview).HasForeignKey(m => m.PreviewId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(it => it.Video).WithMany(file => file.WithVideo).HasForeignKey(m => m.VideoId).OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(it => it.Reviews).WithOne(review => review.Movie).IsRequired().HasForeignKey("MovieId").OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(it => it.UserDatas).WithMany(user => user.Movies);
        }
    }
    public class FileAppConfiguration : IEntityTypeConfiguration<FileApp>
    {
        public void Configure(EntityTypeBuilder<FileApp> builder)
        {
            builder.ToTable("Files");
            builder.HasKey(it => it.Id);
            builder.Property(it=>it.Id).ValueGeneratedOnAdd();
            builder.Property(it => it.Type).HasConversion<string>();
            builder.HasIndex(it=>it.Hash).IsUnique();
            builder.Property(it => it.Hash).HasMaxLength(250);
            builder.HasOne(it=>it.User).WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.SetNull);
        }
    }

    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.ToTable("Persons");
            builder.HasKey(it => it.Id);
            builder.Property(it => it.FirstName).HasMaxLength(250);
            builder.Property(it => it.SecondName).HasMaxLength(250);
            builder.Property(it => it.LastName).HasMaxLength(250);
            builder.HasIndex(it => new { it.FirstName, it.SecondName, it.LastName }).IsUnique();
        }
    }

    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("Reviews");
            builder.HasKey(it => it.Id);
            builder.HasOne(it => it.Movie).WithMany(movie => movie.Reviews).HasForeignKey("MovieId").IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(it => it.User).WithMany(user => user.Reviews).HasForeignKey("UserId").IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.Property(it => it.Text).HasMaxLength(2000);
            builder.Property(it => it.Date).HasColumnType("datetime2");
            // builder.HasIndex(it=>new {it.Movie,it.User});
        }
    }

    public class UserConfiguration : IEntityTypeConfiguration<UserData>
    {
        public void Configure(EntityTypeBuilder<UserData> builder)
        {
            builder.ToTable("UserData");
            builder.HasKey(it => it.Id);
            builder.HasOne(it=>it.User).WithOne().HasForeignKey<UserData>("UserId").IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(it => it.Avatar).WithMany().HasForeignKey("AvatarId").IsRequired(false).OnDelete(DeleteBehavior.SetNull);
            builder.HasMany(it => it.Movies).WithMany(movie => movie.UserDatas);
            builder.HasMany(it => it.Reviews).WithOne(review => review.User);
        }
    }






}