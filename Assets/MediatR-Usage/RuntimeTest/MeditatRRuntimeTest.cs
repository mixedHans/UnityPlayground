using System.Threading.Tasks;
using EasyButtons;
using UnityEngine;
using VContainer;

namespace Mediator_With_MessagePipe_VContainer.Usage
{
    public class MeditatRRuntimeTest : MonoBehaviour
    {
        private IUnityMediatR m_MediatR;

        [Inject]
        public void Construct(IUnityMediatR mediatR)
        {
            m_MediatR = mediatR;
        }

        [Button]
        void SendNotification()
        {
            m_MediatR.Publish(new ExampleNotification());
            m_MediatR.Publish(new AnotherNotification());
        }
        
        [Button]
        void SendNotificationAsync()
        {
            m_MediatR.PublishAsync(new ExampleNotification());
        }

        [Button]
        void SendRequest()
        {
            var request = new ExampleRequest();
            Debug.Log("Sending example request");
            var response = m_MediatR.Send<ExampleRequest, string>(request);
            Debug.Log("Received Response: " + response);
        }
        
        [Button]
        async void SendRequestAsync()
        {
            var request = new ExampleRequest();
            Debug.Log("Sending example request async");
            var response = await m_MediatR.SendAsync<ExampleRequest, string>(request);
            Debug.Log("Received Response: " + response);
        }

        [Button]
        void SendRequestWithoutReturn()
        {
            var request = new ExampleRequest();
            Debug.Log("Sending example request without return");
            m_MediatR.Send<ExampleRequest>(request);
        }
        
        [Button]
        async void SendRequestWithoutReturnAsync()
        {
            var request = new ExampleRequest();
            Debug.Log("Sending example request without return async");
            await m_MediatR.SendAsync<ExampleRequest>(request);
            Debug.Log("Awaited request");
        }
    }
}