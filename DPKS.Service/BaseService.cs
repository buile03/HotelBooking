

using DPKS.Data.EF;
using Microsoft.AspNetCore.Http;

using System.Net.Http.Headers;
using System;
using System.Threading.Tasks;
using System.IO;

namespace DPKS.Service
{
    public class BaseService
    {
        private readonly AppDbContext _context;
        private const string USER_CONTENT_FOLDER_NAME = "content";
        private readonly IStorageService _storageService;
        public string _action;
        public BaseService(AppDbContext context, IStorageService storageService = null)
        {
            _context = context;
            _storageService = storageService;
        }

        public async Task<string> SaveFile(IFormFile file)
        {
            var origanFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(origanFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return "/" + USER_CONTENT_FOLDER_NAME + "/" + fileName;

        }
        public async Task<int> SaveChange()
        {
            return await _context.SaveChangesAsync();
        }
    }
    
}
