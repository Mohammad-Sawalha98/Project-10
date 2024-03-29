﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Project_10.Models;


namespace Project_10.Controllers
{

    public class CartsController : Controller
    {
        private Project10Entities db = new Project10Entities();

        // GET: Carts
        public ActionResult Index()
        {
            var carts = db.Carts.Include(c => c.Customer).Include(c => c.Product);
            return View(carts.ToList());
        }

        public ActionResult Index2()
        {
            //var carts = db.Carts.Include(c => c.Customer).Include(c => c.Product);
            //return View(carts.ToList());

            //string email = User.Identity.GetUserName();
            //var productsInCart = db.Carts
            //    .Where(c => c.Customer.CustomerEmail == email && c.IsCheckedOut == false)
            //    .Join(db.Products, c => c.ProductId, p => p.ProductId, (c, p) => p)
            //    ;
            //return View(productsInCart);

            if (User.Identity.IsAuthenticated)
            {

            var Email = User.Identity.GetUserName();
            ViewBag.Id = db.Customers.Where(x => x.CustomerEmail == Email).FirstOrDefault().CustomerId;
            //ViewBag.userId = id;
            var carts = db.Carts.Where(x => x.Customer.CustomerEmail == Email).Include(c => c.Customer).Include(c => c.Product);
            return View(carts.ToList());
            }
            else
            {
                return RedirectToAction("Login", "Account", "");
            }
        }




     
        public ActionResult Buy(int id,string Quantity)
        {

            if(User.Identity.GetUserId()  == null)
                return RedirectToAction("Login" , "Account", "");
            string email = User.Identity.GetUserName();
            int customerId = db.Customers.Where(x=>x.CustomerEmail == email).FirstOrDefault().CustomerId;
            var customerCart = db.Carts.Where(x => x.CustomerId == customerId).ToList();
            bool there = false;
            foreach (var item in customerCart)
            {
                if (item.ProductId == id)
                {
                    there = true;
                }
            }


            Customer customer = (Customer) db.Customers.SingleOrDefault(c => c.CustomerEmail == email);
            Product product = db.Products.Find(id);
            int? price = Convert.ToInt32(product.price);

            if (there)
            {

                var updatedQuantity = customerCart.Where(x => x.ProductId == id).FirstOrDefault();
                updatedQuantity.ProductId = id;
                updatedQuantity.CustomerId = customerId;
                updatedQuantity.Quantity += int.Parse(Quantity);
                updatedQuantity.TotalPrice += price;
                
            }
            else if (!there)
            {
                var Cart = new Cart()
                {
                    CustomerId = customer.CustomerId,
                    IsCheckedOut = false,
                    ProductId = id,
                    Quantity = int.Parse(Quantity),
                    TotalPrice = price ,

                };
                db.Carts.Add(Cart);
            }
            
            db.SaveChanges();
          
            return RedirectToAction("Index2");
        }


        public ActionResult CheckOut(int id)
        {
            var cart = db.Carts.Where(x => x.CustomerId == id).ToList();
            int totalPrice = 0;
            foreach (var item in cart)
            {
                totalPrice += Convert.ToInt32(item.TotalPrice) * Convert.ToInt32(item.Quantity);
            }
            ViewBag.totalPrice = totalPrice;
            return View("CheckOut",cart);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CheckOut(string Address_one, string Address_two,string email,string phoneNumber,string FirstName,string LastName,string City,string Messege ,string Payment_Method , int? Quantity , int? OrderId , int? ProductId)
        {
            Order newOrder = new Order();
            if (ModelState.IsValid)
            {

                newOrder.FirstName = FirstName;
                newOrder.LastName = LastName;
                newOrder.City = City;
                newOrder.Messege = Messege;
                newOrder.email = email;
                newOrder.phoneNumber = Convert.ToInt32(phoneNumber);
                newOrder.Address_one = Address_one;
                newOrder.Address_two = Address_two;
                newOrder.Payment_Method = Payment_Method;
                db.Orders.Add(newOrder);
                 db.SaveChanges();


            }
           

            Product product = new Product();

            var userID = User.Identity.GetUserId();
            var user = db.AspNetUsers.Find(userID);
            var cart = db.Carts.Where(x => x.Customer.CustomerEmail == user.Email).ToList();




            var orderDetailOrder = db.Orders.Where(x => x.AspNetUser.Email == email).OrderByDescending(x => x.OrderId).FirstOrDefault();
            int totalAmount1 = 0;

            foreach (var item in cart)
            {
                Order_Details order_Details = new Order_Details();
                order_Details.OrderId = newOrder.OrderId;
                order_Details.ProductId = item.ProductId;
                order_Details.Quantity = item.Quantity;
                db.Order_Details.Add(order_Details);
                await db.SaveChangesAsync();





                totalAmount1 += Convert.ToInt32(item.TotalPrice) * Convert.ToInt32(item.Quantity);

                db.Carts.Remove(item);
                

            }

            newOrder.totalAmount = totalAmount1;
            //newOrder.OrderPrice = Convert.ToInt32(Quantity * totalAmount1);
            db.Entry(newOrder).State = EntityState.Modified;
            db.SaveChanges();




            await db.SaveChangesAsync();

            return RedirectToAction("Index", "Home");

        }







        // GET: Carts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // GET: Carts/Create
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CustomerName");
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName");
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CartId,CustomerId,ProductId,IsCheckedOut,TotalPrice,Quantity")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                db.Carts.Add(cart);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CustomerName", cart.CustomerId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", cart.ProductId);
            return View(cart);
        }

        // GET: Carts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CustomerName", cart.CustomerId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", cart.ProductId);
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CartId,CustomerId,ProductId,IsCheckedOut,TotalPrice,Quantity")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CustomerName", cart.CustomerId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", cart.ProductId);
            return View(cart);
        }

        // GET: Carts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cart cart = db.Carts.Find(id);
            db.Carts.Remove(cart);
            db.SaveChanges();
            return RedirectToAction("Index2");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
