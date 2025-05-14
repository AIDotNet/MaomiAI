using MaomiAI.Store.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaomiAI.Store.Queries;

public class QueryFileDownloadUrlCommandHandler : IRequestHandler<QueryFileDownloadUrlCommand, QueryFileDownloadUrlCommandResponse>
{
    private readonly IServiceProvider _serviceProvider;

    public QueryFileDownloadUrlCommandHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<QueryFileDownloadUrlCommandResponse> Handle(QueryFileDownloadUrlCommand request, CancellationToken cancellationToken)
    {
        var fileStore = _serviceProvider.GetRequiredKeyedService<IFileStore>(Enums.FileVisibility.Private);
        var urls = await fileStore.GetFileUrlAsync(request.ObjectKeys, request.ExpiryDuration);
        return new QueryFileDownloadUrlCommandResponse
        {
            Urls = urls
        };
    }
}
