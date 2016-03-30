using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MoostBrand.Models;
using MoostBrand.DAL;

namespace MoostBrand.Controllers
{
    public class ItemController : Controller
    {
        private DBContext _db = new DBContext();

        #region ITEM CRUD
        //
        // GET: /Item/

        public ActionResult Index()
        {
            var items = _db.Items.ToList();
            return View(items);
        }

        //
        // GET: /Item/ItemDetails/5

        public ActionResult ItemDetails(int id = 0)
        {
            var item = _db.Items.Find(id);

            return View(item);
        }

        //
        // GET: /Item/ItemCreate

        public ActionResult ItemCreate()
        {
            ViewBag.BrandID = new SelectList(_db.Brands, "ID", "Description");
            ViewBag.UnitOfMeasurementID = new SelectList(_db.UnitOfMeasurements, "ID", "Description");
            ViewBag.LocationID = new SelectList(_db.Locations, "ID", "Description");

            var item = new Item();
            return View(item);
        }

        //
        // POST: /Item/ItemCreate

        [HttpPost]
        public ActionResult ItemCreate(Item item)
        {
            if (ModelState.IsValid)
            {
                var ite = _db.Items.FirstOrDefault(i => i.Description == item.Description);
                if (ite != null)
                {
                    ModelState.AddModelError("", "The item name already exists.");
                }
                else
                {
                    _db.Items.Add(item);
                    _db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            ViewBag.BrandID = new SelectList(_db.Brands, "ID", "Description", item.BrandID);
            ViewBag.UnitOfMeasurementID = new SelectList(_db.UnitOfMeasurements, "ID", "Description", item.UnitOfMeasurementID);
            ViewBag.LocationID = new SelectList(_db.Locations, "ID", "Description", item.LocationID);

            return View(item);
        }

        //
        // GET: /Item/ItemEdit/5

        public ActionResult ItemEdit(int id = 0)
        {
            var item = _db.Items.FirstOrDefault(i => i.ID == id);
            if (item == null)
            {
                return HttpNotFound();
            }

            ViewBag.BrandID = new SelectList(_db.Brands, "ID", "Description", item.BrandID);
            ViewBag.UnitOfMeasurementID = new SelectList(_db.UnitOfMeasurements, "ID", "Description", item.UnitOfMeasurementID);
            ViewBag.LocationID = new SelectList(_db.Locations, "ID", "Description", item.LocationID);

            return View(item);
        }

        //
        // POST: /Item/ItemEdit/5

        [HttpPost]
        public ActionResult ItemEdit(Item item)
        {
            if (ModelState.IsValid)
            {
                var item1 = _db.Items.FirstOrDefault(i => i.ID == item.ID);
                if (item1 == null)
                {
                    ModelState.AddModelError("", "The Item doesn't exists.");
                }
                else
                {
                    _db.Entry(item1).CurrentValues.SetValues(item);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            ViewBag.BrandID = new SelectList(_db.Brands, "ID", "Description", item.BrandID);
            ViewBag.UnitOfMeasurementID = new SelectList(_db.UnitOfMeasurements, "ID", "Description", item.UnitOfMeasurementID);
            ViewBag.LocationID = new SelectList(_db.Locations, "ID", "Description", item.LocationID);

            return View(item);
        }

        //
        // GET: /Item/ItemDelete/5

        public ActionResult ItemDelete(int id = 0)
        {
            var item = _db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        //
        // POST: /Item/ItemDelete/5

        [HttpPost, ActionName("ItemDelete")]
        public ActionResult ItemDeleteConfirmed(int id)
        {
            var item = _db.Items.Find(id);
            _db.Items.Remove(item);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        #endregion

        #region BRAND CRUD

        //
        // GET: /Item/BrandList

        public ActionResult BrandList()
        {
            var brands = _db.Brands.ToList();
            return View(brands);
        }

        //
        // GET: /Item/BrandCreate

        public ActionResult BrandCreate()
        {
            var brand = new Brand();
            return View(brand);
        }

        //
        // POST: /Item/BrandCreate
        [HttpPost]
        public ActionResult BrandCreate(Brand brand)
        {
            if (ModelState.IsValid)
            {
                _db.Brands.Add(brand);
                _db.SaveChanges();

                return RedirectToAction("BrandList");
            }

            return View(brand);
        }

        //
        // GET: /Item/BrandEdit/5

        public ActionResult BrandEdit(int id = 0)
        {
            var brand = _db.Brands.FirstOrDefault(b => b.ID == id);
            if (brand == null)
            {
                return HttpNotFound();
            }

            return View(brand);
        }

        //
        // POST: /Item/BrandEdit/5
        [HttpPost]
        public ActionResult BrandEdit(Brand brand)
        {
            if (ModelState.IsValid)
            {
                var brnd = _db.Brands.FirstOrDefault(b => b.ID == brand.ID);
                if (brnd == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    _db.Entry(brnd).CurrentValues.SetValues(brand);
                    _db.SaveChanges();
                    return RedirectToAction("BrandList");
                }
            }

            return View(brand);
        }

        //
        // GET: /Item/BrandDelete/5

        public ActionResult BrandDelete(int id = 0)
        {
            var brand = _db.Brands.Find(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View(brand);
        }

        //
        // POST: /User/Delete/5

        [HttpPost, ActionName("BrandDelete")]
        public ActionResult BrandDeleteConfirmed(int id)
        {
            var brand = _db.Brands.Find(id);
            _db.Brands.Remove(brand);
            _db.SaveChanges();
            return RedirectToAction("BrandList");
        }

        #endregion

        #region Unit Of Measurement CRUD

        //
        // GET: /Item/UOMList

        public ActionResult UOMList()
        {
            var uom = _db.UnitOfMeasurements.ToList();
            return View(uom);
        }

        //
        // GET: /Item/UOMCreate

        public ActionResult UOMCreate()
        {
            var uom = new UnitOfMeasurement();
            return View(uom);
        }

        //
        // POST: /Item/UOMCreate
        [HttpPost]
        public ActionResult UOMCreate(UnitOfMeasurement uom)
        {
            if (ModelState.IsValid)
            {
                _db.UnitOfMeasurements.Add(uom);
                _db.SaveChanges();

                return RedirectToAction("UOMList");
            }

            return View(uom);
        }

        //
        // GET: /Item/UOMEdit/5

        public ActionResult UOMEdit(int id = 0)
        {
            var uom = _db.UnitOfMeasurements.FirstOrDefault(u => u.ID == id);
            if (uom == null)
            {
                return HttpNotFound();
            }

            return View(uom);
        }

        //
        // POST: /Item/UOMEdit/5
        [HttpPost]
        public ActionResult UOMEdit(UnitOfMeasurement uom)
        {
            if (ModelState.IsValid)
            {
                var uuom = _db.UnitOfMeasurements.FirstOrDefault(b => b.ID == uom.ID);
                if (uuom == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    _db.Entry(uuom).CurrentValues.SetValues(uom);
                    _db.SaveChanges();
                    return RedirectToAction("UOMList");
                }
            }

            return View(uom);
        }

        //
        // GET: /Item/UOMDelete/5

        public ActionResult UOMDelete(int id = 0)
        {
            var uom = _db.UnitOfMeasurements.Find(id);
            if (uom == null)
            {
                return HttpNotFound();
            }
            return View(uom);
        }

        //
        // POST: /User/UOMDelete/5

        [HttpPost, ActionName("UOMDelete")]
        public ActionResult UOMDeleteConfirmed(int id)
        {
            var uom = _db.UnitOfMeasurements.Find(id);
            _db.UnitOfMeasurements.Remove(uom);
            _db.SaveChanges();
            return RedirectToAction("UOMList");
        }

        #endregion

        #region Location CRUD

        //
        // GET: /Item/LocationList

        public ActionResult LocationList()
        {
            var loc = _db.Locations.ToList();
            return View(loc);
        }

        //
        // GET: /Item/LocationCreate

        public ActionResult LocationCreate()
        {
            ViewBag.LocationTypeID = new SelectList(_db.LocationTypes, "ID", "Description");

            var loc = new Location();
            return View(loc);
        }

        //
        // POST: /Item/LocationCreate
        [HttpPost]
        public ActionResult LocationCreate(Location loc)
        {
            if (ModelState.IsValid)
            {
                var lo = _db.Locations.FirstOrDefault(l => l.Description == loc.Description);
                if (lo != null)
                {
                    ModelState.AddModelError("", "The location already exists.");
                }
                else
                {
                    _db.Locations.Add(loc);
                    _db.SaveChanges();

                    return RedirectToAction("LocationList");
                }
            }

            ViewBag.LocationTypeID = new SelectList(_db.LocationTypes, "ID", "Description", loc.LocationTypeID);

            return View(loc);
        }

        //
        // GET: /Item/LocationEdit/5

        public ActionResult LocationEdit(int id = 0)
        {
            var loc = _db.Locations.FirstOrDefault(l => l.ID == id);
            if (loc == null)
            {
                return HttpNotFound();
            }

            ViewBag.LocationTypeID = new SelectList(_db.LocationTypes, "ID", "Description", loc.LocationTypeID);

            return View(loc);
        }

        //
        // POST: /Item/LocationEdit/5

        [HttpPost]
        public ActionResult LocationEdit(Location loc)
        {
            if (ModelState.IsValid)
            {
                var lo = _db.Locations.FirstOrDefault(l => l.ID == loc.ID);
                if (lo == null)
                {
                    ModelState.AddModelError("", "The Location doesn't exists.");
                }
                else
                {
                    _db.Entry(lo).CurrentValues.SetValues(loc);
                    _db.SaveChanges();
                    return RedirectToAction("LocationList");
                }
            }

            ViewBag.LocationTypeID = new SelectList(_db.LocationTypes, "ID", "Description", loc.LocationTypeID);

            return View(loc);
        }

        //
        // GET: /Item/LocationDelete/5

        public ActionResult LocationDelete(int id = 0)
        {
            var loc = _db.Locations.Find(id);
            if (loc == null)
            {
                return HttpNotFound();
            }
            return View(loc);
        }

        //
        // POST: /Item/LocationDelete/5

        [HttpPost, ActionName("LocationDelete")]
        public ActionResult LocationDeleteConfirmed(int id)
        {
            var loc = _db.Locations.Find(id);
            _db.Locations.Remove(loc);
            _db.SaveChanges();
            return RedirectToAction("LocationList");
        }

        #endregion

        #region LocationType CRUD

        //
        // GET: /Item/LocationTypeList

        public ActionResult LocationTypeList()
        {
            var lt = _db.LocationTypes.ToList();
            return View(lt);
        }

        //
        // GET: /Item/LocationTypeCreate

        public ActionResult LocationTypeCreate()
        {
            var lt = new LocationType();
            return View(lt);
        }

        //
        // POST: /Item/LocationTypeCreate
        [HttpPost]
        public ActionResult LocationTypeCreate(LocationType lt)
        {
            if (ModelState.IsValid)
            {
                _db.LocationTypes.Add(lt);
                _db.SaveChanges();

                return RedirectToAction("LocationTypeList");
            }

            return View(lt);
        }

        //
        // GET: /Item/LocationTypeEdit/5

        public ActionResult LocationTypeEdit(int id = 0)
        {
            var lt = _db.LocationTypes.FirstOrDefault(l => l.ID == id);
            if (lt == null)
            {
                return HttpNotFound();
            }

            return View(lt);
        }

        //
        // POST: /Item/LocationTypeEdit/5
        [HttpPost]
        public ActionResult LocationTypeEdit(LocationType lt)
        {
            if (ModelState.IsValid)
            {
                var lt1 = _db.Brands.FirstOrDefault(l => l.ID == lt.ID);
                if (lt1 == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    _db.Entry(lt1).CurrentValues.SetValues(lt);
                    _db.SaveChanges();
                    return RedirectToAction("LocationTypeList");
                }
            }

            return View(lt);
        }

        //
        // GET: /Item/LocationTypeDelete/5

        public ActionResult LocationTypeDelete(int id = 0)
        {
            var lt = _db.LocationTypes.Find(id);
            if (lt == null)
            {
                return HttpNotFound();
            }
            return View(lt);
        }

        //
        // POST: /User/LocationTypeDelete/5

        [HttpPost, ActionName("LocationTypeDelete")]
        public ActionResult LocationTypeDeleteConfirmed(int id)
        {
            var lt = _db.LocationTypes.Find(id);
            _db.LocationTypes.Remove(lt);
            _db.SaveChanges();
            return RedirectToAction("LocationTypeList");
        }

        #endregion

        #region CATEGORY CRUD

        //
        // GET: /Item/CategoryList

        public ActionResult CategoryList()
        {
            var categories = _db.Categories.ToList();
            return View(categories);
        }

        //
        // GET: /Item/CategoryCreate

        public ActionResult CategoryCreate()
        {
            var category = new Category();
            return View(category);
        }

        //
        // POST: /Item/CategoryCreate
        [HttpPost]
        public ActionResult CategoryCreate(Category category)
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Add(category);
                _db.SaveChanges();

                return RedirectToAction("CategoryList");
            }

            return View(category);
        }

        //
        // GET: /Item/CategoryEdit/5

        public ActionResult CategoryEdit(int id = 0)
        {
            var category = _db.Categories.FirstOrDefault(b => b.ID == id);
            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category);
        }

        //
        // POST: /Item/CategoryEdit/5
        [HttpPost]
        public ActionResult CategoryEdit(Category category)
        {
            if (ModelState.IsValid)
            {
                var cat = _db.Categories.FirstOrDefault(b => b.ID == category.ID);
                if (cat == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    _db.Entry(cat).CurrentValues.SetValues(category);
                    _db.SaveChanges();
                    return RedirectToAction("CategoryList");
                }
            }

            return View(category);
        }

        //
        // GET: /Item/CategoryDelete/5

        public ActionResult CategoryDelete(int id = 0)
        {
            var category = _db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        //
        // POST: /User/CategoryDelete/5

        [HttpPost, ActionName("CategoryDelete")]
        public ActionResult CategoryDeleteConfirmed(int id)
        {
            var category = _db.Categories.Find(id);
            _db.Categories.Remove(category);
            _db.SaveChanges();
            return RedirectToAction("CategoryList");
        }

        #endregion

        #region SUBCATEGORY CRUD

        //
        // GET: /Item/SubCatList

        public ActionResult SubCatList()
        {
            var subcat = _db.SubCategories.ToList();
            return View(subcat);
        }

        //
        // GET: /Item/SubCatCreate

        public ActionResult SubCatCreate()
        {
            ViewBag.CategoryID = new SelectList(_db.Categories, "ID", "Description");

            var subcat = new SubCategory();
            return View(subcat);
        }

        //
        // POST: /Item/SubCatCreate
        [HttpPost]
        public ActionResult SubCatCreate(SubCategory subcat)
        {
            if (ModelState.IsValid)
            {
                _db.SubCategories.Add(subcat);
                _db.SaveChanges();

                return RedirectToAction("SubCatList");
            }

            ViewBag.CategoryID = new SelectList(_db.Categories, "ID", "Description");

            return View(subcat);
        }

        //
        // GET: /Item/SubCatEdit/5

        public ActionResult SubCatEdit(int id = 0)
        {
            var subcat = _db.SubCategories.FirstOrDefault(l => l.ID == id);
            if (subcat == null)
            {
                return HttpNotFound();
            }

            ViewBag.CategoryID = new SelectList(_db.Categories, "ID", "Description", subcat.CategoryID);

            return View(subcat);
        }

        //
        // POST: /Item/SubCatEdit/5

        [HttpPost]
        public ActionResult SubCatEdit(SubCategory subcat)
        {
            if (ModelState.IsValid)
            {
                var subca = _db.SubCategories.FirstOrDefault(l => l.ID == subcat.ID);
                if (subca == null)
                {
                    ModelState.AddModelError("", "The Subcategory doesn't exists.");
                }
                else
                {
                    _db.Entry(subca).CurrentValues.SetValues(subcat);
                    _db.SaveChanges();
                    return RedirectToAction("SubCatList");
                }
            }

            ViewBag.CategoryID = new SelectList(_db.Categories, "ID", "Description", subcat.CategoryID);

            return View(subcat);
        }

        //
        // GET: /Item/SubCatDelete/5

        public ActionResult SubCatDelete(int id = 0)
        {
            var subcat = _db.SubCategories.Find(id);
            if (subcat == null)
            {
                return HttpNotFound();
            }
            return View(subcat);
        }

        //
        // POST: /Item/SubCatDelete/5

        [HttpPost, ActionName("SubCatDelete")]
        public ActionResult SubCatDeleteConfirmed(int id)
        {
            var subcat = _db.SubCategories.Find(id);
            _db.SubCategories.Remove(subcat);
            _db.SaveChanges();
            return RedirectToAction("SubCatList");
        }

        #endregion

        #region COLOR CRUD

        //
        // GET: /Item/ColorList

        public ActionResult ColorList()
        {
            var colors = _db.Colors.ToList();
            return View(colors);
        }

        //
        // GET: /Item/ColorCreate

        public ActionResult ColorCreate()
        {
            ViewBag.CategoryID = new SelectList(_db.Categories, "ID", "Description");

            var color = new Color();
            return View(color);
        }

        //
        // POST: /Item/ColorCreate
        [HttpPost]
        public ActionResult ColorCreate(Color color)
        {
            if (ModelState.IsValid)
            {
                _db.Colors.Add(color);
                _db.SaveChanges();

                return RedirectToAction("ColorList");
            }

            ViewBag.CategoryID = new SelectList(_db.Categories, "ID", "Description");

            return View(color);
        }

        //
        // GET: /Item/ColorEdit/5

        public ActionResult ColorEdit(int id = 0)
        {
            var color = _db.Colors.FirstOrDefault(l => l.ID == id);
            if (color == null)
            {
                return HttpNotFound();
            }

            ViewBag.CategoryID = new SelectList(_db.Categories, "ID", "Description", color.CategoryID);

            return View(color);
        }

        //
        // POST: /Item/ColorEdit/5

        [HttpPost]
        public ActionResult ColorEdit(Color color)
        {
            if (ModelState.IsValid)
            {
                var color1 = _db.Colors.FirstOrDefault(l => l.ID == color.ID);
                if (color1 == null)
                {
                    ModelState.AddModelError("", "The Subcategory doesn't exists.");
                }
                else
                {
                    _db.Entry(color1).CurrentValues.SetValues(color);
                    _db.SaveChanges();
                    return RedirectToAction("ColorList");
                }
            }

            ViewBag.CategoryID = new SelectList(_db.Categories, "ID", "Description", color.CategoryID);

            return View(color);
        }

        //
        // GET: /Item/ColorDelete/5

        public ActionResult ColorDelete(int id = 0)
        {
            var color = _db.Colors.Find(id);
            if (color == null)
            {
                return HttpNotFound();
            }
            return View(color);
        }

        //
        // POST: /Item/ColorDelete/5

        [HttpPost, ActionName("ColorDelete")]
        public ActionResult ColorDeleteConfirmed(int id)
        {
            var color = _db.Colors.Find(id);
            _db.Colors.Remove(color);
            _db.SaveChanges();
            return RedirectToAction("ColorList");
        }

        #endregion

        #region SIZE CRUD

        //
        // GET: /Item/SizeList

        public ActionResult SizeList()
        {
            var sizes = _db.Sizes.ToList();
            return View(sizes);
        }

        //
        // GET: /Item/SizeCreate

        public ActionResult SizeCreate()
        {
            var size = new Size();
            return View(size);
        }

        //
        // POST: /Item/SizeCreate
        [HttpPost]
        public ActionResult SizeCreate(Size size)
        {
            if (ModelState.IsValid)
            {
                _db.Sizes.Add(size);
                _db.SaveChanges();

                return RedirectToAction("SizeList");
            }

            return View(size);
        }

        //
        // GET: /Item/SizeEdit/5

        public ActionResult SizeEdit(int id = 0)
        {
            var size = _db.Sizes.FirstOrDefault(b => b.ID == id);
            if (size == null)
            {
                return HttpNotFound();
            }

            return View(size);
        }

        //
        // POST: /Item/SizeEdit/5
        [HttpPost]
        public ActionResult SizeEdit(Size size)
        {
            if (ModelState.IsValid)
            {
                var size1 = _db.Sizes.FirstOrDefault(b => b.ID == size.ID);
                if (size1 == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    _db.Entry(size1).CurrentValues.SetValues(size);
                    _db.SaveChanges();
                    return RedirectToAction("SizeList");
                }
            }

            return View(size);
        }

        //
        // GET: /Item/SizeDelete/5

        public ActionResult SizeDelete(int id = 0)
        {
            var size = _db.Sizes.Find(id);
            if (size == null)
            {
                return HttpNotFound();
            }
            return View(size);
        }

        //
        // POST: /Item/Delete/5

        [HttpPost, ActionName("SizeDelete")]
        public ActionResult SizeDeleteConfirmed(int id)
        {
            var size = _db.Sizes.Find(id);
            _db.Sizes.Remove(size);
            _db.SaveChanges();
            return RedirectToAction("SizeList");
        }

        #endregion

        #region CONTAINER CRUD

        //
        // GET: /Item/ContainerList

        public ActionResult ContainerList()
        {
            var containers = _db.Containers.ToList();
            return View(containers);
        }

        //
        // GET: /Item/ContainerCreate

        public ActionResult ContainerCreate()
        {
            var container = new Container();
            return View(container);
        }

        //
        // POST: /Item/ContainerCreate
        [HttpPost]
        public ActionResult ContainerCreate(Container container)
        {
            if (ModelState.IsValid)
            {
                _db.Containers.Add(container);
                _db.SaveChanges();

                return RedirectToAction("ContainerList");
            }

            return View(container);
        }

        //
        // GET: /Item/ContainerEdit/5

        public ActionResult ContainerEdit(int id = 0)
        {
            var container = _db.Containers.FirstOrDefault(b => b.ID == id);
            if (container == null)
            {
                return HttpNotFound();
            }

            return View(container);
        }

        //
        // POST: /Item/ContainerEdit/5
        [HttpPost]
        public ActionResult ContainerEdit(Container container)
        {
            if (ModelState.IsValid)
            {
                var cont = _db.Containers.FirstOrDefault(b => b.ID == container.ID);
                if (cont == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    _db.Entry(cont).CurrentValues.SetValues(container);
                    _db.SaveChanges();
                    return RedirectToAction("ContainerList");
                }
            }

            return View(container);
        }

        //
        // GET: /Item/ContainerDelete/5

        public ActionResult ContainerDelete(int id = 0)
        {
            var container = _db.Containers.Find(id);
            if (container == null)
            {
                return HttpNotFound();
            }
            return View(container);
        }

        //
        // POST: /Item/ContainerDelete/5

        [HttpPost, ActionName("ContainerDelete")]
        public ActionResult ContainerDeleteConfirmed(int id)
        {
            var container = _db.Containers.Find(id);
            _db.Containers.Remove(container);
            _db.SaveChanges();
            return RedirectToAction("ContainerList");
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}
