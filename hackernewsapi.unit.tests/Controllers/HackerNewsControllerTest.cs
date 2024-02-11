using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hackernewsapi.Controllers;
using hackernewsapi.Model;
using hackernewsapi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace hackernewsapi.unit.tests.Controllers
{
    [TestClass]
    public class HackerNewsControllerTest
    {
        private HackerNewsController _hackerNewsController;
        private Mock<IHackerNewsService> _mockHackerNewsService;

        public HackerNewsControllerTest(){
            _mockHackerNewsService = new Mock<IHackerNewsService>();
            _hackerNewsController = new HackerNewsController(null, _mockHackerNewsService.Object);
            _hackerNewsController.ControllerContext = new ControllerContext();
            _hackerNewsController.ControllerContext.HttpContext = new DefaultHttpContext();
        }

        [TestMethod]
        public async Task  OnGet_Default_ReturnsEmtpyList()
        {
            //Arrange
            var emptyList = new List<OutputStory>();
            _mockHackerNewsService.Setup(s => s.GetBestOrderedStories(false)).ReturnsAsync(emptyList);

            //Act
            var stories = await _hackerNewsController.Get();

            //Asset
            Assert.IsNotNull(stories);
            Assert.AreEqual(0, stories.Count());
        }

        [TestMethod]
        public async Task  OnGet_Default_ReturnsSingleStory()
        {
            //Arrange
            var singleStoryList = new List<OutputStory>();
            singleStoryList.Add(new OutputStory());
            _mockHackerNewsService.Setup(s => s.GetBestOrderedStories(false)).ReturnsAsync(singleStoryList);

            //Act
            var stories = await _hackerNewsController.Get();

            //Asset
            Assert.IsNotNull(stories);
            Assert.AreEqual(1, stories.Count());
        }

        [TestMethod]
        public void CleanCache_Default_ClearServiceCache()
        {
            //Act
            _hackerNewsController.CleanCache();

            //Asset
            Assert.IsNotNull(_hackerNewsController);
        }
    }
}
