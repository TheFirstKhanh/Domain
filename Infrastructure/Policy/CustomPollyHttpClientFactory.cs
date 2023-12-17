using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flurl.Http.Configuration;

namespace Infrastructure.Policy;
public class CustomPollyHttpClientFactory : DefaultHttpClientFactory
{
    public override HttpMessageHandler CreateMessageHandler()
    {
        return new PolicyHandler
        {
            InnerHandler = base.CreateMessageHandler()
        };
    }
}
