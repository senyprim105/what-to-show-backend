using System.ComponentModel.DataAnnotations;
using System.Linq;
using System;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations.Schema;
using server_api.Data.Models.Interfaces;

namespace server_api.Data.Models
{
    public class Person:ISimpleEntity<int>, IEquatable<Person>
    {
        public int Id { get; set; }
        [NotMapped]
        public string Name { get => getFullName();}
        [Required(ErrorMessage="Имя пользователя обязательное поле")]
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }

        public Person() { }
        public Person(string str)
        {
            if (String.IsNullOrWhiteSpace(str)) return;
            var names = Regex.Split(str, "/s+").Where(name => !String.IsNullOrWhiteSpace(name)).ToArray();
            this.FirstName = names[0];
            this.SecondName = names.Length > 1 ? names[1] : "";
            this.LastName = names.Length > 2 ? names[2] : "";
        }

        public bool Equals(Person other)
        {
            return other != null && (Id == other.Id ||
                   (FirstName == other.FirstName &&
                   SecondName == other.SecondName &&
                   LastName == other.LastName));
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Id, FirstName, SecondName, LastName);
        }
        public string getFullName()
        {
            return $"{this.FirstName ?? ""} {this.LastName ?? ""} {this.SecondName ?? ""}".Trim().Replace("  ", " ");
        }
        
    }

}