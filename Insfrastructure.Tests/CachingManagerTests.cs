using System.Linq.Expressions;
using Infrastructure.Caching;
using Microsoft.Extensions.Logging;


namespace Insfrastructure.Tests;
public class CachingManagerTests
{
    private readonly Mock<ILogger<CachingManager>> _mockLogger;
    private readonly Mock<IRedisConnectionProvider> _mockRedisConnectionProvider;
    private readonly Mock<IRedisConnection> _mockRedisconnecton = new Mock<IRedisConnection>();

    private readonly Mock<IRedisCollection<User>> _mockRedisCollection;
    public CachingManagerTests()
    {
        _mockLogger = new Mock<ILogger<CachingManager>>();
        _mockRedisConnectionProvider = new Mock<IRedisConnectionProvider>();
        _mockRedisconnecton = new Mock<IRedisConnection>();
        _mockRedisCollection = new Mock<IRedisCollection<User>>();
    }

    [Fact]
    public void GetCollectionAsync_Execute_ReturnData()
    {
        // Assign

        _mockRedisCollection.Setup(s => s.GetEnumerator()).Returns(() => UserFactory.CreateListUsers.GetEnumerator());
        _mockRedisCollection.Setup(s => s.Count(It.IsAny<Expression<Func<User, bool>>>())).Returns(500);
        _mockRedisCollection.Setup(s => s.Any()).Returns(true);
        _mockRedisCollection.SetupGet(c => c.ChunkSize).Returns(10000);

        _mockRedisConnectionProvider.Setup(x => x.RedisCollection<User>(10000)).Returns(_mockRedisCollection.Object);

        // Act
        CachingManager cachingManager = new CachingManager(_mockRedisConnectionProvider.Object, _mockLogger.Object);

        // Assert
        List<User> users = cachingManager.GetCollectionAsync<User>(10000).Result.ToList();
        Assert.Equal(6, users.Count());
    }

    public void GetCollectionAsync_CollectionNull_ReturnEmptyResponse()
    {

        // Assign

        _mockRedisConnectionProvider.Setup(x => x.RedisCollection<User>(10000)).Returns(_mockRedisCollection.Object);

        // Act
        CachingManager cachingManager = new CachingManager(_mockRedisConnectionProvider.Object, _mockLogger.Object);

        // Assert
        List<User> users = cachingManager.GetCollectionAsync<User>(10000).Result.ToList();
        Assert.Empty(users);
    }
    [Fact]
    public void GetCollectionAsync_ThrowException_LogErrorReturnEmptyResponse()
    {


        // Act
        CachingManager cachingManager = new CachingManager(_mockRedisConnectionProvider.Object, _mockLogger.Object);

        // Assert
        List<User> users = cachingManager.GetCollectionAsync<User>(10000).Result.ToList();

        _mockLogger.VerifyLog(LogLevel.Error, Times.Once);
        Assert.Empty(users);
    }
    [Fact]
    public async Task SetCollectionAsync_UserIsNull_CallInsertAsyncToCacheData()
    {

        // Assign
        _mockRedisCollection.Setup(s => s.GetEnumerator()).Returns(() => (new List<User> ()).GetEnumerator());
        _mockRedisCollection.Setup(s => s.Count(It.IsAny<Expression<Func<User, bool>>>())).Returns(500);
        _mockRedisCollection.Setup(s => s.Any()).Returns(false);
        _mockRedisCollection.SetupGet(c => c.ChunkSize).Returns(10000);

        _mockRedisConnectionProvider.Setup(x => x.RedisCollection<User>(10000)).Returns(_mockRedisCollection.Object);
       // _mockRedisconnecton.Setup(x => x.DropIndexAndAssociatedRecords(It.IsAny<Type>())).Returns(true);
       // _mockRedisconnecton.Setup(x => x.CreateIndexAsync(It.IsAny<Type>())).ReturnsAsync(true);
        _mockRedisConnectionProvider.Setup(x => x.Connection).Returns(_mockRedisconnecton.Object);

        // Act

        CachingManager cachingManager = new CachingManager(_mockRedisConnectionProvider.Object, _mockLogger.Object);

        // Assert
        List<User> users = (await cachingManager.SetCollectionAsync(new List<User>(), new TimeSpan(0, 5, 0), 10000)).ToList();

        _mockRedisCollection.Verify(x => x.InsertAsync(It.IsAny<IEnumerable<User>>(), It.IsAny<TimeSpan>()), Times.Once);
       
    
    }
}
