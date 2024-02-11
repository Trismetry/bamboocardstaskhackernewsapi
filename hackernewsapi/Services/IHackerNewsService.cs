using System.Collections.Generic;
using System.Threading.Tasks;
using hackernewsapi.Model;

namespace hackernewsapi.Services{
    public interface IHackerNewsService
    {
        Task<IEnumerable<OutputStory>> GetBestOrderedStories(bool disableCache, int numberToTake);

        void CleanCache();
    }
}