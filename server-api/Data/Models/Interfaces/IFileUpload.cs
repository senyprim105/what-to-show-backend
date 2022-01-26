using System.IO;
using System;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace server_api.Data.Models.Interfaces
{
    public interface IFileUpload
    {
        Task<FileInfo> UploadAsync(IFormFile file);
        FileInfo Upload(Uri file);
        bool Delete(string fileName);
        FileInfo GetFile(string fileName);
    }
}
