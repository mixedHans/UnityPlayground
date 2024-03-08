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

        #region RequestHandlerRegistration

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

        #region NotificationRegistration
        
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

        #region NotificationHandlerRegistration
        
        public static RegistrationBuilder RegisterNotificationHandler<TNotification, TNotificationHandler>(this IContainerBuilder builder, Lifetime lifetime = Lifetime.Scoped)
            where TNotification : INotification
            where TNotificationHandler : IAsyncNotificationHandler<TNotification> 
        {
            return builder
                .BindHandlersInBuildCallback<TNotification, TNotificationHandler>()
                .Register<IAsyncNotificationHandler<TNotification>, TNotificationHandler>(lifetime)
                .AsSelf();
        }
        
        public static RegistrationBuilder RegisterNotificationHandler<TNotification1, TNotification2, TNotificationHandler>(
            this IContainerBuilder builder, Lifetime lifetime = Lifetime.Scoped)
            where TNotification1 : INotification where TNotification2 : INotification
            where TNotificationHandler : IAsyncNotificationHandler<TNotification1>,IAsyncNotificationHandler<TNotification2>
        {
            return builder
                .RegisterNotificationHandler<TNotification1, TNotificationHandler>(lifetime)
                .AsAdditionalNotificationHandler<TNotification2, TNotificationHandler>(builder);
        }
        
        public static RegistrationBuilder RegisterNotificationHandler<TNotification1, TNotification2, TNotification3, TNotificationHandler>(
            this IContainerBuilder builder, Lifetime lifetime = Lifetime.Scoped)
            where TNotification1 : INotification where TNotification2 : INotification where TNotification3 : INotification
            where TNotificationHandler : IAsyncNotificationHandler<TNotification1>, IAsyncNotificationHandler<TNotification2>, IAsyncNotificationHandler<TNotification3>
        {
            return builder
                .RegisterNotificationHandler<TNotification1, TNotificationHandler>(lifetime)
                .AsAdditionalNotificationHandler<TNotification2, TNotificationHandler>(builder)
                .AsAdditionalNotificationHandler<TNotification3, TNotificationHandler>(builder);
        }
        
        public static RegistrationBuilder RegisterNotificationHandler<TNotification1, TNotification2, TNotification3, TNotification4, TNotificationHandler>(
            this IContainerBuilder builder, Lifetime lifetime = Lifetime.Scoped)
            where TNotification1 : INotification where TNotification2 : INotification where TNotification3 : INotification where TNotification4 : INotification
            where TNotificationHandler : IAsyncNotificationHandler<TNotification1>, IAsyncNotificationHandler<TNotification2>, IAsyncNotificationHandler<TNotification3>, IAsyncNotificationHandler<TNotification4>
        {
            return builder
                .RegisterNotificationHandler<TNotification1, TNotificationHandler>(lifetime)
                .AsAdditionalNotificationHandler<TNotification2, TNotificationHandler>(builder)
                .AsAdditionalNotificationHandler<TNotification3, TNotificationHandler>(builder)
                .AsAdditionalNotificationHandler<TNotification4, TNotificationHandler>(builder);
        }
        
        public static RegistrationBuilder RegisterNotificationHandler<TNotification1, TNotification2, TNotification3, TNotification4, TNotification5, TNotificationHandler>(
            this IContainerBuilder builder, Lifetime lifetime = Lifetime.Scoped)
            where TNotification1 : INotification where TNotification2 : INotification where TNotification3 : INotification where TNotification4 : INotification where TNotification5 : INotification
            where TNotificationHandler : IAsyncNotificationHandler<TNotification1>, IAsyncNotificationHandler<TNotification2>, IAsyncNotificationHandler<TNotification3>, IAsyncNotificationHandler<TNotification4>, IAsyncNotificationHandler<TNotification5>
        {
            return builder
                .RegisterNotificationHandler<TNotification1, TNotificationHandler>(lifetime)
                .AsAdditionalNotificationHandler<TNotification2, TNotificationHandler>(builder)
                .AsAdditionalNotificationHandler<TNotification3, TNotificationHandler>(builder)
                .AsAdditionalNotificationHandler<TNotification4, TNotificationHandler>(builder)
                .AsAdditionalNotificationHandler<TNotification5, TNotificationHandler>(builder);
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
        
        private static RegistrationBuilder AsAdditionalNotificationHandler<TNotification, TNotificationHandler>(this RegistrationBuilder registrationBuilder, IContainerBuilder containerBuilder) 
            where TNotification : INotification 
            where TNotificationHandler : IAsyncNotificationHandler<TNotification>
        {
            containerBuilder.BindHandlersInBuildCallback<TNotification, TNotificationHandler>();
            return registrationBuilder.As<IAsyncNotificationHandler<TNotification>>();
        }

        #endregion
    }
}