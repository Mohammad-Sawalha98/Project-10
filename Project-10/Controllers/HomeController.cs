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

        public PartialViewResult _Index2()
        {

            return PartialView(db.Categories.ToList());
        }

        public PartialViewResult _Index3()
        {

            return PartialView(db.Occasions.ToList());
        }

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