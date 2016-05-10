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
    public class UnitOfMeasureController : Controller
    {
        MoostBrandEntities entity = new MoostBrandEntities();
        // GET: UnitOfMeasures
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

        // GET: UnitOfMeasure/Details/5
        public ActionResult Details(int id)
        {
            var uom = entity.UnitOfMeasurements.Find(id);
            return View(uom);
        }

        // GET: UnitOfMeasure/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UnitOfMeasure/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                var uom = new UnitOfMeasurement();

                if (collection.Count > 0)
                {
                    uom.Code = collection["Code"];
                    uom.Description = collection["Description"];
                    uom.QuantityOfMeasure = Convert.ToInt32(collection["QuantityOfMeasure"]);
                    try
                    {
                        entity.UnitOfMeasurements.Add(uom);
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

        // GET: UnitOfMeasure/Edit/5
        public ActionResult Edit(int id)
        {
            var uom = entity.UnitOfMeasurements.Find(id);
            return View(uom);
        }

        // POST: UnitOfMeasure/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                var uom = entity.UnitOfMeasurements.Find(id);

                if (collection.Count > 0)
                {
                    uom.Code = collection["Code"];
                    uom.Description = collection["Description"];
                    uom.QuantityOfMeasure = Convert.ToInt32(collection["QuantityOfMeasure"]);

                    try
                    {
                        entity.Entry(uom).State = EntityState.Modified;
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

        // GET: UnitOfMeasure/Delete/5
        public ActionResult Delete(int id)
        {
            var uom = entity.UnitOfMeasurements.Find(id);
            return View(uom);
        }

        // POST: UnitOfMeasure/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
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
