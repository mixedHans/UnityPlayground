using VContainer;
using VContainer.Unity;

namespace Straumann.Mediator.Example
{
    public sealed class ParentLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // Registering NotificationTypes need to be done in the root lifetimescope
            builder.RegisterNotificationType<AnotherNotification>();
            builder.RegisterNotificationType<YetAnotherNotification>();
            
            // Test: Registering multiple times the same notification type --> should result in warning
            // builder.RegisterNotificationType<ExampleNotification>();
            builder.RegisterNotificationType<ExampleNotification>();
        
            // Test: Registering multiple async request handlers for the same request and response type is not allowed! 
            builder.RegisterAsyncMediatRRequestHandler<ExampleRequest, string, ExampleAsyncRequestHandler>();
            builder.RegisterAsyncMediatRRequestHandler<ExampleRequest, string, AnotherAsyncExampleRequestHandler>();
            
            // Register async handler without return
            builder.RegisterAsyncMediatRRequestHandler<ExampleRequest, ExampleAsyncRequestHandlerWithoutReturn>();
            
            // Register mediator --> Gets resolved the first time, when we inject it in to test monobehaviour
            builder.Register<IMediator,Mediator>(Lifetime.Singleton);
        }
    }
}