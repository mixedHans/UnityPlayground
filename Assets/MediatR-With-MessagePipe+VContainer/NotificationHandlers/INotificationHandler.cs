namespace MediatR_With_MessagePipe_VContainer
{
    public interface INotificationHandler<in TNotification> where TNotification : INotification
    {
        void Handle(TNotification channelTypeMessage);
    }
}