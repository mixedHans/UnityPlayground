namespace MediatR_With_MessagePipe_VContainer
{
    public sealed class ExampleNotification : INotification
    {
        public readonly string ChannelMessage = "Hello Notification";
    }
}