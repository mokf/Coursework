using API.Helpers;
using API.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace API.Services;

public class PhotoService : IPhotoService
{
    private readonly Cloudinary _cloudinary;
    public PhotoService(IOptions<CloudinarySettings> configuration)
    {
        var account = new Account
        (
          "dqau7hcrx",
          "945199616739242",
          "e6FqZ8w0ufuQN9G-Sxb3sA7ES9k"
        );

        _cloudinary = new Cloudinary(account);
    }
    
    public async Task<ImageUploadResult> AddPhotoAsync(IFormFile formFile)
    {
        var uploadResult = new ImageUploadResult();

        if (formFile.Length > 0)
        {
            using var stream = formFile.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(formFile.FileName, stream),
                Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face"),
                Folder = "rent-apartment"
            };

            uploadResult = await _cloudinary.UploadAsync(uploadParams);
        }

        return uploadResult;
    }

    public async Task<DeletionResult> DeletePhotoAsync(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);

        return await _cloudinary.DestroyAsync(deleteParams);
    }
}