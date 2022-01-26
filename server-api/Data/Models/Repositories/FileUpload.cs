using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using server_api.Data.Models.Interfaces;

namespace server_api.Data.Models.Repositories
{
    public class FileUpload : IFileUpload
    {
        public const string UploadDirectoryConfigurationSection = "UploadDirectory";
        private readonly string uploadPath;
        public FileUpload(IWebHostEnvironment env, IConfiguration configuration)
        {
            this.uploadPath = Path.Combine(
                env.WebRootPath,
                configuration.GetSection(UploadDirectoryConfigurationSection).Value
            );
        }
        public async Task<FileInfo> UploadAsync(IFormFile file)
        {
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadPath, fileName);
            var fileInfo = new FileInfo(filePath);
            using (var stream = fileInfo.Create())
            {
                await file.CopyToAsync(stream);
            }
            return fileInfo;
        }
        public FileInfo Upload(Uri file)
        {

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var oldName = Repository.getFileName(file);
            var fileName = Guid.NewGuid() + Path.GetExtension(oldName);
            var filePath = Path.Combine(uploadPath, fileName);
            using (System.Net.WebClient wc = new System.Net.WebClient())
            {
                try
                {
                    wc.DownloadFile(file, filePath);
                    return new FileInfo(filePath);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }
            }
        }

        public bool Delete(string fileName)
        {
            var path = Path.Combine(uploadPath, fileName);
            if (!File.Exists(path))
            {
                return false;
            }
            try
            {
                File.Delete(path);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public FileInfo GetFile(string fileName)=>new FileInfo(Path.Combine(uploadPath,fileName));
    }
}
