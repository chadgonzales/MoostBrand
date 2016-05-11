using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MoostBrand.DAL;
using System.Data.Entity;
using PagedList;
using System.Configuration;

namespace MoostBrand.Controllers
{
    public class VendorController : Controller
    {
        MoostBrandEntities entity = new MoostBrandEntities();


        // GET: Vendor
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "code" : "";
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;


            var vendors = from v in entity.Vendors
                            select v;

            if (!String.IsNullOrEmpty(searchString))
            {
                vendors = vendors.Where(v => v.Code.Contains(searchString)
                                       || v.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "code":
                    vendors = vendors.OrderByDescending(v => v.Code);
                    break;
                case "name":
                    vendors = vendors.OrderByDescending(v => v.Name);
                    break;
                default:
                    vendors = vendors.OrderBy(v => v.ID);
                    break;
            }
            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);
            return View(vendors.ToPagedList(pageNumber, pageSize));
        }

        // GET: Vendor/Details/5
        public ActionResult Details(int id)
        {
            var vendor = entity.Vendors.Find(id);
            return View(vendor);
        }

        // GET: Vendor/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Vendor/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                var vendor = new Vendor();
                // TODO: Add insert logic here

                if (collection.Count > 0)
                {
                    vendor.Code = collection["Code"];
                    vendor.Name = collection["Name"];
                    vendor.GeneralName = collection["GeneralName"];
                    vendor.Attn = collection["Attn"];
                    vendor.Address = collection["Address"];
                    vendor.City = collection["City"];
                    vendor.ContactNo = collection["ContactNo"];
                    vendor.Email = collection["Email"];

                    if (vendor.Code.Trim() == string.Empty ||
                        vendor.Name.Trim() == string.Empty ||
                        vendor.GeneralName.Trim() == string.Empty ||
                        vendor.Attn.Trim() == string.Empty ||
                        vendor.Address.Trim() == string.Empty ||
                        vendor.City.Trim() == string.Empty ||
                        vendor.ContactNo.Trim() == string.Empty ||
                        vendor.Email.Trim() == string.Empty)
                    {
                        ModelState.AddModelError("", "Fill all fields");
                        return View();
                    }

                    var vendr = entity.Colors.ToList().FindAll(b => b.Code == vendor.Code);

                    if (vendr.Count() > 0)
                    {
                        ModelState.AddModelError("", "The code already exists.");
                        return View();
                    }

                    try
                    {
                        entity.Vendors.Add(vendor);
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

        // GET: Vendor/Edit/5
        public ActionResult Edit(int id)
        {
            var vendor = entity.Vendors.Find(id);

            return View(vendor);
        }

        // POST: Vendor/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                var vendor = entity.Vendors.Find(id);

                if (collection.Count > 0)
                {
                    vendor.Code = collection["Code"];
                    vendor.Name = collection["Name"];
                    vendor.GeneralName = collection["GeneralName"];
                    vendor.Attn = collection["Attn"];
                    vendor.Address = collection["Address"];
                    vendor.City = collection["City"];
                    vendor.ContactNo = collection["ContactNo"];
                    vendor.Email = collection["Email"];

                    if (vendor.Code.Trim() == string.Empty ||
                        vendor.Name.Trim() == string.Empty ||
                        vendor.GeneralName.Trim() == string.Empty ||
                        vendor.Attn.Trim() == string.Empty ||
                        vendor.Address.Trim() == string.Empty ||
                        vendor.City.Trim() == string.Empty ||
                        vendor.ContactNo.Trim() == string.Empty ||
                        vendor.Email.Trim() == string.Empty)
                    {
                        ModelState.AddModelError("", "Fill all fields");
                        return View();
                    }

                    try
                    {
                        entity.Entry(vendor).State = EntityState.Modified;
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

        // GET: Vendor/Delete/5
        public ActionResult Delete(int id)
        {
            var vendor = entity.Vendors.Find(id);
            return View(vendor);
        }

        // POST: Vendor/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                var vendor = entity.Vendors.Find(id);

                try
                {
                    entity.Vendors.Remove(vendor);
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
