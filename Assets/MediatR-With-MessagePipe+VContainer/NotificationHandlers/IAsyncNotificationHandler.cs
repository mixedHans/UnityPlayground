using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mediator_With_MessagePipe_VContainer
{
    public interface IAsyncNotificationHandler<in TNotification> : IDisposable
        where TNotification : INotification
    {
        IDisposable NotificationDisposable { get; set; }

        Task HandleAsync(TNotification notification, CancellationToken cancellationToken = default);
    }
}