using System;
using System.Linq;
using System.Web.Mvc;
using MoostBrand.DAL;
using PagedList;
using System.Data.Entity;
using System.Configuration;
using MoostBrand.Models;

namespace MoostBrand.Controllers
{
    public class PRController : Controller
    {
        MoostBrandEntities entity = new MoostBrandEntities();


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
            else if (pr.RequisitionTypeID == 2 || pr.RequisitionTypeID == 3 || pr.RequisitionTypeID == 3) //BR or WR or OR
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
        #endregion

        public ActionResult GenerateRefNumber(string id)
        {
            return Json(Generator(id), JsonRequestBehavior.AllowGet);
        }



        // GET: PR
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
        public ActionResult Details(int id)
        {
            var pr = entity.Requisitions.Find(id);

            return View(pr);
        }

        // GET: PR/Create
        public ActionResult Create()
        {
            var pr = new Requisition();
            pr.RequestedDate = DateTime.Now;

            #region DROPDOWNS
            ViewBag.RequisitionTypeID = new SelectList(entity.RequisitionTypes, "ID", "Type");
            ViewBag.RequestedBy = new SelectList((from s in entity.Employees
                                                  select new
                                                  {
                                                      ID = s.ID,
                                                      FullName = s.FirstName + " " + s.LastName
                                                  }), "ID", "FullName");
            ViewBag.LocationID = new SelectList(entity.Locations, "ID", "Description");
            ViewBag.VendorID = new SelectList(entity.Vendors, "ID", "Name");
            ViewBag.ReservationTypeID = new SelectList(entity.ReservationTypes, "ID", "Type");
            ViewBag.ShipmentTypeID = new SelectList(entity.ShipmentTypes, "ID", "Type");
            ViewBag.DropShipID = new SelectList(entity.DropShipTypes, "ID", "Type");
            ViewBag.ReservedBy = new SelectList((from s in entity.Employees
                                                 select new
                                                 {
                                                     ID = s.ID,
                                                     FullName = s.FirstName + " " + s.LastName
                                                 }), "ID", "FullName");
            ViewBag.ValidatedBy = new SelectList((from s in entity.Employees
                                                  select new
                                                  {
                                                      ID = s.ID,
                                                      FullName = s.FirstName + " " + s.LastName
                                                  }), "ID", "FullName");
            ViewBag.Destination = new SelectList(entity.Locations, "ID", "Description");
            ViewBag.ApprovalStatus = new SelectList(entity.ApprovalStatus, "ID", "Status");
            ViewBag.ApprovedBy = new SelectList((from s in entity.Employees
                                                 select new
                                                 {
                                                     ID = s.ID,
                                                     FullName = s.FirstName + " " + s.LastName
                                                 }), "ID", "FullName");
            #endregion

            return View(pr);
        }

