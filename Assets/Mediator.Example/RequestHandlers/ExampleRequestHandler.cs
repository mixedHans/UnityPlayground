using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Straumann.Mediator.Example
{
    public sealed class ExampleRequestHandler : IAsyncRequestHandler<ExampleRequest, string>
    {
        public Task<string> InvokeAsync(ExampleRequest request, CancellationToken cancellationToken = default)
        {
            Debug.Log("[ExampleRequestHandler] Handled ExampleRequest!");
            return Task.FromResult("Hello From ExampleRequestHandler!");
        }
    }
}