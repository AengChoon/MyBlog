namespace Data.Models;

public class Comment
{
    public string? Id { get; set; }
    public required string PostId { get; set; }
    public string Text { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}