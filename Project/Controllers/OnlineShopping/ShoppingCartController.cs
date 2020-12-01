using Microsoft.AspNet.Identity;
using Project.Models;
using Project.Models.OnlineShopping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Controllers.OnlineShopping
{
    public class ShoppingCartController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: ShoppingCart
        public ActionResult Index()
        {

            return RedirectToAction("Index", "Products");
        }
        public ActionResult CheckoutDetails()
        {
            return View();
        }
       
        public ActionResult DecreaseQty(int productId)
        {
            Product product = db.Products.Find(productId);
            if (Session["cart"] != null)
            {
                List<CartItem> cartItems = (List<CartItem>)Session["cart"];
                foreach (var item in cartItems)
                {
                    if (item.Product.ProductID == productId)
                    {
                        int prevQty = item.Quantity;
                        if (prevQty > 0)
                        {
                            cartItems.Remove(item);
                            cartItems.Add(new CartItem(db.Products.Find(productId), prevQty - 1));
                        }
                        break;
                    }
                }
                Session["cart"] = cartItems;

                ViewBag.ListCart = cartItems.Count();
                Session["count"] = ViewBag.ListCart;
            }
            product.Quantity++;
            db.Entry(product).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Redirect("CheckoutDetails");
        }

        public ActionResult AddToCart(int productId, string url)
        {
            Product product = db.Products.Find(productId);
            if (Session["cart"] == null)
            {
                List<CartItem> cartItems = new List<CartItem>
                {
                    new CartItem(db.Products.Find(productId),1)
                };
                Session["cart"] = cartItems;

                ViewBag.ListCart = cartItems.Count();
                Session["count"] = ViewBag.ListCart;
            }
            else
            {
                List<CartItem> cartItems = (List<CartItem>)Session["cart"];
                int check = isExistingCheck(productId);

                if (check == -1)
                {
                    cartItems.Add(new CartItem(db.Products.Find(productId), 1));
                }
                else
                {
                    cartItems[check].Quantity++;
                }
                Session["cart"] = cartItems;

                ViewBag.ListCart = cartItems.Count();
                Session["count"] = ViewBag.ListCart;
            }
            product.Quantity--;
            db.Entry(product).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction(url);
        }
        private int isExistingCheck(int? productId)
        {

            List<CartItem> cartItems = (List<CartItem>)Session["cart"];
            for (int i = 0; i < cartItems.Count; i++)
            {
                if (cartItems[i].Product.ProductID == productId) return i;


            }
            return -1;
        }
        public ActionResult RemoveFromCart(int productId)
        {
            Product product = db.Products.Find(productId);
            List<CartItem> cartItems = (List<CartItem>)Session["cart"];
            foreach (var item in cartItems)
            {
                if (item.Product.ProductID == productId)
                {
                    cartItems.Remove(item);
                    break;
                }
            }
            Session["cart"] = cartItems;

            ViewBag.ListCart = cartItems.Count();
            Session["count"] = ViewBag.ListCart;
            product.Quantity++;
            db.Entry(product).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("CheckoutDetails");
        }
    }
}