using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using hackernewsapi.Services;
using hackernewsapi.Model;
using Microsoft.AspNetCore.Http;

namespace hackernewsapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HackerNewsController : ControllerBase
    {
        private readonly ILogger<HackerNewsController> _logger;
        private IHackerNewsService _hackerNewsService;
        public HackerNewsController(ILogger<HackerNewsController> logger, IHackerNewsService hackerNewsService){
            _hackerNewsService = hackerNewsService;
            _logger = logger;
        }

        /// <summary>
        ///     Return the first n hacker news ordered by score
        /// </summary>
        /// <remarks>
        ///     For default I cache the results but you can disable the cache 
        ///     setting the DisableCache header attribute to false
        ///     
        ///     Sample request:
        ///     GET /hackernews --header 'DisableCache: true'
        ///     To change number of returned stories please use --header 'NumberToTake: n'
        /// </remarks>
        /// <returns>
        ///     A list of OutputStory ordered by score
        /// </returns>
        /// <response code="200">The best hacker news</response>
        /// <response code="500">Server internal error</response>
        [HttpGet]
        [Route("/hackernews")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces(typeof(IEnumerable<OutputStory>))]
        public async Task<IEnumerable<OutputStory>> Get()
        {
            bool disableCache = false;
            bool.TryParse(Request.Headers["DisableCache"], out disableCache);

            int numberToTake = 0;
            int.TryParse(Request.Headers["NumberToTake"], out numberToTake);
            if (numberToTake == 0) numberToTake = 22;

            return await _hackerNewsService.GetBestOrderedStories(disableCache, numberToTake);
        }


        /// <summary>
        ///     Clean the news cache
        /// </summary>
        /// <remarks>
        ///     Sample request:
        ///     GET /clean
        /// </remarks>        
        /// <response code="200">Cache cleaned successfully</response>
        /// <response code="500">Server internal error</response>
        [HttpGet]
        [Route("/clean")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CleanCache()
        {
            _hackerNewsService.CleanCache();
            return Ok();
        }
    }
}
