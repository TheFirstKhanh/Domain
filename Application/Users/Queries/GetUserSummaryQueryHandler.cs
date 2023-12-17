using Application.Interface;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Users.Queries;

public class GetUserSummaryQueryHandler : IRequestHandler<GetUserSummaryQuery, GetUserSummaryQueryResponse>
{
    private readonly ILogger<GetUserSummaryQueryHandler> _logger;
    private readonly IUserEngine _userEngine;
    private readonly ICacheManager _cacheManager;

    public GetUserSummaryQueryHandler(
        ILogger<GetUserSummaryQueryHandler> logger,
        IUserEngine userEngine,
        ICacheManager cacheManager)
    {
        _logger = logger;
        _userEngine = userEngine;
        _cacheManager = cacheManager;
    }
    public async Task<GetUserSummaryQueryResponse> Handle(GetUserSummaryQuery request, CancellationToken cancellationToken)
    {
        GetUserSummaryQueryResponse? getUserSummaryQueryResponse = null;
        try
        {
            IEnumerable<User> users = await _cacheManager.GetCollectionAsync<User>();

            if (!users.Any())
            {
                IEnumerable<User> usersDb = await _userEngine.GetUsersAsync();
                if (usersDb.Any())
                {
                    users = await _cacheManager.SetCollectionAsync(usersDb, new TimeSpan(0, 30, 0));
                }
                else 
                {
                    return new GetUserSummaryQueryResponse();
                }
            }

            getUserSummaryQueryResponse = _setGetUserSummaryQueryResponse(users.ToHashSet());
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex.Message);

            return new GetUserSummaryQueryResponse();
        }

        return getUserSummaryQueryResponse;
    }

    private GetUserSummaryQueryResponse _setGetUserSummaryQueryResponse(HashSet<User> users)
    {
        return new GetUserSummaryQueryResponse()
        {
            UserFullName = users
            .Where(user => user.Id == 42 && !string.IsNullOrEmpty(user.First) && !string.IsNullOrEmpty(user.Last))
            .Select(x => string.Format("{0} {1}", x.First, x.Last))
            .ToList(),

            FirstName = string.Join(",", users.
                                                Where(user => !string.IsNullOrEmpty(user.First) && user.Age == 23)
                                                .Select(x => x.First).ToList()),

            GenderPerAges = users.GroupBy(x => new { x.Age }).Select(x => new GenderPerAge
            {
                Age = x.Key.Age ?? 0,
                Male = x.Where(g => g.Age == x.Key.Age && g.Gender == "M").Count(),
                Female = x.Where(g => g.Age == x.Key.Age && g.Gender == "Y").Count()
            }).OrderBy(user => user.Age).ToList()

        };
    }
}
