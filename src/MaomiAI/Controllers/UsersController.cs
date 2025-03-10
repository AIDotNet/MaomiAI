// <copyright file="UsersController.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.User.Shared;
using MaomiAI.User.Shared.Commands;
using MaomiAI.User.Shared.Models;
using MaomiAI.User.Shared.Queries;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaomiAI.Controllers;

/// <summary>
/// 用户管理控制器.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="UsersController"/> class.
    /// </summary>
    /// <param name="mediator">中介者.</param>
    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// 获取用户列表.
    /// </summary>
    /// <param name="query">查询参数.</param>
    /// <param name="cancellationToken">取消令牌.</param>
    /// <returns>用户列表.</returns>
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<PagedResult<UserDto>>> GetUsers(
        [FromQuery] GetUsersQuery query,
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(query, cancellationToken);
    }

    /// <summary>
    /// 根据ID获取用户.
    /// </summary>
    /// <param name="id">用户ID.</param>
    /// <param name="cancellationToken">取消令牌.</param>
    /// <returns>用户信息.</returns>
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<UserDto>> GetUser(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery { Id = id };
        var result = await _mediator.Send(query, cancellationToken);
        if (result == null)
        {
            return NotFound();
        }

        return result;
    }

    /// <summary>
    /// 创建用户.
    /// </summary>
    /// <param name="command">创建用户命令.</param>
    /// <param name="cancellationToken">取消令牌.</param>
    /// <returns>新创建的用户ID.</returns>
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateUser(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var userId = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetUser), new { id = userId }, userId);
    }

    /// <summary>
    /// 更新用户信息.
    /// </summary>
    /// <param name="id">用户ID.</param>
    /// <param name="command">更新用户命令.</param>
    /// <param name="cancellationToken">取消令牌.</param>
    /// <returns>操作结果.</returns>
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateUser(Guid id, UpdateUserCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// 删除用户.
    /// </summary>
    /// <param name="id">用户ID.</param>
    /// <param name="cancellationToken">取消令牌.</param>
    /// <returns>操作结果.</returns>
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteUser(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteUserCommand { Id = id };
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// 修改密码.
    /// </summary>
    /// <param name="command">修改密码命令.</param>
    /// <param name="cancellationToken">取消令牌.</param>
    /// <returns>操作结果.</returns>
    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword(ChangePasswordCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// 用户登录.
    /// </summary>
    /// <param name="command">登录命令.</param>
    /// <param name="cancellationToken">取消令牌.</param>
    /// <returns>登录结果.</returns>
    [HttpPost("login")]
    public async Task<ActionResult<LoginResult>> Login(LoginCommand command, CancellationToken cancellationToken)
    {
        return await _mediator.Send(command, cancellationToken);
    }

    /// <summary>
    /// 启用或禁用用户.
    /// </summary>
    /// <param name="command">启用或禁用用户命令.</param>
    /// <param name="cancellationToken">取消令牌.</param>
    /// <returns>操作结果.</returns>
    [HttpPost("toggle-status")]
    [Authorize]
    public async Task<IActionResult> ToggleUserStatus(
        [FromBody] ToggleUserStatusCommand command,
        CancellationToken cancellationToken = default)
    {
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }
} 