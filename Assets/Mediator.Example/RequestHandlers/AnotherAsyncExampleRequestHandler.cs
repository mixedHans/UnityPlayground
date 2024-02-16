using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Straumann.Mediator.Example
{
    public sealed class AnotherAsyncExampleRequestHandler : IAsyncRequestHandler<ExampleRequest, string>
    {
        public async Task<string> InvokeAsync(ExampleRequest request, CancellationToken cancellationToken)
        {
            Debug.Log("[AnotherAsyncExampleRequestHandler] Start handling AsyncExampleRequest...");
            await Task.Delay(1000, cancellationToken);
            return "Hello From AnotherAsyncExampleRequestHandler!";
        }
    }
}