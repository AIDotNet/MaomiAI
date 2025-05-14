using System.Threading;
using System.Threading.Tasks;
using MaomiAI.Database;
using MaomiAI.Database.Queries;
using MaomiAI.Document.Shared.Queries;
using MaomiAI.Document.Shared.Queries.Response;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.Document.Core.Queries
{
    public class QueryWikiFileCommandHandler : IRequestHandler<QueryWikiFileCommand, QueryWikiFileListItem>
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IMediator _mediator;

        public QueryWikiFileCommandHandler(DatabaseContext databaseContext, IMediator mediator)
        {
            _databaseContext = databaseContext;
            _mediator = mediator;
        }

        public async Task<QueryWikiFileListItem> Handle(QueryWikiFileCommand request, CancellationToken cancellationToken)
        {
            var query = _databaseContext.TeamWikiDocuments.Where(x => x.WikiId == request.WikiId && x.Id == request.DocumentId);

            var result = await query.Join(_databaseContext.Files, a => a.FileId, b => b.Id, (a, b) => new QueryWikiFileListItem
            {
                DocumentId = a.Id,
                FileName = b.FileName,
                FileSize = b.FileSize,
                ContentType = b.ContentType,
                CreateTime = a.CreateTime,
                CreateUserId = a.CreateUserId,
                UpdateTime = a.UpdateTime,
                UpdateUserId = a.UpdateUserId
            }).FirstOrDefaultAsync();

            await _mediator.Send(new FillUserInfoCommand
            {
                Items = new List<AuditsInfo> { result }
            });

            return result;
        }
    }
}
