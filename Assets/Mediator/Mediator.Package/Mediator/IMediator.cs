using System.Threading;
using System.Threading.Tasks;

namespace Straumann.Mediator
{
    /// <summary>
    /// Defines the interface for a mediator to encapsulate request/response and publishing interaction patterns.
    /// </summary>
    public interface IMediator
    {
        /// <summary>
        /// Asynchronously sends a request to a single handler.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <param name="request">The request object.</param>
        /// <param name="cancellationToken">Optional cancellation token to cancel the request.</param>
        /// <returns>A task representing the asynchronous operation, which, upon completion, will have sent the request to its handler.</returns>
        /// <remarks>
        /// This method is used when a response is not expected from the handler.
        /// </remarks>
        Task SendAsync<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest;

        /// <summary>
        /// Asynchronously sends a request to a single handler and expects a response of a specific type.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response from the handler.</typeparam>
        /// <param name="request">The request object.</param>
        /// <param name="cancellationToken">Optional cancellation token to cancel the request.</param>
        /// <returns>A task representing the asynchronous operation, which, upon completion, yields the handler's response.</returns>
        Task<TResponse> SendAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest;

        /// <summary>
        /// Asynchronously publishes a notification to multiple handlers.
        /// </summary>
        /// <typeparam name="TNotification">The type of the notification.</typeparam>
        /// <param name="notification">The notification object.</param>
        /// <param name="cancellationToken">Optional cancellation token to cancel the publishing.</param>
        /// <returns>A task representing the asynchronous operation, which, upon completion, will have published the notification to its handlers.</returns>
        /// <remarks>
        /// This method is used for broadcasting messages to multiple handlers. Each handler processes the notification independently.
        /// </remarks>
        Task PublishAsync<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification; 
    }
}
