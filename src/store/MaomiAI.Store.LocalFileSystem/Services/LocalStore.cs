namespace MaomiAI.Store.Services;

public class LocalStore : IFileStore
{
    private readonly string _serviceUrl;
    private readonly string _storePath;

    public LocalStore(string serviceUrl, string storePath)
    {
        _serviceUrl = serviceUrl;
        _storePath = storePath;
    }

    public async Task DeleteFilesAsync(IEnumerable<string> objectKeys)
    {
        foreach (var key in objectKeys)
        {
            var filePath = Path.Combine(_storePath, key);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
        await Task.CompletedTask;
    }

    public async Task<bool> FileExistsAsync(string objectKey)
    {
        var filePath = Path.Combine(_storePath, objectKey);
        return await Task.FromResult(File.Exists(filePath));
    }

    public async Task<IReadOnlyDictionary<string, long>> GetFileSizeAsync(IEnumerable<string> objectKeys)
    {
        var result = new Dictionary<string, long>();
        foreach (var key in objectKeys)
        {
            var filePath = Path.Combine(_storePath, key);
            if (File.Exists(filePath))
            {
                var fileInfo = new FileInfo(filePath);
                result[key] = fileInfo.Length;
            }
        }
        return await Task.FromResult(result);
    }

    public async Task<IReadOnlyDictionary<string, Uri>> GetFileUrlAsync(IEnumerable<string> objectKeys, TimeSpan expiryDuration)
    {
        var result = new Dictionary<string, Uri>();
        foreach (var key in objectKeys)
        {
            var filePath = Path.Combine(_storePath, key);
            if (File.Exists(filePath))
            {
                var fileUri = new Uri(Path.Combine(_serviceUrl, key));
                result[key] = fileUri;
            }
        }
        return await Task.FromResult(result);
    }

    public async Task<Uri> UploadFileAsync(Stream inputStream, string objectKey)
    {
        var filePath = Path.Combine(_storePath, objectKey);
        using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
            await inputStream.CopyToAsync(fileStream);
        }
        return await Task.FromResult(new Uri(Path.Combine(_serviceUrl, objectKey)));
    }
}
