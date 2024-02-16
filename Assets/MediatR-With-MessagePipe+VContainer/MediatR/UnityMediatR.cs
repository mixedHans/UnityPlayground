using MessagePipe;
using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using VContainer;

namespace MediatR_With_MessagePipe_VContainer
{
    public sealed class UnityMediatR : IUnityMediatR
    {
        private readonly IObjectResolver m_resolver;

        public UnityMediatR(IObjectResolver resolver)
        {
            m_resolver = resolver;
        }
        
        public void Send<TRequest>(TRequest request) where TRequest : IRequest
        {
            var handler = m_resolver.Resolve<IMediatRRequestHandler<TRequest>>();
            handler.Invoke(request);
        }

        public TResponse Send<TRequest, TResponse>(TRequest request) where TRequest : IRequest
        {
            var handler = m_resolver.Resolve<IMediatRRequestHandler<TRequest, TResponse>>();
            return handler.Invoke(request);
        }
        
        public Task SendAsync<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest
        {
            var handler = m_resolver.Resolve<IAsyncMediatRRequestHandler<TRequest>>();
            return handler.InvokeAsync(request, cancellationToken);
        }

        public Task<TResponse> SendAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest
        {
            var handler = m_resolver.Resolve<IAsyncMediatRRequestHandler<TRequest, TResponse>>();
            return handler.InvokeAsync(request, cancellationToken);
        }

        public void Publish<TNotification>(TNotification notification) where TNotification : INotification
        {
            var publisher = m_resolver.Resolve<IPublisher<TNotification>>();
            publisher.Publish(notification);
        }

        public Task PublishAsync<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification
        {
            var publisher = m_resolver.Resolve<IAsyncPublisher<TNotification>>();
            return publisher.PublishAsync(notification, cancellationToken).AsTask();
        }
    }
}