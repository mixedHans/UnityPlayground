using Cysharp.Threading.Tasks;
using MessagePipe;
using UnityEngine;
using VContainer;

namespace MediatR_With_MessagePipe_VContainer
{
    public static class MediatRExtension
    {
        private static MessagePipeOptions _options;

        #region RequestHandlers

        // Sync RequestHandlers
        public static RegistrationBuilder RegisterMediatRRequestHandler<TRequest, TResponse, TRequestHandler>(
            this IContainerBuilder builder)
            where TRequestHandler : IMediatRRequestHandler<TRequest, TResponse> where TRequest : IRequest
        {
            _options ??= builder.RegisterMessagePipe();
            if (!builder.Exists(typeof(IMediatRRequestHandler<TRequest, TResponse>), true))
                return builder.Register(typeof(TRequestHandler), Lifetime.Singleton).As(typeof(IMediatRRequestHandler<TRequest, TResponse>));
            
            Debug.LogWarning($"You are trying to register multiple MediatRRequestHandlers for the same type. This is not allowed! " +
                             $"Handler: {typeof(TRequestHandler)} RequestType: {typeof(TRequest)} ResponseType: {typeof(TResponse)}");
            return null;
        }

        public static RegistrationBuilder RegisterMediatRRequestHandler<TRequest, TRequestHandler>(
            this IContainerBuilder builder)
            where TRequestHandler : IMediatRRequestHandler<TRequest> where TRequest : IRequest
        {
            _options ??= builder.RegisterMessagePipe();
            if (!builder.Exists(typeof(IMediatRRequestHandler<TRequest>), true))
                return builder.Register(typeof(TRequestHandler), Lifetime.Singleton).As(typeof(IMediatRRequestHandler<TRequest>));
            
            Debug.LogWarning($"You are trying to register multiple MediatRRequestHandlers for the same type. This is not allowed! " +
                             $"Handler: {typeof(TRequestHandler)} RequestType: {typeof(TRequest)}");
            return null;
        }

        // Async RequestHandlers
        public static RegistrationBuilder RegisterAsyncMediatRRequestHandler<TRequest, TResponse, TRequestHandler>(
            this IContainerBuilder builder)
            where TRequestHandler : IAsyncMediatRRequestHandler<TRequest, TResponse> where TRequest : IRequest
        {
            _options ??= builder.RegisterMessagePipe();
            if (!builder.Exists(typeof(IAsyncMediatRRequestHandler<TRequest, TResponse>), true))
                return builder.Register(typeof(TRequestHandler), Lifetime.Singleton).As(typeof(IAsyncMediatRRequestHandler<TRequest, TResponse>));
            
            Debug.LogWarning($"You are trying to register multiple MediatRRequestHandlers for the same type. This is not allowed! " +
                             $"Handler: {typeof(TRequestHandler)} RequestType: {typeof(TRequest)} ResponseType: {typeof(TResponse)}");
            return null;
        }

        public static RegistrationBuilder RegisterAsyncMediatRRequestHandler<TRequest, TRequestHandler>(
            this IContainerBuilder builder)
            where TRequestHandler : IAsyncMediatRRequestHandler<TRequest> where TRequest : IRequest
        {
            _options ??= builder.RegisterMessagePipe();
            if (!builder.Exists(typeof(IAsyncMediatRRequestHandler<TRequest>), true))
                return builder.Register(typeof(TRequestHandler), Lifetime.Singleton).As(typeof(IAsyncMediatRRequestHandler<TRequest>));
            
            Debug.LogWarning($"You are trying to register multiple MediatRRequestHandlers for the same type. This is not allowed! " +
                             $"Handler: {typeof(TRequestHandler)} RequestType: {typeof(TRequest)}");
            return null;
        }

        #endregion

        #region Notification
        public static void RegisterNotificationType<TNotificationType>(
            this IContainerBuilder builder)
            where TNotificationType : INotification
        {
            _options ??= builder.RegisterMessagePipe();
            builder.RegisterMessageBroker<TNotificationType>(_options);
        }
        #endregion