        // POST: PR/Create
        [HttpPost]
        public ActionResult Create(Requisition pr)
        {
            //var pr = new Requisition();

            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: Add insert logic here


                    //pr.RefNumber = collection["RefNumber"];
                    //pr.RequisitionTypeID = Convert.ToInt32(collection["RequisitionTypes"]);
                    //pr.RequestedBy = Convert.ToInt32(collection["RequestedBy"]);
                    //pr.RequestedDate = Convert.ToDateTime(collection["RequestedDate"]);
                    //pr.LocationID = Convert.ToInt32(collection["Locations"]);
                    //pr.DateRequired = Convert.ToDateTime(collection["DateRequired"]);
                    //pr.VendorID = Convert.ToInt32(collection["Vendors"]);
                    //pr.ShipmentDate = Convert.ToDateTime(collection["ShipmentDate"]);
                    //pr.Customer = collection["Customer"];
                    //pr.ReservationTypeID = Convert.ToInt32(collection["ReservationTypes"]);
                    //pr.ShipmentTypeID = Convert.ToInt32(collection["ShipmentType"]);
                    //pr.DropShipID = Convert.ToInt32(collection["DropShipTypes"]);
                    //pr.PaymentStatus = collection["PaymentStatus"];
                    //pr.InvoiceNumber = collection["InvoiceNumber"];
                    //pr.ReservedBy = Convert.ToInt32(collection["ReservedBy"]);
                    //pr.ValidatedBy = Convert.ToInt32(collection["ValidatedBy"]);
                    //pr.Destination = Convert.ToInt32(collection["Destinations"]);
                    //pr.ApprovalStatus = Convert.ToInt32(collection["ApprovalStatus"]);
                    //pr.ApprovedBy = Convert.ToInt32(collection["ApprovedBy"]);
                    //pr.Remarks = collection["Remarks"];

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
            ViewBag.RequisitionTypeID = new SelectList(entity.RequisitionTypes, "ID", "Type", pr.RequisitionTypeID);
            ViewBag.RequestedBy = new SelectList((from s in entity.Employees
                                                  select new
                                                  {
                                                      ID = s.ID,
                                                      FullName = s.FirstName + " " + s.LastName
                                                  }), "ID", "FullName", pr.RequestedBy);
            ViewBag.LocationID = new SelectList(entity.Locations, "ID", "Description", pr.LocationID);
            ViewBag.VendorID = new SelectList(entity.Vendors, "ID", "Name", pr.VendorID);
            ViewBag.ReservationTypeID = new SelectList(entity.ReservationTypes, "ID", "Type", pr.ReservationTypeID);
            ViewBag.ShipmentTypeID = new SelectList(entity.ShipmentTypes, "ID", "Type", pr.ShipmentTypeID);
            ViewBag.DropShipID = new SelectList(entity.DropShipTypes, "ID", "Type", pr.DropShipID);
            ViewBag.ReservedBy = new SelectList((from s in entity.Employees
                                                 select new
                                                 {
                                                     ID = s.ID,
                                                     FullName = s.FirstName + " " + s.LastName
                                                 }), "ID", "FullName", pr.ReservedBy);
            ViewBag.ValidatedBy = new SelectList((from s in entity.Employees
                                                  select new
                                                  {
                                                      ID = s.ID,
                                                      FullName = s.FirstName + " " + s.LastName
                                                  }), "ID", "FullName", pr.ValidatedBy);
            ViewBag.Destination = new SelectList(entity.Locations, "ID", "Description", pr.Destination);
            ViewBag.ApprovalStatus = new SelectList(entity.ApprovalStatus, "ID", "Status", pr.ApprovalStatus);
            ViewBag.ApprovedBy = new SelectList((from s in entity.Employees
                                                 select new
                                                 {
                                                     ID = s.ID,
                                                     FullName = s.FirstName + " " + s.LastName
                                                 }), "ID", "FullName", pr.ApprovedBy);
            #endregion

            return View(pr);
        }

        // GET: PR/Edit/5
        public ActionResult Edit(int id)
        {
            var pr = entity.Requisitions.Find(id);

            #region DROPDOWNS
            ViewBag.RequisitionTypeID = new SelectList(entity.RequisitionTypes, "ID", "Type", pr.RequisitionTypeID);
            ViewBag.RequestedBy = new SelectList((from s in entity.Employees
                                                  select new
                                                  {
                                                      ID = s.ID,
                                                      FullName = s.FirstName + " " + s.LastName
                                                  }), "ID", "FullName", pr.RequestedBy);
            ViewBag.LocationID = new SelectList(entity.Locations, "ID", "Description", pr.LocationID);
            ViewBag.VendorID = new SelectList(entity.Vendors, "ID", "Name", pr.VendorID);
            ViewBag.ReservationTypeID = new SelectList(entity.ReservationTypes, "ID", "Type", pr.ReservationTypeID);
            ViewBag.ShipmentTypeID = new SelectList(entity.ShipmentTypes, "ID", "Type", pr.ShipmentTypeID);
            ViewBag.DropShipID = new SelectList(entity.DropShipTypes, "ID", "Type", pr.DropShipID);
            ViewBag.ReservedBy = new SelectList((from s in entity.Employees
                                                 select new
                                                 {
                                                     ID = s.ID,
                                                     FullName = s.FirstName + " " + s.LastName
                                                 }), "ID", "FullName", pr.ReservedBy);
            ViewBag.ValidatedBy = new SelectList((from s in entity.Employees
                                                  select new
                                                  {
                                                      ID = s.ID,
                                                      FullName = s.FirstName + " " + s.LastName
                                                  }), "ID", "FullName", pr.ValidatedBy);
            ViewBag.Destination = new SelectList(entity.Locations, "ID", "Description", pr.Destination);
            ViewBag.ApprovalStatus = new SelectList(entity.ApprovalStatus, "ID", "Status", pr.ApprovalStatus);
            ViewBag.ApprovedBy = new SelectList((from s in entity.Employees
                                                 select new
                                                 {
                                                     ID = s.ID,
                                                     FullName = s.FirstName + " " + s.LastName
                                                 }), "ID", "FullName", pr.ApprovedBy);
            #endregion

            return View(pr);
        }

