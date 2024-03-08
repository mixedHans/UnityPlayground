using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Straumann.Mediator.Example
{
    public sealed class AnotherExampleRequestHandler : IAsyncRequestHandler<ExampleRequest, string>
    {
        public Task<string> InvokeAsync(ExampleRequest request, CancellationToken cancellationToken = default)
        {
            Debug.Log("[AnotherExampleRequestHandler] Handled ExampleRequest!");
            return Task.FromResult("Hello From AnotherExampleRequestHandler!");
        }
    }
}