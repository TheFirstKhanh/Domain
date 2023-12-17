using Newtonsoft.Json;
using Redis.OM.Modeling;

namespace Domain;

[Document(StorageType = StorageType.Json, Prefixes = new[] { "User" }, IndexName = "users")]
public class User
{
   
    [Indexed]
    [JsonProperty("id")]
    public int Id { get; set; }

    [Indexed]
    [JsonProperty("first")]
    public string? First { get; set; }

    [Indexed]
    [JsonProperty("last")]
    public string? Last { get; set; }

    [Indexed(Sortable = true)]
    [JsonProperty("age")]
    public int? Age { get; set; }

    [Indexed(Sortable = true)]
    [JsonProperty("gender")]
    public string? Gender { get; set; }
}


