using UnityEngine;

namespace Mediator_With_MessagePipe_VContainer.Usage
{
    public sealed class ExampleRequestHandler : IRequestHandler<ExampleRequest, string>
    {
        public string Invoke(ExampleRequest request)
        {
            Debug.Log("[ExampleRequestHandler] Handled ExampleRequest!");
            return "Hello From ExampleRequestHandler!";
        }
    }
    
    public sealed class AnotherExampleRequestHandler : IRequestHandler<ExampleRequest, string>
    {
        public string Invoke(ExampleRequest request)
        {
            Debug.Log("[AnotherExampleRequestHandler] Handled ExampleRequest!");
            return "Hello From AnotherExampleRequestHandler!";
        }
    }
}