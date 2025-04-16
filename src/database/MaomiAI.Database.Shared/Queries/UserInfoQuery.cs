using MaomiAI.Infra.Models;
using MediatR;

namespace MaomiAI.Database.Queries;

/// <summary>
/// 用户信息查询填充.
/// </summary>
/// <typeparam name="T">带有审计属性的.</typeparam>
public class UserInfoQuery<T> : IRequest<ICollection<T>>
    where T : AuditsInfo
{
    public ICollection<T> Items { get; init; }
}
