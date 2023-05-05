using Project_10.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_10.Controllers
{

    public class StatisticsController : Controller

    {
        private Project10Entities db = new Project10Entities();

        // GET: Statistics
        public ActionResult Index()
        {
            return View();
        }

       




        public ActionResult Index2()
        {

            int customers = db.Customers.Count();
            int orders = db.Orders.Count();
            int products = db.Products.Count();
            int subscribers = db.Subscribers.Count();
            int reviews = db.Reviews.Count();

           

            var sales = db.Orders.ToList();
            int totalPrice = 0;
            foreach (var item in sales)
            {
                totalPrice +=Convert.ToInt32( item.totalAmount);
            }
            ViewBag.totalPrice = totalPrice;

            var Profits = totalPrice * 0.15;
            ViewBag.profits = Profits;

            //int aspuser = db.AspNetUsers.Count();
            ////// Get all devices from the database
            //var dev = db.Devices.ToList();

            //int x = 0;

            //// Loop through each device
            //foreach (var dev in devices)
            //{
            //    // Count the accepted appointments for the current device

            //}
            //x = db.Appointments.Count(a => a.IsAccepted == true);
            //int i=dev.

            //int specificDeviceID = 13;
            //int deviceCount = db.Appointments.Count(a => a.IsAccepted == true);
            //int deviceCount0 = Convert.ToInt32(x * 0.1);

            ViewBag.customers = customers;
            ViewBag.orders = orders;
            ViewBag.products = products;
            ViewBag.subscribers = subscribers;
            ViewBag.reviews = reviews;
            //ViewBag.aspuser = aspuser;
            //ViewBag.deviceCount = deviceCount0;




            return View();
        }




    }
}