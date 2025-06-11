// --------------------------------------------------------------------------------------
// IRequestHandler.cs
//
// Copyright (c) 2025 Steve Bang
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
// Description:
//   Defines the IRequestHandler interfaces for handling requests in the Mediator pattern.
// --------------------------------------------------------------------------------------

namespace MediatorLight.Abstractions;


/// <summary>
/// Defines a handler for a request with a response type.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public interface IRequestHandler<in TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Handles the specified request asynchronously.
    /// </summary>
    /// <param name="request">The request to handle.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A task representing the asynchronous operation, with a result of type <typeparamref name="TResponse"/>.</returns>
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}

/// <summary>
/// Defines a handler for a request that does not return a value.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
public interface IRequestHandler<in TRequest> : IRequestHandler<TRequest, Unit>
    where TRequest : IRequest<Unit>
{ }