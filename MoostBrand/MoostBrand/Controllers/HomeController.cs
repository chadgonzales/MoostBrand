using MoostBrand.DAL;
using MoostBrand.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoostBrand.Controllers
{
    public class HomeController : Controller
    {
        private MoostBrandEntities entity = new MoostBrandEntities();
        public ActionResult Index()
        {
            var pr = new R();
            pr.RequestedDate = DateTime.Now;

            #region DROPDOWNS
            var employees = from s in entity.Employees
                            select new
                            {
                                ID = s.ID,
                                FullName = s.FirstName + " " + s.LastName
                            };
            ViewBag.RequisitionTypeID = new SelectList(entity.RequisitionTypes, "ID", "Type");
            ViewBag.RequestedBy = new SelectList(employees, "ID", "FullName");
            ViewBag.LocationID = new SelectList(entity.Locations, "ID", "Description");
            ViewBag.VendorID = new SelectList(entity.Vendors, "ID", "Name");
            ViewBag.ReservationTypeID = new SelectList(entity.ReservationTypes, "ID", "Type");
            ViewBag.ShipmentTypeID = new SelectList(entity.ShipmentTypes, "ID", "Type");
            ViewBag.DropShipID = new SelectList(entity.DropShipTypes, "ID", "Type");
            ViewBag.ReservedBy = new SelectList(employees, "ID", "FullName");
            ViewBag.ValidatedBy = new SelectList(employees, "ID", "FullName");
            ViewBag.Destination = new SelectList(entity.Locations, "ID", "Description");
            ViewBag.ApprovalStatus = new SelectList(entity.ApprovalStatus, "ID", "Status");
            ViewBag.ApprovedBy = new SelectList(employees, "ID", "FullName");
            #endregion

            return View(pr);
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Index(R pr)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    pr.ApprovalStatus = 1; //submitted

                    var checkPR = entity.Requisitions.Where(r => r.RefNumber == pr.RefNumber);

                    if (checkPR.Count() > 0)
                    {
                        ModelState.AddModelError("", "The ref number already exists.");
                    }
                    else
                    {
                        if (pr != null)
                        {
                            pr.IsSync = false;
                            pr.ID = 6;

                            Repo _r = new Repo();
                            string _result = await _r.PostLive(pr);

                            return RedirectToAction("Index");
                        }
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "There's an error.");
                }
            }

            #region DROPDOWNS
            var employees = from s in entity.Employees
                            select new
                            {
                                ID = s.ID,
                                FullName = s.FirstName + " " + s.LastName
                            };
            ViewBag.RequisitionTypeID = new SelectList(entity.RequisitionTypes, "ID", "Type", pr.RequisitionTypeID);
            ViewBag.RequestedBy = new SelectList(employees, "ID", "FullName", pr.RequestedBy);
            ViewBag.LocationID = new SelectList(entity.Locations, "ID", "Description", pr.LocationID);
            ViewBag.VendorID = new SelectList(entity.Vendors, "ID", "Name", pr.VendorID);
            ViewBag.ReservationTypeID = new SelectList(entity.ReservationTypes, "ID", "Type", pr.ReservationTypeID);
            ViewBag.ShipmentTypeID = new SelectList(entity.ShipmentTypes, "ID", "Type", pr.ShipmentTypeID);
            ViewBag.DropShipID = new SelectList(entity.DropShipTypes, "ID", "Type", pr.DropShipID);
            ViewBag.ReservedBy = new SelectList(employees, "ID", "FullName", pr.ReservedBy);
            ViewBag.ValidatedBy = new SelectList(employees, "ID", "FullName", pr.ValidatedBy);
            ViewBag.Destination = new SelectList(entity.Locations, "ID", "Description", pr.Destination);
            ViewBag.ApprovalStatus = new SelectList(entity.ApprovalStatus, "ID", "Status", pr.ApprovalStatus);
            ViewBag.ApprovedBy = new SelectList(employees, "ID", "FullName", pr.ApprovedBy);
            #endregion

            return View(pr);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}