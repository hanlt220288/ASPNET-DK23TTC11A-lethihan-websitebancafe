﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebsiteBanCafe.Models;

namespace WebsiteBanCafe.Areas.Admin.Controllers
{
    public class OrderController : BaseController
    {
        private WebsiteBanCafeDbContext db = new WebsiteBanCafeDbContext();

        public ActionResult Index()
        {
            ViewBag.countTrash = db.Orders.Where(m => m.Trash == 1).Count();
            var results = (from od in db.Orderdetails
                           join o in db.Orders on od.OrderID equals o.ID
                           where o.Trash != 1

                           group od by new { od.OrderID, o } into groupb
                           orderby groupb.Key.o.CreateDate descending
                           select new ListOrder
                           {
                               ID = groupb.Key.OrderID,
                               SAmount = groupb.Sum(m => m.Amount),
                               CustomerName = groupb.Key.o.DeliveryName,
                               Status = groupb.Key.o.Status,
                               CreateDate = groupb.Key.o.CreateDate,
                               ExportDate = groupb.Key.o.ExportDate,


                           });

            return View(results.ToList());
        }
        public ActionResult Trash()
        {
            ViewBag.countTrash = db.Orders.Where(m => m.Status == 0).Count();
            var results = (from od in db.Orderdetails
                           join o in db.Orders on od.OrderID equals o.ID
                           where o.Trash == 1

                           group od by new { od.OrderID, o } into groupb
                           orderby groupb.Key.o.CreateDate descending
                           select new ListOrder
                           {
                               ID = groupb.Key.OrderID,
                               SAmount = groupb.Sum(m => m.Amount),
                               CustomerName = groupb.Key.o.DeliveryName,
                               Status = groupb.Key.o.Status,
                               CreateDate = groupb.Key.o.CreateDate,
                               ExportDate = groupb.Key.o.ExportDate,


                           });

            return View(results.ToList());
        }

        public ActionResult DelTrash(int? id)
        {
            MOrder mOrder = db.Orders.Find(id);
            mOrder.Trash = 1;

            mOrder.Updated_at = DateTime.Now;
            mOrder.Updated_by = 1;
            db.Entry(mOrder).State = EntityState.Modified;
            db.SaveChanges();
            Notification.set_flash("Xóa đơn hàng thành công!", "success");
            return RedirectToAction("Index");
        }
        public ActionResult Undo(int? id)
        {
            MOrder mOrder = db.Orders.Find(id);
            mOrder.Trash = 0;

            mOrder.Updated_at = DateTime.Now;
            mOrder.Updated_by = int.Parse(Session["Admin_ID"].ToString());
            db.Entry(mOrder).State = EntityState.Modified;
            db.SaveChanges();
            Notification.set_flash("Khôi phục thành công!", "success");
            return RedirectToAction("Trash");
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                Notification.set_flash("Không tồn tại đơn hàng!", "warning");
                return RedirectToAction("Index", "Order");
            }
            MOrder mOrder = db.Orders.Find(id);
            if (mOrder == null)
            {
                Notification.set_flash("Không tồn tại  đơn hàng!", "warning");
                return RedirectToAction("Index", "Order");
            }
            ViewBag.orderDetails = db.Orderdetails.Where(m => m.OrderID == id).ToList();
            ViewBag.productOrder = db.Products.ToList();
            return View(mOrder);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                Notification.set_flash("Không tồn tại đơn hàng!", "warning");
                return RedirectToAction("Trash", "Order");
            }
            MOrder mOrder = db.Orders.Find(id);
            if (mOrder == null)
            {
                Notification.set_flash("Không tồn tại đơn hàng!", "warning");
                return RedirectToAction("Trash", "Order");
            }
            ViewBag.orderDetails = db.Orderdetails.Where(m => m.OrderID == id).ToList();
            ViewBag.productOrder = db.Products.ToList();
            return View(mOrder);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MOrder mOrder = db.Orders.Find(id);
            db.Orders.Remove(mOrder);
            db.SaveChanges();
            Notification.set_flash("Đã xóa đơn hàng!", "success");
            return RedirectToAction("Trash");
        }
        [HttpPost]
        public JsonResult changeStatus(int id, int op)
        {
            MOrder mOrder = db.Orders.Find(id);
            if (op == 1) { mOrder.Status = 1; } else if (op == 2) { mOrder.Status = 2; } else { mOrder.Status = 3; }

            mOrder.ExportDate = DateTime.Now;
            mOrder.Updated_at = DateTime.Now;
            mOrder.Updated_by = int.Parse(Session["Admin_ID"].ToString());
            db.Entry(mOrder).State = EntityState.Modified;
            db.SaveChanges();
            return Json(new { s = mOrder.Status, t = mOrder.ExportDate.ToString() });
        }


    }
}
