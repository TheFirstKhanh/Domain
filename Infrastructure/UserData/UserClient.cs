using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Domain;
using System.Collections.Generic;
using Common.ConfigOptions;
using System;
using Newtonsoft.Json.Linq;
using System.Net.Http.Json;
using Newtonsoft.Json.Schema;
using Common.Helpers;
using Newtonsoft.Json.Schema.Generation;
using System.Security.Principal;
using Newtonsoft.Json;

namespace Infrastructure.UserData;

public class UserClient<T> : IUserClient<T> where T : class
{
    private readonly IFlurlClient _flurlClient;
    private readonly ILogger<UserClient<T>> _logger;
    private readonly IOptions<EndPointConfig> _endPointConfig;

    public UserClient(IFlurlClientFactory flurlClientFactory,
                        ILogger<UserClient<T>> logger,
                        IOptions<EndPointConfig> endPointConfig) 
    {
        _logger = logger;
        _endPointConfig = endPointConfig;
        _flurlClient = flurlClientFactory.Get(_endPointConfig.Value.UserApiEndPoint);
    }
    public async Task<IEnumerable<T>> GetDataAsync()
    {
        try
        {
            IFlurlResponse flurlResponse = await _flurlClient.Request().AllowAnyHttpStatus().GetAsync();

            string responseBody = await flurlResponse.ResponseMessage.Content.ReadAsStringAsync();
         
            if(string.IsNullOrEmpty(responseBody)) return Enumerable.Empty<T>();

            bool validationResult = JsonHelper.Validate<T[]>(responseBody);

            if (validationResult)
            {
                IEnumerable<T> users = await flurlResponse.GetJsonAsync<IEnumerable<T>>();

                return users;
            }
            else
            {

                _logger.LogError($"Invalid Json");

                return Enumerable.Empty<T>();
            }

        }
        catch (Exception ex) when (   ex is JSchemaReaderException 
                                   || ex is JsonReaderException
                                   || ex is FlurlHttpException
                                   )
        {

            _logger.LogError($"{ex.Message}");

            return Enumerable.Empty<T>();
 
        }

    }
}
