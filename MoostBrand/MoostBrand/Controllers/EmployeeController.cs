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
    public class EmployeeController : Controller
    {
        MoostBrandEntities entity = new MoostBrandEntities();
        

        // GET: Employee
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "lastname" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;


            var employees = from e in entity.Employees
                             select e;

            if (!String.IsNullOrEmpty(searchString))
            {
                employees = employees.Where(e => e.LastName.Contains(searchString)
                                       || e.FirstName.Contains(searchString)
                                       || e.Position.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "lastname":
                    employees = employees.OrderByDescending(e => e.LastName);
                    break;
                default:
                    employees = employees.OrderBy(e => e.ID);
                    break;
            }
            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);
            return View(employees.ToPagedList(pageNumber, pageSize));
        }

        // GET: Employee/Details/5
        public ActionResult Details(int id)
        {
            var employee = entity.Employees.Find(id);
            return View(employee);
        }

        // GET: Employee/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                Employee employee = new Employee();
                // TODO: Add insert logic here

                if(collection.Count > 0)
                {
                    employee.LastName = collection["LastName"];
                    employee.FirstName = collection["FirstName"];
                    employee.Position = collection["Position"];

                    if (employee.LastName.Trim() == string.Empty || employee.FirstName.Trim() == string.Empty || employee.Position.Trim() == string.Empty)
                    {
                        ModelState.AddModelError("", "Fill all fields");
                        return View();
                    }

                    try
                    {
                        entity.Employees.Add(employee);
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

        // GET: Employee/Edit/5
        public ActionResult Edit(int id)
        {
            var employee = entity.Employees.Find(id);

            return View(employee);
        }

        // POST: Employee/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                var employee = entity.Employees.Find(id);

                if(collection.Count > 0)
                {
                    employee.LastName = collection["LastName"];
                    employee.FirstName = collection["FirstName"];
                    employee.Position = collection["Position"];

                    if (employee.LastName.Trim() == string.Empty || employee.FirstName.Trim() == string.Empty || employee.Position.Trim() == string.Empty)
                    {
                        ModelState.AddModelError("", "Fill all fields");
                        return View();
                    }

                    try
                    {
                        entity.Entry(employee).State = EntityState.Modified;
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

        // GET: Employee/Delete/5
        public ActionResult Delete(int id)
        {
            var employee = entity.Employees.Find(id);
            return View(employee);
        }

        // POST: Employee/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                var employee = entity.Employees.Find(id);

                try
                {
                    entity.Employees.Remove(employee);
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
