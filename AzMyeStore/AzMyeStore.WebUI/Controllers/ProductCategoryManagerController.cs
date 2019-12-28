using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AzMyeStore.Core.Models;
using AzMyeStore.DataAccess.InMemory;

namespace AzMyeStore.WebUI.Controllers
{
    public class ProductCategoryManagerController : Controller
    {
        // GET: ProductCategoryManager
        ProductCategoryRepository context;
        public ProductCategoryManagerController()
        {
            context = new ProductCategoryRepository();
        }

        public ActionResult Index() //Making the Index page return a list of products by pulling in the list from collections list on repository page

        {
            List<ProductCategory> productCategories = context.Collection().ToList();
            return View(productCategories);
        }

        public ActionResult Create()  // this method is to display a page to fill in the product details
        {
            ProductCategory productCategory = new ProductCategory();
            return View(productCategory);
        }

        [HttpPost]
        public ActionResult Create(ProductCategory productCategory) // this method is to post the product details of the newly created product
        {
            if (!ModelState.IsValid)  //this is neccessary to check to make sure any validation set on the page is correct else return to product list 
                                      //with the neccessary validation errors 
            {
                return View(productCategory);
            }

            else
            {
                context.Insert(productCategory);
                context.Commit();

                return RedirectToAction("Index");  // returning to the index after successfully adding a product
            }
        }

        public ActionResult Edit(string Id)
        {
            ProductCategory productCategory = context.Find(Id);  //to load the product to be edited from the database

            if (productCategory == null)
            {
                return HttpNotFound();
            }

            else
            {
                return View(productCategory); //else return the view with the product that we have found
            }
        }

        [HttpPost]
        public ActionResult Edit(ProductCategory productCategory, String Id)  //default template requires to send through the updateed product along with the original product ID
                                                                              //just in case you want to update the original product ID
        {
            ProductCategory productcategorytoupdate = context.Find(Id);

            if (productcategorytoupdate == null)
            {
                return HttpNotFound();
            }

            else
            {
                if (!ModelState.IsValid)  //this is neccessary to check to make sure any validation set on the page is correct else return to product list 
                                          //with the neccessary validation errors 
                {
                    return View(productCategory);
                }

                productcategorytoupdate.Category = productCategory.Category;
                context.Commit();

                return RedirectToAction("Index");
            }
        }

        public ActionResult Delete(string Id) // the first action is to load the product from the DB that needs to be deleted
        {
            ProductCategory productcategorytodelete = context.Find(Id);

            if (productcategorytodelete == null)
            {
                return HttpNotFound();
            }

            else
            {
                return View(productcategorytodelete);
            }
        }

        [HttpPost]
        [ActionName("Delete")] //Alternative action name of Delete
        public ActionResult ConfirmDelete(string Id)  // the second action is so that the user can confirm before they can actually delete the product
        {
            ProductCategory productcategorytodelete = context.Find(Id);
            if (productcategorytodelete == null)
            {
                return HttpNotFound();
            }

            else
            {
                context.Delete(Id);
                context.Commit();
                return RedirectToAction("Index");

            }
        }

    }
}