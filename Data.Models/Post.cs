namespace Data.Models;

public class Post
{
    public string? Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public Category? Category { get; set; }
    public List<Tag> Tags { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}