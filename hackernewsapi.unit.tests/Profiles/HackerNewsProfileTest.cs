using System;
using AutoMapper;
using hackernewsapi.Model;
using hackernewsapi.Profiles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace hackernewsapi.unit.tests.Profiles
{
    [TestClass]
    public class HackerNewsProfileTest
    {
        private IMapper _mapper;

        public HackerNewsProfileTest()
        {
            var configuration = new MapperConfiguration(mc => { mc.AddProfile(new HackerNewsProfile()); });
            _mapper = configuration.CreateMapper();
        }

        [TestMethod]
        public void Map_Validate_MapAllTheFields()
        {
            var todayUnixTime = DateTimeOffset.Now.ToUnixTimeSeconds();
            var todayLocalDate = DateTimeOffset.FromUnixTimeSeconds(todayUnixTime).LocalDateTime.ToString("yyyy-MM-ddTHH:mm:ssK");

            //Arrange
            var story = new Story()
            {
                by = "1",
                descendants = 2,
                score = 3,
                time = todayUnixTime,
                title = "5",
                url = "6",
            };

            //Act
            var outputStory = _mapper.Map<OutputStory>(story);

            //Assert
            Assert.IsNotNull(outputStory);
            Assert.AreEqual(outputStory.postedBy, story.by);
            Assert.AreEqual(outputStory.commentCount, story.descendants);
            Assert.AreEqual(outputStory.score, story.score);
            Assert.AreEqual(outputStory.time, todayLocalDate);
            Assert.AreEqual(outputStory.title, story.title);
            Assert.AreEqual(outputStory.uri, story.url);
        }
    }
}
