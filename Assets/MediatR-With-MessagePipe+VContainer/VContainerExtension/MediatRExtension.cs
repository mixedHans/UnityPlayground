using MessagePipe;
using UnityEngine;
using VContainer;

namespace MediatR_With_MessagePipe_VContainer
{
    public static class MediatRExtension
    {
        private static MessagePipeOptions _options;

        public static IContainerBuilder RegisterMediatRRequestHandler<TRequest, TResponse, TRequestHandler>(
            this IContainerBuilder builder)
            where TRequestHandler : IMediatRRequestHandler<TRequest, TResponse>
            where TRequest : IRequest
        {
            _options ??= builder.RegisterMessagePipe();
            if (!builder.Exists(typeof(IMediatRRequestHandler<TRequest, TResponse>), true))
            {
                builder.Register(typeof(TRequestHandler), Lifetime.Singleton)
                    .As(typeof(IMediatRRequestHandler<TRequest, TResponse>));
            }
            else
            {
                Debug.LogWarning("You are trying to register multiple MediatRRequestHandlers. This is not allowed!" +
                                 $"Handler: {typeof(TRequestHandler)}");
            }
            return builder;
        }

        public static IContainerBuilder RegisterMediatRRequestHandler<TRequest, TRequestHandler>(
            this IContainerBuilder builder)
            where TRequestHandler : IMediatRRequestHandler<TRequest>
            where TRequest : IRequest
        {
            _options ??= builder.RegisterMessagePipe();
            if (!builder.Exists(typeof(IMediatRRequestHandler<TRequest, NullResponse>), true))
            {
                builder.Register(typeof(TRequestHandler), Lifetime.Singleton)
                    .As(typeof(IMediatRRequestHandler<TRequest, NullResponse>));
            }
            else
            {
                Debug.LogWarning("You are trying to register multiple MediatRRequestHandlers. This is not allowed!" +
                                 $"Handler: {typeof(TRequestHandler)}");
            }
            return builder;
        }

        public static IContainerBuilder RegisterNotificationType<TNotificationType>(
            this IContainerBuilder builder)
            where TNotificationType : INotification
        {
            _options ??= builder.RegisterMessagePipe();
            builder.RegisterMessageBroker<TNotificationType>(_options);
            return builder;
        }

        public static IContainerBuilder RegisterNotificationTypes(this IContainerBuilder builder)
        {
            _options ??= builder.RegisterMessagePipe();
            foreach (var assembly in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (typeof(INotification).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                    {
                        // builder.Register(type, typeof(MessageBroker<>))
                    }
                }
            }
            return builder;
        }
        
        public static IContainerBuilder RegisterMediatRNotificationHandler<TNotification, TNotificationHandler>(
            this IContainerBuilder builder, Lifetime lifetime = Lifetime.Singleton)
            where TNotificationHandler : INotificationHandler<TNotification> where TNotification : INotification
        {
            builder.Register<INotificationHandler<TNotification>, TNotificationHandler>(lifetime);
            builder.RegisterBuildCallback(container =>
            {
                builder.RegisterBuildCallback(container =>
                {
                    var bag = DisposableBag.CreateBuilder();

                    // T1
                    var notificationHandler1 = container.Resolve<INotificationHandler<TNotification>>();
                    var mediatr = container.Resolve<IUnityMediatR>();
                    mediatr.Register<TNotification>(notificationHandler1.Handle).AddTo(bag);

                    // Add to disposables
                    notificationHandler1.MediatRDisposables?.AddTo(bag);
                    notificationHandler1.MediatRDisposables = bag.Build();
                });
            });
            return builder;
        }

        public static IContainerBuilder RegisterMediatRNotificationHandler<T1, T2, TNotificationHandler>(
            this IContainerBuilder builder, Lifetime lifetime = Lifetime.Singleton)
            where T1 : INotification
            where T2 : INotification
        {
            builder.Register<TNotificationHandler>(lifetime)
                .As<INotificationHandler<T1>, INotificationHandler<T2>>();

            builder.RegisterBuildCallback(container =>
            {
                var mediatr = container.Resolve<IUnityMediatR>();
                var bag = DisposableBag.CreateBuilder();

                // T1
                var notificationHandler1 = container.Resolve<INotificationHandler<T1>>();
                mediatr.Register<T1>(notificationHandler1.Handle).AddTo(bag);

                // T2
                var notificationHandler2 = (INotificationHandler<T2>)notificationHandler1;
                mediatr.Register<T2>(notificationHandler2.Handle).AddTo(bag);

                // Add to disposables
                notificationHandler1.MediatRDisposables?.AddTo(bag);
                notificationHandler1.MediatRDisposables = bag.Build();
            });
            return builder;
        }

