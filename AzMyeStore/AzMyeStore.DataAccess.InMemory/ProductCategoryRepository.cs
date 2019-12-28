using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
using AzMyeStore.Core.Models;

namespace AzMyeStore.DataAccess.InMemory
{
    public class ProductCategoryRepository
    {
        ObjectCache cache = MemoryCache.Default;
        List<ProductCategory> productcategories;

        public ProductCategoryRepository()
        {
            productcategories = cache["productcategories"] as List<ProductCategory>;
            if (productcategories == null)
            {
                productcategories = new List<ProductCategory>();
            }
        }
        public void Commit()  //this method is used to not commit the products immediately they are first cached and then added to the product list
        {
            cache["productcategories"] = productcategories;
        }

        public void Insert(ProductCategory p)
        {
            productcategories.Add(p);
        }
        public void Update(ProductCategory productCategory)
        {
            ProductCategory ProductCategorytoUpdate = productcategories.Find(p => p.Id == productCategory.Id);

            if (ProductCategorytoUpdate != null) // so if a product is found ,  this if statement helps to assign the produttoupdate to the product we are looking for i.e 'product' we passed in as an argument
            {
                ProductCategorytoUpdate = productCategory;
            }

            else
            {
                throw new Exception("Product Not Found");
            }

        }

        public ProductCategory Find(string Id)  //since this method will return a single product the method is named as public Product Find(string Id)
        {
            ProductCategory productCategory = productcategories.Find(p => p.Id == Id);

            if (productCategory != null)
            {
                return productCategory;
            }

            else
            {
                throw new Exception("Product Not Found");
            }
        }

        public IQueryable<ProductCategory> Collection() //this will return a list of products that can be queried
        {
            return productcategories.AsQueryable();  // this will return an internal list of products
        }

        public void Delete(string Id)
        {
            ProductCategory productcategorytodelete = productcategories.Find(p => p.Id == Id);

            if (productcategorytodelete != null)
            {
                productcategories.Remove(productcategorytodelete);
            }

            else
            {
                throw new Exception("Product not found");
            }
        }

    }
}
