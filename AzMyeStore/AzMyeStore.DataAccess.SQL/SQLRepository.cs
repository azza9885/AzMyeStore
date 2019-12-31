using AzMyeStore.Core.Contracts;
using AzMyeStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzMyeStore.DataAccess.SQL
{
    public class SQLRepository<T> : IRepository<T> where T : BaseEntity
        //after adding IRepository<T> where T:BaseEntity an error is shown under IRepository and the suggestion that are shown
        //indicate to implement the interface related to IRepository.Doing that will implement a broiler  plate code which 
        //contains nothing but throw new NotImplementedException(); in the method
    {
        //for each of the following methods we have to hook up our datacontext here, so that it exposes the underlying tables so that we can implement each of these methods
        //to make our repository work we have to inject into it our Datacontext class  here , we also need a way of mapping an underlying product to the underlying product tables itself
        //we do that using few internal datacontext commands for that we need few internal variables for DataContext and DbSet

        internal DataContext context; //this is the context of the DataContext class that implements DbContext and DbSet is the underlying table we want to access
        internal DbSet<T> dbSet;

        public SQLRepository(DataContext context)
        {
            this.context = context;
            this.dbSet = context.Set<T>();
        }
        public IQueryable<T> Collection()
        {
            return dbSet;
        }

        public void Commit()
        {
            context.SaveChanges();
        }

        public void Delete(string Id)
        {
            var t = Find(Id);
            if (context.Entry(t).State == EntityState.Detached) // check the status of the entry to see if it is detached, if yes then attach it and then remove it
                dbSet.Attach(t);

            dbSet.Remove(t);

        }

        public T Find(string Id)
        {
            return dbSet.Find(Id);
        }

        public void Insert(T t)
        {
            dbSet.Add(t);
        }

        public void Update(T t)  // update is a little complicated we first need  to attach our model and then set its state to modified so that 
                                 // entity framework knows how to update the  underlying database, this is because entity framework esssentially caches data 
                                 //and does not submit it immediately to the database and that is why we have SaveChanges method seperately and that is why 
                                 //we have to explicityly tell it to do that
        {
            dbSet.Attach(t);  // atatching the model to our dbSet
            context.Entry(t).State = EntityState.Modified; // setting the state of the entry
        }
    }
}
