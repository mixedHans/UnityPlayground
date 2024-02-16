using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace MediatR_With_MessagePipe_VContainer.Usage
{
    public sealed class ExampleMixedNotificationHandler : 
        IAsyncNotificationHandler<ExampleNotification>,
        INotificationHandler<ExampleNotification>
    {
        public IDisposable MediatRDisposables { get; set; }

        public ExampleMixedNotificationHandler()
        {
            Debug.Log("[ExampleMixedNotificationHandler] Constructed");
        }
        
        public void Handle(ExampleNotification notification)
        {
            Debug.Log("[MixedHandler] Handle ExampleNotification");
        }
        
        public async Task HandleAsync(ExampleNotification notification, CancellationToken cancellationToken)
        {
            Debug.Log("[ExampleMixedNotificationHandler] Start handling notification...");
            await Task.Delay(1000, cancellationToken);
            Debug.Log("[ExampleMixedNotificationHandler] Handled notification: " + notification.Message);
        }

        public void Dispose()
        {
            Debug.Log("[ExampleMixedNotificationHandler] Disposing NotificationHandlers");
            MediatRDisposables?.Dispose();
        }
    }
}