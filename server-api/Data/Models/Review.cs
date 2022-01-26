using System;
using server_api.Data.Models.Interfaces;

namespace server_api.Data.Models
{
    public class Review:ISimpleEntity<int>,IEquatable<Review>
    {
        public int Id {get;set;}
        public string Name => User.Id.ToString();
        public Movie Movie {get;set;}
        public UserData User {get;set;}
        public string Text {get;set;}
        public DateTime Date {get;set;}
        public int? Rating {get;set;}

        public override bool Equals(object obj)
        {
            return Equals(obj as Review);
        }

        public bool Equals(Review other)
        {
            return other != null &&
                   Id == other.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
