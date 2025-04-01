using MediatR;
using Microsoft.Extensions.Logging;
using TrueStory.Common.Application.Interfaces;
using TrueStory.Domain.Interfaces;

namespace PersonRegistry.Application.Common.Behaviours;

/// <summary>
/// A pipeline behavior for handling database transactions in MediatR requests.
/// </summary>
/// <typeparam name="TRequest">The request type.</typeparam>
/// <typeparam name="TResponse">The response type.</typeparam>
public class RequestTransactionBehavior<TRequest, TResponse>(ILogger<RequestTransactionBehavior<TRequest, TResponse>> _logger, IUnitOfWork _unitOfWork)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{

    /// <summary>
    /// Handles transaction management for MediatR requests.
    /// </summary>
    /// <param name="request">The incoming request.</param>
    /// <param name="next">The next delegate in the pipeline.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response from the request handler.</returns>
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = default(TResponse);
        if (request is ITransactionalRequest)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var result = await next();

                await _unitOfWork.CommitTransactionAsync();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Rollback Db Transaction for {typeof(TRequest).Name}!" +
                    $"{ex.Message}");

                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
        else
            response = await next();

        return response;
    }
}
