using MessagePipe;

namespace MediatR_With_MessagePipe_VContainer
{
    // With response
    public interface IMediatRRequestHandler<in TRequest, out TResponse> : IRequestHandler<TRequest, TResponse> 
        where TRequest : IRequest
    {
    }

    // Without response
    public interface IMediatRRequestHandler<in TRequest> : IRequestHandler<TRequest, NullResponse> 
        where TRequest : IRequest
    {
        void InvokeWithoutReturn(TRequest request);

        NullResponse IRequestHandlerCore<TRequest, NullResponse>.Invoke(TRequest request)
        {
            InvokeWithoutReturn(request);
            return null;
        }
    }
}