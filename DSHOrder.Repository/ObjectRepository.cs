using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Entity;
using System.Data;
using System.Linq.Expressions;
using System.Data.Objects;

namespace DSHOrder.Repository
{

    public abstract class ObjectRepository:IRepository
    {
        private DSHOrderManagementEntities context;

         /// <summary>
        /// T所对应的ObjectSet名称
        /// </summary>
        protected abstract string ObjectSetName
        {
            get;
        }

        #region Contructor
        public ObjectRepository()
        {
            context = new DSHOrderManagementEntities();
        }
        public ObjectRepository(DSHOrderManagementEntities dshContext)
        {
            context = dshContext;
        }
        #endregion

        #region IRepository 成员

        public T Add<T>(T entity,bool isMultipleSave=false) where T : class
        {
            this.context.CreateObjectSet<T>().AddObject(entity);
            if (!isMultipleSave)
            {
                context.SaveChanges();
            }
            return entity;
        }

        public T Update<T>(T entity, bool isMultipleSave = false) where T : class
        {
            EntityKey key = this.context.CreateEntityKey(this.ObjectSetName, entity);
            Object obj;
            if (this.context.TryGetObjectByKey(key, out obj))
            {
                this.context.ApplyCurrentValues<T>(this.ObjectSetName, entity); 
            }
            else
            {
                this.context.CreateObjectSet<T>().AddObject(entity);
            }
            if (!isMultipleSave)
            {
                this.context.SaveChanges();
            }
            return entity;
        }



        public int Delete<T>(T entity, bool isMultipleSave=false) where T : class
        {
            this.context.DeleteObject(entity);
            if (!isMultipleSave)
            {
                return this.context.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public T GetBy<T>(Expression<Func<T, bool>> condition) where T : class
        {
            return  this.context.CreateObjectSet<T>().Where(condition).FirstOrDefault();
        }

        public IList<T> GetAllBy<T>(Expression<Func<T, bool>> condition) where T: class
        {
            return this.context.CreateObjectSet<T>().Where(condition).ToList();
        }

        public IList<T> GetAll<T>() where T : class
        {
            return this.context.CreateObjectSet<T>().ToList();
        }

        public IQueryable<T> CreateQuery<T>() where T : class
        {
            return context.CreateObjectSet<T>();   
        }

        public ObjectResult<T> ExecuteFunction<T>(string funName,
                                params ObjectParameter[] @params)
        {
            return context.ExecuteFunction<T>(funName, @params);
        }

        public virtual int SaveChanges()
        {
            var result = context.SaveChanges(SaveOptions.AcceptAllChangesAfterSave);
            return result;
        }

        #endregion

        public void UpdateEntityWithDelegate<T>(Expression<Func<T, bool>> predicate, Action<T> action) where T:class
        {
            var entity = context.CreateObjectSet<T>().SingleOrDefault(predicate);
            action(entity);
        }

        public DSHOrderManagementEntities GetContext()
        {
            return context;
        }
    }
}