        public static IContainerBuilder RegisterMediatRNotificationHandler<T1, T2, T3, TNotificationHandler>(
            this IContainerBuilder builder, Lifetime lifetime = Lifetime.Singleton)
            where T1 : INotification
            where T2 : INotification
            where T3 : INotification
        {
            builder.Register<TNotificationHandler>(lifetime)
                .As<INotificationHandler<T1>, INotificationHandler<T2>, INotificationHandler<T3>>();

            builder.RegisterBuildCallback(container =>
            {
                var mediatr = container.Resolve<IUnityMediatR>();
                var bag = DisposableBag.CreateBuilder();

                // T1
                var notificationHandler1 = container.Resolve<INotificationHandler<T1>>();
                mediatr.Register<T1>(notificationHandler1.Handle).AddTo(bag);

                // T2
                var notificationHandler2 = (INotificationHandler<T2>)notificationHandler1;
                mediatr.Register<T2>(notificationHandler2.Handle).AddTo(bag);

                // T3
                var notificationHandler3 = (INotificationHandler<T3>)notificationHandler1;
                mediatr.Register<T3>(notificationHandler3.Handle).AddTo(bag);

                // Add to disposables
                notificationHandler1.MediatRDisposables?.AddTo(bag);
                notificationHandler1.MediatRDisposables = bag.Build();
            });
            return builder;
        }

        public static IContainerBuilder RegisterMediatRNotificationHandler<T1, T2, T3, T4, TNotificationHandler>(
            this IContainerBuilder builder, Lifetime lifetime = Lifetime.Singleton)
            where T1 : INotification
            where T2 : INotification
            where T3 : INotification
            where T4 : INotification
        {
            builder.Register<TNotificationHandler>(lifetime)
                .As<INotificationHandler<T1>, INotificationHandler<T2>, INotificationHandler<T3>,
                    INotificationHandler<T4>>();

            builder.RegisterBuildCallback(container =>
            {
                var mediatr = container.Resolve<IUnityMediatR>();
                var bag = DisposableBag.CreateBuilder();

                // T1
                var notificationHandler1 = container.Resolve<INotificationHandler<T1>>();
                mediatr.Register<T1>(notificationHandler1.Handle).AddTo(bag);

                // T2
                var notificationHandler2 = (INotificationHandler<T2>)notificationHandler1;
                mediatr.Register<T2>(notificationHandler2.Handle).AddTo(bag);

                // T3
                var notificationHandler3 = (INotificationHandler<T3>)notificationHandler1;
                mediatr.Register<T3>(notificationHandler3.Handle).AddTo(bag);

                // T4
                var notificationHandler4 = (INotificationHandler<T4>)notificationHandler1;
                mediatr.Register<T4>(notificationHandler4.Handle).AddTo(bag);

                // Add to disposables
                notificationHandler1.MediatRDisposables?.AddTo(bag);
                notificationHandler1.MediatRDisposables = bag.Build();
            });
            return builder;
        }

        public static IContainerBuilder RegisterMediatRNotificationHandler<T1, T2, T3, T4, T5, TNotificationHandler>(
            this IContainerBuilder builder, Lifetime lifetime = Lifetime.Singleton)
            where T1 : INotification
            where T2 : INotification
            where T3 : INotification
            where T4 : INotification
            where T5 : INotification
        {
            builder.Register<TNotificationHandler>(lifetime)
                .As(typeof(INotificationHandler<T1>),
                    typeof(INotificationHandler<T2>),
                    typeof(INotificationHandler<T3>),
                    typeof(INotificationHandler<T4>),
                    typeof(INotificationHandler<T5>));

            builder.RegisterBuildCallback(container =>
            {
                var mediatr = container.Resolve<IUnityMediatR>();
                var bag = DisposableBag.CreateBuilder();

                // T1
                var notificationHandler1 = container.Resolve<INotificationHandler<T1>>();
                mediatr.Register<T1>(notificationHandler1.Handle).AddTo(bag);

                // T2
                var notificationHandler2 = (INotificationHandler<T2>)notificationHandler1;
                mediatr.Register<T2>(notificationHandler2.Handle).AddTo(bag);

                // T3
                var notificationHandler3 = (INotificationHandler<T3>)notificationHandler1;
                mediatr.Register<T3>(notificationHandler3.Handle).AddTo(bag);

                // T4
                var notificationHandler4 = (INotificationHandler<T4>)notificationHandler1;
                mediatr.Register<T4>(notificationHandler4.Handle).AddTo(bag);

                // T5
                var notificationHandler5 = (INotificationHandler<T5>)notificationHandler1;
                mediatr.Register<T5>(notificationHandler5.Handle).AddTo(bag);

                // Add to disposables
                notificationHandler1.MediatRDisposables?.AddTo(bag);
                notificationHandler1.MediatRDisposables = bag.Build();
            });
            return builder;
        }
    }
}