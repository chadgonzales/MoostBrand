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
    public class LocationController : Controller
    {
        MoostBrandEntities entity = new MoostBrandEntities();

        [AccessCheckerForDisablingButtons(ModuleID = 21)]
        [AccessChecker(Action = 1, ModuleID = 21)]
        // GET: Location
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "code" : "";
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "desc" : "";
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "type" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;


            var locations = from l in entity.Locations
                        select l;

            if (!String.IsNullOrEmpty(searchString))
            {
                locations = locations.Where(l => l.Code.Contains(searchString)
                                       || l.Description.Contains(searchString)
                                       || l.LocationType.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "code":
                    locations = locations.OrderByDescending(l => l.Code);
                    break;
                case "desc":
                    locations = locations.OrderByDescending(l => l.Description);
                    break;
                case "type":
                    locations = locations.OrderByDescending(l => l.LocationType.Name);
                    break;
                default:
                    locations = locations.OrderBy(l => l.ID);
                    break;
            }

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);
            return View(locations.ToPagedList(pageNumber, pageSize));
        }

        [AccessChecker(Action = 1, ModuleID = 21)]
        // GET: Location/Details/5
        public ActionResult Details(int id)
        {
            var location = entity.Locations.Find(id);
            return View(location);
        }

        [AccessChecker(Action = 2, ModuleID = 21)]
        // GET: Location/Create
        public ActionResult Create()
        {
            ViewBag.LocationTypes = entity.LocationTypes.ToList();
            return View();
        }

        [AccessChecker(Action = 2, ModuleID = 21)]
        // POST: Location/Create
        [HttpPost]
        public ActionResult Create(Location location)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var loc = entity.Locations.ToList().FindAll(b => b.Code == location.Code);

                    if (loc.Count() > 0)
                    {
                        ModelState.AddModelError("", "The code already exists.");
                    }
                    else
                    {
                        entity.Locations.Add(location);
                        entity.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                catch
                {

                }
            }

            ViewBag.LocationTypes = entity.LocationTypes.ToList();
            return View(location);
        }

        [AccessChecker(Action = 2, ModuleID = 21)]
        // GET: Location/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.LocationTypes = entity.LocationTypes.ToList();

            var location = entity.Locations.Find(id);
            if (location == null)
                return HttpNotFound();

            return View(location);
        }

        [AccessChecker(Action = 2, ModuleID = 21)]
        // POST: Location/Edit/5
        [HttpPost]
        public ActionResult Edit(Location location)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    entity.Entry(location).State = EntityState.Modified;
                    entity.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    ModelState.AddModelError("", "Fill all fields");
                }
            }

            ViewBag.LocationTypes = entity.LocationTypes.ToList();
            return View(location);
        }

        [AccessChecker(Action = 3, ModuleID = 21)]
        // GET: Location/Delete/5
        public ActionResult Delete(int id = 0)
        {
            var location = entity.Locations.Find(id);
            return View(location);
        }

        [AccessChecker(Action = 3, ModuleID = 21)]
        // POST: Location/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id = 0)
        {
            try
            {
                var location = entity.Locations.Find(id);

                try
                {
                    entity.Locations.Remove(location);
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
