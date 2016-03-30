using MoostBrand.DAL;
using MoostBrand.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace MoostBrand.Controllers
{
    public class VendorController : Controller
    {
        private DBContext _db = new DBContext();

        //
        // GET: /Vendor/

        public ActionResult Index(int id = 1)
        {
            var vendors = _db.Vendors.ToList();

            int? page = id;

            int pageSize = 1;
            int pageNumber = (page ?? 1);
            return View(vendors.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /Vendor/Details

        public ActionResult Details(int id = 0)
        {
            var vendor = _db.Vendors.Find(id);
            return View(vendor);
        }

        //
        // GET: /Vendor/Create

        public ActionResult Create()
        {
            var vendor = new Vendor();
            return View(vendor);
        }

        //
        // POST: /Vendor/Create
        [HttpPost]
        public ActionResult Create(Vendor vendor)
        {
            if (ModelState.IsValid)
            {
                _db.Vendors.Add(vendor);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(vendor);
        }

        //
        // GET: /Vendor/Edit/5

        public ActionResult Edit(int id = 0)
        {
            var vendor = _db.Vendors.FirstOrDefault(v => v.ID == id);
            if (vendor == null)
            {
                return HttpNotFound();
            }

            return View(vendor);
        }

        //
        // POST: /Vendor/Edit/5
        [HttpPost]
        public ActionResult Edit(Vendor vendor)
        {
            if (ModelState.IsValid)
            {
                var vndor = _db.Vendors.FirstOrDefault(v => v.ID == vendor.ID);
                if (vendor == null)
                {
                    return HttpNotFound();
                }

                _db.Entry(vndor).CurrentValues.SetValues(vendor);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(vendor);
        }

        //
        // GET: /Vendor/Delete/5

        public ActionResult Delete(int id = 0)
        {
            var vendor = _db.Vendors.Find(id);
            if (vendor == null)
            {
                return HttpNotFound();
            }

            return View(vendor);
        }

        //
        // POST: /Vendor/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var vendor = _db.Vendors.Find(id);
            _db.Vendors.Remove(vendor);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }



        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}
