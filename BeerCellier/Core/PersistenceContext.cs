using BeerCellier.Entities;
using System;
using System.Data.Entity;
using System.Linq;

namespace BeerCellier.Core
{
    public interface IPersistenceContext : IDisposable
    {
        IQueryable<T> Query<T>() where T : class;

        T Add<T>(T entity) where T : class;
        T Remove<T>(T entity) where T : class;

        int SaveChanges();
    }

    public class PersistenceContext : DbContext, IPersistenceContext
    {
        public IDbSet<Beer> Beers { get; set; }
        public IDbSet<User> Users { get; set; }

        public T Add<T>(T entity) where T : class
        {
            return Set<T>().Add(entity);
        }

        public IQueryable<T> Query<T>() where T : class
        {
            return Set<T>();
        }

        public T Remove<T>(T entity) where T : class
        {
            return Set<T>().Remove(entity);
        }
    }
}