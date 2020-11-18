namespace ProSeeker.Services.Data.Cloud
{
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using Microsoft.AspNetCore.Http;

    public interface ICloudinaryApplicationService
    {
        Task<string> UploadImageAsync(IFormFile imageFile, string imageName);

        Task DeleteImageAsync(string imageName);

        bool IsFileValid(IFormFile imageFile);
    }
}
