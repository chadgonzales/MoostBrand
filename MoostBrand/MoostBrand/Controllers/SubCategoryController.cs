using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MoostBrand.DAL;
using PagedList;
using System.Configuration;

namespace MoostBrand.Controllers
{
    public class SubCategoryController : Controller
    {
        MoostBrandEntities entity = new MoostBrandEntities();
        // GET: SubCategory
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "code" : "";
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "desc" : "";
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "catdesc" : "";
            ViewBag.Categories = entity.Categories.ToList();

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;


            var sub = from c in entity.SubCategories
                             select c;

            if (!String.IsNullOrEmpty(searchString))
            {
                sub = sub.Where(c => c.Code.Contains(searchString)
                                       || c.Description.Contains(searchString)
                                       || c.Category.Description.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "code":
                    sub = sub.OrderByDescending(c => c.Code);
                    break;
                case "desc":
                    sub = sub.OrderByDescending(c => c.Description);
                    break;
                case "catdesc":
                    sub = sub.OrderByDescending(c => c.Category.Description);
                    break;
                default:
                    sub = sub.OrderBy(c => c.ID);
                    break;
            }

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);
            return View(sub.ToPagedList(pageNumber, pageSize));
        }

        // GET: SubCategory/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SubCategory/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SubCategory/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: SubCategory/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SubCategory/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: SubCategory/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SubCategory/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
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
