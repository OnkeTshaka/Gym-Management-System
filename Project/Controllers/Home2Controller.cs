using Project.Excel;
using Project.Models;
//using Project.Models.AdminModels;
using Project.Models.Supplier;
////using ReturnGym.Models.AdminModels.AdminProduct;
//using ReturnGym.Models.OnlineShopping;
using Project.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Project.Controllers
{
    public class Home2Controller : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index(string searching)
        {
            HomeIndexViewModel LBDV = new HomeIndexViewModel
            {
                supplierProducts = db.supplierProducts.OrderByDescending(c => c.ProductID).Where(m => m.ProductName.Contains(searching) || m.supplierCategory.CategoryName.Contains(searching) || searching == null).ToList()
            };
            return View(LBDV);
        }

        public ActionResult Checkout(supplierProduct order)
        {
            //List<myCart> cartItems = (List<myCart>)Session["cart"];
            //if (ModelState.IsValid)
            //{
            //    Random randm = new Random();
            //    string upr = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            //    string downr = "abcdefghijklmnopqrstuvwxyz";
            //    string digir = "1234567890";
            //    char[] tno = new char[8];
            //    int r1 = randm.Next(0, 25);
            //    int r2 = randm.Next(0, 25);
            //    int r3 = randm.Next(0, 9);
            //    tno[0] = upr[r1];
            //    tno[1] = downr[r2];
            //    tno[2] = digir[r3];
            //    r1 = randm.Next(0, 25);
            //    r2 = randm.Next(0, 25);
            //    r3 = randm.Next(0, 9);
            //    tno[3] = upr[r2];
            //    tno[4] = downr[r1];
            //    tno[5] = digir[r3];
            //    string t_no = new string(tno);
            //    //order.ProductID = t_no;
            //    order.CreatedDate = DateTime.Now;


            //    db.supplierProducts.Add(order);
            //    db.SaveChanges();

            //    foreach (myCart cart in cartItems)
            //    {
            //        supplierProduct orderDetail = new supplierProduct()
            //        {
            //            CategoryID = order.CategoryID,
            //            ProductID = cart.supplierProducts.ProductID,
            //            Quantity = cart.supplierProducts.ProductID,
            //            //Price = cart.Product.Price
            //        };
            //        db.supplierProducts.Add(orderDetail);
            //        db.SaveChanges();
            //    }
            //    MailMessage nn = new MailMessage();
            //    nn.To.Add(order.CustomerEmail);
            //    nn.From = new MailAddress("thefirewalls8@gmail.com", "Return Of Firewalls Team");
            //    nn.Subject = "Order successful ";
            //    nn.Body = "<div style='background-color:#111;color:#fff;'>Invoice</div>" + "<br/>" +


            //        " <b>Booking for :</b>  " + "<b>" + order.CustomerName + "<br/>"
            //        + "<b>" + "Reference Code:" + "</b> " + order.ProductID + "<br/>" +
            //        order.CreatedDate + "<br/>";
            //    nn.IsBodyHtml = true;

            //    SmtpClient smtp = new SmtpClient();
            //    smtp.Host = "smtp.gmail.com";
            //    smtp.Port = 587;
            //    smtp.EnableSsl = true;

            //    NetworkCredential nc = new NetworkCredential("thefirewalls8@gmail.com", "Dut@1234");
            //    smtp.UseDefaultCredentials = true;
            //    smtp.Credentials = nc;
            //    smtp.Send(nn);
            //    TempData["message"] = "Email sent";

            //    Session.Remove("cart");
            //    return RedirectToAction("Details", "Orders", new { id = order.ProductID });
            //}

            return View();
        }
         
        public ActionResult CheckoutDetails()
        {
            return View();
        }
        //public void Excel()
        public void Excel()
        {

            OrderExcel excel = new OrderExcel();
            Response.ClearContent();
            Response.BinaryWrite(excel.GenerateExcel(GetOrders()));
            Response.AddHeader("content-disposition", "attachment; filename=FitnessCompanyOrder.xlsx");
            Response.ContentType = "application/vnd.openxmlformarts-officedocument.spreadsheet";
            Response.Flush();
            Response.End();
            //return RedirectToAction("Index");
        }

        public List<supplierProduct> GetOrders()
        {
            return db.supplierProducts.ToList();
        }


        public ActionResult DecreaseQty(int productId)
        {
            if (Session["cart1"] != null)
            {
                List<Item> cart = (List<Item>)Session["cart1"];
                var product = db.supplierProducts.Find(productId);
                foreach (var item in cart)
                {
                    if (item.supplierProduct.ProductID == productId)
                    {
                        int prevQty = item.Quantity;
                        if (prevQty > 0)
                        {
                            cart.Remove(item);
                            cart.Add(new Item()
                            {
                                supplierProduct = product,
                                Quantity = prevQty - 1
                            });
                        }
                        break;
                    }
                }
                Session["cart1"] = cart;
            }
            return Redirect("Checkout");
        }
        public ActionResult AddToCart(int productId, string url)
        {
            if (Session["cart1"] == null)
            {
                List<Item> cart = new List<Item>();
                var product = db.supplierProducts.Find(productId);
                cart.Add(new Item()
                {
                    supplierProduct = product,
                    Quantity = 1
                });
                Session["cart1"] = cart;
            }
            else
            {
                List<Item> cart = (List<Item>)Session["cart1"];
                var count = cart.Count();
                var product = db.supplierProducts.Find(productId);
                for (int i = 0; i < count; i++)
                {
                    if (cart[i].supplierProduct.ProductID == productId)
                    {
                        int prevQty = cart[i].Quantity;
                        cart.Remove(cart[i]);
                        cart.Add(new Item()
                        {
                            supplierProduct = product,
                            Quantity = prevQty + 1
                        });
                        break;
                    }
                    else
                    {
                        var prd = cart.Where(x => x.supplierProduct.ProductID == productId).SingleOrDefault();
                        if (prd == null)
                        {
                            cart.Add(new Item()
                            {
                                supplierProduct = product,
                                Quantity = 1
                            });
                        }
                    }
                }
                Session["cart1"] = cart;
            }
            return Redirect(url);
        }
        public ActionResult RemoveFromCart(int productId)
        {
            List<Item> cart = (List<Item>)Session["cart1"];
            foreach (var item in cart)
            {
                if (item.supplierProduct.ProductID == productId)
                {
                    cart.Remove(item);
                    break;
                }
            }
            Session["cart1"] = cart;
            return Redirect("Checkout");
        }
    }
}