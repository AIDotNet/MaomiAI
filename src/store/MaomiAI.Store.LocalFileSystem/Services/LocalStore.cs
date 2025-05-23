﻿//// <copyright file="LocalStore.cs" company="MaomiAI">
//// Copyright (c) MaomiAI. All rights reserved.
//// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//// Github link: https://github.com/AIDotNet/MaomiAI
//// </copyright>

//using MaomiAI.Infra.Service;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.FileProviders;
//using Microsoft.Extensions.Primitives;

//namespace MaomiAI.Store.Services;

//public class LocalStore : IFileStore
//{
//    private readonly string _serviceUrl;
//    private readonly string _storePath;
//    private readonly IAESProvider _aesProvider;

//    public LocalStore(IAESProvider aesProvider, string serviceUrl, string storePath)
//    {
//        _aesProvider = aesProvider;
//        _serviceUrl = serviceUrl;
//        _storePath = storePath;
//    }

//    public async Task DeleteFilesAsync(IEnumerable<string> objectKeys)
//    {
//        foreach (string? key in objectKeys)
//        {
//            string? filePath = Path.Combine(_storePath, key);
//            if (File.Exists(filePath))
//            {
//                File.Delete(filePath);
//            }
//        }

//        await Task.CompletedTask;
//    }

//    public async Task<bool> FileExistsAsync(string objectKey)
//    {
//        string? filePath = Path.Combine(_storePath, objectKey);
//        return await Task.FromResult(File.Exists(filePath));
//    }

//    public async Task<IReadOnlyDictionary<string, long>> GetFileSizeAsync(IEnumerable<string> objectKeys)
//    {
//        Dictionary<string, long>? result = new();
//        foreach (string? key in objectKeys)
//        {
//            string? filePath = Path.Combine(_storePath, key);
//            if (File.Exists(filePath))
//            {
//                FileInfo? fileInfo = new(filePath);
//                result[key] = fileInfo.Length;
//            }
//        }

//        return await Task.FromResult(result);
//    }

//    public async Task<IReadOnlyDictionary<string, Uri>> GetFileUrlAsync(IEnumerable<string> objectKeys,
//        TimeSpan expiryDuration)
//    {
//        Dictionary<string, Uri>? result = new();
//        foreach (string? key in objectKeys)
//        {
//            string? filePath = Path.Combine(_storePath, key);
//            if (File.Exists(filePath))
//            {
//                string? token = GenerateToken(key, expiryDuration);
//                Uri? fileUri = new($"{_serviceUrl}/{key}?token={token}");
//                result[key] = fileUri;
//            }
//        }

//        return await Task.FromResult(result);
//    }

//    public async Task<Uri> UploadFileAsync(Stream inputStream, string objectKey)
//    {
//        string? filePath = Path.Combine(_storePath, objectKey);
//        using (FileStream? fileStream = new(filePath, FileMode.Create, FileAccess.Write))
//        {
//            await inputStream.CopyToAsync(fileStream);
//        }

//        return await Task.FromResult(new Uri(Path.Combine(_serviceUrl, objectKey)));
//    }

//    public string GenerateToken(string objectKey, TimeSpan expiryDuration)
//    {
//        long expiry = DateTimeOffset.Now.Add(expiryDuration).ToUnixTimeSeconds();

//        return _aesProvider.Encrypt($"{objectKey}:{expiry}");
//    }

//    public bool ValidateToken(string objectKey, string token)
//    {
//        try
//        {
//            string? plainText = _aesProvider.Decrypt(token);

//            string[]? parts = token.Split(':');
//            if (parts.Length != 2)
//            {
//                return false;
//            }

//            if (parts[0] != objectKey)
//            {
//                return false;
//            }

//            long expiry = long.Parse(parts[1]);

//            if (expiry <= DateTimeOffset.Now.ToUnixTimeSeconds())
//            {
//                return false;
//            }

//            return true;
//        }
//        catch
//        {
//            return false;
//        }
//    }
//}


//public class LocalPhysicalFileProvider : IFileProvider
//{
//    private readonly PhysicalFileProvider _physicalFileProvider;
//    private readonly LocalStore _localStore;
//    private readonly IHttpContextAccessor _httpContextAccessor;

//    public LocalPhysicalFileProvider(string root, LocalStore localStore, IHttpContextAccessor httpContextAccessor)
//    {
//        _physicalFileProvider = new PhysicalFileProvider(root);
//        _localStore = localStore;
//        _httpContextAccessor = httpContextAccessor;
//    }

//    public IDirectoryContents GetDirectoryContents(string subpath)
//    {
//        // 不允许浏览目录
//        return NotFoundDirectoryContents.Singleton;
//    }

//    public IFileInfo GetFileInfo(string subpath)
//    {
//        HttpContext? context = _httpContextAccessor.HttpContext;
//        if (context == null)
//        {
//            return new NotFoundFileInfo(subpath);
//        }

//        // 从查询字符串中获取token
//        if (!context.Request.Query.TryGetValue("token", out StringValues token))
//        {
//            return new NotFoundFileInfo(subpath);
//        }

//        // 验证token
//        if (!_localStore.ValidateToken(subpath, token))
//        {
//            return new NotFoundFileInfo(subpath);
//        }

//        // 如果token验证通过，返回文件
//        return _physicalFileProvider.GetFileInfo(subpath);
//    }

//    public IChangeToken Watch(string filter)
//    {
//        // 不支持文件变更监控
//        return NullChangeToken.Singleton;
//    }
//}