using MediatR;

namespace CleanTemplate.Application.Abstractions;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
		where TQuery : IQuery<TResponse> { }