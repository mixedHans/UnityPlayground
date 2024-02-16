using UnityEngine;

namespace Mediator_With_MessagePipe_VContainer.Usage
{
    public sealed class ExampleRequestHandlerWithoutReturn : IRequestHandler<ExampleRequest>
    {
        public void Invoke(ExampleRequest request)
        {
            Debug.Log("[ExampleRequestHandlerWithoutReturn] Handled ExampleRequest, but did not return anything!");
        }
    }
}