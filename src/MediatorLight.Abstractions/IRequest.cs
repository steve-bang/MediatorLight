// --------------------------------------------------------------------------------------
// IRequest.cs
//
// Copyright (c) 2025 Steve Bang
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
// Description:
//   Defines the IRequest and Unit abstractions for the Mediator pattern.
// --------------------------------------------------------------------------------------

namespace MediatorLight.Abstractions;

/// <summary>
/// Represents a request with a response type for the Mediator pattern.
/// </summary>
/// <typeparam name="TResponse">The type of the response returned by the handler.</typeparam>
public interface IRequest<TResponse> { }

/// <summary>
/// Represents a request that does not return a value.
/// </summary>
public interface IRequest : IRequest<Unit> { }

/// <summary>
/// Represents a void type for requests that do not return a value.
/// </summary>
public record Unit;