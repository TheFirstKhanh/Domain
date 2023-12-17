
using Newtonsoft.Json;

namespace Domain.Tests;
public class UserTests
{
    [Fact]
    public void User_DeserializeJSONData_ReturnUserModel() 
    {
        // Assign
        string JsonData = QueryResponseFactory.UserJsonData;

        // Act

        User user = JsonConvert.DeserializeObject<User>(value: JsonData);

        // Assert
        Assert.Equal("Bill", user.First);
        Assert.Equal("Bryson", user.Last);
        Assert.Equal(53, user.Id);
        Assert.Equal(23, user.Age);
        Assert.Equal("M", user.Gender);
    }
}
