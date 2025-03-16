using MaomiAI.Store.Enums;

namespace MaomiAI.Store.Services;

public interface IFileFactory
{
    IFileStore Create(FileStoreType type);
}
