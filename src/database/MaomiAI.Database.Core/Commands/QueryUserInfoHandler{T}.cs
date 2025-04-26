using MaomiAI.Database.Queries;
using MaomiAI.Infra.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.Database.Commands;

/// <summary>
/// 填充审计属性信息.
/// </summary>
/// <typeparam name="T">数据.</typeparam>
public class QueryUserInfoHandler<T> : IRequestHandler<UserInfoQuery<T>, ICollection<T>>
        where T : AuditsInfo
{
    private readonly DatabaseContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryUserInfoHandler{T}"/> class.
    /// </summary>
    /// <param name="dbContext"></param>
    public QueryUserInfoHandler(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc/>
    public async Task<ICollection<T>> Handle(UserInfoQuery<T> request, CancellationToken cancellationToken)
    {
        var createUserIds = request.Items.Select(x => x.CreateUserId).ToArray();
        var updateUserIds = request.Items.Select(x => x.UpdateUserId).ToArray();
        var userIds = new HashSet<Guid>();
        userIds.UnionWith(createUserIds);
        userIds.UnionWith(updateUserIds);

        var userNames = await _dbContext.Users.Where(x => userIds.Contains(x.Id)).ToDictionaryAsync(x => x.Id, x => x.UserName);

        foreach (var item in request.Items)
        {
            if (userNames.TryGetValue(item.CreateUserId, out var createUserName))
            {
                item.CreateUserName = createUserName;
            }
            else
            {
                item.CreateUserName = string.Empty;
            }
            if (userNames.TryGetValue(item.UpdateUserId, out var updateUserName))
            {
                item.UpdateUserName = updateUserName;
            }
            else
            {
                item.UpdateUserName = string.Empty;
            }
        }

        return request.Items;
    }
}
