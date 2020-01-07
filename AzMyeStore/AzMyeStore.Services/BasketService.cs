using AzMyeStore.Core.Contracts;
using AzMyeStore.Core.Models;
using AzMyeStore.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AzMyeStore.Services
{
    public class BasketService : IBasketService
    {
        // the workflow that is being accomplished here : read the cookie from the user and with in that cookie 
        // look for the basket ID and if it found load the basket, if it is not found return an empty inmemory basket
        //in order to read cookies we use something called httpcontext, this is basically the method of connecting user 
        //to the server and it contains a lot of information clients automatically send when you connect to a web server
        // such as IP address and one other thing that is also included is a list of cookies.Because AzMyestore.Services 
        //has no clue what httpcontext is we need to add the relevant libraries in Reference libraries for services project.
        //System.Web.
        //now our basket service needs access to underlying data and for this we need to access IRepository

        IRepository<Product> productContext;
        IRepository<Basket> basketContext;
        public const string BasketsessionName = "eCommerceBasket";
        //this const string cannot be updated elsewhere in our code
        //this is not neccessarily required however when we are reading and writing a cookie we use a string to identify a 
        // a particular cookie to enforce consistency whenever we read and write to/from a cookie
        public BasketService(IRepository<Product> ProductContext,IRepository<Basket> BasketContext)
        {
            this.productContext = ProductContext;
            this.basketContext = BasketContext;
        }

        //now we can go ahead and write our methods, first one would be to load the user basket ,
        // read the users cookie from the httpcontext looking for the basket ID and look for that 
        //basket ID in the database
        
        private Basket GetBasket(HttpContextBase httpContext,bool createifNull)
        //this  is an internal method which we will use from various places within our service
        // because we want have access to user's httpcontext we will force the consuming service 
        //to send this httpContext as part of the call and we use the boolean variable because sometimes
        // we would want to create the basket if none exists and sometimes we dont.
        {
            HttpCookie cookie = httpContext.Request.Cookies.Get(BasketsessionName);
            //finding and reading a cookie

            Basket basket = new Basket();

            if(cookie!=null)
            {
                string basketId = cookie.Value;
                if (!String.IsNullOrEmpty(basketId))
                {
                    basket = basketContext.Find(basketId);
                }
                
                else
                {
                    if(createifNull)
                    {
                        basket = CreateNewBasket(httpContext);
                    }

                }
            }

            else
            {
                if (createifNull)
                {
                    basket = CreateNewBasket(httpContext);
                }
            }

            return basket;
        }
        
        private Basket CreateNewBasket(HttpContextBase httpContext)
            // this method is also private since we are only going to use internally with in the basket service class
        {
            Basket basket = new Basket();
            basketContext.Insert(basket);
            basketContext.Commit();

            HttpCookie cookie = new HttpCookie(BasketsessionName);
            cookie.Value = basket.Id;
            cookie.Expires = DateTime.Now.AddDays(7);
            httpContext.Response.Cookies.Add(cookie);
            //finally we have to assign the cookie to the httpcontext response, because it has to be sent back to the user.

            return basket;
        }
       
        public void AddtoBasket(HttpContextBase httpContext,string productId)
        {
            Basket basket = GetBasket(httpContext, true);
            BasketItem item = basket.BasketItems.FirstOrDefault(i => i.ProductId == productId);

            if (item == null)
            {
                item = new BasketItem()
                {
                    BasketId = basket.Id,
                    ProductId = productId,
                    Quantity = 1

                };
                basket.BasketItems.Add(item);
            }

            else
            {
                item.Quantity = item.Quantity + 1;
            }

            basketContext.Commit();
        }

        public void RemoveFromBasket(HttpContextBase httpContext, string itemId) // when we are removing 
            //sending the basketitemid rather than the productId like in adding to the basket
            //cannot use the basketitemid when adding because the basketitemid does not exist before
            //the product is added to the basket as a basket item
        {
            Basket basket = GetBasket(httpContext, true);
            BasketItem item = basket.BasketItems.FirstOrDefault(i => i.Id == itemId);

            if(item!= null)
            {
                basket.BasketItems.Remove(item);
                basketContext.Commit();
            }

            
        }

        public List<BasketItemViewModel> GetBasketItems(HttpContextBase httpContext)
        {
            Basket basket = GetBasket(httpContext, false);

            if(basket != null)
            {
                var results = (from b in basket.BasketItems
                              join p in productContext.Collection()
                              on b.ProductId equals p.Id
                              select new BasketItemViewModel()
                              {
                                  Id = b.Id,
                                  Quantity = b.Quantity,
                                  Price = p.Price,
                                  Image = p.Image,
                                  ProductName = p.Name
                              }
                               ).ToList();
                return results;
            }

            else
            {
                return new List<BasketItemViewModel>();
            }
        }

        //this above method provides a list of all the items in the basket which is good when a user clicks to look at the basket page
        //however we need to create another method which will provide a BasketSummary i.e the total list of all the items in the basket
        // and total quantity in the basket for that we need another view model, BasketSummaryViewModel which will have to be created in view models

        public BasketSummaryViewModel GetBasketSummary(HttpContextBase httpContext)
        {
            Basket basket = GetBasket(httpContext, false);
            BasketSummaryViewModel model = new BasketSummaryViewModel(0,0);   //instantiated here because we have a constructor in the BasketSummaryViewModel definition

            if (basket != null)
            {
                int? basketCount = (from item in basket.BasketItems select item.Quantity).Sum();
                decimal? basketTotal = (from item in basket.BasketItems join p in productContext.Collection()
                                       on item.ProductId equals p.Id select item.Quantity*p.Price).Sum();

                model.BasketCount = basketCount ?? 0;
                model.BasketTotal = basketTotal ?? decimal.Zero;

                return model;

            }
            else
            {
                return model;
            }
        }
    }
}