        // POST: PR/Edit/5
        [HttpPost]
        public ActionResult Edit(Requisition pr)
        {
            //var pr = new Requisition();

            try
            {
                // TODO: Add insert logic here
                //pr.ID = Convert.ToInt32(collection["ID"]);
                //pr.RefNumber = collection["RefNumber"];
                //pr.RequisitionTypeID = Convert.ToInt32(collection["RequisitionTypes"]);
                //pr.RequestedBy = Convert.ToInt32(collection["RequestedBy"]);
                //pr.RequestedDate = Convert.ToDateTime(collection["RequestedDate"]);
                //pr.LocationID = Convert.ToInt32(collection["Locations"]);
                //pr.DateRequired = Convert.ToDateTime(collection["DateRequired"]);
                //pr.VendorID = Convert.ToInt32(collection["Vendors"]);
                //pr.ShipmentDate = Convert.ToDateTime(collection["ShipmentDate"]);
                //pr.Customer = collection["Customer"];
                //pr.ReservationTypeID = Convert.ToInt32(collection["ReservationTypes"]);
                //pr.ShipmentTypeID = Convert.ToInt32(collection["ShipmentType"]);
                //pr.DropShipID = Convert.ToInt32(collection["DropShipTypes"]);
                //pr.PaymentStatus = collection["PaymentStatus"];
                //pr.InvoiceNumber = collection["InvoiceNumber"];
                //pr.ReservedBy = Convert.ToInt32(collection["ReservedBy"]);
                //pr.ValidatedBy = Convert.ToInt32(collection["ValidatedBy"]);
                //pr.Destination = Convert.ToInt32(collection["Destinations"]);
                //pr.ApprovalStatus = Convert.ToInt32(collection["ApprovalStatus"]);
                //pr.ApprovedBy = Convert.ToInt32(collection["ApprovedBy"]);
                //pr.Remarks = collection["Remarks"];

                entity.Entry(SetNull(pr)).State = EntityState.Modified;
                entity.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError("", "There's an error.");
            }
            #region DROPDOWNS
            ViewBag.RequisitionTypeID = new SelectList(entity.RequisitionTypes, "ID", "Type", pr.RequisitionTypeID);
            ViewBag.RequestedBy = new SelectList((from s in entity.Employees
                                                  select new
                                                  {
                                                      ID = s.ID,
                                                      FullName = s.FirstName + " " + s.LastName
                                                  }), "ID", "FullName", pr.RequestedBy);
            ViewBag.LocationID = new SelectList(entity.Locations, "ID", "Description", pr.LocationID);
            ViewBag.VendorID = new SelectList(entity.Vendors, "ID", "Name", pr.VendorID);
            ViewBag.ReservationTypeID = new SelectList(entity.ReservationTypes, "ID", "Type", pr.ReservationTypeID);
            ViewBag.ShipmentTypeID = new SelectList(entity.ShipmentTypes, "ID", "Type", pr.ShipmentTypeID);
            ViewBag.DropShipID = new SelectList(entity.DropShipTypes, "ID", "Type", pr.DropShipID);
            ViewBag.ReservedBy = new SelectList((from s in entity.Employees
                                                 select new
                                                 {
                                                     ID = s.ID,
                                                     FullName = s.FirstName + " " + s.LastName
                                                 }), "ID", "FullName", pr.ReservedBy);
            ViewBag.ValidatedBy = new SelectList((from s in entity.Employees
                                                  select new
                                                  {
                                                      ID = s.ID,
                                                      FullName = s.FirstName + " " + s.LastName
                                                  }), "ID", "FullName", pr.ValidatedBy);
            ViewBag.Destination = new SelectList(entity.Locations, "ID", "Description", pr.Destination);
            ViewBag.ApprovalStatus = new SelectList(entity.ApprovalStatus, "ID", "Status", pr.ApprovalStatus);
            ViewBag.ApprovedBy = new SelectList((from s in entity.Employees
                                                 select new
                                                 {
                                                     ID = s.ID,
                                                     FullName = s.FirstName + " " + s.LastName
                                                 }), "ID", "FullName", pr.ApprovedBy);
            #endregion

            return View(pr);
        }

