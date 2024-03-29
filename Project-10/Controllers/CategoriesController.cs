﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Project_10.Models;

namespace Project_10.Controllers
{
    public class CategoriesController : Controller
    {
        private Project10Entities db = new Project10Entities();

        // GET: Categories
        //public ActionResult Index(string Search, string CategoryName)
        //{
        //    var categories = db.Categories.Include(c => c.CategoryName);


        //    if (CategoryName == "CategoryName")
        //    {
        //        categories = categories.Where(x => x.CategoryName.Contains(Search));
        //    }

        //    return View(db.Categories.ToList());
        //}


        public ActionResult Index(string Search, string CategoryName)
        {
            var categories = db.Categories.AsQueryable();

            if (CategoryName == "CategoryName")
            {
                categories = categories.Where(x => x.CategoryName.Contains(Search));
            }

            return View(categories.ToList());
        }


        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryId,CategoryName,CategoryDescription,CategoryImage")] Category category, HttpPostedFileBase CategoryImage)
        {
            if (ModelState.IsValid)
            {
                if (CategoryImage != null && CategoryImage.ContentLength > 0)
                {
                    string path = "../Images/" + CategoryImage.FileName;
                    CategoryImage.SaveAs(Server.MapPath(path));
                    category.CategoryImage = CategoryImage.FileName;
                }
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            Session["categoryImage"]= category.CategoryImage;
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryId,CategoryName,CategoryDescription,CategoryImage")] Category category, HttpPostedFileBase CategoryImage)
        {
           

            if (ModelState.IsValid)
            {
              

                if (CategoryImage != null)
                {

                    string pathpic = Path.GetFileName(CategoryImage.FileName);
                    string path = Path.Combine(Server.MapPath("~/Images/"), pathpic);
                    CategoryImage.SaveAs(path);
                    category.CategoryImage = pathpic;

                }
                else
                {
                    category.CategoryImage = Session["categoryImage"].ToString();
                }



                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", category.CategoryId);

            return View(category);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
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
