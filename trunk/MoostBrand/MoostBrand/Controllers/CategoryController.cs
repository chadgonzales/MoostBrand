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
    public class CategoryController : Controller
    {
        MoostBrandEntities entity = new MoostBrandEntities();
        
        // GET: Category
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


            var categories = from c in entity.Categories
                            select c;

            if (!String.IsNullOrEmpty(searchString))
            {
                categories = categories.Where(c => c.Code.Contains(searchString)
                                       || c.Description.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "code":
                    categories = categories.OrderByDescending(c => c.Code);
                    break;
                case "desc":
                    categories = categories.OrderByDescending(c => c.Description);
                    break;
                default:
                    categories = categories.OrderBy(c => c.ID);
                    break;
            }

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);
            return View(categories.ToPagedList(pageNumber, pageSize));
        }

        // GET: Category/Details/5
        public ActionResult Details(int id)
        {
            var category = entity.Categories.Find(id);

            return View(category);
        }

        // GET: Category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                var category = new Category();

                if(collection.Count >0)
                {
                    category.Code = collection["Code"];
                    category.Description = collection["Description"];

                    if (category.Code.Trim() == string.Empty || category.Description.Trim() == string.Empty)
                    {
                        ModelState.AddModelError("", "Fill all fields");
                        return View();
                    }

                    var cat = entity.Categories.ToList().FindAll(b => b.Code == category.Code);

                    if (cat.Count() > 0)
                    {
                        ModelState.AddModelError("", "The code already exists.");
                        return View();
                    }

                    try
                    {
                        entity.Categories.Add(category);
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

        // GET: Category/Edit/5
        public ActionResult Edit(int id)
        {
            var category = entity.Categories.Find(id);

            return View(category);
        }

        // POST: Category/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                var category = entity.Categories.Find(id);

                if (collection.Count > 0)
                {
                    category.Code = collection["Code"];
                    category.Description = collection["Description"];

                    if (category.Code.Trim() == string.Empty || category.Description.Trim() == string.Empty)
                    {
                        ModelState.AddModelError("", "Fill all fields");
                        return View();
                    }

                    try
                    {
                        entity.Entry(category).State = EntityState.Modified;
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

        // GET: Category/Delete/5
        public ActionResult Delete(int id)
        {
            var category = entity.Categories.Find(id);

            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                var category = entity.Categories.Find(id);

                try
                {
                    entity.Categories.Remove(category);
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

        //// GET: Category/SubCategories/5
        //public ActionResult SubCategories(int categoryid, string sortOrder, string currentFilter, string searchString, int? page)
        //{
        //    Session["categoryID"] = categoryid;
        //    ViewBag.CategoryID = categoryid;
        //    ViewBag.CurrentCategory = "REY";
        //    ViewBag.CurrentSort = sortOrder;
        //    ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "code" : "";
        //    ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "desc" : "";

        //    if (searchString != null)
        //    {
        //        page = 1;
        //    }
        //    else
        //    {
        //        searchString = currentFilter;
        //    }

        //    ViewBag.CurrentFilter = searchString;


        //    var subcategories = from s in entity.SubCategories
        //                     select s;

        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        subcategories = subcategories.Where(s => (s.Code.Contains(searchString)
        //                               || s.Description.Contains(searchString)) && s.CategoryID == categoryid);
        //    }

        //    switch (sortOrder)
        //    {
        //        case "code":
        //            subcategories = subcategories.OrderByDescending(s => s.Code);
        //            break;
        //        case "desc":
        //            subcategories = subcategories.OrderByDescending(s => s.Description);
        //            break;
        //        default:
        //            subcategories = subcategories.OrderBy(s => s.ID);
        //            break;
        //    }
        //    int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
        //    int pageNumber = (page ?? 1);
        //    return View(subcategories.ToPagedList(pageNumber, pageSize));
        //}

        //// GET: Category/SubCategoryDetails/5
        //public ActionResult SubCategoryDetails(int id)
        //{
        //    ViewBag.CategoryID = Session["CategoryID"];

        //    var subcategory = entity.SubCategories.Find(id);

        //    return View(subcategory);
        //}

        //// GET: Category/SubCategoryCreate
        //public ActionResult SubCategoryCreate()
        //{
        //    ViewBag.CategoryID = Session["CategoryID"];
        //    return View();
        //}

        //// POST: Category/SubCategoryCreate
        //[HttpPost]
        //public ActionResult SubCategoryCreate(FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here
        //        var sub = new SubCategory();

        //        if (collection.Count > 0)
        //        {
        //            sub.CategoryID = Convert.ToInt32(Session["CategoryID"]);
        //            sub.Code = collection["Code"];
        //            sub.Description = collection["Description"];
        //            sub.Picture = collection["Picture"];
        //            try
        //            {
        //                entity.SubCategories.Add(sub);
        //                entity.SaveChanges();
        //            }
        //            catch { }
        //        }

        //        return RedirectToAction("SubCategories",new { categoryid = Convert.ToInt32(Session["CategoryID"]) });
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
        //// GET: Category/SubCategoryEdit/5
        //public ActionResult SubCategoryEdit(int id)
        //{
        //    ViewBag.CategoryID = Session["CategoryID"];
        //    var sub = entity.SubCategories.Find(id);

        //    return View(sub);
        //}

        //// POST: Category/SubCategoryEdit/5
        //[HttpPost]
        //public ActionResult SubCategoryEdit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here
        //        var sub = entity.SubCategories.Find(id);

        //        if (collection.Count > 0)
        //        {
        //            sub.CategoryID = Convert.ToInt32(Session["CategoryID"]);
        //            sub.Code = collection["Code"];
        //            sub.Description = collection["Description"];
        //            sub.Picture = collection["Picture"];

        //            try
        //            {
        //                entity.Entry(sub).State = EntityState.Modified;
        //                entity.SaveChanges();
        //            }
        //            catch { }
        //        }

        //        return RedirectToAction("SubCategories", new { categoryid = Convert.ToInt32(Session["CategoryID"]) });
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Category/SubCategoryDelete/5
        //public ActionResult SubCategoryDelete(int id)
        //{
        //    ViewBag.CategoryID = Session["CategoryID"];

        //    var sub = entity.SubCategories.Find(id);

        //    return View(sub);
        //}

        //// POST: Category/SubCategoryDelete/5
        //[HttpPost]
        //public ActionResult SubCategoryDelete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here
        //        var sub = entity.SubCategories.Find(id);

        //        try
        //        {
        //            entity.SubCategories.Remove(sub);
        //            entity.SaveChanges();
        //        }
        //        catch { }

        //        return RedirectToAction("SubCategories", new { categoryid = Convert.ToInt32(Session["CategoryID"]) });
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

    }
}
