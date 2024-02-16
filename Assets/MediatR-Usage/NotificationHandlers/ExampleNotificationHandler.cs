using System;
using UnityEngine;

namespace MediatR_With_MessagePipe_VContainer.Usage
{
    public sealed class ExampleNotificationHandler : 
        INotificationHandler<ExampleNotification>,
        INotificationHandler<AnotherNotification>,
        INotificationHandler<YetAnotherNotification>
    {
        public IDisposable MediatRDisposables { get; set; } 
        
        public ExampleNotificationHandler()
        {
            Debug.Log("[ExampleNotificationHandler] Constructed");
        }
        
        public void Handle(ExampleNotification notification)
        {
            Debug.Log("[ExampleNotificationHandler] Handle ExampleNotification");
        }

        public void Handle(AnotherNotification notification)
        {
            Debug.Log("[ExampleNotificationHandler] Handle AnotherNotification");
        }
        
        public void Handle(YetAnotherNotification notification)
        {
            Debug.Log("[ExampleNotificationHandler] Handle YetAnotherNotification");
        }

        public void Dispose()
        {
            Debug.Log("[ExampleNotificationHandler] Disposing NotificationHandlers");
            MediatRDisposables?.Dispose();
        }
    }
}