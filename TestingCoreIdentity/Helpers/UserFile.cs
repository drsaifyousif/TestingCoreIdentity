using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace TestingCoreIdentity.Helpers
{
    public static class UserFile
    {
        public static bool DeleteOldImage(string WebRootPath, string folder, string oldFileNameToBeDeleted)
        {
            var uploadfolder = Path.Combine(WebRootPath, folder);
            if (!string.IsNullOrEmpty(oldFileNameToBeDeleted))
            {
                var oldfile = Path.Combine(uploadfolder, oldFileNameToBeDeleted);
                if (File.Exists(oldfile))
                {
                    File.Delete(oldfile);
                    return true;
                }
            }
            return false;
        }

        public static bool DeleteOldFile(string WebRootPath, string folder, string oldFileNameToBeDeleted)
        {
            return DeleteOldImage(WebRootPath, folder, oldFileNameToBeDeleted);
        }

        public static async Task<string> UploadeNewImageAsync(string oldFileNameToBeDeleted, IFormFile uploadedFile, string WebRootPath, string folder, int width, int height)
        {
            if ((uploadedFile != null) && (uploadedFile.Length > 0))
            {
                if (!string.IsNullOrEmpty(oldFileNameToBeDeleted) && !string.IsNullOrWhiteSpace(oldFileNameToBeDeleted))
                    DeleteOldImage(WebRootPath, folder, oldFileNameToBeDeleted);
                string newfilename = Path.GetFileNameWithoutExtension(uploadedFile.FileName);
                newfilename = newfilename + DateTime.Now.ToString("yymmssfff") + ".jpg";
                var uploadfolder = Path.Combine(WebRootPath, folder);

                if (!Directory.Exists(uploadfolder))
                {
                    Directory.CreateDirectory(uploadfolder);
                }
                var newfilepath = Path.Combine(uploadfolder, newfilename);
                using (var fileStream = new FileStream(newfilepath, FileMode.Create))
                {
                    using (Image img = Image.FromStream(uploadedFile.OpenReadStream()))
                    {
                        Stream ms = new MemoryStream(img.Resize(400, 300).ToByteArray());
                        FileStreamResult fsr = new FileStreamResult(ms, "image/jpg");
                        await fsr.FileStream.CopyToAsync(fileStream);
                    }
                }

                return newfilename;
            }
            return oldFileNameToBeDeleted;
        }

        internal static Task<string> UploadeNewImageAsync(string imageUrl, IFormFile myfile, object webRootPath, object imgFolder, int v1, int v2)
        {
            throw new NotImplementedException();
        }

        public static async Task<string> UploadeNewFileAsync(string oldFileNameToBeDeleted, IFormFile uploadedFile, string WebRootPath, string folder)
        {
            if ((uploadedFile != null) && (uploadedFile.Length > 0))
            {
                if (!string.IsNullOrEmpty(oldFileNameToBeDeleted) && !string.IsNullOrWhiteSpace(oldFileNameToBeDeleted))
                    DeleteOldImage(WebRootPath, folder, oldFileNameToBeDeleted);
                string newfilename = Path.GetFileNameWithoutExtension(uploadedFile.FileName);
                string fileextention = Path.GetExtension(uploadedFile.FileName);
                newfilename = newfilename + DateTime.Now.ToString("yymmssfff") + fileextention;
                var uploadfolder = Path.Combine(WebRootPath, folder);

                if (!Directory.Exists(uploadfolder))
                {
                    Directory.CreateDirectory(uploadfolder);
                }
                var newfilepath = Path.Combine(uploadfolder, newfilename);
                using (var fileStream = new FileStream(newfilepath, FileMode.Create))
                {
                    await fileStream.CopyToAsync(fileStream);
                }

                return newfilename;
            }
            return oldFileNameToBeDeleted;
        }


    

    }
}
