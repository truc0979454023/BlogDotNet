namespace Blog.Core.SeedWords
{
    public interface IUnitOfWork
    {
        Task<int> CompleteAsync();
    }
}