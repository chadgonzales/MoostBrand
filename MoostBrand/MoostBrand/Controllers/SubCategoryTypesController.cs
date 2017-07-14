using MoostBrand.DAL;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoostBrand.Controllers
{
    public class SubCategoryTypesController : Controller
    {
        MoostBrandEntities entity = new MoostBrandEntities();
        // GET: SubCategory
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "code" : "";
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "desc" : "";
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "catdesc" : "";
            ViewBag.SubCategories = entity.SubCategories.ToList();

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var sub2 = entity.SubCategoriesTypes;


            var sub = from c in entity.SubCategoriesTypes
                      select c;

            if (!String.IsNullOrEmpty(searchString))
            {
                sub = sub.Where(c => c.Code.Contains(searchString)
                                       || c.Description.Contains(searchString)
                                       || c.SubCategory.Description.Contains(searchString));
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
                    sub = sub.OrderByDescending(c => c.SubCategory.Description);
                    break; 
                default:
                    sub = sub.OrderBy(c => c.ID);
                    break;
            }

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);
            return View(sub.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Details(int id)
        {
            var sub = entity.SubCategoriesTypes.Find(id);
            return View(sub);
        }

        public ActionResult Create()
        {
            ViewBag.SubCategories = entity.SubCategories.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Create(SubCategoriesType subcategoriestype)
        {
            if (ModelState.IsValid)
            {
               // subcategoriestype.Items = null;

                try
                {
                    //var subb = entity.SubCategoriesTypes.Where(r => r.Code == subcategoriestype.Code);
                    var subb = entity.SubCategoriesTypes.ToList().FindAll(b => b.Code == subcategoriestype.Code);

                    if (subb.Count() > 0)
                    { 	

                        ModelState.AddModelError("", "The code already exists.");
                    }
                    else
                    { 
                        entity.SubCategoriesTypes.Add(subcategoriestype);
                        entity.SaveChanges();
                        return RedirectToAction("Create");
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "Fill all fields");
                }
            }

            ViewBag.SubCategories = entity.SubCategories.ToList();
            return View(subcategoriestype);
        }

        public ActionResult Edit(int id)
        {
            ViewBag.SubCategories = entity.SubCategories.ToList();

            var sub = entity.SubCategoriesTypes.Find(id);

            return View(sub);
        }

        // POST: SubCategory/Edit/5
        [HttpPost]
        public ActionResult Edit(SubCategoriesType subcategoriestype)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    entity.Entry(subcategoriestype).State = EntityState.Modified;
                    entity.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    ModelState.AddModelError("", "Fill all fields");
                }
            }

            ViewBag.SubCategories = entity.SubCategories.ToList();
            return View(subcategoriestype);
        }

        // GET: SubCategory/Delete/5
        public ActionResult Delete(int id = 0)
        {
            var sub = entity.SubCategoriesTypes.Find(id);
            return View(sub);
        }

        // POST: SubCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id = 0)
        {
            try
            {
                // TODO: Add delete logic here
                var sub = entity.SubCategoriesTypes.Find(id);

                try
                {
                    entity.SubCategoriesTypes.Remove(sub);
                    entity.SaveChanges();
                }
                catch { }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}