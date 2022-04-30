using MediatR;

namespace CleanTemplate.Application.Abstractions;

public interface IQuery<out TResponse> : IRequest<TResponse> { }