using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TVMaze.Infrastructure.Data.Entities;
using TVMaze.Infrastructure.Data.Repository;
using TVMaze.Infrastructure.EntityFramework;

namespace TVMaze.Infrastructure.Data.Managers
{
    public class CastShowManager : IRepository<CastShow>
    {
        readonly ApplicationContext _applicationContext;

        public CastShowManager(ApplicationContext context)
        {
            _applicationContext = context;
        }

        public IEnumerable<CastShow> GetAll()
        {
            return _applicationContext.CastShow.ToList();
        }

        public IEnumerable<CastShow> GetAllByCastId(Int64 castId)
        {
            return _applicationContext.CastShow.Where(cs => cs.CastId == castId).ToList();
        }

        public IEnumerable<CastShow> GetAllByShowId(Int64 showId)
        {
            return _applicationContext.CastShow.Where(cs => cs.ShowId == showId).ToList();
        }

        public CastShow Get(long id)
        {
            return _applicationContext.CastShow.FirstOrDefault(c => c.CastShowId == id);
        }

        public void Insert(CastShow entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            _applicationContext.CastShow.Add(entity);
            _applicationContext.SaveChanges();
        }

        public void Update(CastShow entity, CastShow cast = null)
        {
            // I will never update mapping table.
            throw new NotImplementedException();
        }

        public void Delete(CastShow entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            _applicationContext.CastShow.Remove(entity);
            _applicationContext.SaveChanges();
        }

        public void Merge(List<CastShow> entities)
        {
            throw new NotImplementedException();
        }

        public void Merge(List<CastShow> entities, List<CastShow> dbEntity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CastShow> GetRange(long fromId, long toId)
        {
            throw new NotImplementedException();
        }
    }
}
