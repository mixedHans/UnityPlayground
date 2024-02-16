using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Mediator_With_MessagePipe_VContainer.Usage
{
    public sealed class ExampleAsyncRequestHandler : IAsyncRequestHandler<ExampleRequest, string>
    {
        public async Task<string> InvokeAsync(ExampleRequest request, CancellationToken cancellationToken)
        {
            Debug.Log("[ExampleAsyncRequestHandler] Start handling AsyncExampleRequest...");
            await Task.Delay(1000, cancellationToken);
            return "Hello From ExampleAsyncRequestHandler!";
        }
    }
    
    public sealed class AnotherAsyncExampleRequestHandler : IAsyncRequestHandler<ExampleRequest, string>
    {
        public async Task<string> InvokeAsync(ExampleRequest request, CancellationToken cancellationToken)
        {
            Debug.Log("[AnotherAsyncExampleRequestHandler] Start handling AsyncExampleRequest...");
            await Task.Delay(1000, cancellationToken);
            return "Hello From AnotherAsyncExampleRequestHandler!";
        }
    }
    
    public sealed class ExampleAsyncRequestHandlerWithoutReturn : IAsyncRequestHandler<ExampleRequest>
    {
        public async Task InvokeAsync(ExampleRequest request, CancellationToken cancellationToken)
        {
            Debug.Log("[ExampleAsyncRequestHandlerWithoutReturn] Start handling AsyncExampleRequest...");
            await Task.Delay(1000, cancellationToken);
            Debug.Log("[ExampleAsyncRequestHandlerWithoutReturn] Handled ExampleRequest, but did not return anything!");
        }
    }
}