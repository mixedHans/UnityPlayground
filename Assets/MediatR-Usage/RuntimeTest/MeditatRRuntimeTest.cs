using EasyButtons;
using UnityEngine;
using VContainer;

namespace MediatR_With_MessagePipe_VContainer.Usage
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
        void SendRequest()
        {
            var request = new ExampleRequest();
            var response = m_MediatR.Send<ExampleRequest, string>(request);
            Debug.Log("Recieved Response: " + response);
        }

        [Button]
        void SendRequestWithoutReturn()
        {
            var request = new ExampleRequest();
            m_MediatR.Send<ExampleRequest>(request);
        }
    }
}