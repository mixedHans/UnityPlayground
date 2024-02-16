using System.Threading;
using System.Threading.Tasks;

namespace MediatR_With_MessagePipe_VContainer
{
    // With response
    public interface IAsyncMediatRRequestHandler<in TRequest, TResponse>
        where TRequest : IRequest
    {
        Task<TResponse> InvokeAsync(TRequest request, CancellationToken cancellationToken = default);    
    }

    // Without response
    public interface IAsyncMediatRRequestHandler<in TRequest>
        where TRequest : IRequest
    {
        Task InvokeAsync(TRequest request, CancellationToken cancellationToken = default);    
    }
}