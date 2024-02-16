using VContainer;
using VContainer.Unity;

namespace MediatR_With_MessagePipe_VContainer.Usage
{
    public sealed class ParentLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // Registering NotificationTypes need to be done in the root lifetimescope
            builder.RegisterNotificationType<AnotherNotification>();
            builder.RegisterNotificationType<YetAnotherNotification>();
            
            // Test: Registering multiple times the same notification type --> should result in error
            // builder.RegisterNotificationType<ExampleNotification>();
            builder.RegisterNotificationType<ExampleNotification>();
            
            // Test: Registering multiple request handlers for the same request and response type is not allowed! 
            builder.RegisterMediatRRequestHandler<ExampleRequest, string, ExampleRequestHandler>();
            builder.RegisterMediatRRequestHandler<ExampleRequest, string, AnotherExampleRequestHandler>();
            
            // Register handler without return
            builder.RegisterMediatRRequestHandler<ExampleRequest, ExampleRequestHandlerWithoutReturn>();
        
            // Test: Registering multiple async request handlers for the same request and response type is not allowed! 
            builder.RegisterAsyncMediatRRequestHandler<ExampleRequest, string, ExampleAsyncRequestHandler>();
            builder.RegisterAsyncMediatRRequestHandler<ExampleRequest, string, AnotherAsyncExampleRequestHandler>();
            
            // Register async handler without return
            builder.RegisterAsyncMediatRRequestHandler<ExampleRequest, ExampleAsyncRequestHandlerWithoutReturn>();
            
            // register mediatr
            builder.Register<IUnityMediatR,UnityMediatR>(Lifetime.Singleton);
        }
    }
}