using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueStory.Application.Infrastructure.RestFul.DTO;
using System.Threading;

namespace TrueStory.Application.Infrastructure.RestFul.ServiceImplementators
{
    /// <summary>
    /// Interface for REST API service operations.
    /// </summary>
    public interface IRestFulServiceImplementator
    {
        /// <summary>
        /// Adds a new product to the external API.
        /// </summary>
        /// <param name="requestDTO">The product request DTO containing name and data.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The ID of the created product.</returns>
        public Task<Guid> AddProductAsync(AddProductRequestDTO requestDTO, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes a product from the external API.
        /// </summary>
        /// <param name="ID">The ID of the product to delete.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>True if deletion was successful.</returns>
        public Task<bool> DeleteProductAsync(Guid ID, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves products from the external API by their IDs.
        /// </summary>
        /// <param name="IDs">List of product IDs to retrieve.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>List of retrieved products.</returns>
        public Task<List<ProductResponseDTO>> GetProductsAsync(List<Guid> IDs, CancellationToken cancellationToken);
    }
}
