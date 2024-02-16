using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mediator_With_MessagePipe_VContainer
{
    // Todo: Implement AsyncNotificationHandlers
    public interface INotificationHandler<in TNotification> : IDisposable//, IAsyncNotificationHandler<TNotification>
        where TNotification : INotification
    {
        
        // Task IAsyncNotificationHandler<TNotification>.HandleAsync(TNotification notification, CancellationToken cancellationToken)
        // {
        //     Handle(notification);
        //     return Task.CompletedTask;
        // }
        
        IDisposable NotificationDisposable { get; set; }
        
        void Handle(TNotification notification);
    }
}