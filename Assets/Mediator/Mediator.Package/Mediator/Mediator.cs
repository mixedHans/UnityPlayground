using MessagePipe;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using VContainer;

namespace Straumann.Mediator
{
    public sealed class Mediator : IMediator
    {
        private readonly IObjectResolver m_resolver;
        
        public Mediator(IObjectResolver resolver)
        {
            m_resolver = resolver;
        }

        public Task SendAsync<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest
        {
            var handler = m_resolver.Resolve<IAsyncRequestHandler<TRequest>>();
            return handler.InvokeAsync(request, cancellationToken);
        }

        public Task<TResponse> SendAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest
        {
            var handler = m_resolver.Resolve<IAsyncRequestHandler<TRequest, TResponse>>();
            return handler.InvokeAsync(request, cancellationToken);
        }

        public Task PublishAsync<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification
        {
            var publisher = m_resolver.Resolve<IAsyncPublisher<TNotification>>();
            return publisher.PublishAsync(notification, cancellationToken).AsTask();
        }
    }
}