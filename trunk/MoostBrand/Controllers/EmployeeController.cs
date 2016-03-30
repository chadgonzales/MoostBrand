using MoostBrand.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MoostBrand.Models;
using System.Data.Entity;
using PagedList;

namespace MoostBrand.Controllers
{
    public class EmployeeController : Controller
    {
        private DBContext _db = new DBContext();

        //
        // GET: /Employee/

        public ActionResult Index(int id = 1)
        {
            List<Employee> employees = _db.Employees.ToList();

            int? page = id;

            int pageSize = 1;
            int pageNumber = (page ?? 1);
            return View(employees.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /Employee/Details/5
        public ActionResult Details(int id = 0)
        {
            var employee = _db.Employees.Find(id);

            return View(employee);
        }

        //
        // GET: /Employee/Create
        public ActionResult Create()
        {
            var employee = new Employee();
            return View(employee);
        }

        //
        // POST: /Employee/Create
        [HttpPost]
        public ActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _db.Employees.Add(employee);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(employee);
        }

        //
        // GET: /Employee/Edit/5

        public ActionResult Edit(int id = 0)
        {
            var employee = _db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        //
        // POST: /Employee/Edit/5
        [HttpPost]
        public ActionResult Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                var emp = _db.Employees.Find(employee.ID);
                if (emp == null)
                {
                    ModelState.AddModelError("", "The employee doesn't exists.");
                }
                else
                {
                    _db.Entry(emp).CurrentValues.SetValues(employee);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(employee);
        }

        //
        // GET: /Employee/Delete/5

        public ActionResult Delete(int id = 0)
        {
            var employee = _db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        //
        // POST: /Employee/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var employee = _db.Employees.Include("Users").FirstOrDefault(e => e.ID == id);
            _db.Employees.Remove(employee);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }




        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}
