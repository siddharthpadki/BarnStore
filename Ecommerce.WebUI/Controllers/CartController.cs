using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ecommerce.Domain.Abstract;
using Ecommerce.Domain.Entities;
using Ecommerce.WebUI.Models;

namespace Ecommerce.WebUI.Controllers
{
    public class CartController : Controller
    {
        private IProductRepository repository;
        private IOrderProcessor orderProcessor;
        // GET: Cart
        public CartController(IProductRepository repo, IOrderProcessor proc)
        {
            repository = repo;
            orderProcessor = proc;
        }

        public ViewResult Index(Cart cart, string returnUrl)
        {
            return View(new CartIndexViewModel { Cart = cart, ReturnUrl = returnUrl });
        }

        public RedirectToRouteResult AddToCart(Cart cart, int productID, string returnUrl, string size, string waistSize)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == productID);
            int? waistSizeInt = null;
            if (!string.IsNullOrEmpty(waistSize))
            {
                waistSizeInt = int.Parse(waistSize);
            }

            if (product != null)
            {
                cart.AddItem(product, 1, size, waistSizeInt);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int productID, string returnUrl)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == productID);

            if(product != null)
            {
                cart.RemoveLine(product);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }

        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }

        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }

            if (ModelState.IsValid)
            {
                orderProcessor.ProcessOrder(cart, shippingDetails);
                cart.Clear();
                return View("Completed");
            }
            else
            {
                return View(shippingDetails);
            }
        }
    }
}