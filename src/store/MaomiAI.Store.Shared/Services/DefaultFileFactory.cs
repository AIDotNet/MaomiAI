using Maomi;
using MaomiAI.Store.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace MaomiAI.Store.Services
{
    [InjectOnScoped]
    public class DefaultFileFactory : IFileFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public DefaultFileFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IFileStore Create(FileStoreType type)
        {
            return _serviceProvider.GetRequiredKeyedService<IFileStore>(type);
        }
    }
}