using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Project_10.Models;

namespace Project_10.Controllers
{
    public class ProductsController : Controller
    {
        private Project10Entities db = new Project10Entities();

        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Category).Include(p => p.Occasion);
            return View(products.ToList());
        }

        public ActionResult Index2(int ? id)
        {
            var products = db.Products.Include(p => p.Category).Include(p => p.Occasion).Where(x => x.CategoryId==id);
            return View(products.ToList());
        }

        public ActionResult Index3(int? id)
        {
            var products = db.Products.Include(p => p.Category).Include(p => p.Occasion).Where(x => x.OccasionId == id);
            return View(products.ToList());
        }

        public ActionResult Index4(int? id)
        {
            var products = db.Products.Include(p => p.Category).Include(p => p.Occasion).Where(x => x.ProductId == id);
            return View(products.ToList());


            //ProductModel productModel = new ProductModel();
            //ViewBag.products = productModel.findAll();
            //return View();
        }




        //public ActionResult Cart(int? id)
        //{
        //    var userEmail = User.Identity.GetUserName();
        //    ViewBag.UserEmail = userEmail;
        //    ViewBag.customerId = db.Customers.Where(x=>x.CustomerEmail == userEmail).FirstOrDefault().CustomerId;
        //    var products = db.Carts.Where(x => x.Customer.CustomerEmail == userEmail).ToList();
        //    return View(products);
        //}






        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName");
            ViewBag.OccasionId = new SelectList(db.Occasions, "OccasionId", "OccasionName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductId,ProductName,ProductDescription,ProductImage,price,Quantity,Gender,CategoryId,OccasionId")] Product product, HttpPostedFileBase ProductImage)
        {
            if (ModelState.IsValid)
            {
                if (ProductImage != null && ProductImage.ContentLength > 0)
                {
                    string path = "../Images/" + ProductImage.FileName;
                    ProductImage.SaveAs(Server.MapPath(path));
                    product.ProductImage = ProductImage.FileName;
                }
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewBag.OccasionId = new SelectList(db.Occasions, "OccasionId", "OccasionName", product.OccasionId);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewBag.OccasionId = new SelectList(db.Occasions, "OccasionId", "OccasionName", product.OccasionId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductId,ProductName,ProductDescription,ProductImage,price,Quantity,Gender,CategoryId,OccasionId")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewBag.OccasionId = new SelectList(db.Occasions, "OccasionId", "OccasionName", product.OccasionId);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
