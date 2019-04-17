using E_Auction.BLL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_AuctionDemo_UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly AuctionManagementService service;
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            var auctionInfo = service.GetAuctionInfo();
            return View(auctionInfo);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public HomeController()
        {
            service = new AuctionManagementService();
        }
    }
}