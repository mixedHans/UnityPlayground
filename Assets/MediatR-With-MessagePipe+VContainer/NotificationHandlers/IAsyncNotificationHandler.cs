using System;
using System.Threading;
using System.Threading.Tasks;

namespace MediatR_With_MessagePipe_VContainer
{
    public interface IAsyncNotificationHandler<in TNotification> : IDisposable
        where TNotification : INotification
    {
        IDisposable MediatRDisposables { get; set; }

        Task HandleAsync(TNotification notification, CancellationToken cancellationToken = default);
    }
}