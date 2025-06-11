// --------------------------------------------------------------------------------------
// Mediator.cs
//
// Copyright (c) 2025 Steve Bang
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
// Description:
//   Provides the Mediator implementation for handling requests and dispatching them
//   to their corresponding handlers using dependency injection.
// --------------------------------------------------------------------------------------

using MediatorLight.Abstractions;
using System.Collections.Concurrent;

namespace MediatorLight;

/// <summary>
/// Mediator implementation for handling requests and dispatching them to their corresponding handlers.
/// </summary>
public class Mediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;
    private static readonly ConcurrentDictionary<Type, Type> _requestHandlerTypes = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="Mediator"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider used to resolve handlers.</param>
    public Mediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Sends a request and returns a response asynchronously.
    /// </summary>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <param name="request">The request to send.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A task representing the asynchronous operation, with a result of type <typeparamref name="TResponse"/>.</returns>
    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var handler = GetHandler<IRequest<TResponse>, TResponse>(request.GetType());

        return handler.Handle((dynamic)request, cancellationToken);
    }

    /// <summary>
    /// Sends a request that does not return a value asynchronously.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <param name="request">The request to send.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var handler = GetHandler<TRequest, Unit>(request.GetType());
        return handler.Handle((dynamic)request, cancellationToken);
    }

    /// <summary>
    /// Resolves the appropriate handler for the given request type.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <param name="requestType">The type of the request.</param>
    /// <returns>The resolved request handler.</returns>
    /// <exception cref="ArgumentException">Thrown if the request type does not implement IRequest or IRequest&lt;TResponse&gt;.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the handler is not registered.</exception>
    private IRequestHandler<TRequest, TResponse> GetHandler<TRequest, TResponse>(Type requestType)
        where TRequest : IRequest<TResponse>
    {
        var handlerType = _requestHandlerTypes.GetOrAdd(requestType, static requestType =>
        {
            var requestInterfaceType = requestType
                .GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequest<>));

            if (requestInterfaceType == null)
            {
                throw new ArgumentException($"Type {requestType} does not implement IRequest or IRequest<TResponse>");
            }

            var responseType = requestInterfaceType.GetGenericArguments()[0];
            var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType);
            return handlerType;
        });

        var handler = _serviceProvider.GetService(handlerType);

        if (handler == null)
        {
            throw new InvalidOperationException($"Handler for {requestType} not registered");
        }

        return (IRequestHandler<TRequest, TResponse>)handler;
    }
}