using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AirlineManagementWebApi.Controllers
{
    public class PageController : Controller
    {
        // GET: Page
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetDepartureFlights()
        {
            return new FilePathResult("~/Views/Page/departures.html", "text/html");
        }
        public ActionResult GetLandingFlights()
        {
            return new FilePathResult("~/Views/Page/landing.html", "text/html");
        }
        public ActionResult SearchFlights()
        {
            return new FilePathResult("~/Views/Page/search.html", "text/html");
        }
    }
}