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
    public class UserController : Controller
    {
        MoostBrandEntities entity = new MoostBrandEntities();

        // GET: User
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "lastname" : "";
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "username" : "";
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "location" : "";
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "usertype" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;


            var users = from u in entity.Users
                            select u;

            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(u => u.Username.Contains(searchString)
                                       || u.Employee.LastName.Contains(searchString)
                                       || u.Location.Code.Contains(searchString)
                                       || u.UserType.Description.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "lastname":
                    users = users.OrderByDescending(u => u.Employee.LastName);
                    break;
                case "username":
                    users = users.OrderByDescending(u => u.Username);
                    break;
                case "location":
                    users = users.OrderByDescending(u => u.Location.Code);
                    break;
                case "usertype":
                    users = users.OrderByDescending(u => u.UserType.Description);
                    break;
                default:
                    users = users.OrderBy(u => u.ID);
                    break;
            }

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);
            return View(users.ToPagedList(pageNumber, pageSize));
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            var users = entity.Users.Find(id);
            return View(users);
        }

        // GET: User/Create
        public ActionResult Create()
        {
            ViewBag.Employees = entity.Employees.ToList();
            ViewBag.UserTypes = entity.UserTypes.ToList();
            ViewBag.Locations = entity.Locations.ToList();
            return View();
        }

        // POST: User/Create
        [HttpPost]
        public ActionResult Create(User user)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var usr = entity.Users.ToList().FindAll(b => b.Username == user.Username);

                    if (usr.Count() > 0)
                    {
                        ModelState.AddModelError("", "Username already exists.");
                    }
                    else
                    {
                        entity.Users.Add(user);
                        entity.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "Fill all fields");
                }
            }
            ViewBag.Employees = entity.Employees.ToList();
            ViewBag.UserTypes = entity.UserTypes.ToList();
            ViewBag.Locations = entity.Locations.ToList();
            return View(user);            
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.Employees = entity.Employees.ToList();
            ViewBag.UserTypes = entity.UserTypes.ToList();
            ViewBag.Locations = entity.Locations.ToList();

            var user = entity.Users.Find(id);
            return View(user);
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(User user)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    entity.Entry(user).State = EntityState.Modified;
                    entity.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    ModelState.AddModelError("", "Fill all fields");
                }
            }
            ViewBag.Employees = entity.Employees.ToList();
            ViewBag.UserTypes = entity.UserTypes.ToList();
            ViewBag.Locations = entity.Locations.ToList();
            return View(user);
        }

        // GET: User/Delete/5
        public ActionResult Delete(int id = 0)
        {
            var user = entity.Users.Find(id);
            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id = 0)
        {
            try
            {
                var user = entity.Users.Find(id);

                try
                {
                    entity.Users.Remove(user);
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
