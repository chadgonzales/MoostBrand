using System;
using System.Linq;
using System.Web.Mvc;
using MoostBrand.DAL;
using PagedList;
using System.Data.Entity;
using System.Configuration;
using MoostBrand.Models;
using System.Web.Routing;

namespace MoostBrand.Controllers
{
    [LoginChecker]
    public class ReservationController : Controller
    {
        int UserID = 0;
        int UserType = 0;
        int ModuleID = 3;
        MoostBrandEntities db;
        // GET: Customer
        #region  method
        public ReservationController()
        {
            db = new MoostBrandEntities();
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
            var pr = db.Requisitions.ToList().FindAll(p => p.RefNumber == randomString);
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

            var access = db.UserAccesses.FirstOrDefault(u => u.EmployeeID == UserID && u.ModuleID == ModuleID);

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
        [AccessChecker(Action = 1, ModuleID = 3)]
        #endregion
        #region actionresult
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

            var prs = db.Requisitions.Where(x => x.Status == false && x.RequisitionTypeID == 4); //active

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

        [AccessChecker(Action = 1, ModuleID = 3)]
        public ActionResult Details(int id = 0)
        {
            //var pr = db.Requisitions.FirstOrDefault(r => r.ID == id && (r.RequestedBy == UserID || AcctType == 1 || AcctType == 4));

            var pr = db.Requisitions.Find(id);
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

            var employees = from s in db.Employees
                            select new
                            {
                                ID = s.ID,
                                FullName = s.FirstName + " " + s.LastName
                            };
            ViewBag.reqType = db.RequisitionTypes.Where(model => model.ID == 4).ToList();

            ViewBag.ReqTypeID = new SelectList(db.ReqTypes, "ID", "Type");
            ViewBag.RequisitionTypeID = new SelectList(db.RequisitionTypes, "ID", "Type");
            ViewBag.RequestedBy = new SelectList(employees, "ID", "FullName");
            ViewBag.LocationID = new SelectList(db.Locations, "ID", "Description");
            ViewBag.ReservationTypeID = new SelectList(db.ReservationTypes, "ID", "Type");
            ViewBag.ShipmentTypeID = new SelectList(db.ShipmentTypes, "ID", "Type");
            ViewBag.DropShipID = new SelectList(db.DropShipTypes, "ID", "Type");
            ViewBag.ReservedBy = new SelectList(employees, "ID", "FullName");
            ViewBag.ValidatedBy = new SelectList(employees, "ID", "FullName");

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

                        db.Requisitions.Add(newPR);
                        db.SaveChanges();

                        return RedirectToAction("Index");
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "There's an error.");
                }
            }
            var employees = from s in db.Employees
                            select new
                            {
                                ID = s.ID,
                                FullName = s.FirstName + " " + s.LastName
                            };

