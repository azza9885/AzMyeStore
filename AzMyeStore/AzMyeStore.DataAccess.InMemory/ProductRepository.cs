using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
using AzMyeStore.Core.Models;

namespace AzMyeStore.DataAccess.InMemory
{
    public class ProductRepository
    {
        ObjectCache cache = MemoryCache.Default;
        List<Product> products;
  

        public ProductRepository()
        {
            products = cache["products"] as List<Product>;
            if (products == null)
            {
                products = new List<Product>();
            }
        }
        public void Commit()  //this method is used to not commit the products immediately they are first cached and then added to the product list
        {
            cache["products"] = products;
        }

        public void Insert(Product p)
        {
            products.Add(p);
        }
        public void Update(Product product)
        {
            Product ProducttoUpdate = products.Find(p => p.Id == product.Id);

            if (ProducttoUpdate != null) // so if a product is found ,  this if statement helps to assign the produttoupdate to the product we are looking for i.e 'product' we passed in as an argument
            {
                ProducttoUpdate = product;
            }

            else
            {
                throw new Exception("Product Not Found");
            }

        }

        public Product Find(string Id)  //since this method will return a single product the method is named as public Product Find(string Id)
        {
            Product product = products.Find(p => p.Id == Id);

            if (product != null)
            {
                return product;
            }

            else
            {
                throw new Exception("Product Not Found");
            }
        }

        public IQueryable<Product> Collection() //this will return a list of products that can be queried
        {
            return products.AsQueryable();  // this will return an internal list of products
        }

        public void Delete(string Id)
        {
            Product producttodelete = products.Find(p => p.Id == Id);

            if (producttodelete != null)
            {
                products.Remove(producttodelete);
            }

            else
            {
                throw new Exception("Product not found");
            }
        }

    }
}
