using AzMyeStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzMyeStore.DataAccess.SQL
{
    public class DataContext : DbContext   // Datacontext inherits is going to inherit from a Data entity framework class called DbContext(Added "using System.Data.Entity;" for this)
                                           // DbContext contains all the methods we need for our datacontext to work, see the definition(F12)
    {

        public DataContext() : base("DefaultConnection")   // this is the name for connectionstring in WebConfig file in WebUI project, 
                                                           //this tells the datacontext to connect using this connection string
                                                           // also the entire connection string is copied in appconfig
        {

        }

        //we need to tell the datacontext which models would be stored in tables, for example we dont want to store our view models in the database,
        //we have to be explicit in specifying what we want to store and we do this by using a DbSet command


        public DbSet<Product> Products { get; set; }
         
        public DbSet<ProductCategory> ProductCategories { get; set; }

        public DbSet<Basket> Baskets { get; set; }

        public DbSet<BasketItem> BasketItems { get; set; }

        //now the next step is to physically build our database and this done by running Migration Commands using package manager console
        // to access that go to view--> other windows --> package manager console

    }
}
