using MoostBrand.DAL;
using MoostBrand.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoostBrand.Controllers
{
    public class AccountController : Controller
    {
        MoostBrandEntities entity = new MoostBrandEntities();

        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        // GET: Account/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = entity.Users.FirstOrDefault(u => u.Username == login.Username && u.Password == login.Password);
                    if (user != null)
                    {
                        Session["sessionuid"] = user.EmployeeID;
                        Session["usertype"] = user.UserTypeID;
                        Session["username"] = user.Username;

                        return RedirectToAction("Index", "PR");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid username or password");
                    }
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "There's an error");
                }
            }

            return View(login);
        }
    }
}