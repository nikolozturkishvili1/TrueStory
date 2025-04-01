using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueStory.Application.Infrastructure.RestFul.DTO
{
    public class ProductResponseDTO
    {
        public Guid ID { get; set; }
        public string Name { get; set; } 
        public Object? Data { get; set; }
    }
}
