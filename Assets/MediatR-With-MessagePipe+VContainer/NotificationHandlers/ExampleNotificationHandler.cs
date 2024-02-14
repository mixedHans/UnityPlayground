using System;
using UnityEngine;

namespace MediatR_With_MessagePipe_VContainer
{
    public class YetAnotherNotification : INotification
    {
    }

    public sealed class ExampleNotificationHandler : 
        INotificationHandler<ExampleNotification>, 
        INotificationHandler<AnotherNotification>, 
        INotificationHandler<YetAnotherNotification>,
        IDisposable
    {
        private readonly IDisposable m_Disposable;
        private IUnityMediatR m_UnityMediatR;
    
        public ExampleNotificationHandler(IUnityMediatR mediatR)
        {
            m_UnityMediatR = mediatR;
            m_Disposable = mediatR.Register<ExampleNotification, AnotherNotification, YetAnotherNotification>(Handle, Handle, Handle);
        }

        public void Handle(ExampleNotification channelTypeMessage)
        {
            Debug.Log(channelTypeMessage.ChannelMessage);
        }

        public void Handle(AnotherNotification notificationMessage)
        {
            Debug.Log(notificationMessage.ChannelMessage);
        }
        
        public void Handle(YetAnotherNotification channelTypeMessage)
        {
            throw new NotImplementedException();
        }

        public void Dispose() => m_Disposable.Dispose();
    }
}