using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using hackernewsapi.Services.Api;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using hackernewsapi.Model;
using AutoMapper;
using System.Collections.Concurrent;
using AutoMapper.Configuration;

namespace hackernewsapi.Services
{
    public class HackerNewsService : IHackerNewsService
    {
        private IHackerNewsApi _hackerNewsApi;
        private IMapper _mapper;

        private ConcurrentDictionary<int, Story> _storyCache;
        public HackerNewsService([FromServices] IHackerNewsApi hackerNewsApi, IMapper mapper)
        {
            _hackerNewsApi = hackerNewsApi;
            _mapper = mapper;
            _storyCache = new ConcurrentDictionary<int, Story>();
        }

        public void CleanCache()
        {
            _storyCache.Clear();
        }

        public async Task<IEnumerable<OutputStory>> GetBestOrderedStories(bool disableCache, int numberToTake)
        {
            var outputStories = new List<OutputStory>();

            using (var client = new HttpClient())
            {
                var bestStories = await _hackerNewsApi.GetBestStories();
                var stories = new List<Story>();

                Array.Sort(bestStories);

                var tasks = bestStories.Select(async storyId =>
                {
                    Story story = await GetStoryFromCache(storyId, disableCache);
                    stories.Add(story);
                }).ToList();

                Task.WaitAll(tasks.ToArray());

                outputStories = _mapper.Map<List<OutputStory>>(stories);
            }
            //var storiesToTake = _configuration. ["NumberOfStories"];
            return outputStories.OrderByDescending(s => s.score).Take(numberToTake);
        }

        private async Task<Story> GetStoryFromCache(int storyId, bool disableCache)
        {
            Story story;
            if (disableCache == true)
            {
                story = await _hackerNewsApi.GetStoryById(storyId);
            }
            else
            {
                _storyCache.TryGetValue(storyId, out story);

                if (story == null)
                {
                    story = await _hackerNewsApi.GetStoryById(storyId);
                    _storyCache.TryAdd(storyId, story);
                }
            }

            return story;
        }
    }

}