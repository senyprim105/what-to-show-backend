using System.Linq;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Identity;
using server_api.Data.Models.Interfaces;

namespace server_api.Data.Models
{
    public class UserData: ISimpleEntity<int>,IEquatable<UserData>
    {
        public int Id {get;set;}
        public string Name => User.Email;
        public User User {get;set;}
        public  FileApp Avatar {get;set;}
        public  List<Movie> Movies {get;set;}
        public  ICollection<Review> Reviews {get;set;}
        public void AddUserMovie(Movie movie){
            if (movie == null || Movies.Any(it=>it.Id == movie.Id)) return;
            Movies.Add(movie);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as UserData);
        }

        public bool Equals(UserData other)
        {
            return other != null &&
                   Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
