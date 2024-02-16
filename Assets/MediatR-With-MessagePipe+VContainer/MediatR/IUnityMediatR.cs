using System;
using System.Threading;
using System.Threading.Tasks;

namespace MediatR_With_MessagePipe_VContainer
{
    public interface IUnityMediatR
    {
        // Synchronous 
        public void Send<TRequest>(TRequest request) where TRequest : IRequest;
        TResponse Send<TRequest, TResponse>(TRequest request) where TRequest : IRequest;

        // Asynchronous 
        public Task SendAsync<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest;
        public Task<TResponse> SendAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest;
        
        // Notifications
        void Publish<TNotification>(TNotification notification) where TNotification : INotification; 
        Task PublishAsync<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification; 
    }
}