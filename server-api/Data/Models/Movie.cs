using System.ComponentModel;
using System.Collections.Generic;
using System;
using System.Drawing;
using server_api.Data.Models.Interfaces;

namespace server_api.Data.Models
{
    public class Movie : ISimpleEntity<int>, IEquatable<Movie>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? ReleasedDate { get; set; }
        public List<Genre> Genres { get; set; }
        public Person Director { get; set; }
        public TimeSpan? Duration { get; set; }
        public ISet<Person> Actors { get; set; }
        [DisplayNameAttribute("Цвет фона для страницы фильма")]
        public string BackgroundColor { get; set; }
        public int BackgroundImageId { get; set; }
        [DisplayNameAttribute("Фоновое изображение для страницы фильма")]
        public FileApp BackgroundImage { get; set; }
        public int PosterId { get; set; }
        [DisplayNameAttribute("Постер для фильма")]
        public FileApp Poster { get; set; }
        public FileApp Image { get; set; }
        public int ImageId { get; set; }
        public int PreviewId { get; set; }
        public FileApp Preview { get; set; }
        public int VideoId { get; set; }
        public FileApp Video { get; set; }
        public List<Review> Reviews { get; set; }
        public List<UserData> UserDatas { get; set; }

        public override bool Equals(object obj) => Equals(obj as Movie);

        public bool Equals(Movie other) => other != null && Id == other.Id;

        public override int GetHashCode() => HashCode.Combine(Id);
    }
}
