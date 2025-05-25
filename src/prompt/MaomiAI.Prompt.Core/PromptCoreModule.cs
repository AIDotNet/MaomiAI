using Maomi;
using MaomiAI.Prompt.Api;

namespace MaomiAI.Prompt.Core;

[InjectModule<PromptApiModule>]
public class PromptCoreModule : IModule
{
    public void ConfigureServices(ServiceContext context)
    {
    }
}
