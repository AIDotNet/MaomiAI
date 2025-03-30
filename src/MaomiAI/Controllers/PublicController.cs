// <copyright file="PublicController.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Infra;
using Microsoft.AspNetCore.Mvc;

namespace MaomiAI.Controllers;

/// <summary>
/// 公共访问.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PublicController : ControllerBase
{
    private readonly SystemOptions _systemOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="PublicController"/> class.
    /// </summary>
    /// <param name="systemOptions"></param>
    public PublicController(SystemOptions systemOptions)
    {
        _systemOptions = systemOptions;
    }

    ///// <summary>
    ///// 系统访问配置.
    ///// </summary>
    ///// <returns>系统访问配置</returns>
    //[HttpGet("options")]
    //[EndpointSummary("系统访问配置.")]
    //[EndpointDescription("访问系统时的各项配置.")]
    //public PublicOptions GetPublicOptions()
    //{
    //    return new PublicOptions
    //    {
    //        ServiceUrl = _systemOptions.Server,
    //        PublicStoreUrl = _systemOptions.Server
    //    };
    //}
}
