using System.Threading.Tasks;
using EasyButtons;
using UnityEngine;
using VContainer;

namespace Straumann.Mediator.Example
{
    public class MediatorRuntimeTest : MonoBehaviour
    {
        private IMediator m_Mediator;
        
        [Inject]
        public void Construct(IMediator mediatR)
        {
            m_Mediator = mediatR;
        }

        [Button]
        async Task SendExampleNotificationAsync()
        {
            Debug.Log("[Publisher] Start sending notifications!");
            var publisherHandle = m_Mediator.PublishAsync(new ExampleNotification());
            
            Debug.Log("[Publisher] Doing other stuff and waiting for notification handlers to complete...");
            await publisherHandle;
            
            Debug.Log("[Publisher] All notification handlers handled notification, now resuming in publisher.");
        }
        
        [Button]
        void SendExampleNotificationFireAndForget()
        {
            Debug.Log("[Publisher] Start sending example notification as fire and forget");
            m_Mediator.PublishAsync(new ExampleNotification());
            Debug.Log("[Publisher] I dont care, if the notifications are handles already, I just resume my work.");
        }
        
        [Button]
        async Task SendAnotherNotificationAsync()
        {
            Debug.Log("[Publisher] Start sending notifications!");
            var publisherHandle = m_Mediator.PublishAsync(new AnotherNotification());
            
            Debug.Log("[Publisher] Doing other stuff and waiting for notification handlers to complete...");
            await publisherHandle;
            
            Debug.Log("[Publisher] All notification handlers handled notification, now resuming in publisher.");
        }
        
        [Button]
        void SendAnotherNotificationFireAndForget()
        {
            Debug.Log("[Publisher] Start sending example notification as fire and forget");
            m_Mediator.PublishAsync(new AnotherNotification());
            Debug.Log("[Publisher] I dont care, if the notifications are handles already, I just resume my work.");
        }

        [Button]
        async void SendRequestAsync()
        {
            var request = new ExampleRequest();
            Debug.Log("Sending example request async");
            var response = await m_Mediator.SendAsync<ExampleRequest, string>(request);
            Debug.Log("Received Response: " + response);
        }
        
        [Button]
        async void SendRequestWithoutReturnAsync()
        {
            var request = new ExampleRequest();
            Debug.Log("Sending example request without return async");
            await m_Mediator.SendAsync<ExampleRequest>(request);
            Debug.Log("Awaited request");
        }
    }
}