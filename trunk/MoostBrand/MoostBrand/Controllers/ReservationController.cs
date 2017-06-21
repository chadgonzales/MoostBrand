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

        public int getCommited(int itemID)
        {
            int c = 0;
            var com = db.RequisitionDetails.Where(model => model.ItemID == itemID && model.AprovalStatusID == 2 && model.Requisition.RequisitionTypeID == 4);
            var committed = com.Sum(x => x.Quantity);
            c = Convert.ToInt32(committed);
            if (committed == null)
            {
                c = 0;
            }
            return c;
        }
        public int getPurchaseOrder(int itemID)
        {
            int po = 0;
            var pur = db.RequisitionDetails.Where(model => model.Requisition.RequisitionTypeID == 1 && model.AprovalStatusID == 2 && model.ItemID == itemID);
            var porder = pur.Sum(x => x.Quantity);
            po = Convert.ToInt32(porder);
            if (porder == null)
            {
                po = 0;
            }
            return po;
        }
        public int getInstocked(string description)
        {
            int getIS = 0;
            var query = db.Inventories.FirstOrDefault(x => x.Description == description);
            if (query != null)
            {
                getIS = Convert.ToInt32(query.InStock);
            }
            return getIS;
        }

        #endregion

        #region JSON

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
        [HttpPost]
        public JsonResult getInstock(string Code)
        {
            var instock = db.Inventories.FirstOrDefault(x => x.ItemCode == Code);
            int total;
            if (instock != null)
            {
                total = Convert.ToInt32(instock.InStock);
            }
            else
            {
                total = 0;
            }
            return Json(total, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult getCommit(int ItemID)
        {
            var com = db.RequisitionDetails.Where(x => x.Requisition.RequisitionTypeID == 4 && x.ItemID == ItemID && x.AprovalStatusID == 2);
            var total = com.Sum(x => x.Quantity);
            if (total == null)
            {
                total = 0;
            }
            return Json(total, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult getPO(int ItemID)
        {
            var pur = db.RequisitionDetails.Where(model => model.Requisition.RequisitionTypeID == 1 && model.AprovalStatusID == 2 && model.ItemID == ItemID);
            var total = pur.Sum(x => x.Quantity);
            if (total == null)
            {
                total = 0;
            }
            return Json(total, JsonRequestBehavior.AllowGet);
        }

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

        [AccessChecker(Action = 1, ModuleID = 9)]
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
            ViewBag.PaymentStatus = new SelectList(db.PaymentStatus, "ID", "Status");
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

            ViewBag.PaymentStatus = new SelectList(db.PaymentStatus, "ID", "Status");
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

        [AccessChecker(Action = 2, ModuleID = 9)]
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

                ViewBag.PaymentStatus = new SelectList(db.PaymentStatus, "ID", "Status");
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
        [AccessChecker(Action = 2, ModuleID = 9)]
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

            ViewBag.PaymentStatus = new SelectList(db.PaymentStatus, "ID", "Status");
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
        [AccessChecker(Action = 3, ModuleID = 9)]
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
        [AccessChecker(Action = 3, ModuleID = 9)]
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
        [AccessChecker(Action = 5, ModuleID = 9)]
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
        [AccessChecker(Action = 5, ModuleID = 9)]
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
        [AccessChecker(Action = 5, ModuleID = 9)]
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
        [AccessChecker(Action = 1, ModuleID = 9)]
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
        [AccessChecker(Action = 1, ModuleID = 9)]
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
        [AccessChecker(Action = 1, ModuleID = 9)]
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
        [AccessChecker(Action = 1, ModuleID = 9)]
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
       

        // POST: PR/DenyItem/5
        [AccessChecker(Action = 1, ModuleID = 9)]
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

        [AccessChecker(Action = 1, ModuleID = 9)]
        public ActionResult ApproveItem(int id, int itemID, int reqID)
        {
            try
            {
                var req = db.RequisitionDetails.Find(reqID);
                var itm = db.Items.FirstOrDefault(x => x.ID == itemID);
                var inventory = db.Inventories.FirstOrDefault(x => x.Description == itm.Description);

                var quantity = req.Quantity;

                if (req != null)
                {
                    req.AprovalStatusID = 2;
                    req.IsSync = false;
                    if (req.Requisition.ReqTypeID != 2)
                    {
                        req.Ordered = Convert.ToInt32(getPurchaseOrder(req.ItemID) + req.Ordered);
                    }
                    req.Committed = getCommited(itemID) + req.Quantity;
                    int avail = (Convert.ToInt32(req.InStock) + Convert.ToInt32(req.Ordered)) - Convert.ToInt32(req.Committed);
                    req.Available = avail;
                    req.InStock -= req.Quantity;
                    db.Entry(req).State = EntityState.Modified;
                }
                if (inventory!=null)
                {
                    if (req.Requisition.ReqTypeID == 1)
                    {
                        inventory.Ordered = Convert.ToInt32(getPurchaseOrder(req.ItemID) + req.Ordered);
                    }
                    inventory.Committed = getCommited(req.ItemID) + quantity;
                    inventory.Available = (Convert.ToInt32(inventory.InStock) + Convert.ToInt32(inventory.Ordered)) - Convert.ToInt32(inventory.Committed);
                    inventory.InStock -= req.Quantity;
                    db.Entry(inventory).State = EntityState.Modified;
                }
                db.SaveChanges();
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
            ViewBag.ItemID = new SelectList(db.Items, "ID", "Description");

            return PartialView();
        }
        
        // POST: PR/AddItemPartial/5
        [HttpPost]
        public ActionResult AddItemPartial(int id, RequisitionDetail rd)
        {
            var itm = db.Items.FirstOrDefault(x => x.ID == rd.ItemID);
            var itmID = rd.ItemID;
            var desc = itm.Description;
            try
            {
                // TODO: Add insert logic here
                rd.RequisitionID = id;
                rd.AprovalStatusID = 1; //submitted
                
                rd.Committed = getCommited(itmID);

                rd.Ordered = getPurchaseOrder(itmID);

                rd.InStock = getInstocked(desc);

                rd.Available = (rd.InStock + rd.Ordered) - rd.Committed;

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
        public ActionResult EditItemPartial(int id,RequisitionDetail rd)
        {
            try
            {
                var prq = db.RequisitionDetails.Find(rd.ID);
                var itm = db.Items.Find(rd.ItemID);
                if (prq.ItemID != rd.ItemID || prq.Quantity != rd.Quantity)
                {
                    rd.PreviousItemID = prq.ItemID;
                    rd.PreviousQuantity = prq.Quantity;

                    rd.Ordered = getPurchaseOrder(rd.ItemID);
                    rd.Committed = getCommited(rd.ItemID);
                    rd.InStock = getInstocked(itm.Description);

                    rd.Available = (rd.InStock + rd.Ordered) - rd.Committed;
                    rd.Remarks = rd.Remarks;
                }
                else
                {
                    rd.InStock = getInstocked(itm.Description);
                    rd.Ordered = getPurchaseOrder(rd.ItemID);
                    rd.Committed = getCommited(rd.ItemID);
                    rd.Available = (rd.InStock + rd.Ordered) - rd.Committed;

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
                db.Entry(prq).CurrentValues.SetValues(rd);
                db.SaveChanges();
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

            return RedirectToAction("Details", new { id = reqID });
        }
        #endregion
    }
}