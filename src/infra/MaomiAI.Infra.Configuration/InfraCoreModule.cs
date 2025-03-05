using MaomAI.Infra;
using Maomi;

namespace MaomiAI.Infra;

[InjectModule<InfraSharedModule>]
public class InfraConfigurationModule : IModule
{
    public void ConfigureServices(ServiceContext context)
    {
    }
}
