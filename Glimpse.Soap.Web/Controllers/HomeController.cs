using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Glimpse.Soap.Web;

namespace Glimpse.Soap.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (var service = new localhost.Service1())
            {
                //System.Web.Services.Protocols.SoapHttpClientProtocol
                ViewBag.Message = service.HelloWorld();
                ViewBag.Test = service.Test("input1", "input2");
            }

            

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
