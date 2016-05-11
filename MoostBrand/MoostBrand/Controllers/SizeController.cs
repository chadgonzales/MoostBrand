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
    public class SizeController : Controller
    {
        MoostBrandEntities entity = new MoostBrandEntities();
        // GET: Sizes
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


            var sizes = from s in entity.Sizes
                         select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                sizes = sizes.Where(s => s.Code.Contains(searchString)
                                       || s.Description.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "code":
                    sizes = sizes.OrderByDescending(s => s.Code);
                    break;
                case "desc":
                    sizes = sizes.OrderByDescending(s => s.Description);
                    break;
                default:
                    sizes = sizes.OrderBy(s => s.ID);
                    break;
            }

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);
            return View(sizes.ToPagedList(pageNumber, pageSize));
        }

        // GET: Size/Details/5
        public ActionResult Details(int id)
        {
            var size = entity.Sizes.Find(id);
            return View(size);
        }

        // GET: Size/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Size/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                var size = new Size();

                if (collection.Count > 0)
                {
                    size.Code = collection["Code"];
                    size.Description = collection["Description"];

                    if (size.Code.Trim() == string.Empty || size.Description.Trim() == string.Empty)
                    {
                        ModelState.AddModelError("", "Fill all fields");
                        return View();
                    }

                    var siz = entity.Colors.ToList().FindAll(b => b.Code == size.Code);

                    if (siz.Count() > 0)
                    {
                        ModelState.AddModelError("", "The code already exists.");
                        return View();
                    }

                    try
                    {
                        entity.Sizes.Add(size);
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

        // GET: Size/Edit/5
        public ActionResult Edit(int id)
        {
            var size = entity.Sizes.Find(id);
            return View(size);
        }

        // POST: Size/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                var size = entity.Sizes.Find(id);

                if (collection.Count > 0)
                {
                    size.Code = collection["Code"];
                    size.Description = collection["Description"];

                    if (size.Code.Trim() == string.Empty || size.Description.Trim() == string.Empty)
                    {
                        ModelState.AddModelError("", "Fill all fields");
                        return View();
                    }

                    try
                    {
                        entity.Entry(size).State = EntityState.Modified;
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

        // GET: Size/Delete/5
        public ActionResult Delete(int id)
        {
            var size = entity.Sizes.Find(id);
            return View(size);
        }

        // POST: Size/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var size = entity.Sizes.Find(id);

                try
                {
                    entity.Sizes.Remove(size);
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
