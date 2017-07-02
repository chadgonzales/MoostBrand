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
    public class ContainerStorageController : Controller
    {
        MoostBrandEntities entity = new MoostBrandEntities();

        [AccessChecker(Action = 1, ModuleID = 1)]
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

        [AccessChecker(Action = 1, ModuleID = 1)]
        // GET: ContainerStorage/Details/5
        public ActionResult Details(int id)
        {
            var storage = entity.ContainerStorages.Find(id);
            return View(storage);
        }

        [AccessChecker(Action = 2, ModuleID = 1)]
        // GET: ContainerStorage/Create
        public ActionResult Create()
        {
            return View();
        }

        [AccessChecker(Action = 2, ModuleID = 1)]
        // POST: ContainerStorage/Create
        [HttpPost]
        public ActionResult Create(ContainerStorage storage)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var cstorage = entity.ContainerStorages.ToList().FindAll(b => b.Code == storage.Code);

                    if (cstorage.Count() > 0)
                    {
                        ModelState.AddModelError("", "The code already exists.");
                    }
                    else
                    {
                        entity.ContainerStorages.Add(storage);
                        entity.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "Fill all fields");
                }
            }
            return View(storage);
        }

        [AccessChecker(Action = 2, ModuleID = 1)]
        // GET: ContainerStorage/Edit/5
        public ActionResult Edit(int id)
        {
            var storage = entity.ContainerStorages.Find(id);
            if (storage == null)
                return HttpNotFound();

            return View(storage);
        }

        [AccessChecker(Action = 2, ModuleID = 1)]
        // POST: ContainerStorage/Edit/5
        [HttpPost]
        public ActionResult Edit(ContainerStorage storage)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    entity.Entry(storage).State = EntityState.Modified;
                    entity.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    ModelState.AddModelError("", "Fill all fields");
                }
            }
            return View(storage);
        }

        [AccessChecker(Action = 3, ModuleID = 1)]
        // GET: ContainerStorage/Delete/5
        public ActionResult Delete(int id = 0)
        {
            var storage = entity.ContainerStorages.Find(id);
            return View(storage);
        }

        [AccessChecker(Action = 3, ModuleID = 1)]
        // POST: ContainerStorage/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id = 0)
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
