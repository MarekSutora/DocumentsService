using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace DocumentsService.API.Common.JsonConverters
{
    public class DictionaryToJsonConverter : ValueConverter<Dictionary<string, object>, string>
    {
        public DictionaryToJsonConverter() : base(
            dict => JsonSerializer.Serialize(dict, null as JsonSerializerOptions),
            json => JsonSerializer.Deserialize<Dictionary<string, object>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringToObjectConverter() }
            })!)
        {
        }
    }
}