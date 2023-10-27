using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Minio;
using WordApp.Persistence.Interfaces;

namespace WordApp.Persistence.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly IConfiguration _config;
        public string BucketName => _config.GetValue<string>("S3ImageStorageOptions:BucketName");
        public string Endpoint => _config.GetValue<string>("S3ImageStorageOptions:Endpoint");
        public string Password => _config.GetValue<string>("S3ImageStorageOptions:Password");
        public string PersonalAccauntDomain => _config.GetValue<string>("S3ImageStorageOptions:PersonalAccauntDomain");
        public string Region => _config.GetValue<string>("S3ImageStorageOptions:Region");
        public string User => _config.GetValue<string>("S3ImageStorageOptions:User");
        public string ImageBaseUrl => _config.GetValue<string>("S3ImageStorageOptions:ImageBaseUrl");
        public ImageRepository(IConfiguration config)
        {
            _config = config;
        }
        public async Task<string> AddImage(string imageBase64, string imageName)
        {
            imageBase64 = imageBase64.Replace("data:image/png;base64,", "");

            byte[] imageAsBytes = Convert.FromBase64String(imageBase64);

            using (MemoryStream ms = new MemoryStream(imageAsBytes))
            {

                var file = new FormFile(ms, 0, ms.Length, "name", "png")
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/png"
                };

                var minio = new MinioClient()
                    .WithEndpoint(Endpoint)
                    .WithCredentials(User, Password)
                    .WithSSL()
                    .WithRegion(Region)
                    .Build();
                    
                var args = new PutObjectArgs()
                       .WithBucket(BucketName)
                       .WithObject(imageName)
                       .WithStreamData(file.OpenReadStream())
                       .WithObjectSize(file.Length)
                       .WithContentType("image/png");

                try
                {
                    await minio.PutObjectAsync(args);
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return String.Concat("https://yaelapp.ru/img/", imageName);
        }
    }
}
