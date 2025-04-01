using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TrueStory.Infrastructure.Service.restful_api.dev.Model
{
    public class AddProductRequestModel
    {
        public string name { get; set; }
        public JsonElement data { get; set; }
    }
}
