using Microsoft.EntityFrameworkCore;
using MomAndChildren.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomAndChildren.Data.Base
{
    public class GenericRepository<T> where T : class
    {
        protected Net1710_221_3_MomAndChildrenContext _context;

        public GenericRepository(Net1710_221_3_MomAndChildrenContext context)
        {
            _context = context;
        }

        public GenericRepository()
        {
            _context ??= new Net1710_221_3_MomAndChildrenContext();
        }

        //#region Separating asign entity and save operators

        //public GenericRepository(Net1710_221_3_MomAndChildrenContext context)
        //{
        //    _context = context;
        //    _dbSet = _context.Set<T>();
        //}

        //public void PrepareCreate(T entity)
        //{
        //    _dbSet.Add(entity);
        //}

        //public void PrepareUpdate(T entity)
        //{
        //    var tracker = _context.Attach(entity);
        //    tracker.State = EntityState.Modified;
        //}

        //public void PrepareRemove(T entity)
        //{
        //    _dbSet.Remove(entity);
        //}

        //public int Save()
        //{
        //    return _context.SaveChanges();
        //}

        //public async Task<int> SaveAsync()
        //{
        //    return await _context.SaveChangesAsync();
        //}

        //#endregion Separating asign entity and save operators


        //public List<T> GetAll()
        //{
        //    return _dbSet.ToList();
        //}
        //public async Task<List<T>> GetAllAsync()
        //{
        //    return await _dbSet.ToListAsync();
        //}
        //public void Create(T entity)
        //{
        //    _dbSet.Add(entity);
        //    _context.SaveChanges();
        //}

        //public async Task<int> CreateAsync(T entity)
        //{
        //    _dbSet.Add(entity);
        //    return await _context.SaveChangesAsync();
        //}

        //public async Task<int> CreateListAsync(List<T> entity)
        //{
        //    _dbSet.AddRange(entity);
        //    return await _context.SaveChangesAsync();
        //}

        //public void Update(T entity)
        //{
        //    var tracker = _context.Attach(entity);
        //    tracker.State = EntityState.Modified;
        //    _context.SaveChanges();
        //}

        //public async Task<int> UpdateAsync(T entity)
        //{
        //    var tracker = _context.Attach(entity);
        //    tracker.State = EntityState.Modified;
        //    return await _context.SaveChangesAsync();
        //}

        //public bool Remove(T entity)
        //{
        //    _dbSet.Remove(entity);
        //    _context.SaveChanges();
        //    return true;
        //}

        //public async Task<bool> RemoveAsync(T entity)
        //{
        //    _dbSet.Remove(entity);
        //    await _context.SaveChangesAsync();
        //    return true;
        //}

        //public T GetById(int id)
        //{
        //    return _dbSet.Find(id);
        //}

        //public async Task<T> GetByIdAsync(int id)
        //{
        //    return await _dbSet.FindAsync(id);
        //}

        //public T GetById(string code)
        //{
        //    return _dbSet.Find(code);
        //}

        //public async Task<T> GetByIdAsync(string code)
        //{
        //    return await _dbSet.FindAsync(code);
        //}

        //public T GetById(Guid code)
        //{
        //    return _dbSet.Find(code);
        //}

        //public async Task<T> GetByIdAsync(Guid code)
        //{
        //    return await _dbSet.FindAsync(code);
        //}

        #region Separating asign entity and save operators

        public void PrepareCreate(T entity)
        {
            _context.Add(entity);
        }

        public void PrepareUpdate(T entity)
        {
            var tracker = _context.Attach(entity);
            tracker.State = EntityState.Modified;
        }

        public void PrepareRemove(T entity)
        {
            _context.Remove(entity);
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        #endregion Separating asign entity and save operators


        public List<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public void Create(T entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
        }

        public async Task<int> CreateAsync(T entity)
        {
            _context.Add(entity);
            return await _context.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            var tracker = _context.Attach(entity);
            tracker.State = EntityState.Modified;
            _context.SaveChanges();
        }

        public async Task<int> UpdateAsync(T entity)
        {
            var tracker = _context.Attach(entity);
            tracker.State = EntityState.Modified;

            return await _context.SaveChangesAsync();
        }

        public bool Remove(T entity)
        {
            _context.Remove(entity);
            _context.SaveChanges();
            return true;
        }

        public async Task<bool> RemoveAsync(T entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public T GetById(string code)
        {
            return _context.Set<T>().Find(code);
        }

        public async Task<T> GetByIdAsync(string code)
        {
            return await _context.Set<T>().FindAsync(code);
        }

        public T GetById(Guid code)
        {
            return _context.Set<T>().Find(code);
        }

        public async Task<T> GetByIdAsync(Guid code)
        {
            return await _context.Set<T>().FindAsync(code);
        }
    }
}
