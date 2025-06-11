
using MediatorKit.Abstractions;
using Moq;

namespace MediatorKit.Tests
{
    public class MediatorTests
    {
        private readonly Mock<IServiceProvider> _serviceProviderMock = new();
        private readonly IMediator _mediator;

        public MediatorTests()
        {
            _mediator = new Mediator(_serviceProviderMock.Object);
        }

        // [Fact]
        // public async Task Send_ShouldThrowArgumentNullException_WhenRequestIsNull()
        // {
        //     await Assert.ThrowsAsync<ArgumentNullException>(() => _mediator.Send<TestResponse>(null));
        // }

        // [Fact]
        // public async Task Send_ShouldReturnResponse_WhenHandlerExists()
        // {
        //     // Arrange
        //     var request = new TestRequest();
        //     var expectedResponse = new TestResponse();
        //     var handlerMock = new Mock<IRequestHandler<TestRequest, TestResponse>>();
        //     handlerMock.Setup(x => x.Handle(request, It.IsAny<CancellationToken>()))
        //         .ReturnsAsync(expectedResponse);

        //     _serviceProviderMock.Setup(x => x.GetService(typeof(IRequestHandler<TestRequest, TestResponse>)))
        //         .Returns(handlerMock.Object);

        //     // Act
        //     var response = await _mediator.Send(request);

        //     // Assert
        //     Assert.Equal(expectedResponse, response);
        //     handlerMock.Verify(x => x.Handle(request, It.IsAny<CancellationToken>()), Times.Once);
        // }

        public record TestRequest : IRequest<TestResponse>;
        public record TestResponse;
        public record TestVoidRequest : IRequest<MediatorKit.Abstractions.Unit>;
    }
}