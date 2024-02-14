using MessagePipe;
using System;
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
            var handler = m_resolver.Resolve<IRequestHandler<TRequest, NullResponse>>();
            handler.Invoke(request);
        }


        public TResponse Send<TRequest, TResponse>(TRequest request) where TRequest : IRequest
        {
            var handler = m_resolver.Resolve<IRequestHandler<TRequest, TResponse>>();
            return handler.Invoke(request);
        }

        public void Publish<TNotification>(TNotification notification) where TNotification : INotification
        {
            var publisher = m_resolver.Resolve<IPublisher<TNotification>>();
            
            publisher.Publish(notification);
        }

        public IDisposable Register<TType>(Action<TType> handle) where TType : INotification
        {
            var subscriber = m_resolver.Resolve<ISubscriber<TType>>();
            return subscriber.Subscribe(handle);
        }

        public IDisposable Register<T1, T2>(Action<T1> handleCallback1, Action<T2> handleCallback2) where T1 : INotification where T2 : INotification
        {
            var disposable1 = Register(handleCallback1);
            var disposable2 = Register(handleCallback2);
            return DisposableBagBuilder(disposable1, disposable2);
        }
        
        public IDisposable Register<T1, T2, T3>(Action<T1> handleCallback1, Action<T2> handleCallback2, Action<T3> handleCallback3)
            where T1 : INotification where T2 : INotification where T3 : INotification
        {
            var disposable1 = Register(handleCallback1);
            var disposable2 = Register(handleCallback2);
            var disposable3 = Register(handleCallback3);
            return DisposableBagBuilder(disposable1, disposable2, disposable3);
        }

        private IDisposable DisposableBagBuilder(params IDisposable[] disposables)
        {
            var bag = DisposableBag.CreateBuilder();
            foreach (var disposable in disposables)
                disposable.AddTo(bag);
            return bag.Build();
        }
    }
}