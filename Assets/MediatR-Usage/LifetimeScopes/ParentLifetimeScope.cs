using System;
using VContainer;
using VContainer.Unity;

namespace MediatR_With_MessagePipe_VContainer.Usage
{
    public sealed class ParentLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // Todo: register via assembly
            // builder.RegisterNotificationTypes();
            // builder.RegisterMediatRRequestHandlers();
            
            builder.RegisterNotificationType<ExampleNotification>();
            builder.RegisterNotificationType<AnotherNotification>();
            builder.RegisterNotificationType<YetAnotherNotification>();
            
            builder.RegisterMediatRRequestHandler<ExampleRequest, string, ExampleRequestHandler>();
            builder.RegisterMediatRRequestHandler<ExampleRequest, string, AnotherExampleRequestHandler>();
            builder.RegisterMediatRRequestHandler<ExampleRequest, ExampleRequestHandlerWithoutResponseWithoutReturn>();
        
            // register mediatr
            builder.Register<IUnityMediatR,UnityMediatR>(Lifetime.Singleton);
        }
    }
}