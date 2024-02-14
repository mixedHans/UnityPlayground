using MessagePipe;

namespace MediatR_With_MessagePipe_VContainer
{
    // Todo: RequestHandler without return type
    public sealed class ExampleRequestHandler : IRequestHandler<string, string>
    {
        public string Invoke(string request)
        {
            return request + " pong";
        }
    }
}