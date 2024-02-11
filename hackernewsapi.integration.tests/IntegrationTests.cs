using Microsoft.VisualStudio.TestTools.UnitTesting;
using hackernewsapi;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;
using hackernewsapi.Model;
using System.Net;

namespace hackernewsapi.integration.tests
{
    [TestClass]
    public class IntegrationTests
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public IntegrationTests()
        {
            _factory = new IntegrationTestFactory();
        }

        [TestMethod]
        public async Task Root_CheckApi_ReturnsOk()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/");

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task HackerNews_FetchNoCachedNews_Returns20News()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/hackernews");

            // Assert
            List<OutputStory> responseContent = JsonSerializer.Deserialize<List<OutputStory>>(await response.Content.ReadAsStringAsync());

            Assert.IsNotNull(responseContent);
            Assert.AreEqual(20, responseContent.Count);
        }

        [TestMethod]
        public async Task HackerNews_FetchCachedNews_Returns20News()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("DisableCache", "true");

            // Act
            var response = await client.GetAsync("/hackernews");

            // Assert
            List<OutputStory> responseContent = JsonSerializer.Deserialize<List<OutputStory>>(await response.Content.ReadAsStringAsync());

            Assert.IsNotNull(responseContent);
            Assert.AreEqual(20, responseContent.Count);
        }

        [TestMethod]
        public async Task Clean_CleanCache_CleanSuccessfully()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/clean");

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
