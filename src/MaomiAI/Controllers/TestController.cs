// <copyright file="TestController.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using Microsoft.AspNetCore.Mvc;

namespace MaomiAI.Controllers
{
    /// <summary>
    /// 测试控制器.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly UserContext _userContext;
        private readonly ILogger<TestController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestController"/> class.
        /// </summary>
        /// <param name="userContext">用户上下文.</param>
        /// <param name="logger">日志记录器.</param>
        public TestController(
            UserContext userContext,
            ILogger<TestController> logger)
        {
            _userContext = userContext;
            _logger = logger;
        }

        /// <summary>
        /// 获取公开信息（无需认证）.
        /// </summary>
        /// <returns>公开信息.</returns>
        [HttpGet("public")]
        public IActionResult GetPublicInfo()
        {
            return Ok(new { message = "这是公开信息，无需认证即可访问" });
        }

        ///// <summary>
        ///// 获取私有信息（需要认证）.
        ///// </summary>
        ///// <returns>私有信息.</returns>
        //[HttpGet("private")]
        //public IActionResult GetPrivateInfo()
        //{
        //    try
        //    {
        //        if (!_userContext.IsAuthenticated())
        //        {
        //            return Unauthorized(new { message = "用户未认证" });
        //        }

        //        _logger.LogInformation("用户 {UserName} ({UserId}) 访问了私有信息", _userContext.UserName, _userContext.UserId);

        //        return Ok(new
        //        {
        //            message = "这是私有信息，需要认证才能访问",
        //            userId = _userContext.UserId,
        //            userName = _userContext.UserName,
        //            nickName = _userContext.NickName,
        //            email = _userContext.Email,
        //            avatarUrl = _userContext.AvatarUrl,
        //            roles = _userContext.Roles
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "获取私有信息失败");
        //        return StatusCode(500, new { message = "获取私有信息失败，请稍后重试" });
        //    }
        //}

        ///// <summary>
        ///// 获取管理员信息（需要Admin角色）.
        ///// </summary>
        ///// <returns>管理员信息.</returns>
        //[HttpGet("admin")]
        //public IActionResult GetAdminInfo()
        //{
        //    _logger.LogInformation("管理员 {UserName} ({UserId}) 访问了管理员信息", _userContext.UserName, _userContext.UserId);

        //    return Ok(new
        //    {
        //        message = "这是管理员信息，需要Admin角色才能访问",
        //        userId = _userContext.UserId,
        //        userName = _userContext.UserName,
        //        roles = _userContext.Roles
        //    });
        //}

        ///// <summary>
        ///// 获取超级管理员信息（需要同时具有Admin和SuperAdmin角色）.
        ///// </summary>
        ///// <returns>超级管理员信息.</returns>
        //[HttpGet("super-admin")]
        //public IActionResult GetSuperAdminInfo()
        //{
        //    _logger.LogInformation("超级管理员 {UserName} ({UserId}) 访问了超级管理员信息", _userContext.UserName,
        //        _userContext.UserId);

        //    return Ok(new
        //    {
        //        message = "这是超级管理员信息，需要同时具有Admin和SuperAdmin角色才能访问",
        //        userId = _userContext.UserId,
        //        userName = _userContext.UserName,
        //        roles = _userContext.Roles
        //    });
        //}
    }
}