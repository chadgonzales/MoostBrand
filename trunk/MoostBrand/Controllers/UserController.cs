using MoostBrand.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MoostBrand.Models;
using PagedList;

namespace MoostBrand.Controllers
{
    public class UserController : Controller
    {
        private DBContext _db = new DBContext();

        //
        // GET: /User/

        public ActionResult Index(int id = 1)
        {
            List<User> users = _db.Users.ToList();

            int? page = id;

            int pageSize = 1;
            int pageNumber = (page ?? 1);
            return View(users.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /User/Details/5

        public ActionResult Details(int id = 0)
        {
            var user = _db.Users.Find(id);

            return View(user);
        }

        //
        // GET: /User/Create

        public ActionResult Create()
        {
            var emps = _db.Employees.ToList();

            IEnumerable<SelectListItem> employees = from e in emps
                                                    select new SelectListItem
                                                    {
                                                        Value = e.ID.ToString(),
                                                        Text = string.Format("{0}, {1}", e.LastName, e.FirstName) 
                                                    };
            ViewBag.EmployeeID = new SelectList(employees, "Value", "Text");
            ViewBag.UserTypeID = new SelectList(_db.UserTypes, "ID", "Description");
            var users = new User();
            return View(users);
        }

        //
        // POST: /User/Create

        [HttpPost]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                var em = _db.Employees.FirstOrDefault(e => e.ID == user.EmployeeID);
                if (em == null)
                {
                    return HttpNotFound();
                }

                var emp = _db.Users.FirstOrDefault(u => u.EmployeeID == user.EmployeeID);
                if (emp != null)
                {
                    ModelState.AddModelError("", "The employee has an existing account.");
                }
                else
                {
                    var usr = _db.Users.FirstOrDefault(u => u.Username == user.Username);
                    if (usr != null)
                    {
                        ModelState.AddModelError("", "The username already exists.");
                    }
                    else
                    {
                        _db.Users.Add(user);
                        _db.SaveChanges();

                        return RedirectToAction("Index");
                    }
                }
            }

            var emps = _db.Employees.ToList();

            IEnumerable<SelectListItem> employees = from e in emps
                                                    select new SelectListItem
                                                    {
                                                        Value = e.ID.ToString(),
                                                        Text = string.Format("{0}, {1}", e.LastName, e.FirstName)
                                                    };
            ViewBag.EmployeeID = new SelectList(employees, "Value", "Text");
            ViewBag.UserTypeID = new SelectList(_db.UserTypes, "ID", "Description");

            return View(user);
        }

        //
        // GET: /User/Edit/5

        public ActionResult Edit(int id = 0)
        {
            var user = _db.Users.FirstOrDefault(u => u.EmployeeID == id);
            if (user == null)
            {
                return HttpNotFound();
            }

            ViewBag.UserTypeID = new SelectList(_db.UserTypes, "ID", "Description", user.UserTypeID);

            return View(user);
        }

        //
        // POST: /User/Edit/5

        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                var usr = _db.Users.FirstOrDefault(u => u.ID == user.ID && u.EmployeeID == user.EmployeeID && u.Username == user.Username);
                if (usr == null)
                {
                    ModelState.AddModelError("", "The user doesn't exists.");
                }
                else
                {
                    _db.Entry(usr).CurrentValues.SetValues(user);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            ViewBag.UserTypeID = new SelectList(_db.UserTypes, "ID", "Description", user.UserTypeID);

            return View(user);
        }

        //
        // GET: /User/Delete/5

        public ActionResult Delete(int id = 0)
        {
            var user = _db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /User/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var user = _db.Users.Find(id);
            _db.Users.Remove(user);
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