            ViewBag.RequestedBy = new SelectList(employees, "ID", "FullName", req.RequestedBy);
            ViewBag.LocationID = new SelectList(db.Locations, "ID", "Description", req.LocationID);
            ViewBag.ReservationTypeID = new SelectList(db.ReservationTypes, "ID", "Type", req.ReservationTypeID);
            ViewBag.ShipmentTypeID = new SelectList(db.ShipmentTypes, "ID", "Type", req.ShipmentTypeID);
            ViewBag.DropShipID = new SelectList(db.DropShipTypes, "ID", "Type", req.DropShipID);
            ViewBag.ReservedBy = new SelectList(employees, "ID", "FullName", req.ReservedBy);
            ViewBag.ValidatedBy = new SelectList(employees, "ID", "FullName", req.ValidatedBy);
            ViewBag.ApprovalStatus = new SelectList(db.ApprovalStatus, "ID", "Status", req.ApprovalStatus);
            return View(req);
        }

        [AccessChecker(Action = 2, ModuleID = 3)]
        public ActionResult Edit(int id)
        {
            //var pr = db.Requisitions.FirstOrDefault(r => r.ID == id && (r.RequestedBy == UserID || AcctType == 1 || AcctType == 4));
            var pr = db.Requisitions.FirstOrDefault(r => r.ID == id);
            if (pr.ApprovalStatus == 1)
            {
                #region DROPDOWNS
                var employees = from s in db.Employees
                                select new
                                {
                                    ID = s.ID,
                                    FullName = s.FirstName + " " + s.LastName
                                };
                ViewBag.RequestedBy = new SelectList(employees, "ID", "FullName", pr.RequestedBy);
                ViewBag.LocationID = new SelectList(db.Locations, "ID", "Description", pr.LocationID);
                ViewBag.ReservationTypeID = new SelectList(db.ReservationTypes, "ID", "Type", pr.ReservationTypeID);
                ViewBag.ShipmentTypeID = new SelectList(db.ShipmentTypes, "ID", "Type", pr.ShipmentTypeID);
                ViewBag.DropShipID = new SelectList(db.DropShipTypes, "ID", "Type", pr.DropShipID);
                ViewBag.ReservedBy = new SelectList(employees, "ID", "FullName", pr.ReservedBy);
                ViewBag.ValidatedBy = new SelectList(employees, "ID", "FullName", pr.ValidatedBy);
                ViewBag.ApprovalStatus = new SelectList(db.ApprovalStatus, "ID", "Status", pr.ApprovalStatus);
                ViewBag.ApprovedBy = new SelectList(employees, "ID", "FullName", pr.ApprovedBy);
                #endregion

                return View(pr);
            }

            return RedirectToAction("Details", new { id = id });
        }

        // POST: PR/Edit/5
        [AccessChecker(Action = 2, ModuleID = 3)]
        [HttpPost]
        public ActionResult Edit(Requisition req)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //var r = db.Requisitions.FirstOrDefault(r1 => r1.ID == pr.ID && (r1.RequestedBy == UserID || AcctType == 1 || AcctType == 4)).ApprovalStatus;
                    var r = db.Requisitions.FirstOrDefault(model => model.ID == req.ID);
                    if (r.ApprovalStatus == 1)
                    {

                        req.IsSync = false;
                        req.Status = false;
                        var newPR = setValue(req);
                        newPR.ApprovalStatus = r.ApprovalStatus;
                        db.Entry(r).CurrentValues.SetValues(newPR);
                        db.SaveChanges();

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
            var employees = from s in db.Employees
                            select new
                            {
                                ID = s.ID,
                                FullName = s.FirstName + " " + s.LastName
                            };
            ViewBag.RequestedBy = new SelectList(employees, "ID", "FullName", req.RequestedBy);
            ViewBag.LocationID = new SelectList(db.Locations, "ID", "Description", req.LocationID);
            ViewBag.ReservationTypeID = new SelectList(db.ReservationTypes, "ID", "Type", req.ReservationTypeID);
            ViewBag.ShipmentTypeID = new SelectList(db.ShipmentTypes, "ID", "Type", req.ShipmentTypeID);
            ViewBag.DropShipID = new SelectList(db.DropShipTypes, "ID", "Type", req.DropShipID);
            ViewBag.ReservedBy = new SelectList(employees, "ID", "FullName", req.ReservedBy);
            ViewBag.ValidatedBy = new SelectList(employees, "ID", "FullName", req.ValidatedBy);
            ViewBag.ApprovalStatus = new SelectList(db.ApprovalStatus, "ID", "Status", req.ApprovalStatus);
            ViewBag.ApprovedBy = new SelectList(employees, "ID", "FullName", req.ApprovedBy);
            #endregion

            return View(req);
        }
        [AccessChecker(Action = 3, ModuleID = 3)]
        public ActionResult Delete(int id)
        {
            var pr = db.Requisitions.FirstOrDefault(r => r.ID == id);
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
        [AccessChecker(Action = 3, ModuleID = 3)]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {
            try
            {
                var pr = db.Requisitions.Find(id);
                pr.Status = true;
                db.Entry(pr).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
            }
            return RedirectToAction("Delete", new { id });
        }
        #endregion
        #region actions 
        [AccessChecker(Action = 5, ModuleID = 3)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Approve(int id)
        {
            try
            {
                // TODO: Add delete logic here
                //var pr = db.Requisitions.FirstOrDefault(r => r.ID == id && (r.RequestedBy == UserID || AcctType == 1 || AcctType == 4));
                var pr = db.Requisitions.Find(id);

                if (pr.RequisitionDetails.Count() > 0)
                {
                    pr.ApprovalStatus = 2;
                    pr.IsSync = false;

                    db.Entry(pr).State = EntityState.Modified;
                    db.SaveChanges();

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
        [AccessChecker(Action = 5, ModuleID = 3)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Revise(int id)
        {
            try
            {
                // TODO: Add delete logic here
                //var pr = db.Requisitions.FirstOrDefault(r => r.ID == id && (r.RequestedBy == UserID || AcctType == 1 || AcctType == 4));
                var pr = db.Requisitions.Find(id);

                if (pr.RequisitionDetails.Count() > 0)
                {
                    pr.ApprovalStatus = 4;
                    pr.IsSync = false;

                    db.Entry(pr).State = EntityState.Modified;
                    db.SaveChanges();

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
        [AccessChecker(Action = 5, ModuleID = 3)]
        [HttpPost]
        public ActionResult Denied(int id)
        {
            try
            {
                // TODO: Add delete logic here
                var pr = db.Requisitions.Find(id);
                pr.ApprovalStatus = 3;
                pr.IsSync = false;

                db.Entry(pr).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: PR/Items/5
        [AccessChecker(Action = 1, ModuleID = 3)]
        public ActionResult Items(int id, int? page)
        {
            var items = db.RequisitionDetails
                        .ToList()
                        .FindAll(rd => rd.RequisitionID == id);

            ViewBag.PRid = id;

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        // GET: PR/ApprovedItems/5
        [AccessChecker(Action = 1, ModuleID = 3)]
        public ActionResult ApprovedItems(int id, int? page)
        {
            var items = db.RequisitionDetails
                        .ToList()
                        .FindAll(rd => rd.RequisitionID == id && rd.AprovalStatusID == 2);

            ViewBag.PRid = id;

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        // GET: PR/PendingItems/5
        [AccessChecker(Action = 1, ModuleID = 3)]
        public ActionResult PendingItems(int id, int? page)
        {
            int UserID = Convert.ToInt32(Session["sessionuid"]);
            int UserType = Convert.ToInt32(Session["usertype"]);

            var requisition = db.Requisitions.FirstOrDefault(r => r.ID == id);
            if (requisition.RequestedBy != UserID && UserType != 1 && UserType != 4)
            {
                return RedirectToAction("Index", "Home");
            }

            var items = db.RequisitionDetails
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
        [AccessChecker(Action = 1, ModuleID = 3)]
        public ActionResult DeniedItems(int id, int? page)
        {
            var items = db.RequisitionDetails
                        .ToList()
                        .FindAll(rd => rd.RequisitionID == id && rd.AprovalStatusID == 3);

            ViewBag.PRid = id;

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        // POST: PR/ApproveItem/5
        [AccessChecker(Action = 1, ModuleID = 3)]
        public ActionResult ApproveItem(int id, int itemID)
        {
            try
            {
                var item = db.RequisitionDetails.Find(itemID);
                if (item != null)
                {
                    item.AprovalStatusID = 2;
                    item.IsSync = false;

                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch
            {

            }
            return RedirectToAction("PendingItems", "Customer", new { id = id });
        }

        // POST: PR/DenyItem/5
        [AccessChecker(Action = 1, ModuleID = 3)]
        public ActionResult DenyItem(int id, int itemID)
        {
            try
            {
                var item = db.RequisitionDetails.Find(itemID);
                if (item != null)
                {
                    item.AprovalStatusID = 3;
                    item.IsSync = false;

                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch
            {

            }
            return RedirectToAction("PendingItems", "Customer", new { id = id });
        }

        public ActionResult ViewHistoryPartial(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "type" : "";
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "reqno" : "";

            var prs = from o in db.RequisitionDetails
                      select o;

            switch (sortOrder)
            {
                case "type":
                    prs = prs.OrderByDescending(o => o.Item.Description);
                    break;
                case "reqno":
                    prs = prs.OrderByDescending(o => o.Item.Code);
                    break;
                default:
                    prs = prs.OrderBy(o => o.ID);
                    break;
            }

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);
            return PartialView(prs.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public JsonResult GetCategories(string name)
        {
            var categories = db.Categories.Where(x => x.Description.Contains(name))
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
            var items = db.Items.Where(x => x.CategoryID == catID && x.Description.Contains(name))
                            .Select(x => new
                            {
                                ID = x.ID,
                                Name = x.Description,
                                UOM = x.UnitOfMeasurement.Description
                            });
            return Json(items, JsonRequestBehavior.AllowGet);
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
                    var req = db.Requisitions.Find(id);
                    req.ApprovalStatus = 3;
                    req.Remarks = Reason;
                    db.Entry(req).State = EntityState.Modified;

                    db.SaveChanges();
                }
                else
                {
                    TempData["Error"] = "Reason is required";
                }
            }
            catch { TempData["Error"] = "There's an error"; }

            return RedirectToAction("Index");
        }


        // GET: PR/AddItemPartial/5
        public ActionResult AddItemPartial(int id)
        {
            ViewBag.PRid = id;
            ViewBag.ItemID = new SelectList(db.Items, "ID", "Description");

            return PartialView();
        }

        // POST: PR/AddItemPartial/5
        [HttpPost]
        public ActionResult AddItemPartial(int id, RequisitionDetail rd)
        {
            try
            {
                // TODO: Add insert logic here
                rd.RequisitionID = id;
                rd.AprovalStatusID = 1; //submitted

                var rd1 = db.RequisitionDetails.Where(r => r.RequisitionID == rd.RequisitionID && r.ItemID == rd.ItemID).ToList();

                if (rd1.Count() > 0)
                {
                    TempData["PartialError"] = "Item is already in the list.";
                }
                else
                {
                    rd.IsSync = false;
                    db.RequisitionDetails.Add(rd);
                    db.SaveChanges();
                }
            }
            catch
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
            var rd = db.RequisitionDetails.Find(id);

            ViewBag.AprovalStatusID = new SelectList(db.ApprovalStatus, "ID", "Status", rd.AprovalStatusID);
            ViewBag.ItemID = new SelectList(db.Items, "ID", "Description", rd.ItemID);

            return PartialView(rd);
        }

        // POST: PR/EditItemPartial/5
        [HttpPost]
        public ActionResult EditItemPartial(int id, RequisitionDetail rd)
        {
            try
            {
                //rd.IsSync = false;
                //Robi
                var prvrequiDetail = db.RequisitionDetails.Find(rd.ID);

                if (prvrequiDetail.ItemID != rd.ItemID)
                {
                    rd.PreviousItemID = prvrequiDetail.ItemID;
                    rd.PreviousQuantity = prvrequiDetail.Quantity;
                }

                prvrequiDetail.ItemID = rd.ItemID;
                prvrequiDetail.Quantity = rd.Quantity;
                prvrequiDetail.Remarks = rd.Remarks;
                prvrequiDetail.PreviousItemID = rd.PreviousItemID;
                prvrequiDetail.PreviousQuantity = rd.PreviousQuantity;
                prvrequiDetail.IsSync = false;

                db.Entry(prvrequiDetail).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch
            {
                throw;
                //TempData["PartialError"] = "There's an error.";
            }

            if (rd.AprovalStatusID == 1)
            {
                return RedirectToAction("PendingItems", new { id = rd.RequisitionID });
            }
            return RedirectToAction("ApprovedItems", new { id = rd.RequisitionID });
        }

        // GET: PR/DeleteItemPartial/5
        public ActionResult DeleteItemPartial(int id)
        {
            var rd = db.RequisitionDetails.Find(id);

            return PartialView(rd);
        }

        // POST: PR/DeleteItemPartial/5
        [HttpPost, ActionName("DeleteItemPartial")]
        public ActionResult DeleteItemPartialConfirm(int id)
        {
            var rd = db.RequisitionDetails.Find(id);

            int? reqID = rd.RequisitionID;
            try
            {
                db.RequisitionDetails.Remove(rd);
                db.SaveChanges();
            }
            catch
            {
                TempData["PartialError"] = "There's an error.";
            }

            return RedirectToAction("PendingItems", new { id = reqID });
        }
        #endregion
    }
}