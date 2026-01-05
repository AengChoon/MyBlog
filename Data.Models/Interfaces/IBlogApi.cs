namespace Data.Models.Interfaces;

public interface IBlogApi
{
   Task<List<Post>> GetPostsAsync(int skip, int take);
   Task<List<Category>> GetCategoriesAsync();
   Task<List<Tag>> GetTagsAsync();
   Task<List<Comment>> GetCommentsAsync(string postId);
   
   Task<Post?> FindPostAsync(string id);
   Task<Category?> FindCategoryAsync(string id);
   Task<Tag?> FindTagAsync(string id);
   
   Task<int> GetBlogPostsCountAsync();
   
   Task<Post?> SavePostAsync(Post item);
   Task<Category?> SaveCategoryAsync(Category item);
   Task<Tag?> SaveTagAsync(Tag item);
   Task<Comment?> SaveCommentAsync(Comment item);
   
   Task DeletePostAsync(string id);
   Task DeleteCategoryAsync(string id);
   Task DeleteTagAsync(string id);
   Task DeleteCommentAsync(string id);
}
