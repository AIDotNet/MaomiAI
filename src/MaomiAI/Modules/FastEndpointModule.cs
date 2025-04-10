using FastEndpoints;
using Maomi;

namespace MaomiAI.Modules;

public class FastEndpointModule : IModule
{
    public void ConfigureServices(ServiceContext context)
    {
        context.Services.AddFastEndpoints();
    }
}
