using Application;
using Common.ConfigOptions;
using Infrastructure;
using Redis.OM;
using Redis.OM.Contracts;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddApplicationServices();
builder.Services.AddInsfrastructure();


builder.Services.AddOptions<EndPointConfig>()
            .Bind(builder.Configuration.GetSection(EndPointConfig.EndPointSection))
            .ValidateDataAnnotations();

builder.Services.AddOptions<CachingConfig>()
            .Bind(builder.Configuration.GetSection(CachingConfig.CachingSection))
            .ValidateDataAnnotations();

builder.Services.AddSingleton<IRedisConnectionProvider>
    (new RedisConnectionProvider(builder.Configuration.GetSection("Caching").GetValue<string>(key: "RedisConnectionEndpoint")));



builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

app.Run();
