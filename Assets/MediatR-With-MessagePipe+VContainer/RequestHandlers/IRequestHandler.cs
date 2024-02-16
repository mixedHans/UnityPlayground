namespace Mediator_With_MessagePipe_VContainer
{
    // With response
    public interface IRequestHandler<in TRequest, out TResponse>
        where TRequest : IRequest
    {
        TResponse Invoke(TRequest request);    
    }

    // Without response
    public interface IRequestHandler<in TRequest>
        where TRequest : IRequest
    {
        void Invoke(TRequest request);
    }
}