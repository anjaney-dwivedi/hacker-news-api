using api.Models;
using Newtonsoft.Json;
using api.Common;


namespace api.Repository
{
    public class HackerNewsRepository : IHackerNewsRepository
    {
        public async Task<List<int>> NewestStoryIdsAsync(string? searchValue)
        {
            List<int> stories = new List<int>();
            using (HttpClient httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"{Constants.HackersApiBaseUrl}/newstories.json");
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    if (!string.IsNullOrEmpty(result))
                    {
                        var resultList = JsonConvert.DeserializeObject<List<int>>(result);
                        if (resultList != null)
                        {
                            stories = resultList;
                        }
                    }
                }
            }
            return stories;
        }

        public async Task<HackerNewsModel> GetStoryByIdAsyc(int storyId)
        {
            HackerNewsModel story = new HackerNewsModel();
            using (HttpClient httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"{Constants.HackersApiBaseUrl}/item/{storyId}.json");
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    if (!string.IsNullOrEmpty(result))
                    {
                        var storyObj = JsonConvert.DeserializeObject<HackerNewsModel>(result);
                        if (storyObj != null)
                            story = storyObj;
                    }
                }
            }
            return story;
        }
    }
}
