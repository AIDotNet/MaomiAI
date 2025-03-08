using Maomi;
using MaomiAI.Infra;

namespace PostgreScaffold;

[InjectModule<InfraCoreModule>]
public class DBModule : IModule
{
    public void ConfigureServices(ServiceContext context)
    {

    }
}
