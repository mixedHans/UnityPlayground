using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace MediatR_With_MessagePipe_VContainer.Usage
{
    public sealed class ChildLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterMediatRNotificationHandler<
                AnotherNotification, 
                AnotherExampleNotificationHandler>();
            
            builder.RegisterMediatRNotificationHandler<
                ExampleNotification, 
                ExampleNotificationHandler>();

            builder.RegisterMediatRNotificationHandler<
                ExampleNotification, 
                ExampleMixedNotificationHandler>();
            
            builder.RegisterAsyncMediatRNotificationHandler<
                ExampleNotification, 
                ExampleMixedNotificationHandler>();
        }
    }
}