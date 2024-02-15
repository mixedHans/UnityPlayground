using System;

namespace MediatR_With_MessagePipe_VContainer
{
    public interface INotificationHandler<in TNotification> : IDisposable
        where TNotification : INotification
    {
        IDisposable MediatRDisposables { get; set; }

        void Handle(TNotification notification);
    }
}