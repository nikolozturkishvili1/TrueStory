using Flurl.Http;
using Flurl.Http.Configuration;
using Newtonsoft.Json;
using TrueStory.Common.Exceptions;
using TrueStory.Common.Mappers;
using TrueStory.Domain.Aggregate.Product;
using TrueStory.Infrastructure.Service.restful_api.dev.Model;
using TrueStory.Application.Resources;
using TrueStory.Infrastructure.Resources;
using System.Net.NetworkInformation;
using System.Net.Http.Headers;

namespace TrueStory.Infrastructure.Service.restful_api.dev.RestClient
{
    public interface IRestFulRestClient
    {
        /// <summary>
        /// Adds a new product to the external API.
        /// </summary>
        /// <param name="request">The product request model containing name and data.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The ID of the created product.</returns>
        /// <exception cref="FlurlHttpException">Thrown when the API request fails.</exception>
        public Task<Guid> AddProductAsync(AddProductRequestModel request, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes a product from the external API.
        /// </summary>
        /// <param name="ID">The ID of the product to delete.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>True if deletion was successful.</returns>
        /// <exception cref="NotFoundException">Thrown when the product is not found.</exception>
        public Task<bool> DeleteProductAsync(Guid ID, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves products from the external API by their IDs.
        /// </summary>
        /// <param name="IDs">List of product IDs to retrieve.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>List of retrieved products.</returns>
        public Task<List<ProductResponse>> GetProductsAsync(List<Guid> IDs, CancellationToken cancellationToken);
    }

    /// <summary>
    /// Client for interacting with the external REST API.
    /// </summary>
    public class RestFulClient(IHttpClientFactory httpClientFactory) : IRestFulRestClient
    {
        private readonly IFlurlClient _flurlClient = new FlurlClientBuilder("https://api.restful-api.dev/")
            .Build();

        /// <summary>
        /// Adds a new product to the external API.
        /// </summary>
        /// <param name="request">The product request model containing name and data.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The ID of the created product.</returns>
        /// <exception cref="FlurlHttpException">Thrown when the API request fails.</exception>
        public async Task<Guid> AddProductAsync(AddProductRequestModel request, CancellationToken cancellationToken)
        {
            try
            {
                var requestBody = new
                {
                    name = request.name,
                    data = JsonConvert.DeserializeObject<Dictionary<string, object>>(request.data.GetRawText())
                };

                var response = await _flurlClient.Request($"objects")
                      .WithHeader("Content-Type", "application/json")
                      .PostJsonAsync(requestBody,cancellationToken: cancellationToken)
                      .ReceiveJson<AddProductResponse>();

                return new Guid(response.Id);
            }
            catch (FlurlHttpException ex)
            {
                Console.WriteLine($"A FluentException occurred: {ex.Message}");
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Deletes a product from the external API.
        /// </summary>
        /// <param name="ID">The ID of the product to delete.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>True if deletion was successful.</returns>
        /// <exception cref="NotFoundException">Thrown when the product is not found.</exception>
        public async Task<bool> DeleteProductAsync(Guid ID, CancellationToken cancellationToken)
        {
            var response = await _flurlClient.Request($"objects/{ID.FormatGuidToString()}")
                    .WithHeader("Content-Type", "application/json")
                    .DeleteAsync(cancellationToken:cancellationToken);

            if (response.StatusCode != 200)
            {
                throw new NotFoundException(string.Format(InfrastructureExceptionResource.MockAPIProductNotFind, ID));
            }

            var deleteResponse = await response.GetJsonAsync<DeleteProductResponse>();
            return true;
        }

        /// <summary>
        /// Retrieves products from the external API by their IDs.
        /// </summary>
        /// <param name="IDs">List of product IDs to retrieve.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>List of retrieved products.</returns>
        public async Task<List<ProductResponse>> GetProductsAsync(List<Guid> IDs, CancellationToken cancellationToken)
        {
            var response = await _flurlClient.Request($"objects")
                      .WithHeader("Content-Type", "application/json")
                      .SetQueryParams(new { id = IDs.FormatGuidListToString() })
                      .GetAsync(cancellationToken:cancellationToken)
                      .ReceiveJson<List<ProductResponse>>();
            return response;
        }
    }
}
