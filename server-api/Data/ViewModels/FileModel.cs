using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using server_api.Data.Models;

namespace server_api.Data.ViewModels
{
    public class FileModel
    {
        public static Regex urlValidate = new Regex(@"(https?://(?:www\.|(?!www))[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|www\.[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|https?://(?:www\.|(?!www))[a-zA-Z0-9]+\.[^\s]{2,}|www\.[a-zA-Z0-9]+\.[^\s]{2,})");
        const string FileValidateMessage = "Загрузите файл или введите Url ресурса";
        public IFormFile File {get;set;}
        [Required(ErrorMessage = "Необходим Url или путь к файлу")]
        [Remote(controller:"File", action:"Verify", AdditionalFields = nameof(File))]
        public string FullPath{get;set;}
        public string Description{get;set;}
        public FileAppType FileType {get;set;}

        public static IList<string> Verify(IFormFile file, string fileName){
            var errors = new List<string>();
            var isUrl = urlValidate.IsMatch(fileName??"");
            if (isUrl || file!=null) return errors;
            errors.Add(FileValidateMessage);
            return errors;
        }
        public bool isUrl =>urlValidate.IsMatch(this.FullPath??"");
    }
}
