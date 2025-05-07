using MaomiAI.Database.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.Database.Commands;

/// <summary>
/// 填充审计属性信息.
/// </summary>
/// <typeparam name="T">数据.</typeparam>
public class FillUserInfoFuncCommandHandler : IRequestHandler<FillUserInfoFuncCommand, FillUserInfoFuncCommandResponse>
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
    public async Task<FillUserInfoFuncCommandResponse> Handle(FillUserInfoFuncCommand request, CancellationToken cancellationToken)
    {
        var userIds = request.GetUserId(request.Items);

        if (userIds.Count == 0)
        {
            return new FillUserInfoFuncCommandResponse { Items = request.Items };
        }

        var userNames = await _dbContext.Users.Where(x => userIds.Contains(x.Id)).ToDictionaryAsync(x => x.Id, x => x.NickName);

        foreach (var item in request.Items)
        {
            request.SetUserName(userNames, item);
        }

        return new FillUserInfoFuncCommandResponse { Items = request.Items };
    }
}
