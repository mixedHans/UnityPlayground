using UnityEngine;

namespace MediatR_With_MessagePipe_VContainer.Usage
{
    public sealed class ExampleRequestHandlerWithoutReturn : IMediatRRequestHandler<ExampleRequest>
    {
        public void Invoke(ExampleRequest request)
        {
            Debug.Log("[ExampleRequestHandlerWithoutReturn] Handled ExampleRequest, but did not return anything!");
        }
    }
}