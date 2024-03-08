using System.Threading;
using System.Threading.Tasks;

namespace Straumann.Mediator
{
    // With response
    public interface IAsyncRequestHandler<in TRequest, TResponse>
        where TRequest : IRequest
    {
        Task<TResponse> InvokeAsync(TRequest request, CancellationToken cancellationToken = default);    
    }

    // Without response
    public interface IAsyncRequestHandler<in TRequest>
        where TRequest : IRequest
    {
        Task InvokeAsync(TRequest request, CancellationToken cancellationToken = default);    
    }
}