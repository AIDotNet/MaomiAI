using Maomi;
using MaomiAI.Store.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace MaomiAI.Store.Services;

public interface IFileStore
{
    /// <summary>
    /// 上传文件并返回文件地址.
    /// </summary>
    /// <param name="inputStream"></param>
    /// <param name="objectKey"></param>
    /// <returns>文件地址.</returns>
    Task<Uri> UploadFileAsync(Stream inputStream, string objectKey);

    /// <summary>
    /// 批量获取文件地址，返回文件地址字典.
    /// </summary>
    /// <param name="objectKeys"></param>
    /// <param name="expiryDuration"></param>
    /// <returns></returns>
    Task<IReadOnlyDictionary<string, Uri>> GetFileUrlAsync(IEnumerable<string> objectKeys, TimeSpan expiryDuration);

    /// <summary>
    /// 判断文件是否存在.
    /// </summary>
    /// <param name="objectKey"></param>
    /// <returns></returns>
    Task<bool> FileExistsAsync(string objectKey);

    /// <summary>
    /// 批量获取文件大小，返回文件大小字典.
    /// </summary>
    /// <param name="objectKeys"></param>
    /// <returns></returns>
    Task<IReadOnlyDictionary<string, long>> GetFileSizeAsync(IEnumerable<string> objectKeys);

    /// <summary>
    /// 批量删除文件.
    /// </summary>
    /// <param name="objectKeys"></param>
    /// <returns></returns>
    Task DeleteFilesAsync(IEnumerable<string> objectKeys);
}

public interface IFileFactory
{
    IFileStore Create(FileStoreType type);
}

[InjectOnScoped]
public class DefaultFileFactory : IFileFactory
{
    private readonly IServiceProvider _serviceProvider;
    public DefaultFileFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public IFileStore Create(FileStoreType type) => _serviceProvider.GetRequiredKeyedService<IFileStore>(type);
}