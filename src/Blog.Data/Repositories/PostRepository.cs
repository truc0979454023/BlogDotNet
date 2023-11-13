using Blog.Core;
using Blog.Core.Domain.Content;
using Blog.Core.Repositories;
using Blog.Data.SeedWords;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data.Repositories
{
    public class PostRepository : RepositotyBase<Post, Guid>, IPostRepository
    {
        public PostRepository(BlogContext context) : base(context)
        {
        }

        public Task<List<Post>> GetPopularPostsAsync(int count)
        {
            return _context.Posts.OrderByDescending(x => x.ViewCount).Take(count).ToListAsync();
        }
    }
}