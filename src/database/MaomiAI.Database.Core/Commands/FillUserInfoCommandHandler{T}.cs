using MaomiAI.Database.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.Database.Commands;

/// <summary>
/// 填充审计属性信息.
/// </summary>
/// <typeparam name="T">数据.</typeparam>
public class FillUserInfoCommandHandler<T> : IRequestHandler<FillUserInfoCommand<T>, ICollection<T>>
        where T : AuditsInfo
{
    private readonly DatabaseContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="FillUserInfoCommandHandler{T}"/> class.
    /// </summary>
    /// <param name="dbContext"></param>
    public FillUserInfoCommandHandler(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc/>
    public async Task<ICollection<T>> Handle(FillUserInfoCommand<T> request, CancellationToken cancellationToken)
    {
        var createUserIds = request.Items.Select(x => x.CreateUserId).ToArray();
        var updateUserIds = request.Items.Select(x => x.UpdateUserId).ToArray();
        var userIds = new HashSet<Guid>();
        userIds.UnionWith(createUserIds);
        userIds.UnionWith(updateUserIds);

        if (userIds.Count == 0)
        {
            return request.Items;
        }

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

/// <summary>
/// 填充审计属性信息.
/// </summary>
/// <typeparam name="T">数据.</typeparam>
public class FillUserInfoFuncCommandHandler<T> : IRequestHandler<FillUserInfoFuncCommand<T>, ICollection<T>>
{
    private readonly DatabaseContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="FillUserInfoFuncCommandHandler{T}"/> class.
    /// </summary>
    /// <param name="dbContext"></param>
    public FillUserInfoFuncCommandHandler(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc/>
    public async Task<ICollection<T>> Handle(FillUserInfoFuncCommand<T> request, CancellationToken cancellationToken)
    {
        var userIds = request.GetUserId(request.Items);

        if (userIds.Count == 0)
        {
            return request.Items;
        }

        var userNames = await _dbContext.Users.Where(x => userIds.Contains(x.Id)).ToDictionaryAsync(x => x.Id, x => x.UserName);

        foreach (var item in request.Items)
        {
            request.SetUserName(userNames, item);
        }

        return request.Items;
    }
}
