namespace Application.Interface;

public interface ICacheManager
{
    Task<IEnumerable<T>> GetCollectionAsync<T>(int chunkSize = 10000) where T : notnull;
    Task<IEnumerable<T>> SetCollectionAsync<T>(IEnumerable<T> value, TimeSpan cacheTime, int chunkSize = 10000) 
        where T : notnull;
}
