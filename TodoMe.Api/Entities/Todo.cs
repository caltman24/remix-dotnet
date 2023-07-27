using System.Text.Json.Serialization;

namespace TodoMe.Api.Entities;

internal sealed class Todo
{
    [JsonPropertyName("id")] public int Id { get; set; }

    [JsonPropertyName("title")] public required string Title { get; set; } = null!;

    [JsonPropertyName("isComplete")] public bool IsComplete { get; set; }
}