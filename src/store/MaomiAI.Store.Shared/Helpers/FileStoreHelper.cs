﻿// <copyright file="FileStoreHelper.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

#pragma warning disable CA1054 // 类 URI 参数不应为字符串

namespace MaomiAI.Team.Shared.Helpers;

/// <summary>
/// 文件存储助手类.
/// </summary>
public static class FileStoreHelper
{
    /// <summary>
    /// 常用的图片扩展名.
    /// </summary>
    public static readonly IReadOnlyCollection<string> ImageExtensions = new string[]
    {
        ".jpg",
        ".jpeg",
        ".png",
        ".gif",
        ".bmp",
        ".webp",
        ".svg",
        ".tiff",
        ".ico"
    };

    /// <summary>
    /// 常用文档格式.
    /// </summary>
    public static readonly IReadOnlyCollection<string> DocumentFormats = new string[]
    {
        ".md",     // Markdown
        ".pdf",    // Portable Document Format
        ".doc",    // Microsoft Word 97-2003
        ".docx",   // Microsoft Word
        ".xls",    // Microsoft Excel 97-2003
        ".xlsx",   // Microsoft Excel
        ".ppt",    // Microsoft PowerPoint 97-2003
        ".pptx",   // Microsoft PowerPoint
        ".txt",    // Plain Text
        ".rtf",    // Rich Text Format
        ".odt",    // OpenDocument Text
        ".ods",    // OpenDocument Spreadsheet
        ".odp",    // OpenDocument Presentation
        ".csv",    // Comma-Separated Values
        ".json",   // JSON (JavaScript Object Notation)
        ".xml",    // XML (eXtensible Markup Language)
        ".html",   // HTML (HyperText Markup Language)
        ".htm",    // HTML (HyperText Markup Language)
        ".epub",   // EPUB (Electronic Publication)
        ".mobi",   // MOBI (Mobipocket)
        ".ps",     // PostScript
        ".tex",    // LaTeX Source Document
        ".dvi",    // Device Independent File Format (LaTeX)
        ".djvu",   // DjVu
        ".msg",    // Microsoft Outlook Email Message
        ".eml",    // EML (Email Message)
        ".xps",    // XML Paper Specification
        ".gdoc",   // Google Docs
        ".gsheet", // Google Sheets
        ".gslides" // Google Slides
    };

    /// <summary>
    /// 生成文件 ObjectKey.
    /// </summary>
    /// <param name="md5"></param>
    /// <param name="fileName">文件名称.</param>
    /// <param name="prefix"></param>
    /// <returns>ObjectKey.</returns>
    public static string GetObjectKey(string md5, string fileName, string? prefix = "")
    {
        var fileExtensions = Path.GetExtension(fileName);
        fileExtensions = fileExtensions.TrimStart('.');

        var objectKey = $"{md5}.{fileExtensions}";
        if (!string.IsNullOrEmpty(prefix))
        {
            return $"{prefix}/{objectKey}";
        }

        return objectKey;
    }

    /// <summary>
    /// 根据给定文件大小计算允许的上限值（大出 10%）.
    /// </summary>
    /// <param name="fileSize">文件大小（字节）.</param>
    /// <returns>允许的上限值（字节）.</returns>
    public static long GetAllowedFileSizeLimit(long fileSize)
    {
        return (long)(fileSize * 1.1);
    }

    /// <summary>
    /// 根据给定文件大小计算允许的上限值（大出 10%）.
    /// </summary>
    /// <param name="fileSize">文件大小（字节）.</param>
    /// <returns>允许的上限值（字节）.</returns>
    public static int GetAllowedFileSizeLimit(int fileSize)
    {
        return (int)(fileSize * 1.1);
    }

    /// <summary>
    /// 拼接 URL 地址.
    /// </summary>
    /// <param name="baseUrl">前缀地址.</param>
    /// <param name="relativePath">后缀地址.</param>
    /// <returns>拼接后的完整 URL 地址.</returns>
    public static Uri CombineUrl(string baseUrl, string relativePath)
    {
        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            throw new ArgumentException("Base URL 不能为空.", nameof(baseUrl));
        }

        if (string.IsNullOrWhiteSpace(relativePath))
        {
            throw new ArgumentException("Relative path 不能为空.", nameof(relativePath));
        }

        return new Uri($"{baseUrl.TrimEnd('/')}/{relativePath.TrimStart('/')}");
    }
}