        #region SyncNotificationHandlers
        public static RegistrationBuilder RegisterMediatRNotificationHandler<TNotification, TNotificationHandler>(
            this IContainerBuilder builder, Lifetime lifetime = Lifetime.Scoped)
            where TNotificationHandler : INotificationHandler<TNotification> where TNotification : INotification
        {
            builder.RegisterBuildCallback(container =>
            {
                var bagBuilder = DisposableBag.CreateBuilder();

                container.BindHandler<TNotification>(bagBuilder);

                var handler = container.Resolve<INotificationHandler<TNotification>>();
                handler.MediatRDisposables?.AddTo(bagBuilder);
                handler.MediatRDisposables = bagBuilder.Build();
            });
            return builder.Register<INotificationHandler<TNotification>, TNotificationHandler>(lifetime);
        }
        
        public static RegistrationBuilder RegisterMediatRNotificationHandler<T1, T2, TNotificationHandler>(
            this IContainerBuilder builder, Lifetime lifetime = Lifetime.Scoped)
            where TNotificationHandler : INotificationHandler<T1> 
            where T1 : INotification where T2 : INotification
        {
            builder.RegisterBuildCallback(container =>
            {
                var bagBuilder = DisposableBag.CreateBuilder();

                container.BindHandler<T1>(bagBuilder);
                container.BindHandler<T2>(bagBuilder);

                var handler = container.Resolve<INotificationHandler<T1>>();
                handler.MediatRDisposables?.AddTo(bagBuilder);
                handler.MediatRDisposables = bagBuilder.Build();
            });
            
            return builder.Register<TNotificationHandler>(lifetime).As<INotificationHandler<T1>, INotificationHandler<T2>>();
        }
        
        public static RegistrationBuilder RegisterMediatRNotificationHandler<T1, T2, T3, TNotificationHandler>(
            this IContainerBuilder builder, Lifetime lifetime = Lifetime.Scoped)
            where TNotificationHandler : INotificationHandler<T1>
            where T1 : INotification where T2 : INotification where T3 : INotification
        {
            builder.RegisterBuildCallback(container =>
            {
                var bagBuilder = DisposableBag.CreateBuilder();

                container.BindHandler<T1>(bagBuilder);
                container.BindHandler<T2>(bagBuilder);
                container.BindHandler<T3>(bagBuilder);
            
                var handler = container.Resolve<INotificationHandler<T1>>();
                handler.MediatRDisposables?.AddTo(bagBuilder);
                handler.MediatRDisposables = bagBuilder.Build();
            });

            return builder.Register<TNotificationHandler>(lifetime).As<INotificationHandler<T1>, INotificationHandler<T2>, INotificationHandler<T3>>();
        }

        public static RegistrationBuilder RegisterMediatRNotificationHandler<T1, T2, T3, T4, TNotificationHandler>(
            this IContainerBuilder builder, Lifetime lifetime = Lifetime.Scoped)
            where TNotificationHandler : INotificationHandler<T1> 
            where T1 : INotification where T2 : INotification where T3 : INotification where T4 : INotification
        {
            builder.RegisterBuildCallback(container =>
            {
                var bagBuilder = DisposableBag.CreateBuilder();

                container.BindHandler<T1>(bagBuilder);
                container.BindHandler<T2>(bagBuilder);
                container.BindHandler<T3>(bagBuilder);
                container.BindHandler<T4>(bagBuilder);

                var handler = container.Resolve<INotificationHandler<T1>>();
                handler.MediatRDisposables?.AddTo(bagBuilder);
                handler.MediatRDisposables = bagBuilder.Build();
            });
            
            return builder.Register<TNotificationHandler>(lifetime).As<INotificationHandler<T1>, INotificationHandler<T2>, INotificationHandler<T3>, INotificationHandler<T4>>();
        }

