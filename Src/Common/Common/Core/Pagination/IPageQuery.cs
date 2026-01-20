using MediatR;

namespace Sport.Common.Core;

/// <summary>
/// A MediatR Query that specifically asks for a paginated result.
/// </summary>
public interface IPageQuery<out TResponse> : IPageRequest, IRequest<TResponse>
    where TResponse : class
{ }