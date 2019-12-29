using AzMyeStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;


namespace AzMyeStore.DataAccess.InMemory
{
    public class InMemoryRepository<T> where T:BaseEntity  //<T> is an indicator for marking this as a generic class instead of T it can be anyword , 
                                       //this 'T' used here will be used to indicate usage of this class
                                       //so when we pass an object for example T it must be of type Base Entity or atleast inherit from BaseEntity
                                       //because BaseEntity has Id, so that whenever we reference "Id" , our generic class know what that is.
    {
        ObjectCache cache = MemoryCache.Default;
        List<T> items; // list of product or product category.
        string className; // extra internal variable, this is to give us an easy handle to set how we are going to store our 
                          //objects in the cache , because everytime we create cache we would have a seperate cache for 
                          //"Product" and "ProductCategory", we need to be able to tell it what that name is going to be, 
                          //since it would need to be different each time

        public InMemoryRepository()
        {
            className = typeof(T).Name;  // this is reflection concept, for this we use a command called typeof, we pass to it the object we are using and 
                                         //from there we get its internal name, this code gets the actual name of our class i.e if we pass product, 
                                         // the name will be "Product" and if we pass product category the name will be "ProductCategory"
            items = cache[className] as List<T>;

                 if(items == null)
            {
                items = new List<T>();
            }
            
        }

        public void Commit()
        {
            cache[className] = items;
        }
   
        public void Insert(T t)
        {
            items.Add(t);
        }

        public void Update(T t)
        {
            T tToUpdate = items.Find(p => p.Id == t.Id);  // the "Id" here has to match the exact text case in Base Entity
                if(tToUpdate != null)
            {
                tToUpdate = t;
            }

            else
            {
                throw new Exception(className +"not found!!!!");
            }
        }

        public T Find(string ID)  // this ID being passed does not have match the exact Text in Base Entity
        {
            T t = items.Find(p => p.Id == ID); // the "Id" here has to match the exact text case in Base Entity i.e in p.Id
            if (t != null)
            {
                return t;
            }

                else
            {
                throw new Exception(className + "not found!!!!");
            }
        }

        public IQueryable<T> Collection()
        {
            return items.AsQueryable();
        }

        public void Delete(string ID)
        {
            T tToDelete = items.Find(p => p.Id == ID);
            if(tToDelete != null)
            {
                items.Remove(tToDelete);
            }

            else
            {
                throw new Exception(className + "not found!!!!");
            }
        }
    }
}
