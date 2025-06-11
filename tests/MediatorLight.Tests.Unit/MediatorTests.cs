
using MediatorLight.Abstractions;
using Moq;

namespace MediatorLight.Tests
{
    public class MediatorTests
    {
        private readonly Mock<IServiceProvider> _serviceProviderMock = new();
        private readonly IMediator _mediator;

        public MediatorTests()
        {
            _mediator = new Mediator(_serviceProviderMock.Object);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenServiceProviderIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Mediator(null));
        }

        [Fact]
        public async Task Send_ShouldThrowArgumentNullException_WhenRequestIsNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _mediator.Send<TestResponse>(null));
        }

        [Fact]
        public async Task Send_ShouldReturnResponse_WhenHandlerExists()
        {
            // Arrange
            var request = new TestRequest();
            var expectedResponse = new TestResponse();
            var handlerMock = new Mock<IRequestHandler<TestRequest, TestResponse>>();
            handlerMock.Setup(x => x.Handle(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            _serviceProviderMock.Setup(x => x.GetService(typeof(IRequestHandler<TestRequest, TestResponse>)))
                .Returns(handlerMock.Object);

            // Act
            var response = await _mediator.Send(request);

            // Assert
            Assert.Equal(expectedResponse, response);
            handlerMock.Verify(x => x.Handle(request, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Send_ShouldThrowInvalidOperationException_WhenHandlerNotRegistered()
        {
            // Arrange
            var request = new TestRequest();
            _serviceProviderMock.Setup(x => x.GetService(typeof(IRequestHandler<TestRequest, TestResponse>)))
                .Returns(null);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _mediator.Send(request));
        }


        [Fact]
        public async Task Send_ShouldPassCancellationToken_ToHandler()
        {
            // Arrange
            var request = new TestRequest();
            var cancellationToken = new CancellationToken(true);
            var handlerMock = new Mock<IRequestHandler<TestRequest, TestResponse>>();
            handlerMock.Setup(x => x.Handle(request, cancellationToken))
                .ReturnsAsync(new TestResponse());

            _serviceProviderMock.Setup(x => x.GetService(typeof(IRequestHandler<TestRequest, TestResponse>)))
                .Returns(handlerMock.Object);

            // Act
            await _mediator.Send(request, cancellationToken);

            // Assert
            handlerMock.Verify(x => x.Handle(request, cancellationToken), Times.Once);
        }

        public record TestRequest : IRequest<TestResponse>;
        public record TestResponse;
        public record TestVoidRequest : IRequest<MediatorLight.Abstractions.Unit>;
    }
}