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
    public class ContainerStorageController : Controller
    {
        MoostBrandEntities entity = new MoostBrandEntities();
        // GET: ContainerStorage
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


            var storages = from c in entity.ContainerStorages
                       select c;

            if (!String.IsNullOrEmpty(searchString))
            {
                storages = storages.Where(c => c.Code.Contains(searchString)
                                       || c.Description.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "code":
                    storages = storages.OrderByDescending(c => c.Code);
                    break;
                case "desc":
                    storages = storages.OrderByDescending(c => c.Description);
                    break;
                default:
                    storages = storages.OrderBy(c => c.ID);
                    break;
            }

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);
            return View(storages.ToPagedList(pageNumber, pageSize));
        }

        // GET: ContainerStorage/Details/5
        public ActionResult Details(int id)
        {
            var storage = entity.ContainerStorages.Find(id);
            return View(storage);
        }

        // GET: ContainerStorage/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ContainerStorage/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                var storage = new ContainerStorage();

                if (collection.Count > 0)
                {
                    storage.Code = collection["Code"];
                    storage.Description = collection["Description"];
                    try
                    {
                        entity.ContainerStorages.Add(storage);
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

        // GET: ContainerStorage/Edit/5
        public ActionResult Edit(int id)
        {
            var storage = entity.ContainerStorages.Find(id);
            return View(storage);
        }

        // POST: ContainerStorage/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                var storage = entity.ContainerStorages.Find(id);

                if (collection.Count > 0)
                {
                    storage.Code = collection["Code"];
                    storage.Description = collection["Description"];

                    try
                    {
                        entity.Entry(storage).State = EntityState.Modified;
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

        // GET: ContainerStorage/Delete/5
        public ActionResult Delete(int id)
        {
            var storage = entity.ContainerStorages.Find(id);
            return View(storage);
        }

        // POST: ContainerStorage/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var storage = entity.ContainerStorages.Find(id);

                try
                {
                    entity.ContainerStorages.Remove(storage);
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
