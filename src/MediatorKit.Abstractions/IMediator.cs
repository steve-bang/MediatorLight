// --------------------------------------------------------------------------------------
// IMediator.cs
//
// Copyright (c) 2025 Steve Bang
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
// Description:
//   Defines the IMediator interface for sending requests and receiving responses
//   in a decoupled manner.
// --------------------------------------------------------------------------------------

/// <summary>
/// Defines the contract for a mediator that sends requests to their corresponding handlers.
/// </summary>
namespace MediatorKit.Abstractions;

public interface IMediator
{
    /// <summary>
    /// Sends a request and returns a response asynchronously.
    /// </summary>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <param name="request">The request to send.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A task representing the asynchronous operation, with a result of type <typeparamref name="TResponse"/>.</returns>
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a request that does not return a value asynchronously.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <param name="request">The request to send.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest;
}