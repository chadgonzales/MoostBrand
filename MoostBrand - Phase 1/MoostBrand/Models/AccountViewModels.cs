using MoostBrand.DAL;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MoostBrand.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    #region Login Checker
    public class LoginChecker : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext != null)
            {
                HttpSessionStateBase Session = filterContext.HttpContext.Session;
                if (Session["sessionuid"] == null)
                {
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary
                        {
                            { "controller", "Account" },
                            { "action", "Login" }
                        });
                }

            }

        }
    }
    #endregion

    #region User Access Checker
    public class AccessChecker : ActionFilterAttribute
    {
        MoostBrandEntities entity = new MoostBrandEntities();

        public int Action { get; set; }
        public int ModuleID { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext != null)
            {
                HttpSessionStateBase Session = filterContext.HttpContext.Session;
                if (Session["sessionuid"] == null)
                {
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary
                        {
                            { "controller", "Account" },
                            { "action", "Login" }
                        });
                }
                else
                {
                    int UserID = System.Convert.ToInt32(Session["sessionuid"]);

                    var access = entity.UserAccesses.FirstOrDefault(u => u.EmployeeID == UserID && u.ModuleID == ModuleID);
                    try
                    {
                        entity.Entry(access).Reload();
                   

                    switch (Action)
                    {
                        case 1: //CanView
                            if (!access.CanView)
                            {
                                filterContext.Result = new RedirectToRouteResult(
                                    new RouteValueDictionary
                                    {
                                    { "controller", "Home" },
                                    { "action", "Denied" }
                                    });
                            }
                            break;

                        case 2: //CanEdit
                            if (!access.CanEdit)
                            {
                                filterContext.Result = new RedirectToRouteResult(
                                    new RouteValueDictionary
                                    {
                                    { "controller", "Home" },
                                    { "action", "Denied" }
                                    });
                            }
                            break;

                        case 3: //CanDelete
                            if (!access.CanDelete)
                            {
                                filterContext.Result = new RedirectToRouteResult(
                                    new RouteValueDictionary
                                    {
                                    { "controller", "Home" },
                                    { "action", "Denied" }
                                    });
                            }
                            break;

                        case 4: //CanRequest
                            if (!access.CanRequest)
                            {
                                filterContext.Result = new RedirectToRouteResult(
                                    new RouteValueDictionary
                                    {
                                    { "controller", "Home" },
                                    { "action", "Denied" }
                                    });

                                    Session["hideaccess"] = "hidden";
                             }
                            break;

                        case 5: //CanDecide
                            if (!access.CanDecide)
                            {
                                filterContext.Result = new RedirectToRouteResult(
                                    new RouteValueDictionary
                                    {
                                    { "controller", "Home" },
                                    { "action", "Denied" }
                                    });
                            }
                            break;

                        default:
                            filterContext.Result = new RedirectToRouteResult(
                                    new RouteValueDictionary
                                    {
                                    { "controller", "Home" },
                                    { "action", "Denied" }
                                    });
                            break;
                    }
                    }
                    catch
                    {
                        filterContext.Result = new RedirectToRouteResult(
                                    new RouteValueDictionary
                                    {
                                    { "controller", "Home" },
                                    { "action", "Denied" }
                                    });

                    }

                }

            }

        }
    }
    #endregion

    #region User Access For Disabling of Buttons
    public class AccessCheckerForDisablingButtons : ActionFilterAttribute
    {
        MoostBrandEntities entity = new MoostBrandEntities();
        public int ModuleID { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext != null)
            {
                HttpSessionStateBase Session = filterContext.HttpContext.Session;
                if (Session["sessionuid"] == null)
                {
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary
                        {
                            { "controller", "Account" },
                            { "action", "Login" }
                        });
                }
                else
                {
                    int UserID = System.Convert.ToInt32(Session["sessionuid"]);

                    var access = entity.UserAccesses.FirstOrDefault(u => u.EmployeeID == UserID && u.ModuleID == ModuleID);
                    try
                    {
                        entity.Entry(access).Reload();

                        if (!access.CanView)
                        {
                            Session["canview"] = "hidden";
                        }
                        else
                        {
                            Session["canview"] = "";
                        }
               
                        if (!access.CanEdit)
                        {
                            Session["canedit"] = "hidden";
                        }
                        else
                        {
                            Session["canedit"] = "";
                        }

                        if (!access.CanDelete)
                        {
                            Session["candelete"] = "hidden";
                        }
                        else
                        {
                            Session["candelete"] = "";
                        }

                        if (!access.CanRequest)
                        {
                            Session["canrequest"] = "hidden";
                        }
                        else
                        {
                            Session["canrequest"] = "";
                        }

                        if (!access.CanDecide)
                        {
                            Session["candecide"] = "hidden";
                        }
                        else
                        {
                            Session["candecide"] = "";
                        }

                    }
                    catch
                    {
                        filterContext.Result = new RedirectToRouteResult(
                                    new RouteValueDictionary
                                    {
                                    { "controller", "Home" },
                                    { "action", "Denied" }
                                    });

                    }

                }

            }

        }
    }
    #endregion
}
