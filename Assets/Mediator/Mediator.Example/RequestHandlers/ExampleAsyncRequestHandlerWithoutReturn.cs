using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Straumann.Mediator.Example
{
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