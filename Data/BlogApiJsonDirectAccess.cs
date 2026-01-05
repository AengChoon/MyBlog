using System.Text.Json;
using Data.Models;
using Data.Models.Interfaces;
using Microsoft.Extensions.Options;

namespace Data;

public class BlogApiJsonDirectAccess : IBlogApi
{
    private readonly BlogApiJsonDirectAccessSettings _settings;

    public BlogApiJsonDirectAccess(IOptions<BlogApiJsonDirectAccessSettings> options)
    {
        _settings = options.Value;
        
        ManageDataPaths();
    }
    
    public async Task<List<Post>> GetPostsAsync(int skip, int take)
    {
        var list = await LoadAsync<Post>(_settings.PostsDirectory);
        
        return list.Skip(skip).Take(take).ToList();
    }

    public async Task<List<Category>> GetCategoriesAsync()
    {
        return await LoadAsync<Category>(_settings.CategoriesDirectory);
    }

    public async Task<List<Tag>> GetTagsAsync()
    {
        return await LoadAsync<Tag>(_settings.TagsDirectory);
    }

    public async Task<List<Comment>> GetCommentsAsync(string postId)
    {
        var list = await LoadAsync<Comment>(_settings.CommentsDirectory);
        
        return list.Where(comment => comment.PostId == postId).ToList();
    }

    public async Task<Post?> FindPostAsync(string id)
    {
        var list = await LoadAsync<Post>(_settings.PostsDirectory);
        
        return list.FirstOrDefault(blogPost => blogPost.Id == id);
    }

    public async Task<Category?> FindCategoryAsync(string id)
    {
        var list = await LoadAsync<Category>(_settings.CategoriesDirectory);
        
        return list.FirstOrDefault(category => category.Id == id);
    }

    public async Task<Tag?> FindTagAsync(string id)
    {
        var list = await LoadAsync<Tag>(_settings.TagsDirectory);
        
        return list.FirstOrDefault(tag => tag.Id == id);
    }
    
    public async Task<int> GetBlogPostsCountAsync()
    {
        var list = await LoadAsync<Post>(_settings.PostsDirectory);
        
        return list.Count;
    }

    public async Task<Post?> SavePostAsync(Post item)
    {
        item.Id ??= Guid.NewGuid().ToString();
        await SaveAsync(_settings.PostsDirectory, item.Id, item);
        
        return item;
    }

    public async Task<Category?> SaveCategoryAsync(Category item)
    {
        item.Id ??= Guid.NewGuid().ToString();
        await SaveAsync(_settings.CategoriesDirectory, item.Id, item);
        
        return item;
    }

    public async Task<Tag?> SaveTagAsync(Tag item)
    {
        item.Id ??= Guid.NewGuid().ToString();
        await SaveAsync(_settings.TagsDirectory, item.Id, item);
        
        return item;
    }

    public async Task<Comment?> SaveCommentAsync(Comment item)
    {
        item.Id ??= Guid.NewGuid().ToString();
        await SaveAsync(_settings.CommentsDirectory, item.Id, item);
        
        return item;
    }

    public async Task DeletePostAsync(string id)
    {
        await DeleteAsync(_settings.PostsDirectory, id);
        
        var comments = await GetCommentsAsync(id);
        foreach (var comment in comments)
        {
            if (comment.Id != null)
                await DeleteCommentAsync(comment.Id);
        }
    }

    public async Task DeleteCategoryAsync(string id)
    {
        await DeleteAsync(_settings.CategoriesDirectory, id);
    }

    public async Task DeleteTagAsync(string id)
    {
        await DeleteAsync(_settings.TagsDirectory, id);
    }

    public async Task DeleteCommentAsync(string id)
    {
        await DeleteAsync(_settings.CommentsDirectory, id);
    }

    private void ManageDataPaths()
    {
        CreateDirectoryIfNotExists(_settings.DataPath);
        CreateDirectoryIfNotExists(Path.Combine(_settings.DataPath, _settings.PostsDirectory));
        CreateDirectoryIfNotExists(Path.Combine(_settings.DataPath, _settings.CategoriesDirectory));
        CreateDirectoryIfNotExists(Path.Combine(_settings.DataPath, _settings.TagsDirectory));
        CreateDirectoryIfNotExists(Path.Combine(_settings.DataPath, _settings.CommentsDirectory));
    }
    
    private static void CreateDirectoryIfNotExists(string path)
    {
        if (Directory.Exists(path) == false)
            Directory.CreateDirectory(path);
    }

    private async Task<List<T>> LoadAsync<T>(string directory)
    {
        var list = new List<T>();
        foreach (var file in Directory.GetFiles(Path.Combine(_settings.DataPath, directory)))
        {
            var text = await File.ReadAllTextAsync(file);
            
            var json = JsonSerializer.Deserialize<T>(text);
            if (json != null)
                list.Add(json);
        }

        return list;
    }
    
    private async Task SaveAsync<T>(string directory, string fileName, T item)
    {
        var filePath = Path.Combine(_settings.DataPath, directory, fileName + ".json");
        await File.WriteAllTextAsync(filePath, JsonSerializer.Serialize(item));
    }
    
    private async Task DeleteAsync(string directory, string fileName)
    {
        var filePath = Path.Combine(_settings.DataPath, directory, fileName + ".json");
        if (File.Exists(filePath))
            File.Delete(filePath);
        
        await Task.CompletedTask;
    }
}