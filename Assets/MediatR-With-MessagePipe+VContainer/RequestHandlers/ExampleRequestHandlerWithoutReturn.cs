using UnityEngine;

namespace MediatR_With_MessagePipe_VContainer
{
    public sealed class ExampleRequestHandlerWithoutReturn : IMediatRRequestHandler<ExampleRequest>
    {
        public void InvokeWithoutReturn(ExampleRequest request)
        {
            Debug.Log("Handled Example Request, but did not return anything!");
        }
    }
}