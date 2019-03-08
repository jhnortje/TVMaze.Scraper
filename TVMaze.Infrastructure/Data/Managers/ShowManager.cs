using System;
using System.Collections.Generic;
using System.Linq;
using TVMaze.Infrastructure.Data.Entities;
using TVMaze.Infrastructure.Data.Repository;
using TVMaze.Infrastructure.EntityFramework;

namespace TVMaze.Infrastructure.Data.Managers
{
    public class ShowManager : IRepository<Show>
    {
        readonly ApplicationContext _applicationContext;

        public ShowManager(ApplicationContext context)
        {
            _applicationContext = context;
        }

        public IEnumerable<Show> GetAll()
        {
            return _applicationContext.Show.ToList();
        }

        public IEnumerable<Show> GetRange(long fromId, long toId)
        {
            return _applicationContext.Show.Where(s => s.ShowId >= fromId && s.ShowId <= toId) .ToList();
        }

        public Show Get(long id)
        {
            return _applicationContext.Show.FirstOrDefault(c => c.ShowId == id);
        }

        /// <summary>
        /// Merge into existing Shows
        /// </summary>
        /// <param name="shows"></param>
        /// <param name="showList"></param>
        public void Merge(List<Show> shows, List<Show> showList = null)
        {
            if (shows == null)
            {
                throw new ArgumentNullException("entity");
            }
            
            foreach (var show in shows)
            {
                var source = showList == null ? Get(show.ShowId) : showList.Where(s => s.ShowId == show.ShowId).FirstOrDefault();
                if (source == null)
                {
                    // Re-use exsing cast members in our table to map to this show as well.
                    List<long> castIds = show.ShowCasts.Select(sc => sc.CastId).ToList();
                    List<long> existingCastIds = _applicationContext.Cast.Where(c => castIds.Contains(c.CastId)).Select(c => c.CastId).ToList();
                    
                    foreach (var showCasts in show.ShowCasts.Where(sc => existingCastIds.Contains(sc.CastId)))
                    {
                        showCasts.Cast = null; 
                    }

                    _applicationContext.Show.Add(show);
                }
                else 
                {
                    if (source == null) 
                    {
                        source = _applicationContext.Show.FirstOrDefault(c => c.ShowId == show.ShowId); 
                    }

                    source.Name = show.Name;
                }
            }

            // Delete shows from page that nolonger exists in ApiSource
            List<long> showIdsToMerge = shows.Select(s => s.ShowId).ToList();
            List<Show> showsDeleted = showList.Where(s => !showIdsToMerge.Contains(s.ShowId)).ToList();

            _applicationContext.Show.RemoveRange(showsDeleted);
            _applicationContext.SaveChanges();
        }

        public void Insert(Show entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            _applicationContext.Show.Add(entity);
            _applicationContext.SaveChanges();
        }

        public void Update(Show entity, Show show = null)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            if (show == null)
            {
                show = _applicationContext.Show.FirstOrDefault(c => c.ShowId == entity.ShowId);
            }

            show.Name = entity.Name;

            _applicationContext.SaveChanges();
        }

        public void Delete(Show entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            _applicationContext.Show.Remove(entity);
            _applicationContext.SaveChanges();
        }
    }
}
