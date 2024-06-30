using System.Text.Json;
using System.Text.Json.Serialization;

namespace DocumentsService.Common.JsonConverters
{
    public class JsonStringToObjectConverter : JsonConverter<object>
    {
        public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (var doc = JsonDocument.ParseValue(ref reader))
            {
                return ConvertJsonElement(doc.RootElement);
            }
        }

        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }

        private static object ConvertJsonElement(JsonElement element)
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.Object:
                    var obj = new Dictionary<string, object>();
                    foreach (var prop in element.EnumerateObject())
                    {
                        obj[prop.Name] = ConvertJsonElement(prop.Value);
                    }
                    return obj;
                case JsonValueKind.Array:
                    var arr = new List<object>();
                    foreach (var item in element.EnumerateArray())
                    {
                        arr.Add(ConvertJsonElement(item));
                    }
                    return arr;
                case JsonValueKind.String:
                    if (element.TryGetDateTime(out DateTime datetime))
                        return datetime;
                    return element.GetString()!;
                case JsonValueKind.Number:
                    if (element.TryGetInt32(out int intValue))
                        return intValue;
                    if (element.TryGetInt64(out long longValue))
                        return longValue;
                    if (element.TryGetDouble(out double doubleValue))
                        return doubleValue;
                    break;
                case JsonValueKind.True:
                    return true;
                case JsonValueKind.False:
                    return false;
                case JsonValueKind.Null:
                    return null;
            }
            throw new InvalidOperationException($"Unsupported JsonValueKind {element.ValueKind}");
        }
    }

}
