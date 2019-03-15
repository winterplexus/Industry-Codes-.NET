//
//  HomeController.cs
//
//  Copyright (c) Wiregrass Code Technology 2014-2019
//
using System.Web.Mvc;

namespace IndustryCodes
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}