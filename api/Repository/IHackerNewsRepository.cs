using api.Models;

namespace api.Repository
{
    public interface IHackerNewsRepository
    {
        Task<List<int>> NewestStoryIdsAsync(string? searchValue);
        Task<HackerNewsModel> GetStoryByIdAsyc(int storyId);
    }
}
