using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoChart.Controllers
{
    public class HomeController : Controller
    {
        CAFEEntities1 context = new CAFEEntities1();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetData()
        {

                var userData = context.UserDetails.Where(x => x.UserID != "" && x.isActive == true).ToList();
                int uCount = userData.Count();
                Object obj = uCount;

            return Json(obj, JsonRequestBehavior.AllowGet);
        }
    }
}    
