using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Project_10.Models;

namespace Project_10.Controllers
{
    public class OrdersController : Controller
    {
        private Project10Entities db = new Project10Entities();

        // GET: Orders
        public ActionResult Index(string Search, string FirstName, string city)
        {
            var orders = db.Orders.Include(o => o.AspNetUser);

            if (FirstName == "FirstName")
            {
                orders = orders.Where(x => x.FirstName.Contains(Search));
            }
            else if (city == "city")
            {
                orders = orders.Where(x => x.City.Contains(Search));
            }

            return View(orders.ToList());
        }

        public ActionResult Index2()
        {
            string email = User.Identity.GetUserName();
            //int customerId = db.Customers.Where(x => x.CustomerEmail == email).FirstOrDefault().CustomerId;
            var customerEmail = db.Customers.Where(x => x.CustomerEmail == email).FirstOrDefault();
            var orders = db.Orders.Where(x => x.email == email);

            return View(orders.ToList());
        }


        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.User_id = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderId,OrderPrice,isCheckout,Id,totalAmount,orderDate,Address_one,Address_two,email,phoneNumber,FirstName,LastName,Payment_Method,City,User_id,Messege")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.User_id = new SelectList(db.AspNetUsers, "Id", "Email", order.User_id);
            return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.User_id = new SelectList(db.AspNetUsers, "Id", "Email", order.User_id);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderId,OrderPrice,isCheckout,Id,totalAmount,orderDate,Address_one,Address_two,email,phoneNumber,FirstName,LastName,Payment_Method,City,User_id,Messege")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.User_id = new SelectList(db.AspNetUsers, "Id", "Email", order.User_id);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
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
