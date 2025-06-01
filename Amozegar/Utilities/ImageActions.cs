using Amozegar.Data.UnitOfWork;
using Amozegar.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Amozegar.Utilities
{
    public static class ImageActions
    {
        public static async Task SaveImage(this IFormFile file, string fileName, params string[] paths)
        {
            string filePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "images"
            );
            foreach (string path in paths)
            {
                filePath = Path.Combine(
                    filePath,
                    path
                );
            }

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            filePath = Path.Combine(filePath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
        }

        public static async Task SaveImages(this List<IFormFile> files, string classIdentity,  int recordId, IUnitOfWork context, string type)
        {
            if (files != null && files.Count() > 0)
            {
                int counter = await context.PictureRepository
                    .GetLastImageCountByClassIdentityByRecordIdByTypeForAddAsync(classIdentity, recordId, type);
                foreach (var file in files)
                {
                    string fileName = $"{counter}" + Path.GetExtension(file.FileName);
                    await file.SaveImage(fileName, type.ToLowerInvariant(), $"{recordId}");
                    fileName = Path.Combine($"{recordId}", fileName);
                    await context.PictureRepository
                        .AddImagesAsync(fileName, recordId, type, classIdentity);

                    counter++;
                }
                await context.SaveChangesAsync();
            }
        }

        public static async Task DeleteImages(string classIdentity,  int recordId, string type, IUnitOfWork context)
        {
            await context.PictureRepository.DeleteByClassIdentityByTypeAndRecordIdAsync(classIdentity, recordId, type);
            var imagesPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "images",
                type.ToLowerInvariant(),
                recordId.ToString()
                );
            if (Directory.Exists(imagesPath))
            {
                Directory.Delete(imagesPath, recursive: true);
            }
        }

    }
}
