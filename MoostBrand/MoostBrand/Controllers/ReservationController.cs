using System;
using System.Linq;
using System.Web.Mvc;
using MoostBrand.DAL;
using PagedList;
using System.Data.Entity;
using System.Configuration;
using MoostBrand.Models;
using System.Web.Routing;
using System.Collections.Generic;

namespace MoostBrand.Controllers
{
    [LoginChecker]
    public class ReservationController : Controller
    {
        RequisitionDetailsRepository reqDetailRepo = new RequisitionDetailsRepository();

        int UserID = 0;
        int UserType = 0;
        int ModuleID = 3;
        MoostBrandEntities entity;
        // GET: Customer

        #region COMMENTS

        //[HttpPost]
        //public JsonResult getCodeCommit(int Code)
        //{
        //    var instock = db.Inventories.Find(Code);
        //    var com = db.RequisitionDetails.Where(x => x.Requisition.RequisitionTypeID == 4 && x.AprovalStatusID == 2 && x.Requisition.LocationID == instock.LocationCode);
        //    var total = com.Sum(x => x.Quantity);
        //    if (total == null)
        //    {
        //        total = 0;
        //    }
        //    return Json(total, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //public JsonResult getCodePO(int Code)
        //{
        //    var instock = db.Inventories.Find(Code);
        //    var pur = db.RequisitionDetails.Where(x => x.Requisition.RequisitionTypeID == 1 && x.AprovalStatusID == 2 && x.Requisition.LocationID == instock.LocationCode);
        //    var total = pur.Sum(x => x.Quantity);
        //    if (total == null)
        //    {
        //        total = 0;
        //    }
        //    return Json(total, JsonRequestBehavior.AllowGet);
        //}
        #endregion

        #region  method
        public ReservationController()
        {
            entity = new MoostBrandEntities();
        }
        private string Generator(string prefix)
        {
            //Initiate objects & vars
            startR: Random random = new Random();
            string randomString = "";
            int randNumber = 0;

            //Loop ‘length’ times to generate a random number or character
            for (int i = 0; i < 6; i++)
            {
                if (i == 0)
                {
                    start: randNumber = random.Next(0, 9); //int {0-9}
                    if (randNumber == 0)
                        goto start;
                }
                else
                {
                    randNumber = random.Next(0, 9);
                }
                //append random char or digit to random string
                randomString = randomString + randNumber.ToString();
            }

            randomString = prefix + "-" + randomString;
            var pr = entity.Requisitions.ToList().FindAll(p => p.RefNumber == randomString);
            if (pr.Count() > 0)
            {
                goto startR;
            }

            //return the random string
            return randomString;
        }
        private Requisition setValue(Requisition pr)
        {
            pr.VendorID = null;
            pr.ReqTypeID = 2;
            pr.RequisitionTypeID = 4;
            pr.Destination = null;
            pr.PlateNumber = null;
            pr.Others = null;
            pr.TimeDeparted = null;
            pr.TimeArrived = null;
            if (pr.ShipmentTypeID == 2)
            {
                pr.DropShipID = null;
            }
            return pr;
        }
        private void AccessChecker(int action)
        {
            int UserID = Convert.ToInt32(Session["sessionuid"]);

            var access = entity.UserAccesses.FirstOrDefault(u => u.EmployeeID == UserID && u.ModuleID == ModuleID);

            if (action == 1) //CanView
            {
                if (!access.CanView)
                {
                    RedirectToAction("Index", "Home");
                }
            }
            else if (action == 2)//CanEdit
            {
                if (!access.CanEdit)
                {
                    RedirectToAction("Index", "Home");
                }
            }
            else if (action == 3)//CanDelete
            {
                if (!access.CanDelete)
                {
                    RedirectToAction("Index", "Home");
                }
            }
            else if (action == 4)//CanRequest
            {
                if (!access.CanRequest)
                {
                    RedirectToAction("Index", "Home");
                }
            }
            else if (action == 5)//Can Approve/Deny
            {
                if (!access.CanDecide)
                {
                    RedirectToAction("Index", "Home");
                }
            }
        }
        
