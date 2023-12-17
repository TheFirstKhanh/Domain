using Microsoft.Extensions.Logging;
using Moq;
namespace SampleCode.Shared.TestUtilities;

public static class MockLoggerExtensions
{
    public static void VerifyLog<T>(this Mock<ILogger<T>> loggerMock, LogLevel loglevel, Func<Times> times)
    {
        loggerMock.Verify(logger => logger.Log(
   It.Is<LogLevel>(logLevel => logLevel == loglevel),
   It.Is<EventId>(eventId => eventId.Id == 0),
   It.IsAny<It.IsAnyType>(),
   It.IsAny<Exception>(),
   It.IsAny<Func<It.IsAnyType, Exception?, string>>()), times);
    }

    public static void VerifyLog<T>(this Mock<ILogger<T>> loggerMock, LogLevel loglevel, string message, Func<Times> times)
    {
        loggerMock.Verify(logger => logger.Log(
   It.Is<LogLevel>(logLevel => logLevel == loglevel),
   It.Is<EventId>(eventId => eventId.Id == 0),
   It.Is<It.IsAnyType>((@object, @type) => @object.ToString() == message),
   It.IsAny<Exception>(),
   It.IsAny<Func<It.IsAnyType, Exception?, string>>()), times);
    }
}

