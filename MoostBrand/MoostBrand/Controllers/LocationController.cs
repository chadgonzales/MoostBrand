using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MoostBrand.DAL;
using PagedList;
using System.Data.Entity;
using System.Configuration;

namespace MoostBrand.Controllers
{
    public class LocationController : Controller
    {
        MoostBrandEntities entity = new MoostBrandEntities();

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

        // GET: Location/Details/5
        public ActionResult Details(int id)
        {
            var location = entity.Locations.Find(id);
            return View(location);
        }

        // GET: Location/Create
        public ActionResult Create()
        {
            ViewBag.LocationTypes = entity.LocationTypes.ToList();
            return View();
        }

        // POST: Location/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                var location = new Location();

                if (collection.Count > 0)
                {
                    location.Code = collection["Code"];
                    location.Description = collection["Description"];
                    location.LocationTypeID = Convert.ToInt32(collection["LocationTypeID"]);

                    if (location.Code.Trim() == string.Empty ||
                        location.Description.Trim() == string.Empty ||
                        location.LocationTypeID == 0)
                    {
                        ModelState.AddModelError("", "Fill all fields");
                        return View();
                    }

                    var loc = entity.Colors.ToList().FindAll(b => b.Code == location.Code);

                    if (loc.Count() > 0)
                    {
                        ModelState.AddModelError("", "The code already exists.");
                        return View();
                    }

                    try
                    {
                        entity.Locations.Add(location);
                        entity.SaveChanges();
                    }
                    catch { }
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Location/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.LocationTypes = entity.LocationTypes.ToList();

            var location = entity.Locations.Find(id);
            return View(location);
        }

        // POST: Location/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                var location = entity.Locations.Find(id);

                if (collection.Count > 0)
                {
                    location.Code = collection["Code"];
                    location.Description = collection["Description"];
                    location.LocationTypeID = Convert.ToInt32(collection["LocationTypeID"]);

                    if (location.Code.Trim() == string.Empty ||
                        location.Description.Trim() == string.Empty ||
                        location.LocationTypeID == 0)
                    {
                        ModelState.AddModelError("", "Fill all fields");
                        return View();
                    }

                    try
                    {
                        entity.Entry(location).State = EntityState.Modified;
                        entity.SaveChanges();
                    }
                    catch { }
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Location/Delete/5
        public ActionResult Delete(int id)
        {
            var location = entity.Locations.Find(id);
            return View(location);
        }

        // POST: Location/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
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
