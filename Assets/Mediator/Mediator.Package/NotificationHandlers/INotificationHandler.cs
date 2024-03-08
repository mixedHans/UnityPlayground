using System.Threading;
using System.Threading.Tasks;

namespace Straumann.Mediator
{
    public interface INotificationHandler<in TNotification> : IAsyncNotificationHandler<TNotification>
        where TNotification : INotification
    {
        Task IAsyncNotificationHandler<TNotification>.HandleAsync(TNotification notification, CancellationToken cancellationToken)
        {
            Handle(notification);
            return Task.CompletedTask;
        }
        
        void Handle(TNotification notification);
    }
}