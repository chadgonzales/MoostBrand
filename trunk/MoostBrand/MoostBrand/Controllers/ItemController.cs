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
    public class ItemController : Controller
    {
        MoostBrandEntities entity = new MoostBrandEntities();
        // GET: Items
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


            var items = from i in entity.Items
                       select i;

            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(i => i.Code.Contains(searchString)
                                       || i.Description.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "code":
                    items = items.OrderByDescending(i => i.Code);
                    break;
                case "desc":
                    items = items.OrderByDescending(i => i.Description);
                    break;
                default:
                    items = items.OrderBy(i => i.ID);
                    break;
            }

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        // GET: Item/Details/5
        public ActionResult Details(int id)
        {
            var item = entity.Items.Find(id);
            return View(item);
        }

        // GET: Item/Create
        public ActionResult Create()
        {
            ViewBag.Categories = entity.Categories.ToList();
            ViewBag.Colors = entity.Colors.ToList();
            ViewBag.Sizes = entity.Sizes.ToList();
            ViewBag.UOMS = entity.UnitOfMeasurements.ToList();
            ViewBag.Brands = entity.Brands.ToList();

            return View();
        }

        // POST: Item/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                var item = new Item();

                if (collection.Count > 0)
                {
                    item.Code = collection["Code"];
                    item.Barcode = collection["Barcode"];
                    item.Description = collection["Description"];
                    item.CategoryID = Convert.ToInt32(collection["CategoryID"]);
                    item.SubCategoryID = Convert.ToInt32(collection["SubCategoryID"]);
                    item.BrandID = Convert.ToInt32(collection["BrandID"]);
                    item.ColorID = Convert.ToInt32(collection["ColorID"]);
                    item.SizeID = Convert.ToInt32(collection["SizeID"]);
                    item.UnitOfMeasurementID = Convert.ToInt32(collection["UnitOfMeasurementID"]);
                    item.ReOrderLevel = Convert.ToInt32(collection["ReOrderLevel"]);
                    item.MinimumStock = Convert.ToInt32(collection["MinimumStock"]);
                    item.MaximumStock = Convert.ToInt32(collection["MaximumStock"]);

                    if (item.Code.Trim() == string.Empty ||
                        item.Barcode.Trim() == string.Empty ||
                        item.Description.Trim() == string.Empty ||
                        item.CategoryID == 0 ||
                        item.SubCategoryID == 0||
                        item.BrandID == 0 ||
                        item.ColorID == 0 ||
                        item.SizeID == 0 ||
                        item.UnitOfMeasurementID == 0 ||
                        item.ReOrderLevel == 0 ||
                        item.MinimumStock == 0 ||
                        item.MaximumStock == 0)
                    {
                        ModelState.AddModelError("", "Fill all fields");
                        return View();
                    }

                    var itm = entity.Colors.ToList().FindAll(b => b.Code == item.Code);

                    if (itm.Count() > 0)
                    {
                        ModelState.AddModelError("", "The code already exists.");
                        return View();
                    }

                    try
                    {
                        entity.Items.Add(item);
                        entity.SaveChanges();
                    }
                    catch(Exception err) { }
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Item/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.Categories = entity.Categories.ToList();
            ViewBag.Colors = entity.Colors.ToList();
            ViewBag.Sizes = entity.Sizes.ToList();
            ViewBag.UOMS = entity.UnitOfMeasurements.ToList();
            ViewBag.Brands = entity.Brands.ToList();

            var item = entity.Items.Find(id);
            return View(item);
        }

        // POST: Item/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                var item = entity.Items.Find(id);

                if (collection.Count > 0)
                {
                    item.Code = collection["Code"];
                    item.Barcode = collection["Barcode"];
                    item.Description = collection["Description"];
                    item.CategoryID = Convert.ToInt32(collection["CategoryID"]);
                    item.SubCategoryID = Convert.ToInt32(collection["SubCategoryID"]);
                    item.BrandID = Convert.ToInt32(collection["BrandID"]);
                    item.ColorID = Convert.ToInt32(collection["ColorID"]);
                    item.SizeID = Convert.ToInt32(collection["SizeID"]);
                    item.UnitOfMeasurementID = Convert.ToInt32(collection["UnitOfMeasurementID"]);
                    item.ReOrderLevel = Convert.ToInt32(collection["ReOrderLevel"]);
                    item.MinimumStock = Convert.ToInt32(collection["MinimumStock"]);
                    item.MaximumStock = Convert.ToInt32(collection["MaximumStock"]);

                    if (item.Code.Trim() == string.Empty ||
                        item.Barcode.Trim() == string.Empty ||
                        item.Description.Trim() == string.Empty ||
                        item.CategoryID == 0 ||
                        item.SubCategoryID == 0 ||
                        item.BrandID == 0 ||
                        item.ColorID == 0 ||
                        item.SizeID == 0 ||
                        item.UnitOfMeasurementID == 0 ||
                        item.ReOrderLevel == 0 ||
                        item.MinimumStock == 0 ||
                        item.MaximumStock == 0)
                    {
                        ModelState.AddModelError("", "Fill all fields");
                        return View();
                    }

                    try
                    {
                        entity.Entry(item).State = EntityState.Modified;
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

        // GET: Item/Delete/5
        public ActionResult Delete(int id)
        {
            var item = entity.Items.Find(id);
            return View(item);
        }

        // POST: Item/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var item = entity.Items.Find(id);

                try
                {
                    entity.Items.Remove(item);
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

        [HttpPost]
        public ActionResult GetSubCategory(int categoryID)
        {

            var objsubcat = entity.SubCategories
       .Where(s => s.CategoryID == categoryID)
       .Select(s => new { s.ID, s.Description })
       .OrderBy(s => s.Description);

            return Json(objsubcat, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetItemById(int itemID)
        {
            var item = entity.Items.FirstOrDefault(i => i.ID == itemID);

            return Json(
                new
                {
                    ID = item.ID,
                    Category = item.CategoryID,
                    SubCategory = item.SubCategoryID
                },
                JsonRequestBehavior.AllowGet);
        }
    }
}
