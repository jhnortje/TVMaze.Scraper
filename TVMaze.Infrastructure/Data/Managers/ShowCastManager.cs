using System;
using System.Collections.Generic;
using System.Linq;
using TVMaze.Infrastructure.Data.Repository;
using TVMaze.Infrastructure.EntityFramework;
using TVMaze.Infrastructure.Model;

namespace TVMaze.Infrastructure.Data.Managers
{
    public class ShowCastManager : IRepository<ShowModel>
    {
        readonly ApplicationContext _applicationContext;

        public ShowCastManager(ApplicationContext context)
        {
            _applicationContext = context;
        }

        public IEnumerable<ShowModel> GetAll()
        {
            return _applicationContext.Show
                    .Select(s => new ShowModel()
                    {
                        Id = s.ShowId,
                        Name = s.Name,
                        Cast = s.ShowCasts
                                .Select(c => new CastModel()
                                {
                                    Id = c.CastId,
                                    Name = c.Cast.Name,
                                    Birthday = c.Cast.Birthday
                                }).OrderByDescending(c => c.Birthday).ToList()
                    }).ToList();
        }

        /// <summary>
        /// Will use this to alwys get the same page items back.  As from the Source API
        /// </summary>
        /// <param name="fromId"></param>
        /// <param name="toId"></param>
        /// <returns>Formatted Model for Return</returns>
        public IEnumerable<ShowModel> GetRange(long fromId, long toId)
        {
            return _applicationContext.Show
                    .Where(s => s.ShowId >= fromId && s.ShowId <= toId)
                    .Select(s => new ShowModel()
                    {
                        Id = s.ShowId,
                        Name = s.Name,
                        Cast = s.ShowCasts
                                .Select(c => new CastModel()
                                {
                                    Id = c.CastId,
                                    Name = c.Cast.Name,
                                    Birthday = c.Cast.Birthday
                                }).OrderByDescending(c => c.Birthday).ToList()
                    }).ToList();
        }

        public ShowModel Get(long id)
        {
            throw new NotImplementedException();
        }
        
        public void Insert(ShowModel entity)
        {
            throw new NotImplementedException();
        }

        public void Update(ShowModel entity, ShowModel dbEntity)
        {
            throw new NotImplementedException();
        }
        public void Delete(ShowModel entity)
        {
            throw new NotImplementedException();
        }

        public void Merge(List<ShowModel> entities, List<ShowModel> dbEntity)
        {
            throw new NotImplementedException();
        }
    }
}
