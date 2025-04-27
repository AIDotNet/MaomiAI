using MaomiAI.Database;
using MaomiAI.Infra.Models;
using MaomiAI.User.Shared.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.User.Core.Handlers;

public class QueryRepeatedUserNameCommandHandler : IRequestHandler<QueryRepeatedUserNameCommand, Simple<bool>>
{
    private readonly DatabaseContext _dbContext;

    public QueryRepeatedUserNameCommandHandler(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Simple<bool>> Handle(QueryRepeatedUserNameCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _dbContext.Users
        .Where(u => u.UserName == request.UserName || u.Email == request.UserName || u.Phone == request.UserName)
        .Select(u => new { u.UserName, u.Email, u.Phone })
        .FirstOrDefaultAsync();

        if (existingUser != null)
        {
            return new Simple<bool>() { Data = true };
        }

        return new Simple<bool>() { Data = false };
    }
}
