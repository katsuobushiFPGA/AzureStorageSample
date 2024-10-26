using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;

namespace AzureStorageSample.Context
{
    public class AzureStorageContext
    {
        public BlobServiceClient BlobServiceClient { get; }

        public AzureStorageContext(IConfiguration configuration)
        {
            // appsettings.jsonから接続文字列を取得
            string connectionString = configuration.GetConnectionString("AzureStorageConnection");
            BlobServiceClient = new BlobServiceClient(connectionString);
        }

        public BlobContainerClient GetBlobContainerClient(string containerName)
        {
            return BlobServiceClient.GetBlobContainerClient(containerName);
        }
    }
}
