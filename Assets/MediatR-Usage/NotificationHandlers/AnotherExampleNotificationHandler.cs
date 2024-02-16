using System;
using UnityEngine;

namespace MediatR_With_MessagePipe_VContainer.Usage
{
    public sealed class AnotherExampleNotificationHandler : 
        INotificationHandler<ExampleNotification>,
        INotificationHandler<AnotherNotification>,
        INotificationHandler<YetAnotherNotification>
    {
        public IDisposable MediatRDisposables { get; set; } 
        
        public AnotherExampleNotificationHandler()
        {
            Debug.Log("[AnotherExampleNotificationHandler] Constructed");
        }

        public void Handle(ExampleNotification notification)
        {
            Debug.Log("[AnotherExampleNotificationHandler] Handle ExampleNotification");
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
            MediatRDisposables?.Dispose();
        }
    }
}