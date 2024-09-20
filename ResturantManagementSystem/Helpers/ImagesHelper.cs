using Microsoft.AspNetCore.Hosting;
using ResturantManagementSystem.Models;
using ResturantManagementSystem.ViewModels;

namespace ResturantManagementSystem.Helpers
{
    public class ImagesHelper
    {
        public static string FoodsFolderName { get; } = "foods";
        public static string? UploadImage(IWebHostEnvironment webHostEnvironment,IFormFile Image,string folderName)
        {
            try
            {
                string ImageName = GetFileName(Image);
                var path = GetImagePath(ImageName, folderName, webHostEnvironment);
                CreateImage(ref Image, path);

                return ImageName;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public static string? EditImage(string oldImageName,string oldImageFolder, IWebHostEnvironment webHostEnvironment, IFormFile Image, string folderName)
        {
            string oldImagePath = GetImagePath(oldImageName, oldImageFolder, webHostEnvironment);
            if (!IsImageExist(oldImageName, oldImageFolder, webHostEnvironment))
            {
                return null;
            }

            string? newFileName = null;
            if(Image != null && Image.Length > 0)
            {
                newFileName = GetFileName(Image);
                var path = GetImagePath(newFileName, folderName, webHostEnvironment);

                CreateImage(ref Image, path);
                DeleteImage(oldImagePath);
            } 
            else
            {
                return null;
            }

            return newFileName;
        }

        private static bool IsImageExist(string imageName, string folderName, IWebHostEnvironment webHostEnvironment)
        {
            string imagePath = Path.Combine($"{webHostEnvironment.WebRootPath}/images/{folderName}", imageName);
            return File.Exists(imagePath);
        }
        private static void DeleteImage(string imagePath)
        {
            File.Delete(imagePath);
        }
        private static string GetImagePath(string imageName, string folderName, IWebHostEnvironment webHostEnvironment)
        {
            return Path.Combine($"{webHostEnvironment.WebRootPath}/images/{folderName}", imageName);
        }
        private static string GetFileName(IFormFile File)
        {
            return Path.GetFileName(File.FileName);
        }
        private static void CreateImage(ref IFormFile Image, string path)
        {
            using (var fileStream = File.Create(path))
            {
                Image.CopyTo(fileStream);
            }
        }
    }
}
