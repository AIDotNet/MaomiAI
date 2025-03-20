// <copyright file="TeamsController.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Team.Shared.Commands;
using MaomiAI.Team.Shared.Models;
using MaomiAI.Team.Shared.Queries;
using MaomiAI.User.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MaomiAI.Controllers
{
    /// <summary>
    /// 团队管理控制器.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TeamsController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamsController"/> class.
        /// </summary>
        /// <param name="mediator">中介者.</param>
        public TeamsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// 获取团队列表.
        /// </summary>
        /// <param name="query">查询参数.</param>
        /// <returns>团队列表.</returns>
        [HttpPost("get-team-list")]
        public async Task<ActionResult<PagedResult<TeamDto>>> GetTeamList([FromQuery] GetTeamsQuery query)
        {
            return await _mediator.Send(query);
        }

        /// <summary>
        /// 根据ID获取团队.
        /// </summary>
        /// <param name="id">团队ID.</param>
        /// <returns>团队信息.</returns>
        [HttpGet("get-team/{id}")]
        public async Task<ActionResult<TeamDto>> GetTeam(Guid id)
        {
            GetTeamByIdQuery? query = new(id);
            TeamDto? result = await _mediator.Send(query);
            if (result == null)
            {
                return NotFound();
            }

            return result;
        }

        /// <summary>
        /// 创建团队.
        /// </summary>
        /// <param name="command">创建团队命令.</param>
        /// <returns>新创建的团队ID.</returns>
        [HttpPost("create-team")]
        public async Task<ActionResult<Guid>> CreateTeam(CreateTeamCommand command)
        {
            Guid teamId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetTeam), new { id = teamId }, teamId);
        }

        /// <summary>
        /// 更新团队信息.
        /// </summary>
        /// <param name="id">团队ID.</param>
        /// <param name="command">更新团队命令.</param>
        /// <returns>操作结果.</returns>
        [HttpPut("update-team/{id}")]
        public async Task<IActionResult> UpdateTeam(Guid id, UpdateTeamCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            await _mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// 删除团队.
        /// </summary>
        /// <param name="id">团队ID.</param>
        /// <returns>操作结果.</returns>
        [HttpDelete("delete-team/{id}")]
        public async Task<IActionResult> DeleteTeam(Guid id)
        {
            DeleteTeamCommand? command = new()
            {
                Id = id
            };

            await _mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// 切换团队状态.
        /// </summary>
        /// <param name="command">切换团队状态命令.</param>
        /// <returns>操作结果.</returns>
        [HttpPost("toggle-status")]
        public async Task<IActionResult> ToggleTeamStatus([FromBody] ToggleTeamStatusCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// 邀请用户加入团队.
        /// </summary>
        /// <param name="command">邀请用户加入团队命令.</param>
        /// <returns>新创建的团队成员ID.</returns>
        [HttpPost("invite-user")]
        public async Task<ActionResult<int>> InviteUser([FromBody] InviteUserToTeamCommand command)
        {
            int memberId = await _mediator.Send(command);
            return Ok(memberId);
        }

        /// <summary>
        /// 撤销用户的管理员权限.
        /// </summary>
        /// <param name="command">撤销管理员权限命令.</param>
        /// <returns>操作结果.</returns>
        [HttpPost("revoke-admin")]
        public async Task<IActionResult> RevokeAdminPermission([FromBody] RevokeAdminPermissionCommand command)
        {
            bool result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// 获取团队成员列表.
        /// </summary>
        /// <param name="teamId">团队ID.</param>
        /// <param name="query">查询参数.</param>
        /// <returns>团队成员列表.</returns>
        [HttpGet("get-team-members/{teamId}")]
        public async Task<ActionResult<PagedResult<TeamMemberDto>>> GetTeamMembers(Guid teamId,
            [FromQuery] GetTeamMembersQuery query)
        {
            query.TeamId = teamId;
            return await _mediator.Send(query);
        }
    }
}