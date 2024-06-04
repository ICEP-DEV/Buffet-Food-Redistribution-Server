using Application.Contracts;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Repo
{
    public class FileUploadRepo:IFileUploading
    {
        private IWebHostEnvironment environment;
        private readonly AppDbContext _appDbContext;

        public FileUploadRepo(IWebHostEnvironment env, AppDbContext appDbContext)
        {
            this.environment = env;
            this._appDbContext = appDbContext;
            
        }

        public async Task<Tuple<int, string>> SaveImage(IFormFile imageFile)
        {
            try
            {
                var contentPath = this.environment.ContentRootPath;
                var path = Path.Combine(contentPath, "Uploads");

                if(!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);

                }

                
                // Check allowed extensions
                var ext = Path.GetExtension(imageFile.FileName);
                var allowedExtensions = new string[] { "jpg", "png", "jpeg" };

                if(allowedExtensions.Contains(ext))
                {
                    string msg = string.Format("Only {0} extensions are allowed", string.Join(",",allowedExtensions));
                    return new Tuple<int, string> ( 0, msg );
                }

                string uniqueString = Guid.NewGuid().ToString();
                 
                // Trying to create a unique filename here
                var newFileName = uniqueString + ext;
                var fileWithPath = Path.Combine(path, newFileName);
                var stream = new FileStream(fileWithPath, FileMode.Create);
                imageFile.CopyTo(stream);
                stream.Close();

                


                return new Tuple <int, string> ( 0, newFileName );
                
                 

            }
            catch(Exception ex ) 
            {
                return new Tuple<int, string>(0, "Error has occured"); 
            }


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
    }
}
