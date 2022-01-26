using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server_api.Data.Models.Interfaces
{
    public interface ISimpleRepository<T,K> where T: ISimpleEntity<K> where K: IEquatable<K>
    {
        public IQueryable<T> ReadAll { get; }
        public Task<T> Create(T entity);
        public Task<IEnumerable<T>> CreateIfNotFound(IEnumerable<T> entities);
        public Task<T> Read(K id);
        public Task<T> Update(T entity);
        public Task<T> Delete(K id);
        public Task<T> Find(T entity);
        public DbApp getContext();
    }

    public interface IFileRepository: ISimpleRepository<FileApp,int>{}
    public interface IGenreRepository: ISimpleRepository<Genre,int>{}
    public interface IMovieRepository: ISimpleRepository<Movie,int>{}
    public interface IPersonRepository: ISimpleRepository<Person,int>{}
    public interface IReviewRepository: ISimpleRepository<Review,int>{}
    public interface IUserDataRepository: ISimpleRepository<UserData,int>{}

}
