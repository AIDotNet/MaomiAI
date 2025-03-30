using MaomiAI.Store.Enums;
using MaomiAI.Store.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace MaomiAI.Store.Controllers;

/// <summary>
/// S3 存储.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class S3Controller : ControllerBase
{
    private readonly IServiceProvider _serviceProvider;

    public S3Controller(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    [HttpGet("s3")]
    public IActionResult aaa(string objectKey)
    {
        var s = _serviceProvider.GetRequiredKeyedService<IFileStore>(FileStoreType.Public);
        return Ok(new
        {
            Url = s.GeneratePreSignedUploadUrlAsync(objectKey, TimeSpan.FromMinutes(5)).Result
        });
    }
}
