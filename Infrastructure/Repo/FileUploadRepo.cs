using Application.Contracts;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Repo
{
    /*public class FileUploadRepo:IFileUploading
    {
        private IWebHostEnvironment environment;
        private readonly AppDbContext _appDbContext;

        public FileUploadRepo(IWebHostEnvironment env, AppDbContext appDbContext)
        {
            this.environment = env;
            this._appDbContext = appDbContext;
            
        }

        public async Task< string> SaveImage(IFormFile imageFile)
        {
            var contentPath = this.environment.ContentRootPath;
            var path = Path.Combine(contentPath, "Uploads");
            if (imageFile == null || imageFile.Length == 0)
            {
                throw new ArgumentException("Image file is null or empty");
            }

            // Create a unique file name for the image
            var fileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(imageFile.FileName)}";
            var filePath = Path.Combine(path, fileName);

            // Ensure the directory exists
            Directory.CreateDirectory(path);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return filePath; // Return the path where

        }

        public bool DeleteImage(string imageFileName)
        {
            try
            {
                var wwwPath = this.environment.WebRootPath;
                var path = Path.Combine (wwwPath, "Uploads\\", imageFileName);

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                    return true;
                }
                return false;
            }
            catch(Exception ex)
            {
                return false; 
            }
        }
    }*/
}
