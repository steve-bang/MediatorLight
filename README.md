# MediatorLight

[![NuGet Version](https://img.shields.io/nuget/v/MediatorLight.svg?style=flat-square)](https://www.nuget.org/packages/MediatorLight)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

A lightweight mediator pattern implementation for .NET, inspired by MediatR but with a simpler implementation and fewer dependencies.

## Features

- Simple request/response mediation
- Microsoft DI integration
- Async support
- Cancellation token support
- Lightweight and fast

## Installation
```bash
dotnet add package MediatorLight
```

For DI integration:
```bash
dotnet add package MediatorLight.DependencyInjection
```

## Quick Start

```CSharp
// Define a request
public record GetUserQuery(string UserId) : IRequest<User>;

// Create a handler
public class GetUserQueryHandler : IRequestHandler<GetUserQuery, User>
{
    public Task<User> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new User(request.UserId, "John Doe"));
    }
}

// Register services
services.AddMediatorLight(options => 
{
    options.AddHandler<GetUserQueryHandler>();
});

// Use the mediator
var user = await mediator.Send(new GetUserQuery("123"));
```

## Documentation
- [Getting Started](/docs/getting-started.md)
- [Advanced Usage](/docs/advanced-usage.md)

## Contributing

We welcome contributions! Please see our [ Contribution Guidelines](docs/CONTRIBUTING.md)