using System.Diagnostics.Metrics;

namespace WebFrontToBack.Utilities.Extensions;

public static class FileExtencion
{
    public static bool CheckContentType(this IFormFile file, string contentType) 
    {
        return file.ContentType.Contains(contentType);
    }
    public static bool CheckFileSize(this IFormFile file, double size)
    {
        return file.Length / 1024 < size;
    }
    public async static Task<string> SaveAsync(this IFormFile file,string root)       
    {
        string filename = Guid.NewGuid().ToString() + file.FileName;
        string resultPath = Path.Combine(root, filename);

        using (FileStream fileStream = new FileStream(resultPath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }
        return filename;
    }
}
