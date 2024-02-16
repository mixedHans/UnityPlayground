using System;
using System.Threading;
using System.Threading.Tasks;

namespace Straumann.Mediator
{
    public interface IAsyncNotificationHandler<in TNotification> : IDisposable
        where TNotification : INotification
    {
        IDisposable NotificationDisposable { get; set; }

        Task HandleAsync(TNotification notification, CancellationToken cancellationToken = default);
    }
}