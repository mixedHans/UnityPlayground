using Cysharp.Threading.Tasks;
using MessagePipe;
using UnityEngine;
using VContainer;

namespace Straumann.Mediator
{
    // Todo: Rename mediatR to mediator
    // Todo: Move extensions in own Assembly: VContainer.Mediator
    // Todo: Make all async
    public static class MediatorExtension
    {
        private static MessagePipeOptions _options;

        #region RequestHandlers

        // Async RequestHandlers
        public static RegistrationBuilder RegisterAsyncMediatRRequestHandler<TRequest, TResponse, TAsyncRequestHandler>(
            this IContainerBuilder builder)
            where TAsyncRequestHandler : IAsyncRequestHandler<TRequest, TResponse> where TRequest : IRequest
        {
            _options ??= builder.RegisterMessagePipe();
            if (!builder.Exists(typeof(IAsyncRequestHandler<TRequest, TResponse>), true))
                return builder.Register(typeof(TAsyncRequestHandler), Lifetime.Singleton).As(typeof(IAsyncRequestHandler<TRequest, TResponse>));
            
            Debug.LogWarning($"You are trying to register multiple MediatRRequestHandlers for the same type. This is not allowed! " +
                             $"Handler: {typeof(TAsyncRequestHandler)} RequestType: {typeof(TRequest)} ResponseType: {typeof(TResponse)}");
            return null;
        }

        public static RegistrationBuilder RegisterAsyncMediatRRequestHandler<TRequest, TAsyncRequestHandler>(
            this IContainerBuilder builder)
            where TAsyncRequestHandler : IAsyncRequestHandler<TRequest> where TRequest : IRequest
        {
            _options ??= builder.RegisterMessagePipe();
            if (!builder.Exists(typeof(IAsyncRequestHandler<TRequest>), true))
                return builder.Register(typeof(TAsyncRequestHandler), Lifetime.Singleton).As(typeof(IAsyncRequestHandler<TRequest>));
            
            Debug.LogWarning($"You are trying to register multiple MediatRRequestHandlers for the same type. This is not allowed! " +
                             $"Handler: {typeof(TAsyncRequestHandler)} RequestType: {typeof(TRequest)}");
            return null;
        }

        #endregion

        #region Notification
        public static void RegisterNotificationType<TNotificationType>(
            this IContainerBuilder builder)
            where TNotificationType : INotification
        {
            if (builder.Exists(typeof(MessageBrokerCore<TNotificationType>)) == true)
            {
                Debug.LogWarning($"You are trying to register the same notification type multiple times. This is not allowed! " +
                                 $"NotificationType: {typeof(TNotificationType)}");
                return;
            }
                
            _options ??= builder.RegisterMessagePipe();
            builder.RegisterMessageBroker<TNotificationType>(_options);
        }
        #endregion

        #region SyncNotificationHandlers
        public static RegistrationBuilder RegisterNotificationHandler<TNotification, TNotificationHandler>(this IContainerBuilder builder, Lifetime lifetime = Lifetime.Scoped)
            where TNotification : INotification
            where TNotificationHandler : IAsyncNotificationHandler<TNotification> 
        {
            return builder.BindHandlersInBuildCallback<TNotification, TNotificationHandler>()
                .Register<IAsyncNotificationHandler<TNotification>, TNotificationHandler>(lifetime)
                .AsSelf();
        }

        private static IContainerBuilder BindHandlersInBuildCallback<TNotification, TNotificationHandler>(this IContainerBuilder builder)
            where TNotification : INotification
            where TNotificationHandler : IAsyncNotificationHandler<TNotification> 
        {
            builder.RegisterBuildCallback(container =>
            {
                var notificationHandler = container.Resolve<TNotificationHandler>();
                var bagBuilder = DisposableBag.CreateBuilder();
                
                var subscriber = container.Resolve<IAsyncSubscriber<TNotification>>();
                subscriber.Subscribe((notification, cancellationToken) 
                    => notificationHandler.HandleAsync(notification, cancellationToken)
                        .AsUniTask())
                        .AddTo(bagBuilder);
                
                notificationHandler.NotificationDisposable?.AddTo(bagBuilder);
                notificationHandler.NotificationDisposable = bagBuilder.Build();
            });
            return builder;
        }

        public static RegistrationBuilder RegisterNotificationHandler<TNotification1, TNotification2, TNotificationHandler>(
            this IContainerBuilder builder, Lifetime lifetime = Lifetime.Scoped)
            where TNotification1 : INotification where TNotification2 : INotification
            where TNotificationHandler : IAsyncNotificationHandler<TNotification1>,IAsyncNotificationHandler<TNotification2>
        {
            return builder.RegisterNotificationHandler<TNotification1, TNotificationHandler>(lifetime)
                .AsAdditionalNotificationHandler<TNotification2, TNotificationHandler>(builder);
        }
        
        private static RegistrationBuilder AsAdditionalNotificationHandler<TNotification, TNotificationHandler>(this RegistrationBuilder registrationBuilder, IContainerBuilder containerBuilder) 
            where TNotification : INotification 
            where TNotificationHandler : IAsyncNotificationHandler<TNotification>
        {
            containerBuilder.BindHandlersInBuildCallback<TNotification, TNotificationHandler>();
            return registrationBuilder.As<IAsyncNotificationHandler<TNotification>>();
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
                handler.NotificationDisposable?.AddTo(bagBuilder);
                handler.NotificationDisposable = bagBuilder.Build();
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
                handler.NotificationDisposable?.AddTo(bagBuilder);
                handler.NotificationDisposable = bagBuilder.Build();
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
                handler.NotificationDisposable?.AddTo(bagBuilder);
                handler.NotificationDisposable = bagBuilder.Build();
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

                var handler = container.Resolve<IAsyncNotificationHandler<T1>>();
                handler.NotificationDisposable?.AddTo(bagBuilder);
                handler.NotificationDisposable = bagBuilder.Build();
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
                handler.NotificationDisposable?.AddTo(bagBuilder);
                handler.NotificationDisposable = bagBuilder.Build();
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