using Project_10.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_10.Controllers
{
    public class HomeController : Controller
    {
        Project10Entities db = new Project10Entities(); 
        public ActionResult Index()
        {

            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string Name, string Email, string Comment , int ? flag)
        {
            Review NewReview = new Review();
            NewReview.Name = Name;
            NewReview.Email = Email;
            NewReview.Comment = Comment;
            NewReview.flag = (flag != null) ? (flag == 1) : false;
            db.Reviews.Add(NewReview);
            db.SaveChanges();
            return View();
        }


        //public ActionResult Subscribe()
        //{
        //    return View();
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Subscribe (string Email)
        {
            Subscriber NewSubscriber = new Subscriber();
            NewSubscriber.Email = Email;
            db.Subscribers.Add(NewSubscriber);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");


        }


        public PartialViewResult _Index2()
        {

            return PartialView(db.Categories.ToList());
        }

        public PartialViewResult _Index3()
        {

            return PartialView(db.Occasions.ToList());
        }

        public PartialViewResult _Testimonial()
        {
            var trueReviews = db.Reviews.Where(x => x.flag == true).Count();
            ViewBag.trueReviews = trueReviews;

            return PartialView(db.Reviews.ToList());
        }



        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Review(string Name , string Email , string Comment)
        //{
        //    Review NewReview = new Review();
        //    NewReview.Name = Name;
        //    NewReview.Email = Email;
        //    NewReview.Comment = Comment;
        //    db.Reviews.Add(NewReview);
        //    db.SaveChanges();
        //    return View();
        //}
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}