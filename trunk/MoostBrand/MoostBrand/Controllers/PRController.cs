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
    public class PRController : Controller
    {
        MoostBrandEntities entity = new MoostBrandEntities();

        int UserID = 0;
        int UserType = 0;
        int ModuleID = 3;
        
        #region PRIVATE METHODS
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

        private Requisition SetNull(Requisition pr)
        {
            if (pr.RequisitionTypeID == 1) //PR
            {
                pr.Destination = null;
                pr.PlateNumber = null;
                pr.Others = null;
                pr.TimeDeparted = null;
                pr.TimeArrived = null;
                pr.Driver = null;
                pr.Customer = null;
                pr.ReservationTypeID = null;
                pr.ReservedBy = null;
                pr.PaymentStatus = null;
                pr.InvoiceNumber = null;
                pr.AuthorizedPerson = null;
                pr.ValidatedBy = null;
                pr.ShipmentTypeID = null;
                pr.DropShipID = null;
                pr.Driver = null;
            }
            else if (pr.RequisitionTypeID == 2 || pr.RequisitionTypeID == 3 || pr.RequisitionTypeID == 5) //BR or WR or OR
            {
                pr.VendorID = null;
                pr.Customer = null;
                pr.ReservationTypeID = null;
                pr.ReservedBy = null;
                pr.PaymentStatus = null;
                pr.InvoiceNumber = null;
                pr.AuthorizedPerson = null;
                pr.ValidatedBy = null;
                pr.ShipmentTypeID = null;
                pr.DropShipID = null;
            }
            else if (pr.RequisitionTypeID == 4) //CR
            {
                pr.VendorID = null;
                pr.Destination = null;
                pr.PlateNumber = null;
                pr.Others = null;
                pr.TimeDeparted = null;
                pr.TimeArrived = null;
                if (pr.ShipmentTypeID == 2)
                {
                    pr.DropShipID = null;
                }
            }
            else
            {
                return null;
            }
            return pr;
        }

        private void AccessChecker(int action)
        {
            int UserID = Convert.ToInt32(Session["sessionuid"]);

            var access = entity.UserAccesses.FirstOrDefault(u => u.EmployeeID == UserID && u.ModuleID == ModuleID);

            if(action == 1) //CanView
            {
                if (!access.CanView)
                {
                    RedirectToAction("Index", "Home");
                }
            }
            else if(action == 2)//CanEdit
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
        #endregion

        public ActionResult GenerateRefNumber(string id)
        {
            return Json(Generator(id), JsonRequestBehavior.AllowGet);
        }
        
        // GET: PR
        [AccessChecker(Action = 1, ModuleID = 3)]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "type" : "";
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
            
            var prs = from o in entity.Requisitions
                      select o;

            if (!String.IsNullOrEmpty(searchString))
            {
                prs = prs.Where(o => o.RequisitionType.Type.Contains(searchString)
                                       || o.RefNumber.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "type":
                    prs = prs.OrderByDescending(o => o.RequisitionType.Type);
                    break;
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

        // GET: PR/Details/5
        public ActionResult Details(int id = 0)
        {
            //var pr = entity.Requisitions.FirstOrDefault(r => r.ID == id && (r.RequestedBy == UserID || AcctType == 1 || AcctType == 4));
            var pr = entity.Requisitions.Find(id);
            if (pr == null)
            {
                return HttpNotFound();
            }

            return View(pr);
        }

        // GET: PR/Create
        public ActionResult Create()
        {
            var pr = new Requisition();
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

        // POST: PR/Create
        [HttpPost]
        public ActionResult Create(Requisition pr)
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
                        var newPR = SetNull(pr);

                        if (newPR != null)
                        {
                            newPR.IsSync = false;

                            entity.Requisitions.Add(newPR);
                            entity.SaveChanges();

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

        // GET: PR/Edit/5
        public ActionResult Edit(int id)
        {
            //var pr = entity.Requisitions.FirstOrDefault(r => r.ID == id && (r.RequestedBy == UserID || AcctType == 1 || AcctType == 4));
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

            return RedirectToAction("Details", new { id = id });
        }

        // POST: PR/Edit/5
        [HttpPost]
        public ActionResult Edit(Requisition pr)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //var r = entity.Requisitions.FirstOrDefault(r1 => r1.ID == pr.ID && (r1.RequestedBy == UserID || AcctType == 1 || AcctType == 4)).ApprovalStatus;
                    var r = entity.Requisitions.FirstOrDefault(r1 => r1.ID == pr.ID);
                    if(r.ApprovalStatus == 1)
                    {
                        pr.IsSync = false;
                        var newPR = SetNull(pr);
                        newPR.ApprovalStatus = r.ApprovalStatus;
                        entity.Entry(r).CurrentValues.SetValues(newPR);
                        entity.SaveChanges();

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return RedirectToAction("Details", new { id = pr.ID });
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

        // GET: PR/Delete/5
        public ActionResult Delete(int id)
        {
            //var pr = entity.Requisitions.FirstOrDefault(r => r.ID == id && (r.RequestedBy == UserID || AcctType == 1 || AcctType == 4));
            var pr = entity.Requisitions.FirstOrDefault(r => r.ID == id);
            if(pr == null)
            {
                return HttpNotFound();
            }
            else
            {
                if(pr.ApprovalStatus == 1)
                {
                    return View(pr);
                }
            }
            
            return RedirectToAction("Details", new { id = pr.ID });
        }

        // POST: PR/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {
            var pr = entity.Requisitions.Find(id);

            try
            {
                entity.Requisitions.Remove(pr);
                entity.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
            }

            return View(pr);
        }

        // POST: PR/Approve/5
        [AccessChecker(Action = 5, ModuleID = 3)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Approve(int id)
        {
            try
            {
                // TODO: Add delete logic here
                //var pr = entity.Requisitions.FirstOrDefault(r => r.ID == id && (r.RequestedBy == UserID || AcctType == 1 || AcctType == 4));
                var pr = entity.Requisitions.Find(id);

                if(pr.RequisitionDetails.Count() > 0)
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

        // POST: PR/Denied/5
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
        public ActionResult PendingItems(int id, int? page)
        {
            int UserID = Convert.ToInt32(Session["sessionuid"]);
            int UserType = Convert.ToInt32(Session["usertype"]);

            var requisition = entity.Requisitions.FirstOrDefault(r => r.ID == id);
            if(requisition.RequestedBy != UserID && UserType != 1 && UserType != 4)
            {
                return RedirectToAction("Index", "Home");
            }

            var items = entity.RequisitionDetails
                        .ToList()
                        .FindAll(rd => rd.RequisitionID == id && rd.AprovalStatusID == 1 && (rd.Requisition.RequestedBy == UserID || UserType == 1 || UserType == 4));
            //var items = entity.RequisitionDetails
            //            .ToList()
            //            .FindAll(rd => rd.RequisitionID == id && rd.AprovalStatusID == 1 && rd.Requisition.RequestedBy == UserID);

            ViewBag.PRid = id;
            ViewBag.RequestedBy = 
            ViewBag.UserID = UserID;
            ViewBag.AcctType = UserType;
            ViewBag.IsApproved = requisition.ApprovalStatus;

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        // GET: PR/DeniedItems/5
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

        // POST: PR/ApproveItem/5
        public ActionResult ApproveItem(int id, int itemID)
        {
            try
            {
                var item = entity.RequisitionDetails.Find(itemID);
                if(item != null)
                {
                    item.AprovalStatusID = 2;
                    item.IsSync = false;

                    entity.Entry(item).State = EntityState.Modified;
                    entity.SaveChanges();
                }
            }
            catch
            {

            }
            return RedirectToAction("PendingItems", "PR", new { id = id });
        }

        // POST: PR/DenyItem/5
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
            return RedirectToAction("PendingItems", "PR", new { id = id });
        }

        #region PARTIAL
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
            try
            {
                // TODO: Add insert logic here
                rd.RequisitionID = id;
                rd.AprovalStatusID = 1; //submitted

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
            catch
            {
                TempData["PartialError"] = "There's an error.";
            }

            //ViewBag.PRid = id;
            //ViewBag.ItemID = new SelectList(entity.Items, "ID", "Description", rd.ItemID);
            //ViewBag.AprovalStatusID = new SelectList(entity.ApprovalStatus, "ID", "Status", rd.AprovalStatusID);

            return RedirectToAction("PendingItems", new { id = id });
        }

        // GET: PR/EditItemPartial/5
        public ActionResult EditItemPartial(int id)
        {
            var rd = entity.RequisitionDetails.Find(id);

            ViewBag.AprovalStatusID = new SelectList(entity.ApprovalStatus, "ID", "Status", rd.AprovalStatusID);

            return PartialView(rd);
        }

        // POST: PR/EditItemPartial/5
        [HttpPost]
        public ActionResult EditItemPartial(int id, RequisitionDetail rd)
        {
            try
            {
                rd.IsSync = false;

                entity.Entry(rd).State = EntityState.Modified;
                entity.SaveChanges();
            }
            catch
            {
                TempData["PartialError"] = "There's an error.";
            }

            if(rd.AprovalStatusID == 1)
            {
                return RedirectToAction("PendingItems", new { id = rd.RequisitionID });
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

            return RedirectToAction("PendingItems", new { id = reqID });
        }
        #endregion
    }
}
