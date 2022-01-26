using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using server_api.Data.Models;
using server_api.Data.Models.Interfaces;
using server_api.Data.Models.Repositories;
using server_api.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace server_api.Controllers
{
    public class ImageController : Controller
    {
        private static string[] avalaibeType = { "jpg", "jpeg","png", "gif", "icon", "png", "tiff", "wmf", "webp" };
        public const string UploadDirectoryConfigurationSection = "UploadDirectory";//Откуда берем изображения
        private readonly IFileRepository fileRepository;

        private readonly string uploadPath;
        public ImageController(IFileRepository fileRepository,IConfiguration configuration, IWebHostEnvironment env)
        {
            this.fileRepository = fileRepository;
            this.uploadPath = Path.Combine(
                env.WebRootPath,
                configuration.GetSection(UploadDirectoryConfigurationSection).Value
            );
        }
        public static bool isAvalaibleType(string type) => Array.IndexOf(avalaibeType, type.ToLower()) >= 0;
        
        //Выводит изображение по id
        public async Task<IActionResult> GetImageAsync(int id) {
            //Находим имя файл по id
            var fileName = (await this.fileRepository.Read(id)).RealName;
            //Формируем путь по имени и заданой в настройках папке с изоображениями
            var path = string.Concat(uploadPath, fileName);
            //Берем его расширение
            var extension = Path.GetExtension(fileName);
            //Проверяем что расширение из обрабатывемых
            if (!isAvalaibleType(extension))
            {
                throw new ArgumentException($"Type {extension} not avalaible");
            }
            //Формируем MIME type 
            string mimeType;
            new FileExtensionContentTypeProvider().TryGetContentType(fileName, out mimeType);
            //Выводим изображение
            return new FileStreamResult(new FileStream(path, FileMode.Open), $"image/{mimeType}");
        }
    }
}
