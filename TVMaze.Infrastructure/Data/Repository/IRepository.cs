using System;
using System.Collections.Generic;
using System.Text;
using TVMaze.Infrastructure.Data.Entities;

namespace TVMaze.Infrastructure.Data.Repository
{
    public interface IRepository<TEntity>
    {
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetRange(long fromId, long toId);
        TEntity Get(long id);
        void Insert(TEntity entity);
        void Merge(List<TEntity> entities, List<TEntity> dbEntity);
        void Update(TEntity entity, TEntity dbEntity);
        void Delete(TEntity entity);
    }
}
