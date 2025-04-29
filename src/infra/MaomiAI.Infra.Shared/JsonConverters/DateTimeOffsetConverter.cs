using System.Text.Json;
using System.Text.Json.Serialization;

namespace MaomiAI.Infra.JsonConverters;

public class DateTimeOffsetConverter : JsonConverter<DateTimeOffset>
{
    public override void Write(Utf8JsonWriter writer, DateTimeOffset date, JsonSerializerOptions options)
    {
        writer.WriteStringValue(date.ToUnixTimeMilliseconds().ToString());
    }

    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString() ?? throw new JsonException($"Invalid date format,position: {reader.Position}");
        if (!long.TryParse(value, out var source))
        {
            throw new JsonException($"Invalid date format: {value}");
        }

        return DateTimeOffset.FromUnixTimeMilliseconds(source);
    }
}