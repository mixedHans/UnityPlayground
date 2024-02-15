using UnityEngine;

namespace MediatR_With_MessagePipe_VContainer.Usage
{
    public sealed class ExampleRequestHandler : IMediatRRequestHandler<ExampleRequest, string>
    {
        public string Invoke(ExampleRequest request)
        {
            Debug.Log("Handled Example Request!");
            return "Hello From ExampleRequestHandler!";
        }
    }
    
    public sealed class AnotherExampleRequestHandler : IMediatRRequestHandler<ExampleRequest, string>
    {
        public string Invoke(ExampleRequest request)
        {
            Debug.Log("Handled Example Request!");
            return "Hello From ExampleRequestHandler!";
        }
    }
}