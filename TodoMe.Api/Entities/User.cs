using System.Text.Json.Serialization;

namespace TodoMe.Api.Entities;

internal sealed class User
{
    // We use type string instead of UserId for easier dapper serialization
    [JsonPropertyName("id")] public required string Id { get; set; }

    [JsonPropertyName("name")] public required string Name { get; set; }
}

internal struct UserId
{
    public UserId(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static implicit operator string(UserId userId) => userId.Value;
    public static implicit operator UserId(string value) => new(value);
}
