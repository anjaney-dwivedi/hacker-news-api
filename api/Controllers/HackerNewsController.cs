using api.Models;
using api.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HackerNewsController : ControllerBase
    {
        private readonly IHackerNewsRepository _hackersRepository;
        private readonly IMemoryCache _memoryCache;

        public HackerNewsController(IHackerNewsRepository hackersRepository,
            IMemoryCache memoryCache)
        {
            //Injecting dependencies
            _hackersRepository = hackersRepository;
            _memoryCache = memoryCache;
        }

        /* 
          <summary>
            Get newest stories 
            GET: /api/HackerNews/GetNewestStories
          </summary>
          <param name="searchValue"></param>
          <returns>List of HackerNews</returns>
         */
        [HttpGet]
        public async Task<IActionResult> GetNewestStoriesAsync(string? searchValue)
        {
            try
            {
                List<HackerNewsModel> stories = new List<HackerNewsModel>();

                var newStoryIds = await _hackersRepository.NewestStoryIdsAsync(searchValue);

                var topStoryIdsTasks = newStoryIds.Take(200)?.Select(GetStoryByIdAsync);
                if (topStoryIdsTasks != null)
                {
                    stories = (await Task.WhenAll(topStoryIdsTasks)).ToList();
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        searchValue = searchValue.ToLower();
                        stories = stories.Where(x => x.Title?.ToLower().Contains(searchValue) == true).ToList();
                    }
                }
                return Ok(stories);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /*
          <summary>
          Return news details by ID
          </summary>
          <param name="storyId"></param>
          <returns>Return HackersNews</returns>
        */
        [HttpGet]
        public async Task<HackerNewsModel> GetStoryByIdAsync(int storyId)
        {
            try
            {
                return await _memoryCache.GetOrCreateAsync(storyId,
                 async cacheEntry =>
                 {
                     return await _hackersRepository.GetStoryByIdAsyc(storyId);
                 });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
