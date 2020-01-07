using AzMyeStore.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AzMyeStore.WebUI.Controllers
{
    public class BasketController : Controller
    {
        // GET: Basket
        IBasketService basketService;

        public BasketController(IBasketService Basketservice)
        {
            this.basketService = Basketservice;
        }


        public ActionResult Index()  // using the index page to return the BasketItems list
        {
            var model = basketService.GetBasketItems(this.HttpContext);  
            return View(model);
        }

        public ActionResult AddToBasket(string Id)
        {
            basketService.AddtoBasket(this.HttpContext, Id);
            return RedirectToAction("Index");
        }

        public ActionResult RemoveFromBasket(string Id)
        {
            basketService.RemoveFromBasket(this.HttpContext, Id);
            return RedirectToAction("Index");
        }

        public PartialViewResult BasketSummary()
        {
            var basketsummary = basketService.GetBasketSummary(this.HttpContext);
            return PartialView(basketsummary);
        }

    }
}