        // GET: PR/Delete/5
        public ActionResult Delete(int id)
        {
            var pr = entity.Requisitions.Find(id);
            return View(pr);
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


        // GET: PR/AddItemPartial/5
        public ActionResult AddItemPartial(int id)
        {
            ViewBag.PRid = id;
            ViewBag.ItemID = new SelectList(entity.Items, "ID", "Description");
            ViewBag.AprovalStatusID = new SelectList(entity.ApprovalStatus, "ID", "Status");

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

                var rd1 = entity.RequisitionDetails.Where(r => r.RequisitionID == rd.RequisitionID && r.ItemID == rd.ItemID).ToList();

                if (rd1.Count() > 0)
                {
                    TempData["PartialError"] = "Item is already in the list.";
                }
                else
                {
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

        // POST: PR/EditItem/5
        [HttpPost]
        public ActionResult EditItemPartial(int id, RequisitionDetail rd)
        {
            try
            {
                entity.Entry(rd).State = EntityState.Modified;
                entity.SaveChanges();
            }
            catch
            {
                TempData["PartialError"] = "There's an error.";
            }

            return RedirectToAction("PendingItems", new { id = rd.RequisitionID });
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

        // GET: PR/PendingItems/5
        public ActionResult PendingItems(int id, int? page)
        {
            var items = entity.RequisitionDetails
                        .ToList()
                        .FindAll(rd => rd.RequisitionID == id && rd.AprovalStatusID == 1);

            ViewBag.PRid = id;

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

        // GET: PR/AddItem/5
        public ActionResult AddItem(int id)
        {
            ViewBag.PRid = id;
            ViewBag.ItemID = new SelectList(entity.Items, "ID", "Description");
            ViewBag.AprovalStatusID = new SelectList(entity.ApprovalStatus, "ID", "Status");
            return View();
        }

        [HttpPost]
        public ActionResult AddItem(int id, RequisitionDetail rd)
        {
            try
            {
                // TODO: Add insert logic here
                rd.RequisitionID = id;

                var rd1 = entity.RequisitionDetails.Where(r => r.RequisitionID == rd.RequisitionID && r.ItemID == rd.ItemID).ToList();

                if (rd1.Count() > 0)
                {
                    ModelState.AddModelError("", "Item is already in the list.");
                }
                else
                {
                    entity.RequisitionDetails.Add(rd);
                    entity.SaveChanges();
                    return RedirectToAction("Items", new { id = id });
                }
            }
            catch
            {
                ModelState.AddModelError("", "There's an error.");
            }

            ViewBag.PRid = id;
            ViewBag.ItemID = new SelectList(entity.Items, "ID", "Description", rd.ItemID);
            ViewBag.AprovalStatusID = new SelectList(entity.ApprovalStatus, "ID", "Status", rd.AprovalStatusID);

            return View();
        }

        // GET: PR/EditItem/5
        public ActionResult EditItem(int id)
        {
            var rd = entity.RequisitionDetails.Find(id);

            ViewBag.Items = new SelectList(entity.Items, "ID", "Description", rd.ItemID);

            return View(rd);
        }

        // POST: PR/EditItem/5
        [HttpPost]
        public ActionResult EditItem(int id, FormCollection collection)
        {
            var rd = new RequisitionDetail();

            try
            {
                // TODO: Add insert logic here
                rd.ID = Convert.ToInt32(collection["ID"]);
                rd.RequisitionID = id;
                rd.ItemID = Convert.ToInt32(collection["Items"]);
                rd.Quantity = Convert.ToInt32(collection["Quantity"]);

                entity.Entry(rd).State = EntityState.Modified;
                entity.SaveChanges();

                return RedirectToAction("Items", new { id = id });
            }
            catch
            {
                ModelState.AddModelError("", "There's an error.");
            }

            ViewBag.Items = new SelectList(entity.Items, "ID", "Description", rd.ItemID);

            return View(rd);
        }

        // POST: PR/Approve/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Approve(int id)
        {
            try
            {
                // TODO: Add delete logic here
                var pr = entity.Requisitions.Find(id);
                pr.ApprovalStatus = 2;

                entity.Entry(pr).State = EntityState.Modified;
                entity.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
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

                entity.Entry(pr).State = EntityState.Modified;
                entity.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
