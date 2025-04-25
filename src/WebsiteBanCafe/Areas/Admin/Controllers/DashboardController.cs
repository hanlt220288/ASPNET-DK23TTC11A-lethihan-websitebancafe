using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanCafe.Models;

namespace WebsiteBanCafe.Areas.Admin.Controllers
{
    public class DashboardController : BaseController
    {
        private WebsiteBanCafeDbContext db = new WebsiteBanCafeDbContext();
        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            /*ViewBag.CountOrderSuccess = db.Orders.Where(m => m.Status == 3).Count();
            ViewBag.CountOrderPending = db.Orders.Where(m => m.Status == 2).Count();
            ViewBag.CountOrderCancel = db.Orders.Where(m => m.Status == 1).Count();
           
            ViewBag.Doanhthu = db.Orderdetails.Sum(m => m.Quantity);
            ViewBag.Doanhth = db.Orderdetails.Sum(m => m.Amount);
            ViewBag.CountContactDoneReply = db.Contacts.Where(m => m.Flag == 0).Count();
            ViewBag.CountUser = db.Users.Where(m => m.Status != 0).Count();
            return View();*/

            return RedirectToAction("Index", "Product");
        }
    }
}