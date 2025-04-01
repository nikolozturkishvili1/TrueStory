using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TrueStory.Application.Product.Commands.Create;
using TrueStory.Application.Product.Commands.Delete;
using TrueStory.Application.Product.Queries.GetProducts;
using TrueStory.Common.Application.Paging;

namespace TrueStory.Presentation.Web.API.Test.TestData
{
    public static class ProductTestData
    {
        public readonly static Guid Product_ID = new Guid("ff808181932badb60195ed58b91371c5");
        public const string DEFAULT_NAME = "Test Name";
        public const bool DEFAULT_ISDELETED = false;
        public readonly static JsonElement DEFAULT_DATA = JsonDocument.Parse("{\"data\": { \"price\": 50 }}").RootElement;
        public readonly static DateTime DEFAULT_CREATEDATE = DateTime.Now;
        public const int PAGE_SIZE = 10;
        public const int PAGE_NUMBER = 1;


        internal static DeleteProductModelRequest BuildDeleteProductRequest(Guid id) => new DeleteProductModelRequest(id);

        internal static GetProductsModelRequest BuildGetProductsModelRequest(
        string name = DEFAULT_NAME,
        int pageSize = PAGE_SIZE,
        int pageNumber = PAGE_NUMBER)
        {
            return new GetProductsModelRequest
            {
                Name = name,
                PageSize = pageSize,
                PageNumber = pageNumber
            };
        }


        internal static CreateProductModelRequest BuildCreateProductRequest(
         string name)
        {
            return BuildCreateProductRequest(name, DEFAULT_DATA);
        }

        internal static CreateProductModelRequest BuildCreateProductRequest(
            string name,
            JsonElement data)
        {
            return new CreateProductModelRequest(name, data);
        }


        internal static PagedResult<GetProductsModelResponse> BuildPagedProducts()
        {
            return new PagedResult<GetProductsModelResponse>
            {
                Items =
                [
                    new GetProductsModelResponse(
                    new Guid("ff808181932badb60195ed58b91371c5"),
                    DEFAULT_NAME

                   ){Data = DEFAULT_DATA},
                ],
                TotalItemCount = 2,
                PageSize = 10,
                PageNumber = 1
            };
        }
    }
}
