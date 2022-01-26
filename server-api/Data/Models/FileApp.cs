using System.Runtime.Serialization;
using System;
using System.IO;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using server_api.Data.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using server_api.Data.Models.Repositories;

namespace server_api.Data.Models
{
    public enum FileAppType {
        [Display(Name = "Image File")]
        image,
        [Display(Name = "Video File")]
        video,
        [Display(Name = "File")]
        file,
    }
    public class FileApp:ISimpleEntity<int>,IEquatable<FileApp>
    {
        public int Id {get;set;}
        public FileAppType Type {get;set;}
        public User User {get;set;}
        [IgnoreDataMember]
        public string Name {get=>OldName;}
        public string OldName {get;set;}
        public string OldPath{get;set;}
        public string RealName{get;set;}
        public string Description {get;set;}
        public int? Height { get; set; } 
        public int? With { get; set; } 
        public DateTime Upload{get;set;}
        public string Hash {get;set;}
        [InverseProperty(nameof(Movie.BackgroundImage))]
        public virtual ICollection<Movie> WithBackgroundImage{get;set;}
        [InverseProperty(nameof(Movie.Poster))]
        public virtual ICollection<Movie> WithPoster{get;set;}
        [InverseProperty(nameof(Movie.Image))]
        public virtual ICollection<Movie> WithImage{get;set;}
        [InverseProperty(nameof(Movie.Preview))]
        public virtual ICollection<Movie> WithPreview{get;set;}
        [InverseProperty(nameof(Movie.Video))]
        public virtual ICollection<Movie> WithVideo{get;set;}

        public bool Equals(FileApp other)
        {
            return other != null
                && Id == other.Id;
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as FileApp);
        }

        public override int GetHashCode()
        {
            return Id;
        }
        public FileApp(
            string fullPath,
            string newName,
            string hash,
            string description=null,
            User user=null,
            FileAppType type = FileAppType.file,
            DateTime? upload = null, 
            int? width=null, 
            int? height=null)
            {
            this.Upload = upload??DateTime.Now;
            this.OldName = Repository.getFileName(fullPath);
            this.OldPath = Path.GetDirectoryName(fullPath);
            this.RealName = newName;
            this.Description = description;
            this.Hash = hash;
            this.User = user;
            this.Type=type;
            this.With = width;
            this.Height = height;
        }
        public FileApp(){}
    }
}