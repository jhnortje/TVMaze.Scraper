using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TVMaze.Infrastructure.Data.Entities;
using TVMaze.Infrastructure.Data.Managers;
using TVMaze.Infrastructure.Data.Repository;
using TVMaze.Infrastructure.EntityFramework;
using TVMaze.ScraperService.Constrants;
using TVMaze.ScraperService.RequestHandlers;

namespace TVMaze.ScraperService.Service
{
    public class ScrappingDataService : IHostedService, IDisposable
    {
        private IRequestHandler _httpWebRequestHandler = new HttpWebRequestHandler();
        private bool _isLocked = false;
        private const int _pageSize = 250;
        private DateTime lastUpdateExistingDataDataTime;
        
        private Timer _timer;

        /// <summary>
        /// Services Start Async
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Sync once a day when this services is running as Windows Service.
            TimeSpan timerTickInterval = new TimeSpan(24, 0, 0);

            _timer = new Timer(
                (e) => ScrappingApiToDatabase(),
                null,
                TimeSpan.Zero,
                timerTickInterval); 

            return Task.CompletedTask;
        }

        /// <summary>
        /// Core logic to scrapping the API data to persistent local database
        /// </summary>
        private void ScrappingApiToDatabase()
        {
            if (!_isLocked)
            {
                try
                {
                    _isLocked = true;
                    var page = "?page=";

                    using (var context = new ApplicationContext())
                    {
                        var _castShowRepository = new ShowManager(context);

                        long pageNumber = 0;

                        // Always update our dataset on start of this Services or once a week if service is running.
                        if (lastUpdateExistingDataDataTime == null || (DateTime.Now - lastUpdateExistingDataDataTime).TotalDays > 7)
                        {
                            lastUpdateExistingDataDataTime = DateTime.Now;
                        }
                        else
                        {
                            var allShows = _castShowRepository.GetAll().ToList();
                            var maxShowId = allShows.Count > 0 ? allShows.Max(s => s.ShowId) : 1;
                            
                            // 250 row per page.
                            pageNumber = Convert.ToInt64(Math.Round(Convert.ToDouble(maxShowId / 250)));
                        }

                        var showPageContent = string.Empty;
                        do
                        {
                            long rangeToId = (pageNumber + 1) * _pageSize;
                            var localPageShowList = _castShowRepository.GetRange((rangeToId - _pageSize) + 1, rangeToId);

                            try
                            {
                                showPageContent = _httpWebRequestHandler.GetSourceTvMazeContent($"{RequestConstants.Url}{page}{(pageNumber).ToString()}");
                                if (!showPageContent.Equals(string.Empty) && !showPageContent.Equals("404"))
                                {
                                    JArray showJsonArray = JArray.Parse(showPageContent);

                                    var shows = new List<Show>();
                                    for (int i = 0; i < showJsonArray.Count; i++)
                                    {
                                        dynamic showSourceData = JObject.Parse(showJsonArray[i].ToString());
                                        var showId = (long)showSourceData.id;

                                        shows.Add(new Show()
                                        {
                                            ShowId = showId,
                                            Name = showSourceData.name,
                                            ShowCasts = GetShowCastDetailFromSource(showId)
                                        });
                                    }

                                    // Merge the show data to local persists storage.
                                    _castShowRepository.Merge(shows, localPageShowList.ToList());
                                    pageNumber++;
                                }
                            }
                            catch(WebException ex)
                            {
                                // 404: we have reached the end of the pages.  
                                if (ex.Message.Contains("404"))
                                {
                                    showPageContent = "404";
                                }
                                else
                                {
                                    // Wait a hour and try the page again.
                                    TimeSpan sleepTimeSpan = new TimeSpan(1, 0, 0);
                                    Thread.Sleep(sleepTimeSpan);
                                }
                            }

                        } 
                        while (!showPageContent.Equals("404")); 
                    }
                }
                finally
                {
                    _isLocked = false;
                }
            }
            else
            {
                // Skip this iteration because there is one still busy.
            }
        }

        /// <summary>
        /// Map Json to our local Cast and CastShow Entities.
        /// </summary>
        /// <param name="showId"></param>
        /// <returns>List of Cast Members to show.</returns>
        private List<CastShow> GetShowCastDetailFromSource(long showId)
        {
            List<CastShow> castShows = new List<CastShow>();
            var castPageContent = _httpWebRequestHandler.GetSourceTvMazeContent($"{RequestConstants.Url}/{showId}/cast");
            JArray castJsonArray = JArray.Parse(castPageContent);

            for (int j = 0; j < castJsonArray.Count; j++)
            {
                dynamic castSourceData = JObject.Parse(castJsonArray[j].ToString());
                var castId = (long)castSourceData.person.id; 

                castShows.Add(new CastShow()
                {
                    CastId = castId,
                    ShowId = showId,
                    Cast = new Cast()
                    {
                        CastId = castId,
                        Name = castSourceData.person.name,
                        Birthday = castSourceData.person.birthday
                    }
                });
            }

            return castShows;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
