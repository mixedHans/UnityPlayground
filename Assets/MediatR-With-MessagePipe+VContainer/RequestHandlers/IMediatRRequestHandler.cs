using MessagePipe;

namespace MediatR_With_MessagePipe_VContainer
{
    // With response
    public interface IMediatRRequestHandler<in TRequest, out TResponse>
        where TRequest : IRequest
    {
        TResponse Invoke(TRequest request);    
    }

    // Todo: register without response by itself
    // Without response
    public interface IMediatRRequestHandler<in TRequest> : IMediatRRequestHandler<TRequest, NullResponse> 
        where TRequest : IRequest
    {
        void InvokeWithoutReturn(TRequest request);

        NullResponse IMediatRRequestHandler<TRequest, NullResponse>.Invoke(TRequest request)
        {
            InvokeWithoutReturn(request);
            return null;
        }
    }
}