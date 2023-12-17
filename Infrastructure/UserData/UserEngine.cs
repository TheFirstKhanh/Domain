using Application.Interface;
using Domain;


namespace Infrastructure.UserData;

public class UserEngine : IUserEngine
{
    private readonly IUserClient<User> _userClient;

    public UserEngine(IUserClient<User> userClient) {
        _userClient = userClient;
    }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        return await _userClient.GetDataAsync();
    }
}
