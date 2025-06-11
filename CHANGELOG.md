# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- Initial project structure and core functionality
- Basic request/response mediation
- Microsoft DI integration
- Comprehensive test suite
- GitHub Actions CI pipeline

## [1.0.0] - 2025-06-11

### Added
- Initial release of MediatorLight
- Core interfaces: `IRequest<TResponse>`, `IRequest`, `IRequestHandler<,>`, `IMediatorLight`
- Implementation of `Mediator` class
- Dependency Injection extensions with `AddMediatorLight()`
- Unit and integration test coverage
- Documentation and samples

### Features
- Support for request/response pattern
- Support for void requests
- Async/await support
- CancellationToken propagation
- Handler type caching for performance
- Clean, simple API surface