        public static RegistrationBuilder RegisterMediatRNotificationHandler<T1, T2, T3, T4, T5, TNotificationHandler>(
            this IContainerBuilder builder, Lifetime lifetime = Lifetime.Scoped)
            where TNotificationHandler : INotificationHandler<T1> 
            where T1 : INotification where T2 : INotification where T3 : INotification where T4 : INotification where T5 : INotification
        {
            builder.RegisterBuildCallback(container =>
            {
                var bagBuilder = DisposableBag.CreateBuilder();

                container.BindHandler<T1>(bagBuilder);
                container.BindHandler<T2>(bagBuilder);
                container.BindHandler<T3>(bagBuilder);
                container.BindHandler<T4>(bagBuilder);
                container.BindHandler<T5>(bagBuilder);

                var handler = container.Resolve<INotificationHandler<T1>>();
                handler.MediatRDisposables?.AddTo(bagBuilder);
                handler.MediatRDisposables = bagBuilder.Build();
            });
            
            return builder.Register<TNotificationHandler>(lifetime).As(typeof(INotificationHandler<T1>), typeof(INotificationHandler<T2>), typeof(INotificationHandler<T3>), typeof(INotificationHandler<T4>), typeof(INotificationHandler<T5>));
        }

        // Todo: Check Multiple Registrations of the same handler!
        private static bool CheckMultipleRegistrations<TNotification, TNotificationHandler>(IContainerBuilder builder)
            where TNotificationHandler : INotificationHandler<TNotification> where TNotification : INotification
        {
            if (!builder.Exists(typeof(INotificationHandler<TNotification>), true)) return false;
            
            Debug.LogWarning($"NotificationHandlerRegistration: You are trying to register a NotificationHandler in the same scope multiple times. This is not allowed!" +
                             $"Handler: {typeof(INotificationHandler<TNotification>)}, Notification: {typeof(TNotification)}");
            return true;
        }
        
        private static void BindHandler<TNotification>(this IObjectResolver resolver, DisposableBagBuilder bagBuilder)
            where TNotification : INotification
        {
            var notificationHandler = resolver.Resolve<INotificationHandler<TNotification>>();
            var subscriber = resolver.Resolve<ISubscriber<TNotification>>();
            subscriber.Subscribe(notificationHandler.Handle).AddTo(bagBuilder);
        }

        #endregion

        #region Async NotificationHandlers
        public static RegistrationBuilder RegisterAsyncMediatRNotificationHandler<TNotification, TAsyncNotificationHandler>(
            this IContainerBuilder builder, Lifetime lifetime = Lifetime.Scoped)
            where TAsyncNotificationHandler : IAsyncNotificationHandler<TNotification> where TNotification : INotification
        {
            builder.RegisterBuildCallback(container =>
            {
                var bagBuilder = DisposableBag.CreateBuilder();

                container.BindAsyncHandler<TNotification>(bagBuilder);

                var handler = container.Resolve<IAsyncNotificationHandler<TNotification>>();
                handler.MediatRDisposables?.AddTo(bagBuilder);
                handler.MediatRDisposables = bagBuilder.Build();
            });
            return builder.Register<IAsyncNotificationHandler<TNotification>, TAsyncNotificationHandler>(lifetime);
        }

        public static RegistrationBuilder RegisterAsyncMediatRNotificationHandler<T1, T2, TAsyncNotificationHandler>(
            this IContainerBuilder builder, Lifetime lifetime = Lifetime.Scoped)
            where TAsyncNotificationHandler : IAsyncNotificationHandler<T1> 
            where T1 : INotification where T2 : INotification
        {
            builder.RegisterBuildCallback(container =>
            {
                var bagBuilder = DisposableBag.CreateBuilder();

                container.BindAsyncHandler<T1>(bagBuilder);
                container.BindAsyncHandler<T2>(bagBuilder);

                var handler = container.Resolve<IAsyncNotificationHandler<T1>>();
                handler.MediatRDisposables?.AddTo(bagBuilder);
                handler.MediatRDisposables = bagBuilder.Build();
            });
            
            return builder.Register<TAsyncNotificationHandler>(lifetime).As<IAsyncNotificationHandler<T1>, IAsyncNotificationHandler<T2>>();
        }
        
