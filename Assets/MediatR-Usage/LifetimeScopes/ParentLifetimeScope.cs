using VContainer;
using VContainer.Unity;

namespace MediatR_With_MessagePipe_VContainer
{
    public class ParentLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterNotificationType<ExampleNotification>();
            builder.RegisterNotificationType<AnotherNotification>();
            builder.RegisterNotificationType<YetAnotherNotification>();
            builder.RegisterMediatRRequestHandler<ExampleRequest, string, ExampleRequestHandler>();
            builder.RegisterMediatRRequestHandler<ExampleRequest, ExampleRequestHandlerWithoutReturn>();
        
            // register mediatr
            builder.Register<IUnityMediatR,UnityMediatR>(Lifetime.Singleton);
        }
    }
}