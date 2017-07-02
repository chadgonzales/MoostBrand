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
    public class ColorController : Controller
    {
        MoostBrandEntities entity = new MoostBrandEntities();
        // GET: Colors

        [AccessChecker(Action = 1, ModuleID = 1)]
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


            var colors = from c in entity.Colors
                         select c;

            if (!String.IsNullOrEmpty(searchString))
            {
                colors = colors.Where(c => c.Code.Contains(searchString)
                                       || c.Description.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "code":
                    colors = colors.OrderByDescending(c => c.Code);
                    break;
                case "desc":
                    colors = colors.OrderByDescending(c => c.Description);
                    break;
                default:
                    colors = colors.OrderBy(c => c.ID);
                    break;
            }

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);
            return View(colors.ToPagedList(pageNumber, pageSize));
        }

        [AccessChecker(Action = 1, ModuleID = 1)]
        // GET: Color/Details/5
        public ActionResult Details(int id)
        {
            var color = entity.Colors.Find(id);
            return View(color);
        }

        [AccessChecker(Action = 2, ModuleID = 1)]
        // GET: Color/Create
        public ActionResult Create()
        {
            return View();
        }

        [AccessChecker(Action = 2, ModuleID = 1)]
        // POST: Color/Create
        [HttpPost]
        public ActionResult Create(Color color)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var colorr = entity.Colors.ToList().FindAll(b => b.Code == color.Code);

                    if (colorr.Count() > 0)
                    {
                        ModelState.AddModelError("", "The code already exists.");
                    }
                    else
                    {
                        entity.Colors.Add(color);
                        entity.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "Fill all fields");
                }
            }
            return View(color);
        }

        [AccessChecker(Action = 2, ModuleID = 1)]
        // GET: Color/Edit/5
        public ActionResult Edit(int id)
        {
            var color = entity.Colors.Find(id);
            if (color == null)
                return HttpNotFound();

            return View(color);
        }

        [AccessChecker(Action = 2, ModuleID = 1)]
        // POST: Color/Edit/5
        [HttpPost]
        public ActionResult Edit(Color color)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    entity.Entry(color).State = EntityState.Modified;
                    entity.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    ModelState.AddModelError("", "Fill all fields");
                }
            }

            return View(color);
        }

        [AccessChecker(Action = 3, ModuleID = 1)]
        // GET: Color/Delete/5
        public ActionResult Delete(int id = 0)
        {
            var color = entity.Colors.Find(id);
            return View(color);
        }

        [AccessChecker(Action = 3, ModuleID = 1)]
        // POST: Color/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id = 0)
        {
            try
            {
                var color = entity.Colors.Find(id);

                try
                {
                    entity.Colors.Remove(color);
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
