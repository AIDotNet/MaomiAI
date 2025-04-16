// <copyright file="FileStoreHelper.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

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
    /// <returns>ObjectKey.</returns>
    public static string GetObjectKey(string md5, string fileName)
    {
        var fileExtensions = Path.GetExtension(fileName);
        return $"{DateTimeOffset.Now.ToString("yyyyMMdd")}/{md5}.{fileExtensions}";
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
}
