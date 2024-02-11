using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using hackernewsapi.Model;
using hackernewsapi.Services;
using hackernewsapi.Services.Api;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace hackernewsapi.unit.tests.Services
{
    [TestClass]
    public class HackerNewsServiceTest
    {
        private HackerNewsService _hackerNewsService;
        private Mock<IHackerNewsApi> _mockHackerNewsApi;
        private Mock<IMapper> _mockMapper;

        public HackerNewsServiceTest(){
            _mockHackerNewsApi = new Mock<IHackerNewsApi>();
            _mockMapper = new Mock<IMapper>();
            _hackerNewsService = new HackerNewsService(_mockHackerNewsApi.Object, _mockMapper.Object);
        }

        [TestMethod]
        public async Task  GetBestOrderedStories_DisablingCache_ReturnsTwoOrderedStories()
        {
            //Arrange
            var firstOutputStory = new OutputStory() { score = 100 };
            var secondOutputStory = new OutputStory() { score = 50 };
            var outputs = new List<OutputStory> { firstOutputStory, secondOutputStory }; 

            _mockHackerNewsApi.Setup(s => s.GetBestStories()).ReturnsAsync(new int[]{1, 2});
            _mockHackerNewsApi.Setup(s => s.GetStoryById(It.IsAny<int>())).ReturnsAsync(new Story());
            _mockMapper.Setup(s => s
                                .Map<List<OutputStory>>(It.IsAny<List<Story>>()))
                                .Returns(outputs);

            //Act
            var stories = await _hackerNewsService.GetBestOrderedStories(false);

            //Asset
            Assert.IsNotNull(stories);
            Assert.AreEqual(2, stories.Count());
            Assert.AreEqual(100, stories.First().score);
            Assert.AreEqual(50, stories.Last().score);
        }

        [TestMethod]
        public async Task  GetBestOrderedStories_EnablingCache_ReturnsTwoOrderedStories()
        {
            //Arrange
            var firstOutputStory = new OutputStory() { score = 100 };
            var secondOutputStory = new OutputStory() { score = 50 };
            var outputs = new List<OutputStory> { firstOutputStory, secondOutputStory }; 

            _mockHackerNewsApi.Setup(s => s.GetBestStories()).ReturnsAsync(new int[]{1, 2});
            _mockHackerNewsApi.Setup(s => s.GetStoryById(It.IsAny<int>())).ReturnsAsync(new Story());
            _mockMapper.Setup(s => s
                                .Map<List<OutputStory>>(It.IsAny<List<Story>>()))
                                .Returns(outputs);

            //Act
            var stories = await _hackerNewsService.GetBestOrderedStories(true);

            //Asset
            Assert.IsNotNull(stories);
            Assert.AreEqual(2, stories.Count());
            Assert.AreEqual(100, stories.First().score);
            Assert.AreEqual(50, stories.Last().score);
        }

        [TestMethod]
        public void  CleanCache_Default_ClearServiceCache()
        {
            //Act
            _hackerNewsService.CleanCache();

            //Asset
            Assert.IsNotNull(_hackerNewsService);
        }
    }
}
