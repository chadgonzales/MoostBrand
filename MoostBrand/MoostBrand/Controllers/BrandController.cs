using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MoostBrand.DAL;
using PagedList;
using System.Data.Entity;
using System.Configuration;
using MoostBrand.Models;

namespace MoostBrand.Controllers
{
    public class BrandController : Controller
    {
        MoostBrandEntities entity = new MoostBrandEntities();
        // GET: Brand

        [AccessChecker(Action = 1, ModuleID = 22)]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "code" : "";
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "desc" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;


            var brands = from b in entity.Brands
                        select b;

            if (!String.IsNullOrEmpty(searchString))
            {
                brands = brands.Where(b => b.Code.Contains(searchString)
                                       || b.Description.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "code":
                    brands = brands.OrderByDescending(b => b.Code);
                    break;
                case "desc":
                    brands = brands.OrderByDescending(b => b.Description);
                    break;
                default:
                    brands = brands.OrderBy(b => b.ID);
                    break;
            }

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);
            return View(brands.ToPagedList(pageNumber, pageSize));
        }

        [AccessChecker(Action = 1, ModuleID = 22)]
        // GET: Brand/Details/5
        public ActionResult Details(int id)
        {
            var brand = entity.Brands.Find(id);
            return View(brand);
        }

        [AccessChecker(Action = 2, ModuleID = 22)]
        // GET: Brand/Create
        public ActionResult Create()
        {
            return View();
        }

        [AccessChecker(Action = 2, ModuleID = 22)]
        // POST: Brand/Create
        [HttpPost]
        public ActionResult Create(Brand brand)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var brnd = entity.Brands.ToList().FindAll(b => b.Code == brand.Code);

                    if (brnd.Count() > 0)
                    {
                        ModelState.AddModelError("", "The code already exists.");
                    }
                    else
                    {
                        entity.Brands.Add(brand);
                        entity.SaveChanges();

                        return RedirectToAction("Index");
                    }                   
                }
                catch
                {
                }
            }

            return View(brand);
        }

        [AccessChecker(Action = 2, ModuleID = 22)]
        // GET: Brand/Edit/5
        public ActionResult Edit(int id)
        {
            var brand = entity.Brands.Find(id);
            return View(brand);
        }

        [AccessChecker(Action = 2, ModuleID = 22)]
        // POST: Brand/Edit/5
        [HttpPost]
        public ActionResult Edit(Brand brand)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    entity.Entry(brand).State = EntityState.Modified;
                    entity.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch
                {
                }
            }

            return View(brand);
        }

        [AccessChecker(Action = 3, ModuleID = 22)]
        // GET: Brand/Delete/5
        public ActionResult Delete(int id = 0)
        {
            var brand = entity.Brands.Find(id);
            return View(brand);
        }

        [AccessChecker(Action = 3, ModuleID = 22)]
        // POST: Brand/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id = 0)
        {
            try
            {
                var brand = entity.Brands.Find(id);

                try
                {
                    entity.Brands.Remove(brand);
                    entity.SaveChanges();
                }
                catch { }
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
