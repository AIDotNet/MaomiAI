using Maomi.MQ;
using Yitter.IdGenerator;

namespace MaomiAI.Infra.Defaults;

/// <summary>
/// Id 提供器.
/// </summary>
public class DefaultIdProvider : IIdProvider
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultIdProvider"/> class.
    /// </summary>
    /// <param name="workId"></param>
    public DefaultIdProvider(ushort workId)
    {
        var options = new IdGeneratorOptions(workId) { SeqBitLength = 10 };
        YitIdHelper.SetIdGenerator(options);
    }

    /// <inheritdoc />
    public long NextId() => YitIdHelper.NextId();

    /// <inheritdoc />
    public long GeneratorId(out string value)
    {
        var id = YitIdHelper.NextId();
        value = id.ToString("x16");
        return id;
    }

    /// <inheritdoc />
    public string GeneratorKey()
    {
        var id = YitIdHelper.NextId();
        return id.ToString("x16");
    }
}
