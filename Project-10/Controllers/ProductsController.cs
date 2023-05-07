using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Project_10.Models;

namespace Project_10.Controllers
{
    public class ProductsController : Controller
    {
        private Project10Entities db = new Project10Entities();

        // GET: Products
        [Authorize(Roles = "Admin")]

        public ActionResult Index(string Search, string ProductName, string ProductPrice , string Occasion)
        {
            decimal searchPrice;
            var products = db.Products.Include(p => p.Category).Include(p => p.Occasion);

            if (ProductName == "ProductName")
            {
                products = products.Where(x => x.ProductName.Contains(Search));
            }


            
           else if (decimal.TryParse(Search, out searchPrice) && ProductPrice == "ProductPrice")
            {
                products = products.Where(x => x.price >= searchPrice && x.price <= (searchPrice + 10));
            }

            else if (Occasion == "Occasion")
            {
                products = products.Where(x => x.Occasion.OccasionName.Contains(Search));
            }

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

        }



        // GET: Products/Details/5
        [Authorize(Roles = "Admin")]

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
        [Authorize(Roles = "Admin")]

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
        [Authorize(Roles = "Admin")]

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
        [Authorize(Roles = "Admin")]

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            Session["ProductImage"] = product.ProductImage;

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
        [Authorize(Roles = "Admin")]

        public ActionResult Edit([Bind(Include = "ProductId,ProductName,ProductDescription,ProductImage,price,Quantity,Gender,CategoryId,OccasionId")] Product product , HttpPostedFileBase ProductImage)
        {
            if (ModelState.IsValid)
            {

                if (ProductImage != null)
                {

                    string pathpic = Path.GetFileName(ProductImage.FileName);
                    string path = Path.Combine(Server.MapPath("~/Images/"), pathpic);
                    ProductImage.SaveAs(path);
                    product.ProductImage = pathpic;

                }
                else
                {
                    product.ProductImage = Session["ProductImage"].ToString();
                }


                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewBag.OccasionId = new SelectList(db.Occasions, "OccasionId", "OccasionName", product.OccasionId);
            return View(product);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Admin")]

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
        [Authorize(Roles = "Admin")]

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
