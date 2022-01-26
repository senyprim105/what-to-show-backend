using System.Net.Mime;
using Microsoft.EntityFrameworkCore;
using server_api.Data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server_api.Data.Models.Repositories
{
    public class EFSimpleRepository<T,K>: ISimpleRepository<T,K> 
    where T : class, ISimpleEntity<K>, new()
    where K :IEquatable<K>
    {
        protected readonly DbApp dbContext;

        public EFSimpleRepository(DbApp context)
        {
            this.dbContext = context;
        }
        // Читать все
        public virtual IQueryable<T> ReadAll => dbContext.Set<T>();

        public virtual async Task<T> Create(T entity)
        {
            var existing = await Read(entity.Id);
            if (existing != null)
            {
                throw new ArgumentException("Can not add existing object");
            };
            dbContext.Set<T>().Add(entity);
            await dbContext.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<IEnumerable<T>> CreateIfNotFound(IEnumerable<T> entities)
        {
            var result = new List<T>();
            foreach (var it in entities)
            {
                var existItem = await Find(it);
                result.Add(existItem ?? await Create(it));
            }
            return result;
        }

        public virtual async Task<T> Delete(K id)
        {
            T removeObj = dbContext.Set<T>().Find(id);
            dbContext.Attach(removeObj);
            dbContext.Remove(removeObj);
            await dbContext.SaveChangesAsync();
            return removeObj;
        }
        // Прочитать по Id

        public virtual async Task<T> Read(K id) => await ReadAll.SingleOrDefaultAsync(i => i.Id.Equals(id));

        public virtual async Task<T> Find(T entity)
        {
            if (entity == null) return null;
            var existingEntity = await Read(entity.Id);
            if (existingEntity == null) return null;
            return existingEntity;
        }

        public virtual async Task<T> Update(T entity)
        {
            dbContext.Attach(entity);
            dbContext.Entry(entity).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
            return entity;
        }

        public DbApp getContext()
        {
            return dbContext;
        }
    };

    public class EFFileRepository: EFSimpleRepository<FileApp, int>,IFileRepository
    {
        public EFFileRepository(DbApp context) : base(context)
        {
        }
    }
    public class EFGenreRepository: EFSimpleRepository<Genre, int>,IGenreRepository
    {
        public EFGenreRepository(DbApp context) : base(context)
        {
        }
    }
    public class EFMovieRepository: EFSimpleRepository<Movie, int>,IMovieRepository
    {
        public EFMovieRepository(DbApp context) : base(context)
        {
        }
    }
    public class EFPersonRepository: EFSimpleRepository<Person, int>,IPersonRepository
    {
        public EFPersonRepository(DbApp context) : base(context)
        {
        }
    }
    public class EFReviewRepository: EFSimpleRepository<Review, int>, IReviewRepository
    {
        public EFReviewRepository(DbApp context) : base(context)
        {
        }
    }
    public class EFUserDataRepository: EFSimpleRepository<UserData, int>, IUserDataRepository
    {
        public EFUserDataRepository(DbApp context) : base(context)
        {
        }
    }
}

