namespace ProSeeker.Services.Data.Cloud
{
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;
    using ProSeeker.Common;

    public class CloudinaryApplicationService : ICloudinaryApplicationService
    {
        private readonly Cloudinary cloudinary;

        public CloudinaryApplicationService(Cloudinary cloudinary)
        {
            this.cloudinary = cloudinary;
        }

        public async Task DeleteImageAsync(string imageName)
        {
            var deletionParams = new DeletionParams(imageName);
            await this.cloudinary.DestroyAsync(deletionParams);
        }

        // This image validation checks only for the file postfix
        public bool IsFileValid(IFormFile imageFile)
        {
            if (imageFile == null)
            {
                return true;
            }

            var imageContentType = imageFile.ContentType;
            var validImagesTypes = new string[]
            {
                "image/gif", "image/jpeg", "image/jpg", "image/png", "image/svg", "image/x-png",
            };

            if (!validImagesTypes.Contains(imageContentType))
            {
                return false;
            }

            return true;
        }

        public async Task<string> UploadImageAsync(IFormFile imageFile, string imageName)
        {
            if (imageFile == null || !this.IsFileValid(imageFile))
            {
                return GlobalConstants.DefaultProfileImagePath;
            }

            byte[] destinationImage;

            using (var memoryStream = new MemoryStream())
            {
                await imageFile.CopyToAsync(memoryStream);
                destinationImage = memoryStream.ToArray();
            }

            using (var memoryStream = new MemoryStream(destinationImage))
            {
                // Cloudinary doesn't support work with the following symbols: [ >, <, &, ?, %, #, \ ]
                imageName = imageName.Replace(">", "greater");
                imageName = imageName.Replace("<", "lower");
                imageName = imageName.Replace("&", "And");
                imageName = imageName.Replace("?", "qMark");
                imageName = imageName.Replace("%", "percent");
                imageName = imageName.Replace("#", "hashTag");
                imageName = imageName.Replace("\\", "dash");

                var uploadParameters = new ImageUploadParams()
                {
                    File = new FileDescription(imageName, memoryStream),

                    // We set the image name as a ID of the file. Otherwise it gets randomly generated. This will help find it easier and delete it afterwards.
                    PublicId = imageName,
                };

                var uploadedResult = await this.cloudinary.UploadAsync(uploadParameters);

                // We need only the newly generated URI of the uploaded image
                return uploadedResult.SecureUrl.AbsoluteUri;
            }
        }
    }
}
