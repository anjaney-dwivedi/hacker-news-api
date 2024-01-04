using api.Controllers;
using api.Models;
using api.Repository;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Microsoft.AspNetCore.Mvc;

namespace api.test.Controllers
{
    
        public class HackerNewsControllerTest
        {
            private HackerNewsController hackerNewsController;
            private Mock<IHackerNewsRepository> hackerNewsRepositoryMock;
            private IMemoryCache memoryCache;

            public List<int> storyIds = new List<int>
        {
            38852197, 38852181, 38852171, 38852163, 38852162, 38852142, 38852133
        };

            public HackerNewsModel story = new HackerNewsModel()
            {
                Title = "test_title",
                Url = "test_url"
            };

            [SetUp]
            public void Setup()
            {
                // arrange
                hackerNewsRepositoryMock = new Mock<IHackerNewsRepository>();
                memoryCache = new MemoryCache(new MemoryCacheOptions());
                hackerNewsController = new HackerNewsController(hackerNewsRepositoryMock.Object, memoryCache);
            }

            [Test]
            public void GetNewestStoriesAsync_VerifyAll_Test()
            {
                // Arrange
                hackerNewsRepositoryMock.Setup(p => p.NewestStoryIdsAsync("")).ReturnsAsync(storyIds);
                hackerNewsRepositoryMock.Setup(p => p.GetStoryByIdAsyc(0000)).ReturnsAsync(story);

                // Act
                var response = hackerNewsController.GetNewestStoriesAsync("").Result;
                var result = response as OkObjectResult;
                var resultData = new List<HackerNewsModel>();
                if (result != null && result.Value != null)
                {
                    resultData = (List<HackerNewsModel>)result.Value;
                }

                // Assert
                Assert.NotNull(response);
                Assert.That(200, Is.EqualTo(result?.StatusCode));
                Assert.That(7, Is.EqualTo(resultData?.Count));
            }

            [Test]
            public void GetStoryByIdAsync_VerifyAll_Test()
            {
                // Arrange
                hackerNewsRepositoryMock.Setup(p => p.GetStoryByIdAsyc(00001)).ReturnsAsync(story);

                // Act
                var response = hackerNewsController.GetStoryByIdAsync(00001).Result;

                // Assert
                Assert.NotNull(response);
                Assert.That(response, Is.EqualTo(story));
            }
        }
    
}
