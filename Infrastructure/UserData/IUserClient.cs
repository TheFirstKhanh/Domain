

using Domain;

namespace Infrastructure.UserData;

public interface IUserClient<T>
{
    Task<IEnumerable<T>> GetDataAsync();
}
