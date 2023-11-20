using Moq;
using Xunit;

namespace TicketBookingCore.Tests
{
    public class TicketBookingRequestProcessorTests
    {
        private readonly TicketBookingRequest _request;
        private readonly Mock<ITicketBookingRepository> _ticketBookingRepositoryMock;
        public readonly TicketBookingRequestProcessor _processor;
        public TicketBookingRequestProcessorTests()
        {
            _request = new TicketBookingRequest
            {
                FirstName = "Gilbert",
                LastName = "Kibet",
                Email = "kibetgilly354@gmail.com"
            };
            _ticketBookingRepositoryMock = new Mock<ITicketBookingRepository>();

            _processor = new TicketBookingRequestProcessor(_ticketBookingRepositoryMock.Object);
        }
        [Fact]
        public void TicketBookingRequestProcessor_ShouldReturnTicketBookingResultWithRequestValues()
        {
            //Act

            TicketBookingResponse response = _processor.Book(_request);
            //Assert
            Assert.NotNull(response);
            Assert.Equal(_request.FirstName, response.FirstName);
            Assert.Equal(_request.LastName, response.LastName);
            Assert.Equal(_request.Email, response.Email);


        }

        [Fact]

        public void TicketBookingRequestProcessor_ShouldThrowExceptionIfRequestIsNull()
        {
            //Arrange
            //Act
            var exception = Assert.Throws<ArgumentNullException>(() => _processor.Book(null));
            //Assert
            Assert.Equal("request", exception.ParamName);


        }

        [Fact]

        public void TicketBookingRequestProcessor_ShouldSaveToDatabase()
        {
            //Arrange
            TicketBooking savedTicketBooking = null;
            _ticketBookingRepositoryMock.Setup(x => x.Save(It.IsAny<TicketBooking>()))
     .Callback<TicketBooking>((ticketBooking) =>
     {
         savedTicketBooking = ticketBooking;
     });
            
            // Act  
            TicketBookingResponse response = _processor.Book(_request);

            // Assert  
            _ticketBookingRepositoryMock.Verify(x => x.Save(It.IsAny<TicketBooking>()), Times.Once);

            Assert.NotNull(savedTicketBooking);
            Assert.Equal(_request.FirstName, savedTicketBooking.FirstName);
            Assert.Equal(_request.LastName, savedTicketBooking.LastName);
            Assert.Equal(_request.Email, savedTicketBooking.Email);

        }
    }
}
