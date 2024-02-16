using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Straumann.Mediator.Example
{
    public sealed class ExampleMixedNotificationHandler : 
        IAsyncNotificationHandler<AnotherNotification>,
        INotificationHandler<ExampleNotification>
    {
        public IDisposable NotificationDisposable { get; set; }

        public ExampleMixedNotificationHandler()
        {
            Debug.Log("[ExampleMixedNotificationHandler] Constructed");
        }
        
        public void Handle(ExampleNotification notification)
        {
            Debug.Log("[MixedHandler] Handle ExampleNotification");
        }

        public async Task HandleAsync(AnotherNotification notification, CancellationToken cancellationToken = default)
        {
            Debug.Log("[ExampleMixedNotificationHandler] Start handling notification...");
            await Task.Delay(1000, cancellationToken);
            Debug.Log("[ExampleMixedNotificationHandler] Handled notification: " + notification.Message);
        }
        
        public void Dispose()
        {
            Debug.Log("[ExampleMixedNotificationHandler] Disposing NotificationHandlers");
            NotificationDisposable?.Dispose();
        }
    }
}