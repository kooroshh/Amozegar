namespace Amozegar.Utilities
{
    public static class ImageSaver
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
    }
}