        public static RegistrationBuilder RegisterAsyncMediatRNotificationHandler<T1, T2, T3, TAsyncNotificationHandler>(
            this IContainerBuilder builder, Lifetime lifetime = Lifetime.Scoped)
            where TAsyncNotificationHandler : IAsyncNotificationHandler<T1> 
            where T1 : INotification where T2 : INotification where T3 : INotification
        {
            builder.RegisterBuildCallback(container =>
            {
                var bagBuilder = DisposableBag.CreateBuilder();

                container.BindAsyncHandler<T1>(bagBuilder);
                container.BindAsyncHandler<T2>(bagBuilder);
                container.BindAsyncHandler<T3>(bagBuilder);

                var handler = container.Resolve<IAsyncNotificationHandler<T1>>();
                handler.MediatRDisposables?.AddTo(bagBuilder);
                handler.MediatRDisposables = bagBuilder.Build();
            });

            return builder.Register<TAsyncNotificationHandler>(lifetime).As<IAsyncNotificationHandler<T1>, IAsyncNotificationHandler<T2>, IAsyncNotificationHandler<T3>>();
        }

        public static RegistrationBuilder RegisterAsyncMediatRNotificationHandler<T1, T2, T3, T4, TAsyncNotificationHandler>(
            this IContainerBuilder builder, Lifetime lifetime = Lifetime.Scoped)
            where TAsyncNotificationHandler : IAsyncNotificationHandler<T1> 
            where T1 : INotification where T2 : INotification where T3 : INotification where T4 : INotification
        {
            builder.RegisterBuildCallback(container =>
            {
                var bagBuilder = DisposableBag.CreateBuilder();

                container.BindAsyncHandler<T1>(bagBuilder);
                container.BindAsyncHandler<T2>(bagBuilder);
                container.BindAsyncHandler<T3>(bagBuilder);
                container.BindAsyncHandler<T4>(bagBuilder);

                var handler = container.Resolve<INotificationHandler<T1>>();
                handler.MediatRDisposables?.AddTo(bagBuilder);
                handler.MediatRDisposables = bagBuilder.Build();
            });
            
            return builder.Register<TAsyncNotificationHandler>(lifetime).As<IAsyncNotificationHandler<T1>, IAsyncNotificationHandler<T2>, IAsyncNotificationHandler<T3>, IAsyncNotificationHandler<T4>>();
        }

        public static RegistrationBuilder RegisterAsyncMediatRNotificationHandler<T1, T2, T3, T4, T5, TAsyncNotificationHandler>(
            this IContainerBuilder builder, Lifetime lifetime = Lifetime.Scoped)
            where TAsyncNotificationHandler : IAsyncNotificationHandler<T1> 
            where T1 : INotification where T2 : INotification where T3 : INotification where T4 : INotification where T5 : INotification
        {
            builder.RegisterBuildCallback(container =>
            {
                var bagBuilder = DisposableBag.CreateBuilder();

                container.BindAsyncHandler<T1>(bagBuilder);
                container.BindAsyncHandler<T2>(bagBuilder);
                container.BindAsyncHandler<T3>(bagBuilder);
                container.BindAsyncHandler<T4>(bagBuilder);
                container.BindAsyncHandler<T5>(bagBuilder);

                var handler = container.Resolve<IAsyncNotificationHandler<T1>>();
                handler.MediatRDisposables?.AddTo(bagBuilder);
                handler.MediatRDisposables = bagBuilder.Build();
            });
            
            return builder.Register<TAsyncNotificationHandler>(lifetime).As(typeof(IAsyncNotificationHandler<T1>), typeof(IAsyncNotificationHandler<T2>), typeof(IAsyncNotificationHandler<T3>), typeof(IAsyncNotificationHandler<T4>), typeof(IAsyncNotificationHandler<T5>));
        }
        
        private static void BindAsyncHandler<TNotification>(this IObjectResolver resolver, DisposableBagBuilder bagBuilder)
            where TNotification : INotification
        {
            var notificationHandler = resolver.Resolve<IAsyncNotificationHandler<TNotification>>();
            var subscriber = resolver.Resolve<IAsyncSubscriber<TNotification>>();
            subscriber.Subscribe((notification, cancellationToken) 
                    => notificationHandler.HandleAsync(notification, cancellationToken).AsUniTask())
                .AddTo(bagBuilder);
        }

        #endregion
    }
}