        public int getPurchaseOrder(int itemID)
        {
            var requi = entity.RequisitionDetails.FirstOrDefault(x => x.AprovalStatusID == 2);
            int po = 0;
            if (requi != null)
            {
                var lstReqDetail = new List<RequisitionDetail>();

                lstReqDetail = entity.RequisitionDetails.Where(x => x.Requisition.RequisitionTypeID == 1 && x.AprovalStatusID == 2 && x.ItemID == itemID && x.Requisition.LocationID == requi.Requisition.LocationID).ToList();

                po = lstReqDetail.Sum(x => x.Quantity) ?? 0;
            }
            return po;
        }

        #endregion

        #region JSON
        [HttpPost]
        public JsonResult GetExp()
        {
            string message = "";
            string noErrJs = "";
            bool success = false;

            var lstreserved = entity.Requisitions.Where(x => x.Status == false && x.RequisitionTypeID == 4);

            foreach (var reserved in lstreserved)
            {
                DateTime startedDate = reserved.RequestedDate;
                DateTime validityDate = Convert.ToDateTime(reserved.ValidityOfReservation);
                DateTime notif = Convert.ToDateTime(reserved.DaysOfNotification);

                //DateTime expiryDateW = startedDate;
                DateTime expiryDateFD = startedDate.AddDays(7);
                if(reserved.PaymentStatusID == 1 && DateTime.Now == notif)
                {
                    noErrJs = "false";
                    message = "Days of notif";
                }
                if (reserved.PaymentStatusID == 1 && DateTime.Now > validityDate)
                {
                    reserved.ReservationStatus = "Cancelled";
                    entity.Entry(lstreserved).State = EntityState.Modified;
                    entity.SaveChanges();
                }
                else if (reserved.PaymentStatusID == 2 || reserved.PaymentStatusID == 3 || reserved.PaymentStatusID == 4 && DateTime.Now > expiryDateFD)
                {

                    noErrJs = "true";
                    message = "Reservation is required on" + startedDate;
                    //reserved.ReservationStatus = "Cancelled";
                    //db.Entry(lstreserved).State = EntityState.Modified;
                    //db.SaveChanges();
                }
            }
            return Json(new { message = message, noErrJs = noErrJs, success = success }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCategories(string name)
        {
            var categories = entity.Categories.Where(x => x.Description.Contains(name))
                            .Select(x => new
                            {
                                ID = x.ID,
                                Name = x.Description
                            });
            return Json(categories, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetItems(int catID, string name)
        {
            var items = entity.Items.Where(x => x.CategoryID == catID && x.Description.Contains(name))
                            .Select(x => new
                            {
                                ID = x.ID,
                                Code = x.Code,
                                Name = x.Description,
                                UOM = x.UnitOfMeasurement.Description
                            });
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetItemCode(int catID, string name)
        {
            var items = entity.Items.Where(x => x.CategoryID == catID && x.Code.Contains(name))
                            .Select(x => new
                            {
                                ID = x.ID,
                                Code = x.Code,
                                Name = x.Description,
                                UOM = x.UnitOfMeasurement.Description
                            });
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        //NEW
        [HttpGet]
        public JsonResult getInstock(int id, string Code)
        {
            int total = reqDetailRepo.getInstocked(id, Code);

            return Json(total, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult getCommit(int id, int ItemID)
        {
            int total = reqDetailRepo.getCommited(id, ItemID);

            return Json(total, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult getPO(int id, int ItemID)
        {
            var requi = entity.Requisitions.Find(id);
            //var requi = entity.RequisitionDetails.FirstOrDefault(x => x.AprovalStatusID == 2);

            int total = 0;
            if (requi != null)
            {
                var lstReqDetail = new List<RequisitionDetail>();

                lstReqDetail = entity.RequisitionDetails.Where(x => x.Requisition.RequisitionTypeID == 1 && x.AprovalStatusID == 2 && x.ItemID == ItemID && x.Requisition.LocationID == requi.LocationID).ToList();

                total = lstReqDetail.Sum(x => x.Quantity) ?? 0;
            }
            return Json(total, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region actionresult

        public ActionResult OrderCheckingIndex(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "reqno" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            //var prs = db.Requisitions.Where(x => x.Status == false && x.RequisitionTypeID == 4); //active

            var prs = entity.Requisitions.Where(x => x.Status == false && x.RequisitionTypeID == 4 && x.ApprovalStatus == 2);

            if (!String.IsNullOrEmpty(searchString))
            {
                prs = prs.Where(o => o.RequisitionTypeID == 4 && o.RefNumber.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "reqno":
                    prs = prs.OrderByDescending(o => o.RefNumber);
                    break;
                default:
                    prs = prs.OrderBy(o => o.ID);
                    break;
            }

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);
            return View(prs.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {


            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "reqno" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            //int locID = Convert.ToInt32(Session["locationID"]);
            //int UserID = Convert.ToInt32(Session["userID"]);

            //var user = db.Users.FirstOrDefault(x => x.ID == UserID);

            var prs = entity.Requisitions.Where(x => x.Status == false && x.RequisitionTypeID == 4); //active

            if (!String.IsNullOrEmpty(searchString))
            {
                prs = prs.Where(o => o.RequisitionTypeID == 4 && o.RefNumber.Contains(searchString) ||
                                                                   o.ApprovalStatu.Status.Contains(searchString) ||
                                                                   o.AuthorizedPerson.Contains(searchString) ||
                                                                    o.Customer.Contains(searchString) ||
                                                                    o.ReservationType.Type.Contains(searchString) ||
                                                                    o.InvoiceNumber.Contains(searchString) ||
                                                                    o.Location.Description.Contains(searchString) ||
                                                                    o.Location.Description.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "reqno":
                    prs = prs.OrderByDescending(o => o.RefNumber);
                    break;
                default:
                    prs = prs.OrderBy(o => o.ID);
                    break;
            }

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);

            //if (user.LocationID != 10)
            //{
            //    prs = prs.Where(x => x.LocationID == locID);
            //    return View(prs.ToPagedList(pageNumber, pageSize));
            //}
            //else
            //    return View(prs.ToPagedList(pageNumber, pageSize));

            return View(prs.ToPagedList(pageNumber, pageSize));
        }

        [AccessChecker(Action = 1, ModuleID = 9)]
        public ActionResult Details(int id = 0)
        {

            Session["requisitionId"] = id;

            //var pr = db.Requisitions.FirstOrDefault(r => r.ID == id && (r.RequestedBy == UserID || AcctType == 1 || AcctType == 4));

            var pr = entity.Requisitions.Find(id);
            if (pr == null)
            {
                return HttpNotFound();
            }

            return View(pr);
        }

        public ActionResult Create()
        {
            var pr = new Requisition();
            pr.RequestedDate = DateTime.Now;
            pr.ReqTypeID = 2;
            pr.RequisitionTypeID = 4;
            pr.RefNumber = Generator("CR");

            var employees = from s in entity.Employees
                            select new
                            {
                                ID = s.ID,
                                FullName = s.FirstName + " " + s.LastName
                            };

            var loc = entity.Locations.Where(x => x.ID != 10)
                      .Select(x => new
                      {
                          ID = x.ID,
                          Description = x.Description
                      });

            ViewBag.reqType = entity.RequisitionTypes.Where(model => model.ID == 4).ToList();
            ViewBag.PaymentStatusID = new SelectList(entity.PaymentStatus, "ID", "Status");
            ViewBag.ReqTypeID = new SelectList(entity.ReqTypes, "ID", "Type");
            ViewBag.RequisitionTypeID = new SelectList(entity.RequisitionTypes, "ID", "Type");
            ViewBag.RequestedBy = new SelectList(employees, "ID", "FullName");
            ViewBag.LocationID = new SelectList(loc, "ID", "Description");
            ViewBag.ReservationTypeID = new SelectList(entity.ReservationTypes, "ID", "Type");
            ViewBag.ShipmentTypeID = new SelectList(entity.ShipmentTypes, "ID", "Type");
            ViewBag.DropShipID = new SelectList(entity.DropShipTypes, "ID", "Type");
            ViewBag.ReservedBy = new SelectList(employees, "ID", "FullName");
            ViewBag.ValidatedBy = new SelectList(employees, "ID", "FullName");
            ViewBag.AuthorizedPerson = new SelectList(employees, "ID", "FullName");

            return View(pr);
        }

        [HttpPost]
        public ActionResult Create(Requisition req)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    req.ApprovalStatus = 1;
                    req.Status = false;

                    var newPR = setValue(req);

                    if (newPR != null)
                    {
                        newPR.IsSync = false;

                        entity.Requisitions.Add(newPR);
                        entity.SaveChanges();

                        //return RedirectToAction("Index");

                        return RedirectToAction("Details", new { id = req.ID });
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "There's an error.");
                }
            }
            var employees = from s in entity.Employees
                            select new
                            {
                                ID = s.ID,
                                FullName = s.FirstName + " " + s.LastName
                            };

            var loc = entity.Locations.Where(x => x.ID != 10)
          .Select(x => new
          {
              ID = x.ID,
              Description = x.Description
          });

            ViewBag.PaymentStatusID = new SelectList(entity.PaymentStatus, "ID", "Status");
            ViewBag.RequestedBy = new SelectList(employees, "ID", "FullName", req.RequestedBy);
            ViewBag.LocationID = new SelectList(loc, "ID", "Description", req.LocationID);
            ViewBag.ReservationTypeID = new SelectList(entity.ReservationTypes, "ID", "Type", req.ReservationTypeID);
            ViewBag.ShipmentTypeID = new SelectList(entity.ShipmentTypes, "ID", "Type", req.ShipmentTypeID);
            ViewBag.DropShipID = new SelectList(entity.DropShipTypes, "ID", "Type", req.DropShipID);
            ViewBag.ReservedBy = new SelectList(employees, "ID", "FullName", req.ReservedBy);
            ViewBag.ValidatedBy = new SelectList(employees, "ID", "FullName", req.ValidatedBy);
            ViewBag.ApprovalStatus = new SelectList(entity.ApprovalStatus, "ID", "Status", req.ApprovalStatus);
            ViewBag.AuthorizedPerson = new SelectList(employees, "ID", "FullName", req.AuthorizedPerson);
            return View(req);
        }

        [AccessChecker(Action = 2, ModuleID = 9)]
        public ActionResult Edit(int id)
        {
            //var pr = db.Requisitions.FirstOrDefault(r => r.ID == id && (r.RequestedBy == UserID || AcctType == 1 || AcctType == 4));
            var pr = entity.Requisitions.FirstOrDefault(r => r.ID == id);
            if (pr.ApprovalStatus == 1)
            {
                #region DROPDOWNS
                var employees = from s in entity.Employees
                                select new
                                {
                                    ID = s.ID,
                                    FullName = s.FirstName + " " + s.LastName
                                };

                var loc = entity.Locations.Where(x => x.ID != 10)
                            .Select(x => new
                            {
                                ID = x.ID,
                                Description = x.Description
                            });

                ViewBag.PaymentStatusID = new SelectList(entity.PaymentStatus, "ID", "Status");
                ViewBag.RequestedBy = new SelectList(employees, "ID", "FullName", pr.RequestedBy);
                ViewBag.LocationID = new SelectList(loc, "ID", "Description", pr.LocationID);
                ViewBag.ReservationTypeID = new SelectList(entity.ReservationTypes, "ID", "Type", pr.ReservationTypeID);
                ViewBag.ShipmentTypeID = new SelectList(entity.ShipmentTypes, "ID", "Type", pr.ShipmentTypeID);
                ViewBag.DropShipID = new SelectList(entity.DropShipTypes, "ID", "Type", pr.DropShipID);
                ViewBag.ReservedBy = new SelectList(employees, "ID", "FullName", pr.ReservedBy);
                ViewBag.ValidatedBy = new SelectList(employees, "ID", "FullName", pr.ValidatedBy);
                ViewBag.ApprovalStatus = new SelectList(entity.ApprovalStatus, "ID", "Status", pr.ApprovalStatus);
                ViewBag.ApprovedBy = new SelectList(employees, "ID", "FullName", pr.ApprovedBy);
                ViewBag.AuthorizedPerson = new SelectList(employees, "ID", "FullName", pr.AuthorizedPerson);
                #endregion

                return View(pr);
            }

            return RedirectToAction("Details", new { id = id });
        }

        // POST: PR/Edit/5
        [AccessChecker(Action = 2, ModuleID = 9)]
        [HttpPost]
        public ActionResult Edit(Requisition req)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //var r = db.Requisitions.FirstOrDefault(r1 => r1.ID == pr.ID && (r1.RequestedBy == UserID || AcctType == 1 || AcctType == 4)).ApprovalStatus;
                    var r = entity.Requisitions.FirstOrDefault(model => model.ID == req.ID);
                    if (r.ApprovalStatus == 1)
                    {

                        req.IsSync = false;
                        req.Status = false;
                        var newPR = setValue(req);
                        newPR.ApprovalStatus = r.ApprovalStatus;
                        entity.Entry(r).CurrentValues.SetValues(newPR);
                        entity.SaveChanges();

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return RedirectToAction("Details", new { id = req.ID });
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "There's an error.");
                    throw;
                }
            }

            #region DROPDOWNS
            var employees = from s in entity.Employees
                            select new
                            {
                                ID = s.ID,
                                FullName = s.FirstName + " " + s.LastName
                            };

            var loc = entity.Locations.Where(x => x.ID != 10)
                        .Select(x => new
                        {
                          ID = x.ID,
                          Description = x.Description
                        });

            ViewBag.PaymentStatusID = new SelectList(entity.PaymentStatus, "ID", "Status");
            ViewBag.RequestedBy = new SelectList(employees, "ID", "FullName", req.RequestedBy);
            ViewBag.LocationID = new SelectList(loc, "ID", "Description", req.LocationID);
            ViewBag.ReservationTypeID = new SelectList(entity.ReservationTypes, "ID", "Type", req.ReservationTypeID);
            ViewBag.ShipmentTypeID = new SelectList(entity.ShipmentTypes, "ID", "Type", req.ShipmentTypeID);
            ViewBag.DropShipID = new SelectList(entity.DropShipTypes, "ID", "Type", req.DropShipID);
            ViewBag.ReservedBy = new SelectList(employees, "ID", "FullName", req.ReservedBy);
            ViewBag.ValidatedBy = new SelectList(employees, "ID", "FullName", req.ValidatedBy);
            ViewBag.ApprovalStatus = new SelectList(entity.ApprovalStatus, "ID", "Status", req.ApprovalStatus);
            ViewBag.ApprovedBy = new SelectList(employees, "ID", "FullName", req.ApprovedBy);
            ViewBag.AuthorizedPerson = new SelectList(employees, "ID", "FullName", req.AuthorizedPerson);
            #endregion

            return View(req);
        }

        [AccessChecker(Action = 3, ModuleID = 9)]
        public ActionResult Delete(int id)
        {
            var pr = entity.Requisitions.FirstOrDefault(r => r.ID == id);
            if (pr == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (pr.ApprovalStatus == 1)
                {
                    return View(pr);
                }
            }

            return RedirectToAction("Details", new { id = pr.ID });
        }

        [AccessChecker(Action = 3, ModuleID = 9)]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {
            try
            {
                var pr = entity.Requisitions.Find(id);
                pr.Status = true;
                entity.Entry(pr).State = EntityState.Modified;
                entity.SaveChanges();

                //Delete requisition details
                entity.RequisitionDetails.RemoveRange(entity.RequisitionDetails.Where(x => x.RequisitionID == id));
                entity.SaveChanges();
                //==========================

                return RedirectToAction("Index");
            }
            catch
            {
            }
            return RedirectToAction("Delete", new { id });
        }
        #endregion

        #region actions 
        [AccessChecker(Action = 5, ModuleID = 9)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Approve(int id)
        {
            try
            {
                // TODO: Add delete logic here
                //var pr = db.Requisitions.FirstOrDefault(r => r.ID == id && (r.RequestedBy == UserID || AcctType == 1 || AcctType == 4));
                var pr = entity.Requisitions.Find(id);

                if (pr.RequisitionDetails.Count() > 0)
                {
                    pr.ApprovalStatus = 2;
                    pr.IsSync = false;

                    entity.Entry(pr).State = EntityState.Modified;
                    entity.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "There's no item");
                }
            }
            catch
            {
            }
            return View();
        }

        // POST: PR/Approve/5
        [AccessChecker(Action = 5, ModuleID = 9)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Revise(int id)
        {
            try
            {
                // TODO: Add delete logic here
                //var pr = db.Requisitions.FirstOrDefault(r => r.ID == id && (r.RequestedBy == UserID || AcctType == 1 || AcctType == 4));
                var pr = entity.Requisitions.Find(id);

                if (pr.RequisitionDetails.Count() > 0)
                {
                    pr.ApprovalStatus = 4;
                    pr.IsSync = false;

                    entity.Entry(pr).State = EntityState.Modified;
                    entity.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "There's no item");
                }
            }
            catch
            {
            }
            return View();
        }

        // POST: PR/Denied/5
        [AccessChecker(Action = 5, ModuleID = 9)]
        [HttpPost]
        public ActionResult Denied(int id)
        {
            try
            {
                // TODO: Add delete logic here
                var pr = entity.Requisitions.Find(id);
                pr.ApprovalStatus = 3;
                pr.IsSync = false;

                entity.Entry(pr).State = EntityState.Modified;
                entity.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: PR/Items/5
        [AccessChecker(Action = 1, ModuleID = 9)]
        public ActionResult Items(int id, int? page)
        {
            var items = entity.RequisitionDetails
                        .ToList()
                        .FindAll(rd => rd.RequisitionID == id);

            ViewBag.PRid = id;

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        // GET: PR/ApprovedItems/5
        [AccessChecker(Action = 1, ModuleID = 9)]
        public ActionResult ApprovedItems(int id, int? page)
        {
            var items = entity.RequisitionDetails
                        .ToList()
                        .FindAll(rd => rd.RequisitionID == id && rd.AprovalStatusID == 2);

            ViewBag.PRid = id;

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        // GET: PR/PendingItems/5
        [AccessChecker(Action = 1, ModuleID = 9)]
        public ActionResult PendingItems(int id, int? page)
        {
            int UserID = Convert.ToInt32(Session["sessionuid"]);
            int UserType = Convert.ToInt32(Session["usertype"]);

            var requisition = entity.Requisitions.FirstOrDefault(r => r.ID == id);
            if (requisition.RequestedBy != UserID && UserType != 1 && UserType != 4)
            {
                return RedirectToAction("Index", "Home");
            }

            var items = entity.RequisitionDetails
                        .ToList()
                        .FindAll(rd => rd.RequisitionID == id && rd.AprovalStatusID == 1 && (rd.Requisition.RequestedBy == UserID || UserType == 1 || UserType == 4));


            //var items = db.RequisitionDetails
            //            .ToList()
            //            .FindAll(rd => rd.RequisitionID == id && rd.AprovalStatusID == 1 && rd.Requisition.RequestedBy == UserID);

            ViewBag.PRid = id;
            ViewBag.RequestedBy =
            ViewBag.UserID = UserID;
            ViewBag.AcctType = UserType;
            ViewBag.IsApproved = requisition.ApprovalStatus;

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);
            return PartialView(items.ToPagedList(pageNumber, pageSize));
        }

        // GET: PR/DeniedItems/5
        [AccessChecker(Action = 1, ModuleID = 9)]
        public ActionResult DeniedItems(int id, int? page)
        {
            var items = entity.RequisitionDetails
                        .ToList()
                        .FindAll(rd => rd.RequisitionID == id && rd.AprovalStatusID == 3);

            ViewBag.PRid = id;

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        // POST: PR/DenyItem/5
        [AccessChecker(Action = 1, ModuleID = 9)]
        public ActionResult DenyItem(int id, int itemID)
        {
            try
            {
                var item = entity.RequisitionDetails.Find(itemID);
                if (item != null)
                {
                    item.AprovalStatusID = 3;
                    item.IsSync = false;

                    entity.Entry(item).State = EntityState.Modified;
                    entity.SaveChanges();
                }
            }
            catch
            {

            }
            return RedirectToAction("PendingItems", "Customer", new { id = id });
        }

        // GET: PR/DenyItemPartial/5
        public ActionResult DenyItemPartial(int id)
        {
            ViewBag.PRid = id;

            return PartialView();
        }

        // POST: PR/DenyItemPartial/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DenyItemPartial(int id, string Reason)
        {
            try
            {
                if (!String.IsNullOrEmpty(Reason))
                {
                    var req = entity.Requisitions.Find(id);
                    req.ApprovalStatus = 3;
                    req.Remarks = Reason;
                    entity.Entry(req).State = EntityState.Modified;

                    entity.SaveChanges();
                }
                else
                {
                    TempData["Error"] = "Reason is required";
                }
            }
            catch { TempData["Error"] = "There's an error"; }

            return RedirectToAction("Index");
        }

        [AccessChecker(Action = 1, ModuleID = 9)]
        public ActionResult ApproveItem(int id, int itemID, int reqID)
        {
            try
            {
                var req = entity.RequisitionDetails.Find(reqID);
                var itm = entity.Items.FirstOrDefault(x => x.ID == itemID);
                var inventory = entity.Inventories.FirstOrDefault(x => x.Description == itm.Description);

                var quantity = req.Quantity;

                if (req != null)
                {
                    req.AprovalStatusID = 2;
                    req.IsSync = false;
                    if (req.Requisition.ReqTypeID != 2)
                    {
                        req.Ordered = Convert.ToInt32(getPurchaseOrder(req.ItemID) + req.Ordered);
                    }
                    req.Committed = reqDetailRepo.getCommited(req.RequisitionID, itemID) + req.Quantity;
                    int avail = (Convert.ToInt32(req.InStock) + Convert.ToInt32(req.Ordered)) - Convert.ToInt32(req.Committed);
                    req.Available = avail;
                    //req.InStock -= req.Quantity;
                    entity.Entry(req).State = EntityState.Modified;
                    //db.SaveChanges();
                }
                //if (inventory != null)
                //{
                //    if (req.Requisition.ReqTypeID == 1)
                //    {
                //        inventory.Ordered = Convert.ToInt32(getPurchaseOrder(req.ItemID) + req.Ordered);
                //    }
                //    inventory.Committed = inventory.Committed.Value + quantity;
                //    entity.Entry(inventory).State = EntityState.Modified;
                //}
                entity.SaveChanges();
            }
            catch
            {

            }
            return RedirectToAction("PendingItems", "Reservation", new { id = id });
        }

        // GET: PR/AddItemPartial/5
        public ActionResult AddItemPartial(int id)
        {
            ViewBag.PRid = id;
            ViewBag.ItemID = new SelectList(entity.Items, "ID", "Description");

            return PartialView();
        }
        
        // POST: PR/AddItemPartial/5
        [HttpPost]
        public ActionResult AddItemPartial(int id, RequisitionDetail rd)
        {
            var itm = entity.Items.FirstOrDefault(x => x.ID == rd.ItemID);
            var itmID = rd.ItemID;
            var desc = itm.Description;
            try
            {
                // TODO: Add insert logic here
                rd.RequisitionID = id;
                rd.AprovalStatusID = 1; //submitted

                rd.Committed = reqDetailRepo.getCommited(id, itmID);

                rd.Ordered = reqDetailRepo.getPurchaseOrder(itmID);

                rd.InStock = reqDetailRepo.getInstocked(id, desc);

                rd.Available = rd.InStock + rd.Ordered - rd.Committed;
                var rd1 = entity.RequisitionDetails.Where(r => r.RequisitionID == rd.RequisitionID && r.ItemID == rd.ItemID).ToList();

                if (rd1.Count() > 0)
                {
                    TempData["PartialError"] = "Item is already in the list.";
                }
                else
                {
                    rd.IsSync = false;
                    entity.RequisitionDetails.Add(rd);
                    entity.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                TempData["PartialError"] = "There's an error.";
            }

            //ViewBag.PRid = id;
            //ViewBag.ItemID = new SelectList(db.Items, "ID", "Description", rd.ItemID);
            //ViewBag.AprovalStatusID = new SelectList(db.ApprovalStatus, "ID", "Status", rd.AprovalStatusID);

            //return RedirectToAction("PendingItems", new { id = id });
            return RedirectToAction("Details", new { id = id });
        }

        // GET: PR/EditItemPartial/5
        public ActionResult EditItemPartial(int id)
        {
            var rd = entity.RequisitionDetails.Find(id);

            ViewBag.AprovalStatusID = new SelectList(entity.ApprovalStatus, "ID", "Status", rd.AprovalStatusID);
            ViewBag.ItemID = new SelectList(entity.Items, "ID", "Description", rd.ItemID);

            return PartialView(rd);
        }

        // POST: PR/EditItemPartial/5
        [HttpPost]
        public ActionResult EditItemPartial(int id,RequisitionDetail rd)
        {
            try
            {
                var prq = entity.RequisitionDetails.Find(rd.ID);
                var itm = entity.Items.Find(rd.ItemID);
                if (prq.ItemID != rd.ItemID || prq.Quantity != rd.Quantity)
                {
                    rd.PreviousItemID = prq.ItemID;
                    rd.PreviousQuantity = prq.Quantity;

                    rd.Ordered = getPurchaseOrder(rd.ItemID);
                    rd.Committed = reqDetailRepo.getCommited(id, rd.ItemID);
                    rd.InStock = reqDetailRepo.getInstocked(id, itm.Description);

                    rd.Available = rd.InStock + rd.Ordered - rd.Committed;
                    rd.Remarks = rd.Remarks;
                }
                else
                {
                    rd.InStock = reqDetailRepo.getInstocked(id, itm.Description);
                    rd.Ordered = getPurchaseOrder(rd.ItemID);
                    rd.Committed = reqDetailRepo.getCommited(id, rd.ItemID);

                    rd.Available = rd.InStock + rd.Ordered - rd.Committed;
                    rd.Remarks = rd.Remarks;
                    rd.PreviousQuantity = prq.Quantity;
                    rd.PreviousItemID = prq.PreviousItemID; 
                }

                prq.ItemID = rd.ItemID;
                prq.Quantity = rd.Quantity;
                prq.InStock = rd.InStock;
                prq.Ordered = rd.Ordered;
                prq.Committed = rd.Committed;
                prq.Available = rd.Available;
                prq.Remarks = rd.Remarks;
                prq.PreviousItemID = rd.PreviousItemID;
                prq.PreviousQuantity = rd.PreviousQuantity;

                prq.IsSync = false;
                entity.Entry(prq).CurrentValues.SetValues(rd);
                entity.SaveChanges();
            }
            catch
            {
                //throw;
                TempData["PartialError"] = "There's an error.";
            }

            if (rd.AprovalStatusID == 1)
            {
                
                return RedirectToAction("Details", new { id = rd.RequisitionID });
            }
            return RedirectToAction("ApprovedItems", new { id = rd.RequisitionID });
        }

        // GET: PR/DeleteItemPartial/5
        public ActionResult DeleteItemPartial(int id)
        {
            var rd = entity.RequisitionDetails.Find(id);

            return PartialView(rd);
        }

        // POST: PR/DeleteItemPartial/5
        [HttpPost, ActionName("DeleteItemPartial")]
        public ActionResult DeleteItemPartialConfirm(int id)
        {
            var rd = entity.RequisitionDetails.Find(id);

            int? reqID = rd.RequisitionID;
            try
            {
                entity.RequisitionDetails.Remove(rd);
                entity.SaveChanges();
            }
            catch
            {
                TempData["PartialError"] = "There's an error.";
            }

            return RedirectToAction("Details", new { id = reqID });
        }
        #endregion

    }
}