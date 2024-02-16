using System.Reflection;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Mediator_With_MessagePipe_VContainer.Usage
{
    public sealed class ChildLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // builder.RegisterMediatRNotificationHandler<
            //     AnotherNotification, ExampleNotification,
            //     AnotherExampleNotificationHandler>();

            // builder.RegisterMediatRNotificationHandler
            //     <AnotherNotification, ExampleNotification>()
            //     .As<ExampleNotificationHandler>();
            //
            // builder.RegisterMediatRNotificationHandler<ExampleNotificationHandler>()
            //     .For<AnotherNotification>()
            //     .For<ExampleNotification>();

            builder.RegisterNotificationHandler<
                AnotherNotification, ExampleNotification, 
                ExampleNotificationHandler>();
            
            builder.RegisterNotificationHandler<
                AnotherNotification, ExampleNotification, 
                AnotherExampleNotificationHandler>();

            builder.RegisterNotificationHandler<
                ExampleNotification, 
                ExampleMixedNotificationHandler>();
            
            builder.RegisterAsyncMediatRNotificationHandler<
                ExampleNotification, 
                ExampleMixedNotificationHandler>();
        }
    }
}