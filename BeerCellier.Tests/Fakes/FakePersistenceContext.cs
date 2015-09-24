using BeerCellier.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BeerCellier.Tests.Fakes
{
    public class FakePersistenceContext : IPersistenceContext
    {
        public Dictionary<Type, Object> Sets = new Dictionary<Type, object>();
        public List<Object> Added = new List<object>();
        public List<Object> Removed = new List<object>();
        public bool Saved = false;

        public T Add<T>(T entity) where T : class
        {
            Added.Add(entity);
            return entity;
        }        

        public IQueryable<T> Query<T>() where T : class
        {
            return Sets[typeof(T)] as IQueryable<T>;
        }

        public T Remove<T>(T entity) where T : class
        {
            Removed.Add(entity);
            return entity;
        }

        public void AddSet<T>(IQueryable<T> objects) where T : class
        {
            Sets.Add(typeof(T), objects);
        }

        public int SaveChanges()
        {
            Saved = true;
            return 0;
        }

        public void Dispose() { }
    }
}
