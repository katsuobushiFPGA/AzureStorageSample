using AzureStorageSample.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AzureStorageSample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // 環境変数の取得
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

            // 設定ファイルの読み込み
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .Build();

            Console.WriteLine($"Current Environment: {environment}");

            // サービスプロバイダーの構築
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IConfiguration>(configuration)
                .AddSingleton<AzureStorageContext>()
                .BuildServiceProvider();

            // サービスの取得
            var context = serviceProvider.GetService<AzureStorageContext>();
            var containerClient = context.GetBlobContainerClient("sample-container");

            // コンテナが存在しない場合は作成
            await containerClient.CreateIfNotExistsAsync();
            Console.WriteLine("コンテナ 'sample-container' を作成しました。");

            // Blob操作のサンプル
            string localFilePath = "sample.txt";
            await File.WriteAllTextAsync(localFilePath, "Hello, Azure Blob Storage!");
            string blobName = Path.GetFileName(localFilePath);

            // sample.txtをコンテナにアップロードする
            var blobClient = containerClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(localFilePath, true);
            Console.WriteLine($"'{blobName}' をアップロードしました。");
        }
    }
}
