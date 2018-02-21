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
        EmployeeRepository empRepo = new EmployeeRepository();

        [AccessCheckerForDisablingButtons(ModuleID = 18)]
        [AccessChecker(Action = 1, ModuleID = 18)]
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

       [AccessChecker(Action = 1, ModuleID = 18)]
        // GET: Employee/Details/5
        public ActionResult Details(int id)
        {
            //UserAccessVM uVM = new UserAccessVM();
            var employee = entity.Employees.Find(id);
            return View(employee);
        }

       [AccessChecker(Action = 2, ModuleID = 18)]
        // GET: Employee/Create
        public ActionResult Create(string module)
        {
            ViewBag.Action = 1;
            var modules = entity.Modules.Where(p => p.ID <= 10).ToList();
            var managementmodules = entity.Modules.Where(p => p.ID > 10).ToList();
            var employee = new Employee();
            //employee.CreateUserAccess(modules);
            employee.EmpID = empRepo.GenerateEmployeeID();
            if (module == "Transaction Module" || module == null)
            {
                //lagyan if wala makukuha
                employee.CreateUserAccess(modules);
            }
            else
            {
                //lagyan if wala makukuha
                employee.CreateUserAccess(managementmodules);
            }
            return View(employee);
        }

       [AccessChecker(Action = 2, ModuleID = 18)]
        // POST: Employee/Create
        [HttpPost]
        public ActionResult Create(Employee employee, string module)
        {
            try
            {
                // TODO: Add insert logic here
                if (employee.LastName != null && employee.FirstName != null && employee.Position != null)
                {
                    employee.EmpID = empRepo.GenerateEmployeeID();
                    entity.Employees.Add(employee);
                    entity.SaveChanges();

                    return RedirectToAction("Edit", new {id=employee.ID, module =module });
                }
                else
                {
                    ModelState.AddModelError("", "Fill all fields");
                }
            }
            catch
            {
            }

            var modules = entity.Modules.Where(p => p.ID <= 10).ToList();
            var managementmodules = entity.Modules.Where(p => p.ID > 10).ToList();
            if (module == "Transaction Module" || module == null)
            {
                employee.CreateUserAccess(modules);
            }
            else
            {
                employee.CreateUserAccess(managementmodules);
            }


            return View(employee);
        }
      
        [AccessChecker(Action = 2, ModuleID = 18)]
        // GET: Employee/Edit/5
        public ActionResult Edit(int? id, string module)
        {
            ViewBag.Action = 2;
            var employee = entity.Employees.Find(id);
            if (module == "Transaction Module" || module == null)
            {
                employee.UserAccesses = employee.UserAccesses.Where(p => p.ModuleID <= 10 && p.EmployeeID == employee.ID).ToList();
                if (employee.UserAccesses.Count==0)
                {
                    var modules = entity.Modules.Where(p => p.ID <= 10).ToList();
                    employee.CreateUserAccess(modules);
                }
            }
            else
            {      
                employee.UserAccesses = employee.UserAccesses.Where(p => p.ModuleID > 10 && p.EmployeeID == employee.ID).ToList();
                if (employee.UserAccesses.Count == 0)
                {
                    var managementmodules = entity.Modules.Where(p => p.ID > 10).ToList();
                    employee.CreateUserAccess(managementmodules);

                }
            }

            return View(employee);
        }

        [AccessChecker(Action = 2, ModuleID = 18)]
        // POST: Employee/Edit/5
        [HttpPost]
        public ActionResult Edit(Employee employee, string module)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (employee.LastName != null && employee.FirstName != null && employee.Position != null)
                    {
                        foreach(UserAccess ua in employee.UserAccesses)
                        {
                            if (ua.EmployeeID == null)
                            {
                                ua.EmployeeID = employee.ID;
                                entity.UserAccesses.Add(ua);
                                entity.SaveChanges();
                            }
                           
                                entity.Entry(ua).State = EntityState.Modified;
                            
                        }

                        entity.Entry(employee).State = EntityState.Modified;
                        entity.SaveChanges();

                       // return RedirectToAction("Index");
                    }
                    
                }
                catch (Exception E)
                {
                    E.ToString();
                }
            }
            if (module == "Transaction Module" || module == null)
            {
                return RedirectToAction("Edit", new { id = employee.ID, module = module });
            }
            else
            {
                foreach (var a in entity.Modules.Where(p => p.ID > 10).ToList())
                {
                    if (entity.UserAccesses.Where(p => p.EmployeeID == employee.ID && p.ModuleID == a.ID).Count() == 0)
                    {
                        UserAccess ua = new UserAccess();
                        ua.EmployeeID = employee.ID;
                        ua.ModuleID = a.ID;
                        entity.UserAccesses.Add(ua);
                        entity.SaveChanges();
                    }
                }

                return RedirectToAction("Edit", new { id = employee.ID, module = module });
            }

           // return View(employee);
        }

       [AccessChecker(Action = 3, ModuleID = 18)]
        // GET: Employee/Delete/5
        public ActionResult Delete(int id)
        {
            var employee = entity.Employees.Find(id);
            return View(employee);
        }

        [AccessChecker(Action = 3, ModuleID = 18)]
        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            //, FormCollection collection
            try
            {
                // TODO: Add delete logic here
                var employee = entity.Employees.Find(id);

                //try
                //{

                    var useraccess = entity.UserAccesses.ToList().FindAll(x => x.EmployeeID == employee.ID);
                    foreach (var ua in useraccess)
                        entity.UserAccesses.Remove(ua);

                    entity.Employees.Remove(employee);
                    entity.SaveChanges();

                    //entity.Employees.Remove(employee);
                //    //entity.SaveChanges();
                //}
                //catch(Exception ex) { }
            }
            catch (Exception ex)
            {
            }
            return RedirectToAction("Index");
        }
    }
}
