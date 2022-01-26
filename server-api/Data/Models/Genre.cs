using System.ComponentModel;
using System.Linq;
using System;
using System.Collections.Generic;
using server_api.Data.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace server_api.Data.Models
{
    public class Genre:ISimpleEntity<int>,IEquatable<Genre>
    {
        public int Id { get; set; }
        [Required(ErrorMessage="Имя жанра - обязательное поле")]
        public string Name { get; set; }
        public string Caption {get;set;}
        public string Description { get; set; }
        public List<Movie> Movies {get;set;}
        public override bool Equals(object obj)
        {
            return Equals(obj as Genre);
        }

        public bool Equals(Genre other)
        {
            return other != null &&
                   Name == other.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }

    }
}
