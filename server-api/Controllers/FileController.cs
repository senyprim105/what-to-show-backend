using System.IO;
using System;
using Microsoft.AspNetCore.Mvc;
using server_api.Data.Models;
using server_api.Data.Models.Interfaces;
using server_api.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using server_api.Data.Models.Repositories;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;
using server_api.Infrastructure;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;

namespace server_api.Controllers
{
    [Authorize(Roles = "admins")]
    public class FileController : Controller
    {
        private IFileRepository fileRepository;
        private IFileUpload fileUpload;
        private readonly UserManager<User> userManager;
        public FileController(IFileRepository fileRepository, IFileUpload fileUpload, UserManager<User> userManager)
        {
            this.fileRepository = fileRepository;
            this.fileUpload = fileUpload;
            this.userManager = userManager;
        }

        private (int?,int?) getImageDimention(Stream stream)
        {
            (int?, int?) result = (null, null);
            try
            {
                var image = Image.Load(stream);
                result= (image.Width, image.Height);
            }
            catch (Exception) { }
            return result;
        }
        private (int?, int?) getVideoDimention(Stream stream)
        {
            (int?, int?) result = (null, null);
            try
            {
                var image = Image.Load(stream);
                result = (image.Width, image.Height);
            }
            catch (Exception) { }
            return result;
        }

        [TempDataToModelStateErrors]
        public IActionResult Index()
        {
            var files = fileRepository.ReadAll.Include(file=>file.User);
            return View(files);
        }
        public IActionResult Create() => View();
        [HttpPost]
        [ModelStateErrorsToTempData]
        public async System.Threading.Tasks.Task<IActionResult> CreateAsync(FileModel model)
        {
            if (ModelState.IsValid)
            {
                string newFileName = null;
                if (!model.isUrl)
                {
                    newFileName = (await fileUpload.UploadAsync(model.File)).Name;
                }
                else
                {
                    newFileName = fileUpload.Upload(new Uri(model.FullPath)).Name;
                }
                string hash = Repository.getSHA1Hash(await System.IO.File.ReadAllBytesAsync(fileUpload.GetFile(newFileName).FullName));
                try { 
                    
                    var file = new FileApp(model.FullPath,newFileName, hash,  model.Description,
                    await userManager.GetUserAsync(HttpContext.User), model.FileType);
                    await fileRepository.Create(file);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    ModelState.AddModelError("", $"Файл не сохранен - {e.Message}");
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(model);
            }
        }
        [HttpPost]
        [ModelStateErrorsToTempData]
        public async Task<IActionResult> Delete(int Id)
        {
            var file = await fileRepository.Read(Id);
            // Если файл с таким id существует
            if (file != null)
            {
                await fileRepository.Delete(Id);
                // И у него есть созхраненное имя (RealName)
                if (!string.IsNullOrWhiteSpace(file.RealName))
                {
                    // Пытаемся удалить
                    if (!fileUpload.Delete(file.RealName))
                    {
                        ModelState.AddModelError("", "Файл не был удален");
                    }
                }
            }
            else // Если же файла стаким id нет 
            {
                ModelState.AddModelError("", "Файл отстутствует");
            }
            // B итоге переходим на Index
            return RedirectToAction(nameof(Index));
        }

        [AcceptVerbsAttribute("GET", "POST")]
        public IActionResult Verify(IFormFile file, string fileName)
        {
            var validate = FileModel.Verify(file, fileName);
            if (validate.Count == 0) return Json(true);
            return Json(validate[0]);
        }

    }
}
