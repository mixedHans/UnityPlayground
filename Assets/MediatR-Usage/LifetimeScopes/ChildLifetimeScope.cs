using VContainer;
using VContainer.Unity;

namespace MediatR_With_MessagePipe_VContainer.Usage
{
    public sealed class ChildLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterMediatRNotificationHandler<
                ExampleNotification, 
                AnotherNotification, 
                YetAnotherNotification, 
                ExampleNotificationHandler>();
        }
    }
}