using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MoostBrand.DAL;
using System.Configuration;
using PagedList;
using MoostBrand.Models;

namespace MoostBrand.Controllers
{
    [LoginChecker]
    public class InventoriesController : Controller
    {
        private MoostBrandEntities entity = new MoostBrandEntities();

        #region JSON
        public JsonResult GetItems(string name)
        {
            var items = entity.Items.Where(x => x.Code.Contains(name))
                            .Select(x => new
                            {
                                ID = x.ID,
                                Code = x.Code,
                                Name = x.Description,
                                UOM = x.UnitOfMeasurement.Description,
                                BarCode = x.Barcode,
                                Year = x.Year,
                                Categories = x.Category.Description
                            });
            return Json(items, JsonRequestBehavior.AllowGet);
        }
       
        #endregion



        // GET: Inventories
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "code" : "";
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "desc" : "";
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "vendor" : "";
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "wsh" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;


            var invt = from i in entity.Inventories
                        select i;

            if (!String.IsNullOrEmpty(searchString))
            {
                invt = invt.Where(i => i.ItemCode.Contains(searchString)
                                       || i.Description.Contains(searchString)
                                       || i.InStock.ToString().Contains(searchString));
            }

            switch (sortOrder)
            {
                case "code":
                    invt = invt.OrderByDescending(i => i.ItemCode);
                    break;
                case "desc":
                    invt = invt.OrderByDescending(i => i.Description);
                    break;
                case "wsh":
                    invt = invt.OrderByDescending(i => i.Location.Description);
                    break;
                default:
                    invt = invt.OrderBy(i => i.ID);
                    break;
            }

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);
            return View(invt.ToPagedList(pageNumber, pageSize));
        }

        // GET: Inventories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventory inventory = entity.Inventories.Find(id);
            if (inventory == null)
            {
                return HttpNotFound();
            }
            return View(inventory);
        }

        // GET: Inventories/Create
        public ActionResult Create()
        {
            #region DROPDOWNS
            ViewBag.InventoryStatus = new SelectList(entity.InventoryStatus, "ID", "Status");
            ViewBag.LocationCode = new SelectList(entity.Locations, "ID", "Description");
            #endregion

            return View();
        }

        // POST: Inventories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inventory inventory)
        {
            if (ModelState.IsValid)
            {
                entity.Inventories.Add(inventory);
                entity.SaveChanges();
                return RedirectToAction("Index");
            }

            #region DROPDOWNS
            ViewBag.InventoryStatus = new SelectList(entity.InventoryStatus, "ID", "Status");
            ViewBag.LocationCode = new SelectList(entity.Locations, "ID", "Description");
            #endregion

            return View(inventory);
        }

        // GET: Inventories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventory inventory = entity.Inventories.Find(id);
            if (inventory == null)
            {
                return HttpNotFound();
            }


            #region DROPDOWNS
            ViewBag.InventoryStatus = new SelectList(entity.InventoryStatus, "ID", "Status");
            ViewBag.LocationCode = new SelectList(entity.Locations, "ID", "Description");
            #endregion

            return View(inventory);
        }

        // POST: Inventories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Inventory inventory)
        {
            if (ModelState.IsValid)
            {
                entity.Entry(inventory).State = EntityState.Modified;
                entity.SaveChanges();
                return RedirectToAction("Index");
            }


            #region DROPDOWNS
            ViewBag.InventoryStatus = new SelectList(entity.InventoryStatus, "ID", "Status");
            ViewBag.LocationCode = new SelectList(entity.Locations, "ID", "Description");
            #endregion

            return View(inventory);
        }

        // GET: Inventories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventory inventory = entity.Inventories.Find(id);
            if (inventory == null)
            {
                return HttpNotFound();
            }
            return View(inventory);
        }

        // POST: Inventories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Inventory inventory = entity.Inventories.Find(id);
            entity.Inventories.Remove(inventory);
            entity.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                entity.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
