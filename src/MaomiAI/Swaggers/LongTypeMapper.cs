using NJsonSchema;
using NJsonSchema.Generation.TypeMappers;

namespace MaomiAI.Swaggers;

// Swagger 模型类过滤器
public class LongTypeMapper : ITypeMapper
{
    Type ITypeMapper.MappedType => typeof(long);

    bool ITypeMapper.UseReference => false;

    void ITypeMapper.GenerateSchema(JsonSchema schema, TypeMapperContext context)
    {
        schema.Type = JsonObjectType.String;
        schema.Format = JsonFormatStrings.Long;
        schema.Example = "1415926535897934852";
        schema.Minimum = 0;
    }
}
