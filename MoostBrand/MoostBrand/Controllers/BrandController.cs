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
    public class BrandController : Controller
    {
        MoostBrandEntities entity = new MoostBrandEntities();
        // GET: Brand
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


            var brands = from b in entity.Brands
                        select b;

            if (!String.IsNullOrEmpty(searchString))
            {
                brands = brands.Where(b => b.Code.Contains(searchString)
                                       || b.Description.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "code":
                    brands = brands.OrderByDescending(b => b.Code);
                    break;
                case "desc":
                    brands = brands.OrderByDescending(b => b.Description);
                    break;
                default:
                    brands = brands.OrderBy(b => b.ID);
                    break;
            }

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);
            return View(brands.ToPagedList(pageNumber, pageSize));
        }

        // GET: Brand/Details/5
        public ActionResult Details(int id)
        {
            var brand = entity.Brands.Find(id);
            return View(brand);
        }

        // GET: Brand/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Brand/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                var brand = new Brand();

                if (collection.Count > 0)
                {
                    brand.Code = collection["Code"];
                    brand.Description = collection["Description"];

                    try
                    {
                        entity.Brands.Add(brand);
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

        // GET: Brand/Edit/5
        public ActionResult Edit(int id)
        {
            var brand = entity.Brands.Find(id);
            return View(brand);
        }

        // POST: Brand/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                var brand = entity.Brands.Find(id);

                if (collection.Count > 0)
                {
                    brand.Code = collection["Code"];
                    brand.Description = collection["Description"];

                    try
                    {
                        entity.Entry(brand).State = EntityState.Modified;
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

        // GET: Brand/Delete/5
        public ActionResult Delete(int id)
        {
            var brand = entity.Brands.Find(id);
            return View(brand);
        }

        // POST: Brand/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var brand = entity.Brands.Find(id);

                try
                {
                    entity.Brands.Remove(brand);
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
