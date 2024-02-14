using System;
using UnityEngine;

namespace MediatR_With_MessagePipe_VContainer
{
    public sealed class ExampleNotificationHandler : 
        INotificationHandler<ExampleNotification>,
        INotificationHandler<AnotherNotification>,
        INotificationHandler<YetAnotherNotification>,
        IDisposable
    {
        private IUnityMediatR m_UnityMediatR;

        public IDisposable MediatRDisposables { get; set; } 

        public ExampleNotificationHandler(IUnityMediatR mediatR)
        {
            m_UnityMediatR = mediatR;
        }

        public void Handle(ExampleNotification notification)
        {
            Debug.Log(notification.Message);
        }

        public void Handle(AnotherNotification notification)
        {
            Debug.Log(notification.Message);
        }
        
        public void Handle(YetAnotherNotification notification)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            MediatRDisposables.Dispose();
        }
    }
}