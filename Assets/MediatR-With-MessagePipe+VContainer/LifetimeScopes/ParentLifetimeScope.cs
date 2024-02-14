using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using EasyButtons;
using MessagePipe;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace MediatR_With_MessagePipe_VContainer
{
    public interface IMyDisposable : IDisposable
    {
        public void Dispose(IDisposable disposable);

        void IDisposable.Dispose()
        {
            
        }
    }
    
    public interface INotHandler : IDisposable
    {
        public IDisposable Disposable { get; set; }
        
        void IDisposable.Dispose()
        {
            Disposable.Dispose();
        }
    }
    
    class NotHandler : INotHandler
    {
        public Action OnDispose { get; set; }
    }


    public static class MediatRExtension
    {
        public static IContainerBuilder RegisterMediatRRequestHandler<TRequest, TResponse, TRequestHandler>(
            this IContainerBuilder builder, MessagePipeOptions options = null) where TRequestHandler : IRequestHandler<TRequest,TResponse>
        // Todo: Interface between TRequestHandler
        {
            options ??= builder.RegisterMessagePipe();
            builder.RegisterRequestHandler<TRequest, TResponse, TRequestHandler>(options);
            return builder;
        }

        
        // Todo: Abstraction of RegisterMessageBroker in RootLifetimeScope 
        public static IContainerBuilder RegisterMediatRNotificationHandler<TNotification, TNotificationHandler>(
            this IContainerBuilder builder, Lifetime lifetime, MessagePipeOptions options = null)
        {
            options ??= builder.RegisterMessagePipe();
            builder.RegisterMessageBroker<TNotification>(options);
            builder.Register<INotificationHandler<TNotification>, TNotificationHandler>(lifetime);
            
            builder.RegisterBuildCallback(container =>
            {
                var mediatr = container.Resolve<IUnityMediatR>();
                var notificationHandler = container.Resolve<INotificationHandler<TNotification>>();
                var disposable = mediatr.Register<TNotification>(notificationHandler.Handle);
                notificationHandler.Disposable.AddTo();
            });
            return builder;
        }
    }
    
    public class ParentLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            var options = builder.RegisterMessagePipe(ConfigureOptions);

            // create pub subs
            builder.RegisterMessageBroker<ExampleNotification>(options);
            builder.RegisterMessageBroker<AnotherNotification>(options);
        
            // register request handlers
            builder.RegisterRequestHandler<string, string, ExampleRequestHandler>(options);
        
            builder.Register<IUnityMediatR,UnityMediatR>(Lifetime.Singleton);
        
            // Todo: move the mediatr test into another class
            // resolve it, so we can use it in this class
            builder.RegisterBuildCallback(container => m_MediatR = container.Resolve<IUnityMediatR>());
        }

        private void ConfigureOptions(MessagePipeOptions obj)
        {
            obj.InstanceLifetime = InstanceLifetime.Scoped;
        }

        private IUnityMediatR m_MediatR;

        [Button]
        void SendNotification()
        {
            m_MediatR.Publish(new ExampleNotification());
            m_MediatR.Publish(new AnotherNotification());
        }
    
        [Button]
        void SendRequest()
        {
            var request = "ping";
            var response = m_MediatR.Send<string, string>(request);
            Debug.Log(response);
        }
    }
}