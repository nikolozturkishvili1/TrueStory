using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueStory.Application.Infrastructure.RestFul.DTO;
using TrueStory.Application.Product.Queries.GetProducts;
using TrueStory.Common.Mappers;
using TrueStory.Domain.Aggregate.Product;

namespace TrueStory.Application.Common.Mapper
{
    public static class ProductMapper
    {

        /// <summary>
        /// Maps a collection of Product entities to a list of GetProductsModelResponse DTOs.
        /// </summary>
        /// <param name="people">The collection of Products.</param>
        /// <returns>A list of mapped DTOs.</returns>
        public static List<GetProductsModelResponse> MapToProductsResponse(IEnumerable<GetProductsModelResponse> products,List<ProductResponseDTO> productResponse)
        {
            ArgumentNullException.ThrowIfNull(products);
            ArgumentNullException.ThrowIfNull(productResponse);

            var resultList = new List<GetProductsModelResponse>();

            foreach (var product in products)
            {
                var responseItem = productResponse.FirstOrDefault(x => x.ID ==  product.ID);
                if (responseItem == null)
                {
                    resultList.Add(new GetProductsModelResponse(product.ID,$"{product.Name} Not Exist in Mock API"));
                    continue;
                }
                resultList.Add(new GetProductsModelResponse(product.ID, product.Name) { Data = responseItem.Data });
            }


            return resultList;
        }

    }
}
