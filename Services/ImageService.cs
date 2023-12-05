using Blog.Enums;
using Blog.Services.Interfaces;

namespace Blog.Services
{
    public class ImageService : IImageService

    {
        private readonly string? _defaultBlogUserImage = "/img/BlogUser.png";
        private readonly string? _defaultBlogPostImage = "/img/BitsAndBytes2.svg";
        private readonly string? _defaultCategoryImage = "/img/BlogCategories.svg";
        private readonly string? _defaultAuthorImage = "/img/BlogAuthor.png";


        public string? ConvertByteArrayToFile(byte[]? fileData, string? extension, DefaultImage defaultImage)
        {
            try
            {
                if (fileData == null || fileData.Length == 0)
                {
                    // show default
                  
                    switch(defaultImage)
                    {
                        case DefaultImage.AuthorImage: return _defaultAuthorImage;
                        case DefaultImage.BlogPostImage: return _defaultBlogPostImage;
                        case DefaultImage.CategoryImage: return _defaultCategoryImage;
                        case DefaultImage.BlogUserImage: return _defaultBlogUserImage;
                    }

                }
    
                string? imageBase64Data = Convert.ToBase64String(fileData!);
                imageBase64Data = string.Format($"data: {extension}; base64, {imageBase64Data}");

                return imageBase64Data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<byte[]> ConvertFileToByteArrayAsync(IFormFile? file)
        {
            try
            {
                if (file != null)
                {
                    using MemoryStream memoryStream = new MemoryStream();
                    await file.CopyToAsync(memoryStream);
                    byte[] byteFile = memoryStream.ToArray();
                    memoryStream.Close();

                    return byteFile;
                }

                return null!;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
