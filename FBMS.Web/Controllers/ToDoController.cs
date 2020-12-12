using FBMS.Core;
using FBMS.Core.Constants.Crawler;
using FBMS.Core.Dtos.Crawler;
using FBMS.Core.Entities;
using FBMS.Core.Interfaces;
using FBMS.SharedKernel.Interfaces;
using FBMS.Web.ApiModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace FBMS.Web.Controllers
{
    public class ToDoController : Controller
    {
        private readonly IRepository _repository;
        private readonly ICrawlerService _crawlerService;

        public ToDoController(IRepository repository, ICrawlerService crawlerService)
        {
            _repository = repository;
            _crawlerService = crawlerService;
        }

        public async Task<IActionResult> Index()
        {
            var items = (await _repository.ListAsync<ToDoItem>())
                            .Select(ToDoItemDTO.FromToDoItem);
            return View(items);
        }

        public IActionResult Populate()
        {
            int recordsAdded = DatabasePopulator.PopulateDatabase(_repository);
            return Ok(recordsAdded);
        }

        public async Task<IActionResult> Crawl()
        {
            var crawlRequest = new CrawlerRequestDto
            {
                Url = "https://www.ebay.com/b/Apple-iPhone/9355/bn_319682",
                Regex = @".*itm/.+",
                TimeOut = 5000,
                DownloderType = CrawlerDownloaderType.FromMemory,
                DownloadPath = @"D:\Freelance\Football\FBMS\FBMS\Crawler\"
            };
            await _crawlerService.CrawlAsync<Catalog>(crawlRequest);
            return Ok();
        }

        public async Task<IActionResult> CrawlIBet()
        {
            var crawlRequest = new CrawlerRequestDto
            {
                Url = "https://ag.ibet789.com/_Age/SubAccsWinLose.aspx?role=ag&userName=jxy83",
                //Regex = @".*itm/.+",
                TimeOut = 5000,
                DownloderType = CrawlerDownloaderType.FromMemory,
                DownloadPath = @"D:\Freelance\Football\FBMS\FBMS\Crawler\"
            };
            await _crawlerService.CrawlAsync<Catalog>(crawlRequest);
            return Ok();
        }
    }
}
