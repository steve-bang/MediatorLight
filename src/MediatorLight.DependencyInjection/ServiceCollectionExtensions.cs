// --------------------------------------------------------------------------------------
// ServiceCollectionExtensions.cs
//
// Copyright (c) 2025 Steve Bang
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
// Description:
//   Provides extension methods for registering MediatorLight services and handlers
//   with the Microsoft dependency injection container.
// --------------------------------------------------------------------------------------


using MediatorLight.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MediatorLight;

/// <summary>
/// Extension methods for registering MediatorLight services with the dependency injection container.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the Mediator and its dependencies in the service collection.
    /// </summary>
    /// <param name="services">The service collection to add the Mediator to.</param>
    /// <param name="lifetime">The service lifetime for the Mediator.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddMediatorLight(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
        services.TryAdd(new ServiceDescriptor(typeof(IMediator), typeof(Mediator), lifetime));
        return services;
    }

    /// <summary>
    /// Registers the Mediator and allows configuration of Mediator options.
    /// </summary>
    /// <param name="services">The service collection to add the Mediator to.</param>
    /// <param name="configure">An action to configure Mediator options.</param>
    /// <param name="lifetime">The service lifetime for the Mediator.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddMediatorLight(this IServiceCollection services, Action<MediatorOptions> configure, ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
        services.AddMediatorLight(lifetime);
        configure(new MediatorOptions(services, lifetime));
        return services;
    }
}

/// <summary>
/// Provides options for configuring Mediator handler registrations.
/// </summary>
public class MediatorOptions
{
    private readonly IServiceCollection _services;
    private readonly ServiceLifetime _lifetime;

    /// <summary>
    /// Initializes a new instance of the <see cref="MediatorOptions"/> class.
    /// </summary>
    /// <param name="services">The service collection to register handlers with.</param>
    /// <param name="lifetime">The service lifetime for handlers.</param>
    public MediatorOptions(IServiceCollection services, ServiceLifetime lifetime)
    {
        _services = services;
        _lifetime = lifetime;
    }

    /// <summary>
    /// Registers a handler type with the service collection for all implemented handler interfaces.
    /// </summary>
    /// <typeparam name="THandler">The handler type to register.</typeparam>
    /// <returns>The current <see cref="MediatorOptions"/> instance for chaining.</returns>
    public MediatorOptions AddHandler<THandler>() where THandler : class
    {
        var handlerType = typeof(THandler);
        var handlerInterfaces = handlerType.GetInterfaces()
            .Where(i => i.IsGenericType &&
                        (i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>) ||
                         i.GetGenericTypeDefinition() == typeof(IRequestHandler<>)));

        foreach (var handlerInterface in handlerInterfaces)
        {
            _services.TryAdd(new ServiceDescriptor(handlerInterface, handlerType, _lifetime));
        }

        return this;
    }
}