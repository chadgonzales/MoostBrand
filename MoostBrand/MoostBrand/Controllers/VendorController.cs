using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MoostBrand.DAL;
using System.Data.Entity;
using PagedList;
using System.Configuration;
using MoostBrand.Models;

namespace MoostBrand.Controllers
{
    public class VendorController : Controller
    {
        MoostBrandEntities entity = new MoostBrandEntities();


        [AccessChecker(Action = 1, ModuleID = 17)]
        // GET: Vendor
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "code" : "";
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;


            var vendors = from v in entity.Vendors
                            select v;

            if (!String.IsNullOrEmpty(searchString))
            {
                vendors = vendors.Where(v => v.Code.Contains(searchString)
                                       || v.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "code":
                    vendors = vendors.OrderByDescending(v => v.Code);
                    break;
                case "name":
                    vendors = vendors.OrderByDescending(v => v.Name);
                    break;
                default:
                    vendors = vendors.OrderBy(v => v.ID);
                    break;
            }
            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);
            return View(vendors.ToPagedList(pageNumber, pageSize));
        }

        [AccessChecker(Action = 1, ModuleID = 17)]
        // GET: Vendor/Details/5
        public ActionResult Details(int id)
        {
            var vendor = entity.Vendors.Find(id);
            return View(vendor);
        }


        [AccessChecker(Action = 2, ModuleID = 17)]
        // GET: Vendor/Create
        public ActionResult Create()
        {
            return View();
        }


        [AccessChecker(Action = 2, ModuleID = 17)]
        // POST: Vendor/Create
        [HttpPost]
        public ActionResult Create(Vendor vendor)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var vendr = entity.Vendors.ToList().FindAll(b => b.Code == vendor.Code);

                    if (vendr.Count() > 0)
                    {
                        ModelState.AddModelError("", "The code already exists.");
                    }
                    else
                    {
                        entity.Vendors.Add(vendor);
                        entity.SaveChanges();
                        return RedirectToAction("Index");
                    }                    
                }
                catch
                {
                    ModelState.AddModelError("", "Fill all fields");
                }
            }

            return View(vendor);
        }


        [AccessChecker(Action = 2, ModuleID = 17)]
        // GET: Vendor/Edit/5
        public ActionResult Edit(int id)
        {
            var vendor = entity.Vendors.Find(id);
            if(vendor == null)
                return HttpNotFound();

            return View(vendor);
        }


        [AccessChecker(Action = 2, ModuleID = 17)]
        // POST: Vendor/Edit/5
        [HttpPost]
        public ActionResult Edit(Vendor vendor)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    entity.Entry(vendor).State = EntityState.Modified;
                    entity.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    ModelState.AddModelError("", "Fill all fields");
                }                
            }

            return View(vendor);
        }

        [AccessChecker(Action = 3, ModuleID = 17)]
        // GET: Vendor/Delete/5
        public ActionResult Delete(int id = 0)
        {
            var vendor = entity.Vendors.Find(id);
            return View(vendor);
        }

        [AccessChecker(Action = 3, ModuleID = 17)]
        // POST: Vendor/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id = 0)
        {
            try
            {
                // TODO: Add delete logic here
                var vendor = entity.Vendors.Find(id);

                try
                {
                    entity.Vendors.Remove(vendor);
                    entity.SaveChanges();
                }
                catch { }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
