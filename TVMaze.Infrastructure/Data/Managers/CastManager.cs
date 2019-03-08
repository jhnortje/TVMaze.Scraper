using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TVMaze.Infrastructure.Data.Entities;
using TVMaze.Infrastructure.Data.Repository;
using TVMaze.Infrastructure.EntityFramework;

namespace TVMaze.Infrastructure.Data.Managers
{
    public class CastManager : IRepository<Cast>
    {
        readonly ApplicationContext _applicationContext;

        public CastManager(ApplicationContext context)
        {
            _applicationContext = context;
        }

        public IEnumerable<Cast> GetAll()
        {
            return _applicationContext.Cast.ToList();
        }

        public Cast Get(long id)
        {
            return _applicationContext.Cast.FirstOrDefault(c => c.CastId == id);
        }

        public void Merge(List<Cast> casts, List<Cast> castList = null)
        {
            foreach (var cast in casts)
            {
                // TODO: Consider getting all the shows between max and min ID in list.  (Minimize calles to DB.)
                var source = castList == null ? Get(cast.CastId) : castList.Where(c => c.CastId == cast.CastId).FirstOrDefault();
                if (source == null)
                {
                    // TODO: Evert time the context Save Changes it commites to Database. (Review saving it at end after all updates are made)
                    Insert(cast);
                }
                else //if (show.Modified >= DateTime.Now.AddDays(1)) // TODO: Think about this!!
                {
                    Update(cast, source);
                }
            }
        }

        public void Insert(Cast entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            _applicationContext.Cast.Add(entity);
            _applicationContext.SaveChanges();
        }

        public void Update(Cast entity, Cast cast = null)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            if (cast == null)
            {
                cast = _applicationContext.Cast.FirstOrDefault(c => c.CastId == entity.CastId);
            }

            cast.Name = entity.Name;
            cast.Birthday = entity.Birthday;

            _applicationContext.SaveChanges();
        }

        public void Delete(Cast entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            _applicationContext.Cast.Remove(entity);
            _applicationContext.SaveChanges();
        }

        public IEnumerable<Cast> GetRange(long fromId, long toId)
        {
            throw new NotImplementedException();
        }
    }
}
