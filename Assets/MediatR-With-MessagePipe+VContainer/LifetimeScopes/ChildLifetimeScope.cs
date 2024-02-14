using VContainer;
using VContainer.Unity;

namespace MediatR_With_MessagePipe_VContainer
{
    public sealed class ChildLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<ExampleNotificationHandler>(Lifetime.Singleton)
                .AsImplementedInterfaces();
        
            builder.RegisterBuildCallback(c => c.Resolve<INotificationHandler<ExampleNotification>>());
        }
    }
}