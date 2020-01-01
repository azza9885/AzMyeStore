using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AzMyeStore.Core.Models;
using AzMyeStore.DataAccess.InMemory;
using AzMyeStore.Core.ViewModels;
using AzMyeStore.Core.Contracts;
using System.IO;

namespace AzMyeStore.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        // GET: ProductManager
        IRepository<Product> context;
        IRepository<ProductCategory> productCategories;
        public ProductManagerController(IRepository<Product> productContext, IRepository<ProductCategory> productCategoryContext) 
                                           // creating a context for product repository | this is a constructor , 
                                          // whenever this is called a new context of productrepository is created
        {
            context = productContext;
            productCategories = productCategoryContext;

        }
        public ActionResult Index() //Making the Index page return a list of products by pulling in the list from collections list on repository page

        {
            List<Product> products = context.Collection().ToList();
            return View(products);
        }

        public ActionResult Create()  // this method is to display a page to fill in the product details
        {
            //this returns a product with all the categories

            //Product product = new Product();   //this only returns a product

            ProductManagerViewModel viewModel = new ProductManagerViewModel();

            viewModel.Product = new Product();
            viewModel.ProductCategories = productCategories.Collection();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(Product product,HttpPostedFileBase file) // this method is to post the product details of the newly created product
                                                                            //HttpPostedFileBase file , telling the method to expect a file input
        {
            if (!ModelState.IsValid)  //this is neccessary to check to make sure any validation set on the page is correct else return to product list 
                                      //with the neccessary validation errors 
            {
                return View(product);
            }

            else
            {
                if(file != null) // checking if  a file exists because it is possible to create a product without uploading a file
                {
                    product.Image = product.Id + Path.GetExtension(file.FileName); //if there is a product then set the image property and it is handy to give name as product ID to make the name unique
                                                                                   //and add the file extension name
                                                                                   //added system.IO for Path.GetExtension  where we are passing file's FileName to get the extension of the file
                                                                                   // then save the file
                    file.SaveAs(Server.MapPath("//Content//ProductImages//") + product.Image);


                }
                context.Insert(product);
                context.Commit();

                return RedirectToAction("Index");  // returning to the index after successfully adding a product
            }
        }

        public ActionResult Edit(string Id)
        {
            Product product = context.Find(Id);  //to load the product to be edited from the database

            if (product == null)
            {
                return HttpNotFound();
            }

            else
            {
                ProductManagerViewModel viewModel = new ProductManagerViewModel();
                viewModel.Product = product;
                viewModel.ProductCategories = productCategories.Collection();

                return View(viewModel); //else return the view with the product that we have found
            }
        }

        [HttpPost]
        public ActionResult Edit(Product product, String Id,HttpPostedFileBase file)  //default template requires to send through the updateed product along with the original product ID
                                                              //just in case you want to update the original product ID
        {
            Product producttoupdate = context.Find(Id);

            if (producttoupdate == null)
            {
                return HttpNotFound();
            }

            else
            {
                if (!ModelState.IsValid)  //this is neccessary to check to make sure any validation set on the page is correct else return to product list 
                                          //with the neccessary validation errors 
                {
                    return View(product);
                }

                if (file != null)
                {
                    producttoupdate.Image = product.Id + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("//Content//ProductImages//") + producttoupdate.Image);
                }
                producttoupdate.Category = product.Category;
                producttoupdate.Description = product.Description;
                producttoupdate.Name = product.Name;
                producttoupdate.Price = product.Price;

                context.Commit();

                return RedirectToAction("Index");
            }
        }

        public ActionResult Delete(string Id) // the first action is to load the product from the DB that needs to be deleted
        {
            Product producttodelete = context.Find(Id);

            if (producttodelete == null)
            {
                return HttpNotFound();
            }

            else
            {
                return View(producttodelete);
            }
        }

        [HttpPost]
        [ActionName("Delete")] //Alternative action name of Delete
        public ActionResult ConfirmDelete(string Id)  // the second action is so that the user can confirm before they can actually delete the product
        {
            Product producttodelete = context.Find(Id);
            if (producttodelete == null)
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