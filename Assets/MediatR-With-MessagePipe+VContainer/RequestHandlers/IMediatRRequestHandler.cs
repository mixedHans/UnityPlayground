using System.Threading.Tasks;
using MessagePipe;

namespace MediatR_With_MessagePipe_VContainer
{
    // With response
    public interface IMediatRRequestHandler<in TRequest, out TResponse>
        where TRequest : IRequest
    {
        TResponse Invoke(TRequest request);    
    }

    // Without response
    public interface IMediatRRequestHandler<in TRequest>
        where TRequest : IRequest
    {
        void Invoke(TRequest request);
    }
    

}