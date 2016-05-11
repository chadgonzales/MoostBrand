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
    public class ColorController : Controller
    {
        MoostBrandEntities entity = new MoostBrandEntities();
        // GET: Colors
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


            var colors = from c in entity.Colors
                         select c;

            if (!String.IsNullOrEmpty(searchString))
            {
                colors = colors.Where(c => c.Code.Contains(searchString)
                                       || c.Description.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "code":
                    colors = colors.OrderByDescending(c => c.Code);
                    break;
                case "desc":
                    colors = colors.OrderByDescending(c => c.Description);
                    break;
                default:
                    colors = colors.OrderBy(c => c.ID);
                    break;
            }

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);
            return View(colors.ToPagedList(pageNumber, pageSize));
        }

        // GET: Color/Details/5
        public ActionResult Details(int id)
        {
            var color = entity.Colors.Find(id);
            return View(color);
        }

        // GET: Color/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Color/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                var color = new Color();

                if (collection.Count > 0)
                {
                    color.Code = collection["Code"];
                    color.Description = collection["Description"];

                    if (color.Code.Trim() == string.Empty || color.Description.Trim() == string.Empty)
                    {
                        ModelState.AddModelError("", "Fill all fields");
                        return View();
                    }

                    var colorr = entity.Colors.ToList().FindAll(b => b.Code == color.Code);

                    if (colorr.Count() > 0)
                    {
                        ModelState.AddModelError("", "The code already exists.");
                        return View();
                    }

                    try
                    {
                        entity.Colors.Add(color);
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

        // GET: Color/Edit/5
        public ActionResult Edit(int id)
        {
            var color = entity.Colors.Find(id);
            return View(color);
        }

        // POST: Color/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                var color = entity.Colors.Find(id);

                if (collection.Count > 0)
                {
                    color.Code = collection["Code"];
                    color.Description = collection["Description"];

                    if (color.Code.Trim() == string.Empty || color.Description.Trim() == string.Empty)
                    {
                        ModelState.AddModelError("", "Fill all fields");
                        return View();
                    }

                    try
                    {
                        entity.Entry(color).State = EntityState.Modified;
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

        // GET: Color/Delete/5
        public ActionResult Delete(int id)
        {
            var color = entity.Colors.Find(id);
            return View(color);
        }

        // POST: Color/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var color = entity.Colors.Find(id);

                try
                {
                    entity.Colors.Remove(color);
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
