using MediatR;

namespace CleanTemplate.Application.Abstractions;

public interface ICommand<out TResponse> : IRequest<TResponse> { }