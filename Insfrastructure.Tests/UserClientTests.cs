using System.Net;
using Common.ConfigOptions;
using Flurl.Http;
using Infrastructure.UserData;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;

namespace Insfrastructure.Tests;

public class UserClientTests
{
    private readonly Mock<IFlurlClientFactory> _flurlFactory;

    private readonly Mock<IOptions<EndPointConfig>> _mockOption;
    private readonly Mock<ILogger<UserClient<User>>> _mockLogger;
    public UserClientTests()
    {
      


        _flurlFactory = new Mock<IFlurlClientFactory>();
        _mockOption = new Mock<IOptions<EndPointConfig>>();
        _mockLogger = new Mock<ILogger<UserClient<User>>>();

        _mockOption.Setup(x => x.Value).Returns(new EndPointConfig() { UserApiEndPoint = "" });
    }


    [Fact]
    public async Task GetDataAsync_Execute_ReturnValidResponse()
    {
        // Assgin
        _voidSetupHttpClient(UserFactory.ListUserJsonData);

   
       
        // Act
        UserClient<User> userClient = new UserClient<User>(_flurlFactory.Object, _mockLogger.Object, _mockOption.Object);
        List<User> users = (await userClient.GetDataAsync()).ToList();

        // Assert
  
        var expectedString = JsonConvert.SerializeObject(users);
        var actualString = JsonConvert.SerializeObject(UserFactory.CreateListUsers);

        // Assert
        Assert.Equal(expectedString, actualString);

    }

    [Fact]
    public async Task GetDataAsync_BrokenJson_ReturnEmptyResponse()
    {
        // Assign
        _voidSetupHttpClient(UserFactory.ListUserBrokenJsonData);

        // Act
        UserClient<User> userClient = new UserClient<User>(_flurlFactory.Object, _mockLogger.Object, _mockOption.Object);
        IEnumerable<User> users = (await userClient.GetDataAsync());

        // Assert
        Assert.Empty(users);
    }

    [Fact]
    public async Task GetDataAsync_BrokenJson_LogError()
    {
        // Assign
        _voidSetupHttpClient(UserFactory.ListUserBrokenJsonData);

        // Act
        UserClient<User> userClient = new UserClient<User>(_flurlFactory.Object, _mockLogger.Object, _mockOption.Object);
        IEnumerable<User> users = (await userClient.GetDataAsync());

        // Assert
        _mockLogger.VerifyLog(LogLevel.Error,  Times.Once);
    }

    [Fact]
    public async Task GetDataAsync_InvalidSchema_ReturnEmptyResponse()
    {
        // Assign
        _voidSetupHttpClient(UserFactory.ListUserInvalidSchemaJsonData);

        // Act
        UserClient<User> userClient = new UserClient<User>(_flurlFactory.Object, _mockLogger.Object, _mockOption.Object);
        IEnumerable<User> users = (await userClient.GetDataAsync());

        // Assert
        Assert.Empty(users);
    }

    private void _voidSetupHttpClient(string responseBody)
    {
        var clientMock = new Mock<IFlurlClient>();
        var flurlRequest = new Mock<IFlurlRequest>();
        var flurlResponse = new Mock<IFlurlResponse>();


        flurlRequest.SetupGet(x => x.Settings).Returns(new FlurlHttpSettings() { AllowedHttpStatusRange = "*" });
        flurlResponse.Setup(x => x.ResponseMessage).Returns(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(responseBody) });
        flurlResponse.Setup(x => x.GetJsonAsync<IEnumerable<User>>()).ReturnsAsync(UserFactory.CreateListUsers);
        flurlRequest.Setup(x => x.SendAsync(It.IsAny<HttpMethod>(), It.IsAny<HttpContent>(), It.IsAny<CancellationToken>(), It.IsAny<HttpCompletionOption>()))
            .Returns(Task.FromResult(flurlResponse.Object));


        clientMock.Setup(x => x.Request()).Returns(flurlRequest.Object);


        _flurlFactory.Setup(x => x.Get(It.IsAny<Url>())).Returns(clientMock.Object);
    }
}