using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;
using TrueStory.Application.Infrastructure.RestFul.DTO;
using TrueStory.Application.Infrastructure.RestFul.ServiceImplementators;
using TrueStory.Infrastructure.Service.restful_api.dev.Model;
using TrueStory.Infrastructure.Service.restful_api.dev.RestClient;

namespace TrueStory.Infrastructure.Service.restful_api.dev.Service
{
    /// <summary>
    /// Service for handling REST API operations.
    /// </summary>
    public class RestFulService(IRestFulRestClient _client) : IRestFulServiceImplementator
    {
        /// <summary>
        /// Adds a new product to the external API.
        /// </summary>
        /// <param name="requestDTO">The product request DTO containing name and data.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The ID of the created product.</returns>
        public async Task<Guid> AddProductAsync(AddProductRequestDTO requestDTO, CancellationToken cancellationToken)
        {
            var request = new AddProductRequestModel
            { 
                name = requestDTO.Name,
                data = requestDTO.Data
            };
            return await _client.AddProductAsync(request, cancellationToken);
        }

        /// <summary>
        /// Deletes a product from the external API.
        /// </summary>
        /// <param name="ID">The ID of the product to delete.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>True if deletion was successful.</returns>
        public async Task<bool> DeleteProductAsync(Guid ID, CancellationToken cancellationToken)
        {
            return await _client.DeleteProductAsync(ID, cancellationToken);
        }

        /// <summary>
        /// Retrieves products from the external API by their IDs.
        /// </summary>
        /// <param name="IDs">List of product IDs to retrieve.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>List of retrieved products.</returns>
        public async Task<List<ProductResponseDTO>> GetProductsAsync(List<Guid> IDs, CancellationToken cancellationToken)
        {
            var resp = await _client.GetProductsAsync(IDs, cancellationToken);

            return resp.Select(x => new ProductResponseDTO 
            { 
                ID = new Guid(x.ID),
                Name = x.Name,
                Data = x.Data,
            }).ToList();
        }
    }
}
