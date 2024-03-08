using VContainer;
using VContainer.Unity;

namespace Straumann.Mediator.Example
{
    public sealed class ChildLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterNotificationHandler<
                AnotherNotification, ExampleNotification, 
                ExampleNotificationHandler>();
            
            builder.RegisterNotificationHandler<
                AnotherNotification, ExampleNotification, 
                AnotherExampleNotificationHandler>();

            builder.RegisterNotificationHandler<
                ExampleNotification, AnotherNotification,
                ExampleMixedNotificationHandler>();
        }
    }
}