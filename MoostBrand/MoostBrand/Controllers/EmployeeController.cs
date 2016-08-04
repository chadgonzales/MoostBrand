using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MoostBrand.DAL;
using System.Data.Entity;
using PagedList;
using System.Configuration;
using MoostBrand.Models;

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
            //UserAccessVM uVM = new UserAccessVM();
            var employee = entity.Employees.Find(id);
            return View(employee);
        }

        // GET: Employee/Create
        public ActionResult Create()
        {
            var modules = entity.Modules.ToList();
            var employee = new Employee();
            employee.CreateUserAccess(modules);
            return View(employee);
        }

        // POST: Employee/Create
        [HttpPost]
        public ActionResult Create(Employee employee)
        {
            try
            {
                // TODO: Add insert logic here
                if (employee.LastName != null && employee.FirstName != null && employee.Position != null)
                {
                    entity.Employees.Add(employee);
                    entity.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Fill all fields");
                }
            }
            catch
            {
            }

            var modules = entity.Modules.ToList();
            employee.CreateUserAccess(modules);

            return View(employee);
        }

        // GET: Employee/Edit/5
        public ActionResult Edit(int id)
        {
            var employee = entity.Employees.Find(id);

            return View(employee);
        }

        // POST: Employee/Edit/5
        [HttpPost]
        public ActionResult Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (employee.LastName != null && employee.FirstName != null && employee.Position != null)
                    {
                        foreach(UserAccess ua in employee.UserAccesses)
                        {
                            entity.Entry(ua).State = EntityState.Modified;
                        }

                        entity.Entry(employee).State = EntityState.Modified;
                        entity.SaveChanges();

                        return RedirectToAction("Index");
                    }
                    
                }
                catch { }
            }

            return View(employee);
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
