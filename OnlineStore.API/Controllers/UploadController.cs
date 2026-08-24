using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using OnlineStore.API.Extensions;
using OnlineStore.Application.Contracts;
using OnlineStore.Domain.Constants;

namespace OnlineStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.Admin)]
    public class UploadController(IFileService fileService) : ControllerBase
    {
        public class UploadImageRequest
        {
            public IFormFile File { get; set; } = default!;
        }

        public class UploadMultipleImagesRequest
        {
            public List<IFormFile> Files { get; set; } = [];
        }

        public enum ImageCategory
        {
            Product,
            Brand,
            Profile
        }

        private static readonly string[] AllowedExtensions =
            [".jpg", ".jpeg", ".png", ".webp"];

        private static readonly string[] AllowedContentTypes =
            ["image/jpeg", "image/png", "image/webp"];

        private const long MaxFileSizeInBytes = 5 * 1024 * 1024;
        private const long MaxProfileImageSizeInBytes = 2 * 1024 * 1024;
        private const int MaxAdditionalImages = 10;

        /// <summary>
        /// Uploads a single image for the given category (Admin only).
        /// </summary>
        [HttpPost("{category}")]
        [EnableRateLimiting(RateLimitingExtensions.Upload)]
        public async Task<IActionResult> UploadImage(
            ImageCategory category,
            [FromForm] UploadImageRequest request,
            CancellationToken cancellationToken)
        {
            var file = request.File;

            if (!ValidateImageFile(file, category, out var error))
                return BadRequest(error);

            using var stream = file.OpenReadStream();

            var fileName =
                $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            var url = await fileService.UploadAsync(
                stream,
                fileName,
                category.ToString(),
                cancellationToken);

            return Ok(new
            {
                url,
                message = "File uploaded successfully."
            });
        }

        /// <summary>
        /// Uploads multiple images for the given category (Admin only).
        /// </summary>
        [HttpPost("{category}/multiple")]
        [EnableRateLimiting(RateLimitingExtensions.Upload)]
        public async Task<IActionResult> UploadMultipleImages(
            ImageCategory category,
            [FromForm] UploadMultipleImagesRequest request,
            CancellationToken cancellationToken)
        {
            if (request is null || request.Files.Count == 0)
                return BadRequest("No files provided.");

            if (request.Files.Count > MaxAdditionalImages)
                return BadRequest(
                    $"You can upload up to {MaxAdditionalImages} images at a time.");

            foreach (var file in request.Files)
            {
                if (!ValidateImageFile(file, category, out var error))
                    return BadRequest($"{file.FileName}: {error}");
            }

            var tasks = request.Files.Select(async file =>
            {
                using var stream = file.OpenReadStream();

                var fileName =
                    $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

                return await fileService.UploadAsync(
                    stream,
                    fileName,
                    category.ToString(),
                    cancellationToken);
            });

            var results = await Task.WhenAll(tasks);

            return Ok(new
            {
                files = results,
                message =
                    $"{results.Length} file(s) uploaded successfully."
            });
        }

        private static bool ValidateImageFile(
            IFormFile file,
            ImageCategory category,
            out string? error)
        {
            if (file is null || file.Length == 0)
            {
                error = "Invalid file.";
                return false;
            }

            var maxSize = category == ImageCategory.Profile
                ? MaxProfileImageSizeInBytes
                : MaxFileSizeInBytes;

            if (file.Length > maxSize)
            {
                error =
                    $"File must not exceed {maxSize / (1024 * 1024)} MB.";

                return false;
            }

            var extension =
                Path.GetExtension(file.FileName)?.ToLowerInvariant();

            if (string.IsNullOrEmpty(extension) ||
                !AllowedExtensions.Contains(extension))
            {
                error =
                    $"Accepted formats: {string.Join(", ", AllowedExtensions)}";

                return false;
            }

            if (!AllowedContentTypes.Contains(
                    file.ContentType?.ToLowerInvariant()))
            {
                error = "Invalid file content type.";
                return false;
            }

            error = null;
            return true;
        }
    }
}