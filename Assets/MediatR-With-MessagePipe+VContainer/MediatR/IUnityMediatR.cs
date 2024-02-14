using System;

namespace MediatR_With_MessagePipe_VContainer
{
    //Todo: Also add async support
    public interface IUnityMediatR
    {
        // Requests / Commands
        public void Send<TRequest>(TRequest request) where TRequest : IRequest;
        TResponse Send<TRequest, TResponse>(TRequest request) where TRequest : IRequest;

        // Notifications
        void Publish<TNotification>(TNotification notification) where TNotification : INotification; 

        // Registration
        IDisposable Register<TNotification>(Action<TNotification> handle)
            where TNotification : INotification;
        IDisposable Register<T1, T2>(Action<T1> handleCallback, Action<T2> handleCallback2)
            where T1 : INotification where T2 : INotification;
        public IDisposable Register<T1, T2, T3>(Action<T1> handleCallback1, Action<T2> handleCallback2, Action<T3> handleCallback3)
            where T1 : INotification where T2 : INotification where T3 : INotification;
    }
}