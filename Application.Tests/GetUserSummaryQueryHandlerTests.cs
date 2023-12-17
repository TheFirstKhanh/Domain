using Application.Interface;
using Application.Users.Queries;
using Domain;
using Microsoft.Extensions.Logging;

namespace Application.Tests;

public class GetUserSummaryQueryHandlerTests
{
    private readonly Mock<ILogger<GetUserSummaryQueryHandler>> _mockLogger;
    private readonly Mock<ICacheManager> _mockCacheManager;
    private readonly Mock<IUserEngine> _mockUserEngine;

    public GetUserSummaryQueryHandlerTests()
    {
        _mockLogger = new Mock<ILogger<GetUserSummaryQueryHandler>>();
        _mockCacheManager = new Mock<ICacheManager>();
        _mockUserEngine = new Mock<IUserEngine>();
    }

    [Fact]
    public async Task GetUserSummaryQueryHandler_UserFromCache_ReturnValidResponse()
    {
        // Assign

        _mockCacheManager.Setup(x => x.GetCollectionAsync<User>(It.IsAny<int>())).ReturnsAsync(UserFactory.CreateListUsers);

        _mockUserEngine.Setup(x => x.GetUsersAsync()).ReturnsAsync(UserFactory.CreateListUsers);

        // Act

        GetUserSummaryQueryHandler getUserSummaryQueryHandler = new GetUserSummaryQueryHandler(
            _mockLogger.Object,
            _mockUserEngine.Object,
            _mockCacheManager.Object);

        GetUserSummaryQueryResponse response = await getUserSummaryQueryHandler.Handle(new GetUserSummaryQuery(),
            CancellationToken.None);

        // Assert

        Assert.Equal("Bill,Frank", response.FirstName);
        Assert.Empty(response.UserFullName);
        Assert.True(QueryResponseFactory.CreateListGenderPerAge.SequenceEqual(response.GenderPerAges));
    }

    [Fact]
    public async Task GetUserSummaryQueryHandler_CacheNewData_ReturnValidResponse()
    {
        // Assign

        _mockCacheManager.Setup(x => x.GetCollectionAsync<User>(It.IsAny<int>())).ReturnsAsync(new List<User>());
        _mockUserEngine.Setup(x => x.GetUsersAsync()).ReturnsAsync(UserFactory.CreateListUsers);
        _mockCacheManager.Setup(x => x.SetCollectionAsync<User>(It.IsAny<IEnumerable<User>>(), It.IsAny<TimeSpan>(),
            It.IsAny<int>()))
            .ReturnsAsync(UserFactory.CreateListUsers);

        // Act

        GetUserSummaryQueryHandler getUserSummaryQueryHandler = new GetUserSummaryQueryHandler(
            _mockLogger.Object,
            _mockUserEngine.Object,
            _mockCacheManager.Object);

        GetUserSummaryQueryResponse response = await getUserSummaryQueryHandler.Handle(new GetUserSummaryQuery(),
            CancellationToken.None);

        // Assert

        Assert.Equal("Bill,Frank", response.FirstName);
        Assert.Empty(response.UserFullName);
        Assert.True(QueryResponseFactory.CreateListGenderPerAge.SequenceEqual(response.GenderPerAges));
    }

    [Fact]
    public async Task GetUserSummaryQueryHandler_UserIsNull_ReturnEmptyResponse()
    {
        // Assign

        _mockCacheManager.Setup(x => x.GetCollectionAsync<User>(It.IsAny<int>())).ReturnsAsync(new List<User>());
        _mockUserEngine.Setup(x => x.GetUsersAsync()).ReturnsAsync(new List<User>());
        _mockCacheManager.Setup(x => x.SetCollectionAsync<User>(It.IsAny<IEnumerable<User>>(), It.IsAny<TimeSpan>(),
            It.IsAny<int>()))
            .ReturnsAsync(new List<User>());

        // Act

        GetUserSummaryQueryHandler getUserSummaryQueryHandler = new GetUserSummaryQueryHandler(
            _mockLogger.Object,
            _mockUserEngine.Object,
            _mockCacheManager.Object);

        GetUserSummaryQueryResponse response = await getUserSummaryQueryHandler.Handle(new GetUserSummaryQuery(),
            CancellationToken.None);

        // Assert

        Assert.Equal(string.Empty, response.FirstName);
        Assert.Empty(response.UserFullName);
        Assert.Empty(response.GenderPerAges);
    }

    [Fact]
    public async Task GetUserSummaryQueryHandler_ThrowException_ReturnValidResponse()
    {
        // Assign

        _mockCacheManager.Setup(x => x.GetCollectionAsync<User>(It.IsAny<int>())).ReturnsAsync(new List<User>());
        _mockUserEngine.Setup(x => x.GetUsersAsync()).ReturnsAsync(UserFactory.CreateListUsers);
        _mockCacheManager.Setup(x => x.SetCollectionAsync<User>(It.IsAny<IEnumerable<User>>(), It.IsAny<TimeSpan>(),
            It.IsAny<int>()))
            .Throws(new SystemException());

        // Act

        GetUserSummaryQueryHandler getUserSummaryQueryHandler = new GetUserSummaryQueryHandler(
            _mockLogger.Object,
            _mockUserEngine.Object,
            _mockCacheManager.Object);

        GetUserSummaryQueryResponse response = await getUserSummaryQueryHandler.Handle(new GetUserSummaryQuery(),
            CancellationToken.None);

        // Assert

        _mockLogger.VerifyLog(LogLevel.Error, "System error.", Times.Once);
    }
}