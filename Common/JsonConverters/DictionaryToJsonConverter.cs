using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace DocumentsService.Common.JsonConverters
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