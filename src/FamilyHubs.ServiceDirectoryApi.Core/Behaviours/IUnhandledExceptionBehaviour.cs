using MediatR;

namespace FamilyHubs.ServiceDirectoryApi.Core.Behaviours
{
    public interface IUnhandledExceptionBehaviour<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next);
    }
}