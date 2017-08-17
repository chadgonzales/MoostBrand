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
    public class SizeController : Controller
    {
        MoostBrandEntities entity = new MoostBrandEntities();

        [AccessChecker(Action = 1, ModuleID = 20)]
        // GET: Sizes
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


            var sizes = from s in entity.Sizes
                         select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                sizes = sizes.Where(s => s.Code.Contains(searchString)
                                       || s.Description.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "code":
                    sizes = sizes.OrderByDescending(s => s.Code);
                    break;
                case "desc":
                    sizes = sizes.OrderByDescending(s => s.Description);
                    break;
                default:
                    sizes = sizes.OrderBy(s => s.ID);
                    break;
            }

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);
            return View(sizes.ToPagedList(pageNumber, pageSize));
        }

        [AccessChecker(Action = 1, ModuleID = 20)]
        // GET: Size/Details/5
        public ActionResult Details(int id)
        {
            var size = entity.Sizes.Find(id);
            return View(size);
        }

        [AccessChecker(Action = 2, ModuleID = 20)]
        // GET: Size/Create
        public ActionResult Create()
        {
            return View();
        }

        [AccessChecker(Action = 2, ModuleID = 20)]
        // POST: Size/Create
        [HttpPost]
        public ActionResult Create(Size size)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var siz = entity.Sizes.ToList().FindAll(b => b.Code == size.Code);

                    if (siz.Count() > 0)
                    {
                        ModelState.AddModelError("", "The code already exists.");
                    }
                    else
                    {
                        entity.Sizes.Add(size);
                        entity.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "Fill all fields");
                }
            }
            return View(size);
        }

        [AccessChecker(Action = 2, ModuleID = 20)]
        // GET: Size/Edit/5
        public ActionResult Edit(int id)
        {
            var size = entity.Sizes.Find(id);
            if (size == null)
                return HttpNotFound();

            return View(size);
        }

        [AccessChecker(Action = 2, ModuleID = 20)]
        // POST: Size/Edit/5
        [HttpPost]
        public ActionResult Edit(Size size)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    entity.Entry(size).State = EntityState.Modified;
                    entity.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    ModelState.AddModelError("", "Fill all fields");
                }
            }
            return View(size);
        }

        [AccessChecker(Action = 3, ModuleID = 20)]
        // GET: Size/Delete/5
        public ActionResult Delete(int id = 0)
        {
            var size = entity.Sizes.Find(id);
            return View(size);
        }

        [AccessChecker(Action = 3, ModuleID = 20)]
        // POST: Size/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id = 0)
        {
            try
            {
                var size = entity.Sizes.Find(id);

                try
                {
                    entity.Sizes.Remove(size);
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
