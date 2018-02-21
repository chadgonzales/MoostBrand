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
    public class ContainerLocationController : Controller
    {
        MoostBrandEntities entity = new MoostBrandEntities();

        [AccessChecker(Action = 1, ModuleID = 15)]
        // GET: ContainerLocation
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


            var locations = from l in entity.ContainerLocations
                           select l;

            if (!String.IsNullOrEmpty(searchString))
            {
                locations = locations.Where(l => l.Code.Contains(searchString)
                                       || l.Description.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "code":
                    locations = locations.OrderByDescending(l => l.Code);
                    break;
                case "desc":
                    locations = locations.OrderByDescending(l => l.Description);
                    break;
                default:
                    locations = locations.OrderBy(l => l.ID);
                    break;
            }

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);
            return View(locations.ToPagedList(pageNumber, pageSize));
        }

        [AccessChecker(Action = 1, ModuleID = 15)]
        // GET: ContainerLocation/Details/5
        public ActionResult Details(int id)
        {
            var location = entity.ContainerLocations.Find(id);
            return View(location);
        }

        [AccessChecker(Action = 2, ModuleID = 15)]
        // GET: ContainerLocation/Create
        public ActionResult Create()
        {
            return View();
        }

        [AccessChecker(Action = 2, ModuleID = 15)]
        // POST: ContainerLocation/Create
        [HttpPost]
        public ActionResult Create(ContainerLocation cLocation)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var loc = entity.ContainerLocations.ToList().FindAll(b => b.Code == cLocation.Code);

                    if (loc.Count() > 0)
                    {
                        ModelState.AddModelError("", "The code already exists.");
                    }
                    else
                    {
                        entity.ContainerLocations.Add(cLocation);
                        entity.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "Fill all fields");
                }
            }
            return View(cLocation);
        }

        [AccessChecker(Action = 2, ModuleID = 15)]
        // GET: ContainerLocation/Edit/5
        public ActionResult Edit(int id)
        {
            var location = entity.ContainerLocations.Find(id);
            if (location == null)
                return HttpNotFound();

            return View(location);
        }

        [AccessChecker(Action = 2, ModuleID = 15)]
        // POST: ContainerLocation/Edit/5
        [HttpPost]
        public ActionResult Edit(ContainerLocation location)
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
            return View(location);
        }

        [AccessChecker(Action = 3, ModuleID = 15)]
        // GET: ContainerLocation/Delete/5
        public ActionResult Delete(int id = 0)
        {
            var location = entity.ContainerLocations.Find(id);
            return View(location);
        }

        [AccessChecker(Action = 3, ModuleID = 15)]
        // POST: ContainerLocation/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id = 0)
        {
            try
            {
                var location = entity.ContainerLocations.Find(id);

                try
                {
                    entity.ContainerLocations.Remove(location);
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
