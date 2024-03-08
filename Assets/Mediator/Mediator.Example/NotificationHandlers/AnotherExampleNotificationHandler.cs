using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Straumann.Mediator.Example
{
    public sealed class AnotherExampleNotificationHandler : 
        IAsyncNotificationHandler<ExampleNotification>,
        INotificationHandler<AnotherNotification>,
        INotificationHandler<YetAnotherNotification>
    {
        public IDisposable NotificationDisposable { get; set; }

        public AnotherExampleNotificationHandler()
        {
            Debug.Log("[AnotherExampleNotificationHandler] Constructed");
        }
        
        public async Task HandleAsync(ExampleNotification notification, CancellationToken cancellationToken = default)
        {
            Debug.Log("[AnotherExampleNotificationHandler] Start handling ExampleNotification...");
            await Task.Delay(2000, cancellationToken);
            Debug.Log("[AnotherExampleNotificationHandler] Handled ExampleNotification");
        }

        public void Handle(AnotherNotification notification)
        {
            Debug.Log("[AnotherExampleNotificationHandler] Handle AnotherNotification");
        }
        
        public void Handle(YetAnotherNotification notification)
        {
            Debug.Log("[AnotherExampleNotificationHandler] Handle YetAnotherNotification");
        }

        public void Dispose()
        {
            Debug.Log("[AnotherExampleNotificationHandler] Disposing NotificationHandlers");
            NotificationDisposable?.Dispose();
        }
    }
}