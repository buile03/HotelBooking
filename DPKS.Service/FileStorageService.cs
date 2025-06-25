using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Service
{
    public interface IStorageService
    {
        //Task SaveFileAsync(Stream mediaBinaryStream, string fileName);
        Task<string> SaveFileAsync(Stream mediaBinaryStream, string fileName, string folderName = "user");

        Task DeleteFileAsync(string fileName, string folderName = "user");
    }
    public class FileStorageService : IStorageService
    {
        private readonly string _userContentFolder;
        private const string USER_CONTENT_FOLDER_NAME = "contents";
        private readonly string _webRootPath;
        public FileStorageService(IWebHostEnvironment webHostEnvironment)
        {
            _userContentFolder = Path.Combine(webHostEnvironment.ContentRootPath, USER_CONTENT_FOLDER_NAME);
            _webRootPath = webHostEnvironment.WebRootPath;
        }

        //public async Task SaveFileAsync(Stream mediaBinaryStream, string fileName)
        //{
        //    try
        //    {
        //        if (mediaBinaryStream == null)
        //            throw new ArgumentNullException(nameof(mediaBinaryStream));
        //        if (!Directory.Exists(_userContentFolder))
        //        {
        //            Directory.CreateDirectory(_userContentFolder);
        //        }
        //        var filePath = Path.Combine(_userContentFolder, fileName);
        //        using var output = new FileStream(filePath, FileMode.Create);
        //        await mediaBinaryStream.CopyToAsync(output);
        //    }
        //    catch (Exception ex)
        //    {
        //        //_logger.LogError(ex, $"Lỗi khi lưu file '{fileName}'");
        //        throw; // hoặc return false để tự xử lý
        //    }
        //}

        public async Task<string> SaveFileAsync(Stream mediaBinaryStream, string fileName, string folderName = "uploads")
        {
            try
            {
                var folderPath = Path.Combine(_webRootPath, folderName);

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var filePath = Path.Combine(folderPath, fileName);
                using var output = new FileStream(filePath, FileMode.Create);
                await mediaBinaryStream.CopyToAsync(output);

                return Path.Combine(folderName, fileName).Replace("\\", "/");
            }
            catch
            {
                throw;
            }
        }

        //public async Task DeleteFileAsync(string fileName)
        //{
        //    var filePath = Path.Combine(_userContentFolder, fileName);
        //    if (File.Exists(filePath))
        //    {
        //        await Task.Run(() => File.Delete(filePath));
        //    }
        //}

        public async Task DeleteFileAsync(string fileName, string folderName = "uploads")
        {
            var filePath = Path.Combine(_webRootPath, folderName, fileName);
            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
        }
    }
}
