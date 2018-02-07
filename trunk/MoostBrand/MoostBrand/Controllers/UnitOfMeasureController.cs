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
    public class UnitOfMeasureController : Controller
    {
        MoostBrandEntities entity = new MoostBrandEntities();
        // GET: UnitOfMeasures

        [AccessCheckerForDisablingButtons(ModuleID = 13)]
        [AccessChecker(Action = 1, ModuleID = 13)]
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


            var uoms = from u in entity.UnitOfMeasurements
                         select u;

            if (!String.IsNullOrEmpty(searchString))
            {
                uoms = uoms.Where(u => u.Code.Contains(searchString)
                                       || u.Description.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "code":
                    uoms = uoms.OrderByDescending(u => u.Code);
                    break;
                case "desc":
                    uoms = uoms.OrderByDescending(u => u.Description);
                    break;
                default:
                    uoms = uoms.OrderBy(u => u.ID);
                    break;
            }

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);
            return View(uoms.ToPagedList(pageNumber, pageSize));
        }

        [AccessChecker(Action = 1, ModuleID = 13)]
        // GET: UnitOfMeasure/Details/5
        public ActionResult Details(int id)
        {
            var uom = entity.UnitOfMeasurements.Find(id);
            return View(uom);
        }

        [AccessChecker(Action = 2, ModuleID = 13)]
        // GET: UnitOfMeasure/Create
        public ActionResult Create()
        {
            return View();
        }

        [AccessChecker(Action = 2, ModuleID = 13)]
        // POST: UnitOfMeasure/Create
        [HttpPost]
        public ActionResult Create(UnitOfMeasurement unitofmeasurement)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var uomm = entity.UnitOfMeasurements.ToList().FindAll(b => b.Code == unitofmeasurement.Code);

                    if (uomm.Count() > 0)
                    {
                        ModelState.AddModelError("", "The code already exists.");
                    }
                    else
                    {
                        entity.UnitOfMeasurements.Add(unitofmeasurement);
                        entity.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "Fill all fields");
                }
            }
            return View(unitofmeasurement);            
        }

        [AccessChecker(Action = 2, ModuleID = 13)]
        // GET: UnitOfMeasure/Edit/5
        public ActionResult Edit(int id)
        {
            var uom = entity.UnitOfMeasurements.Find(id);
            return View(uom);
        }

        [AccessChecker(Action = 2, ModuleID = 13)]
        // POST: UnitOfMeasure/Edit/5
        [HttpPost]
        public ActionResult Edit(UnitOfMeasurement uom)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    entity.Entry(uom).State = EntityState.Modified;
                    entity.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    ModelState.AddModelError("", "Fill all fields");
                }
            }
            return View(uom);
        }

        [AccessChecker(Action = 3, ModuleID = 13)]
        // GET: UnitOfMeasure/Delete/5
        public ActionResult Delete(int id = 0)
        {
            var uom = entity.UnitOfMeasurements.Find(id);
            return View(uom);
        }

        [AccessChecker(Action = 3, ModuleID = 13)]
        // POST: UnitOfMeasure/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id = 0)
        {
            try
            {
                var uom = entity.UnitOfMeasurements.Find(id);

                try
                {
                    entity.UnitOfMeasurements.Remove(uom);
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
