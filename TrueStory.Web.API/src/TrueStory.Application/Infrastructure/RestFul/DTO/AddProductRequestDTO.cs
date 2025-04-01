using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace TrueStory.Application.Infrastructure.RestFul.DTO
{
    public class AddProductRequestDTO
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        
        public JsonElement Data { get; set; }
    }
}
