using Azure.Identity;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;

namespace AzureStorageSample.Context
{
    public class AzureStorageContext
    {
        public BlobServiceClient BlobServiceClient { get; }

        public AzureStorageContext(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("AzureStorageConnection");
#if DEBUG
            BlobServiceClient = new BlobServiceClient(connectionString);
#else
            BlobServiceClient = new BlobServiceClient(
                    new Uri("https://samplekbushi.blob.core.windows.net"),
                    new DefaultAzureCredential()
            );
#endif
        }

        public BlobContainerClient GetBlobContainerClient(string containerName)
        {
            return BlobServiceClient.GetBlobContainerClient(containerName);
        }
    }
}
