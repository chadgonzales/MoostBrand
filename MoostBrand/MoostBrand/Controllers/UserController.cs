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
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                var user = new User();
                
                if(collection.Count > 0)
                {
                    user.Username = collection["Username"];
                    user.Password = collection["Password"];
                    user.UserTypeID = Convert.ToInt32(collection["UserTypeID"]);
                    user.Department = collection["Department"];
                    user.EmployeeID = Convert.ToInt32(collection["EmployeeID"]);
                    user.LocationID = Convert.ToInt32(collection["LocationID"]);

                    if (user.Username.Trim() == string.Empty ||
                        user.Password.Trim() == string.Empty ||
                        user.UserTypeID == 0 ||
                        user.Department.Trim() == string.Empty ||
                        user.EmployeeID == 0 ||
                        user.LocationID == 0)
                    {
                        ModelState.AddModelError("", "Fill all fields");
                        return View();
                    }

                    var usr = entity.Colors.ToList().FindAll(b => b.Code == user.Username);

                    if (usr.Count() > 0)
                    {
                        ModelState.AddModelError("", "Username already exists.");
                        return View();
                    }

                    try
                    {
                        entity.Users.Add(user);
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
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                var user = entity.Users.Find(id);
                
                if(collection.Count >0)
                {
                    user.Username = collection["Username"];
                    user.Password = collection["Password"];
                    user.UserTypeID = Convert.ToInt32(collection["UserTypeID"]);
                    user.Department = collection["Department"];
                    user.EmployeeID = Convert.ToInt32(collection["EmployeeID"]);
                    user.LocationID = Convert.ToInt32(collection["LocationID"]);

                    if (user.Username.Trim() == string.Empty ||
                        user.Password.Trim() == string.Empty ||
                        user.UserTypeID == 0 ||
                        user.Department.Trim() == string.Empty ||
                        user.EmployeeID == 0 ||
                        user.LocationID == 0)
                    {
                        ModelState.AddModelError("", "Fill all fields");
                        return View();
                    }

                    try
                    {
                        entity.Entry(user).State = EntityState.Modified;
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

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            var user = entity.Users.Find(id);
            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
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
