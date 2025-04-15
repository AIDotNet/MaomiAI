using MaomiAI.Infra.Models;
using MediatR;

namespace MaomiAI.Database.Queries;

public class UserInfoQuery<T> : IRequest<ICollection<T>>
    where T : AuditsInfo
{
    public ICollection<T> Items { get; init; }
}
