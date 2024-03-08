using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Straumann.Mediator.Example
{
    public sealed class ExampleRequestHandlerWithoutReturn : IAsyncRequestHandler<ExampleRequest>
    {
        public Task InvokeAsync(ExampleRequest request, CancellationToken cancellationToken = default)
        {
            Debug.Log("[ExampleRequestHandlerWithoutReturn] Handled ExampleRequest, but did not return anything!");
            return Task.CompletedTask;
        }
    }
}