using MoostBrand.DAL;
using System;
using System.Linq;
using PagedList;
using System.Web.Mvc;
using System.Configuration;
using System.Data.Entity;
using MoostBrand.Models;
using System.Collections.Generic;

namespace MoostBrand.Controllers
{
    [LoginChecker]
    public class StockTransferController : Controller
    {
        MoostBrandEntities entity = new MoostBrandEntities();
        InventoryRepository invRepo = new InventoryRepository();

        #region PRIVATE METHODS
        private string Generator(string prefix)
        {
            //Initiate objects & vars
            startR: Random random = new Random();
            String randomString = "";
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
        #endregion

        #region COMMENTS
        //// GET: Receiving/GetStockTransfers/5
        //public ActionResult GetReceivings(int id) //returns Json
        //{
        //    var sts = entity.Receivings
        //             .Include(st => st.Requisition)
        //             .ToList()
        //             .FindAll(st => st.Requisition.RequisitionTypeID == id)
        //             .Select(st => new
        //             {
        //                 ID = st.ID,
        //                 ReceivingID = st.ReceivingID
        //             });

        //    return Json(sts, JsonRequestBehavior.AllowGet);
        //}

        // GET: Receiving/GetRequisition/5
        //public ActionResult GetRequisition(int id) //returns Json
        //{
        //    var req = entity.Receivings
        //             .Include(st => st.Requisition)
        //             .Where(st => st.ID == id)
        //             .Select(st => new
        //             {
        //                 ID = st.RequisitionID,
        //                 RefNumber = st.Requisition.RefNumber,
        //                 RequestedBy = st.Requisition.Employee1.FirstName + " " + st.Requisition.Employee1.LastName,
        //                 Destination = st.Requisition.Location1.Description,
        //                 SourceLoc = st.Requisition.Location.Description,
        //                 Vendor = st.Requisition.Vendor.Name,
        //                 VendorCode = st.Requisition.Vendor.Code,
        //                 VendorContact = st.Requisition.Vendor.Attn,
        //                 CustName = st.Requisition.Customer,
        //                 ShipmentType = st.Requisition.ShipmentType.Type,
        //                 Invoice = st.Requisition.InvoiceNumber
        //             })
        //             .FirstOrDefault();

        //    return Json(req, JsonRequestBehavior.AllowGet);
        //}
        #endregion

        #region ST Action

        // GET: StockTransfer
        [AccessChecker(Action = 1, ModuleID = 4)]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "stid" : "";


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

            //var user = entity.Users.FirstOrDefault(x => x.ID == UserID);
            var sts = from o in entity.StockTransfers
                      select o;

            if (!String.IsNullOrEmpty(searchString))
            {
                sts = sts.Where(o => o.TransferID.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "reqno":
                    sts = sts.OrderByDescending(o => o.TransferID);
                    break;
                default:
                    sts = sts.OrderBy(o => o.ID);
                    break;
            }

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);

            //if (user.LocationID != 10)
            //{
            //    sts = sts.Where(x => x.LocationID == locID);
            //    return View(sts.ToPagedList(pageNumber, pageSize));
            //}
            //else
            //    return View(sts.ToPagedList(pageNumber, pageSize));

            return View(sts.ToPagedList(pageNumber, pageSize));
        }

        // GET: StockTransfer/Details/5
        [AccessChecker(Action = 1, ModuleID = 4)]
        public ActionResult Details(int id = 0)
        {
            var stocktransfer = entity.StockTransfers.Find(id);

            if (stocktransfer == null)
            {
                return HttpNotFound();
            }

            return View(stocktransfer);
        }

        // -Pearl
        public ActionResult GenerateSTRNumber(string id)
        {
            return Json(Generator(id), JsonRequestBehavior.AllowGet);
        }

        public class ReqCustom {
            public int ID { get; set; }
            public string RefNumber { get; set; }
        }

        // GET: StockTransfer/Create/
        [AccessChecker(Action = 2, ModuleID = 4)]
        public ActionResult Create()
        {
            var stocktransfer = new StockTransfer();
            stocktransfer.STDAte = DateTime.Now;
            
            #region DROPDOWNS
            var _types = entity.StockTransferTypes.ToList();
            var _st = entity.StockTransfers.ToList();


            var _receivings = entity.Receivings.Where(r => r.ApprovalStatus == 2 && !_st.Select(p=>p.ID).Contains(r.StockTransferID.Value))
                .Select(r => new
                {
                    ID = r.ID,
                    ReceivingID = (r.ReceivingID.Contains("PR")) ? "PO" + r.ReceivingID.Substring(2) : r.ReceivingID
                });

            List<ReqCustom> lstReqCustom = new List<ReqCustom>();

            //var lstReq = entity.Requisitions.Where(r => r.ApprovalStatus == 2 && !_st.Select(p => p.RequisitionID).Contains(r.ID) && (r.RefNumber.Contains("BR") || r.RefNumber.Contains("WR"))).ToList();

            var lstReq = (from r in entity.Requisitions.ToList()
                          where r.ApprovalStatus == 2 &&
                          !_st.Select(p => p.RequisitionID).Contains(r.ID) &&
                          (r.RefNumber.Contains("BR") || r.RefNumber.Contains("WR"))
                          select r).ToList();

            foreach (var _req in lstReq) {
                string refNumber;

                if (_req.RefNumber.Contains("BR"))
                {
                    refNumber = "BR" + _req.RefNumber.Substring(2);
                }
                else if (_req.RefNumber.Contains("WR"))
                {
                    refNumber = "WR" + _req.RefNumber.Substring(2);
                }
                else {
                    refNumber = _req.RefNumber;
                }

                lstReqCustom.Add(
                    new ReqCustom
                    {
                        ID = _req.ID,
                        RefNumber = refNumber
                    });
            }

            var loc = entity.Locations.Where(x => x.ID != 10)
                      .Select(x => new
                      {
                          ID = x.ID,
                          Description = x.Description
                      });

            ViewBag.StockTransferTypeID = new SelectList(_types, "ID", "Name");
            ViewBag.RequisitionID = new SelectList(lstReqCustom, "ID", "RefNumber");
            ViewBag.ReceivingID = new SelectList(_receivings, "ID", "ReceivingID");
            ViewBag.LocationID = new SelectList(loc, "ID", "Description");
            var empList = new SelectList((from s in entity.Employees
                                          select new
                                          {
                                              ID = s.ID,
                                              FullName = s.FirstName + " " + s.LastName
                                          }), "ID", "FullName");
            ViewBag.EncodedBy = empList;
            ViewBag.ReceivedBy = empList;
            ViewBag.RequestedBy = empList;
            ViewBag.ApprovedBy = empList;
            ViewBag.ReleasedBy = empList;
            ViewBag.CounterCheckedBy = empList;
            ViewBag.PostedBy = empList;
            ViewBag.ApprovedStatus = new SelectList(entity.ApprovalStatus, "ID", "Status");
            #endregion
            return View(stocktransfer);
        }

        [AccessChecker(Action = 2, ModuleID = 4)]
        [HttpPost]
        public ActionResult Create(StockTransfer stocktransfer)
        {
            #region DROPDOWNS
            var _types = entity.StockTransferTypes.ToList();

            var loc = entity.Locations.Where(x => x.ID != 10)
                        .Select(x => new
                        {
                            ID = x.ID,
                            Description = x.Description
                        });

            ViewBag.StockTransferTypeID = new SelectList(_types, "ID", "Name");
            ViewBag.RequisitionID = new SelectList(entity.Requisitions.ToList().FindAll(r => r.ApprovalStatus == 2), "ID", "RefNumber");
            ViewBag.ReceivingID = new SelectList(entity.Receivings.ToList().FindAll(r => r.ApprovalStatus == 2), "ID", "ReceivingID");
            ViewBag.LocationID = new SelectList(loc, "ID", "Description", stocktransfer.LocationID);
            var empList = from s in entity.Employees
                          select new
                          {
                              ID = s.ID,
                              FullName = s.FirstName + " " + s.LastName
                          };

            ViewBag.EncodedBy = new SelectList(empList, "ID", "FullName", stocktransfer.EncodedBy);
            ViewBag.ReceivedBy = new SelectList(empList, "ID", "FullName", stocktransfer.ReceivedBy);
            ViewBag.RequestedBy = new SelectList(empList, "ID", "FullName", stocktransfer.RequestedBy); ;
            ViewBag.ApprovedBy = new SelectList(empList, "ID", "FullName", stocktransfer.ApprovedBy); ;
            ViewBag.ReleasedBy = new SelectList(empList, "ID", "FullName", stocktransfer.ReleasedBy); ;
            ViewBag.CounterCheckedBy = new SelectList(empList, "ID", "FullName", stocktransfer.CounterCheckedBy); ;
            ViewBag.PostedBy = new SelectList(empList, "ID", "FullName", stocktransfer.PostedBy); ;
            ViewBag.ApprovedStatus = new SelectList(entity.ApprovalStatus, "ID", "Status", stocktransfer.ApprovedStatus);
            #endregion

            if (ModelState.IsValid)
            {
                var stransfer = entity.StockTransfers.Where(s => s.TransferID == stocktransfer.TransferID).ToList();

                if (stransfer.Count() > 0)
                {
                    ModelState.AddModelError("", "Stock Transfer ID already exists");
                }
                else
                {
                    #region Helper
                    foreach (Helper h in stocktransfer.Helpers.ToList())
                    {
                        try
                        {
                            if (h.DeletedHelper == 1)
                            {
                                stocktransfer.Helpers.Remove(h);
                            }

                        }
                        catch
                        {
                            ModelState.AddModelError(string.Empty, "There's an error");
                        }
                    }
                    #endregion
                    #region Operator
                    foreach (Operator o in stocktransfer.Operators.ToList())
                    {
                        if (o.DeletedOperator == 1)
                        {
                            stocktransfer.Operators.Remove(o);
                        }
                    }
                    #endregion

                    stocktransfer.ApprovedStatus = 1;
                    stocktransfer.ApprovedBy = Convert.ToInt32(Session["sessionuid"]);
                    stocktransfer.IsSync = false;

                    entity.StockTransfers.Add(stocktransfer);
                    entity.SaveChanges();
                    //return RedirectToAction("Index");

                    return RedirectToAction("Details", new { id = stocktransfer.ID });
                }

                
                return View(stocktransfer);
            }
            else {
                return View(stocktransfer);
            }
        }

        // GET: StockTransfer/Edit/5
        [AccessChecker(Action = 2, ModuleID = 4)]
        public ActionResult Edit(int id = 0)
        {
            var stocktransfer = entity.StockTransfers.Find(id);
            var _types = entity.StockTransferTypes.ToList();

            if (stocktransfer == null)
            {
                return HttpNotFound();
            }

            if (stocktransfer.ApprovedStatus == 1)
            {
                #region DROPDOWNS

                var loc = entity.Locations.Where(x => x.ID != 10)
                            .Select(x => new
                            {
                                ID = x.ID,
                                Description = x.Description
                            });

                ViewBag.StockTransferTypeID = new SelectList(_types, "ID", "Name");
                ViewBag.RequisitionID = new SelectList(entity.Requisitions.ToList().FindAll(r => r.ApprovalStatus == 2), "ID", "RefNumber");
                ViewBag.ReceivingID = new SelectList(entity.Receivings.ToList().FindAll(r => r.ApprovalStatus == 2), "ID", "ReceivingID", stocktransfer.ReceivingID);
                //ViewBag.RequisitionID = new SelectList(entity.Requisitions.ToList().FindAll(r => r.ApprovalStatus == 2), "ID", "RefNumber", stocktransfer.RequisitionID);
                ViewBag.LocationID = new SelectList(loc, "ID", "Description", stocktransfer.LocationID);
                var empList = from s in entity.Employees
                              select new
                              {
                                  ID = s.ID,
                                  FullName = s.FirstName + " " + s.LastName
                              };

                ViewBag.EncodedBy = new SelectList(empList, "ID", "FullName", stocktransfer.EncodedBy);
                ViewBag.ReceivedBy = new SelectList(empList, "ID", "FullName", stocktransfer.ReceivedBy);
                ViewBag.RequestedBy = new SelectList(empList, "ID", "FullName", stocktransfer.RequestedBy); ;
                ViewBag.ApprovedBy = new SelectList(empList, "ID", "FullName", stocktransfer.ApprovedBy); ;
                ViewBag.ReleasedBy = new SelectList(empList, "ID", "FullName", stocktransfer.ReleasedBy); ;
                ViewBag.CounterCheckedBy = new SelectList(empList, "ID", "FullName", stocktransfer.CounterCheckedBy); ;
                ViewBag.PostedBy = new SelectList(empList, "ID", "FullName", stocktransfer.PostedBy); ;
                ViewBag.ApprovedStatus = new SelectList(entity.ApprovalStatus, "ID", "Status", stocktransfer.ApprovedStatus);
                #endregion

                return View(stocktransfer);
            }

            return RedirectToAction("Details", new { id = id });
        }

        // GET: StockTransfer/Edit/5
        [AccessChecker(Action = 2, ModuleID = 4)]
        [HttpPost]
        public ActionResult Edit(StockTransfer stocktransfer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    stocktransfer.IsSync = false;

                    #region Helper
                    foreach (Helper helper in stocktransfer.Helpers.ToList())
                    {
                        try
                        {
                            if (helper.DeletedHelper == 1 && helper.HelperID != 0)
                            {
                                entity.Entry(helper).State = EntityState.Deleted;
                                entity.SaveChanges();
                            }
                            else if (helper.StockTransferID != stocktransfer.ID)
                            {
                                helper.StockTransferID = stocktransfer.ID;

                                if (helper.Name != null && helper.Name.Trim() != string.Empty)
                                {
                                    entity.Entry(helper).State = EntityState.Added;
                                    entity.SaveChanges();
                                }
                            }
                            else if (helper.HelperID != 0)
                            {
                                if (helper.Name != null && helper.Name.Trim() != string.Empty)
                                {
                                    entity.Entry(helper).State = EntityState.Modified;
                                    entity.SaveChanges();
                                }
                            }
                        }
                        catch { }
                        stocktransfer.Helpers.Remove(helper);
                    }
                    #endregion
                    #region Operator
                    foreach (Operator operators in stocktransfer.Operators.ToList())
                    {
                        try
                        {
                            if (operators.DeletedOperator == 1 && operators.OperatorID != 0)
                            {
                                entity.Entry(operators).State = EntityState.Deleted;
                                entity.SaveChanges();
                            }
                            else if (operators.StockTransferID != stocktransfer.ID)
                            {
                                operators.StockTransferID = stocktransfer.ID;

                                if (operators.Name != null && operators.Name.Trim() != string.Empty)
                                {
                                    entity.Entry(operators).State = EntityState.Added;
                                    entity.SaveChanges();
                                }
                            }
                            else if (operators.OperatorID != 0)
                            {
                                if (operators.Name != null && operators.Name.Trim() != string.Empty)
                                {
                                    entity.Entry(operators).State = EntityState.Modified;
                                    entity.SaveChanges();
                                }
                            }
                        }
                        catch { }
                        stocktransfer.Operators.Remove(operators);
                    }
                    #endregion

                    entity.Entry(stocktransfer).State = EntityState.Modified;
                    entity.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch
                {
                    ModelState.AddModelError("", "There's an error.");
                }
            }

            #region DROPDOWNS

            var loc = entity.Locations.Where(x => x.ID != 10)
                        .Select(x => new
                        {
                            ID = x.ID,
                            Description = x.Description
                        });

            ViewBag.StockTransferTypeID = new SelectList(entity.StockTransferTypes.ToList(), "ID", "Name");
            ViewBag.RequisitionID = new SelectList(entity.Requisitions.ToList().FindAll(r => r.ApprovalStatus == 2), "ID", "RefNumber");
            ViewBag.ReceivingID = new SelectList(entity.Receivings.ToList().FindAll(r => r.ApprovalStatus == 2), "ID", "ReceivingID");
            //ViewBag.RequisitionID = new SelectList(entity.Requisitions.ToList().FindAll(r => r.ApprovalStatus == 2), "ID", "RefNumber");
            ViewBag.LocationID = new SelectList(loc, "ID", "Description", stocktransfer.LocationID);
            var empList = from s in entity.Employees
                          select new
                          {
                              ID = s.ID,
                              FullName = s.FirstName + " " + s.LastName
                          };

            ViewBag.EncodedBy = new SelectList(empList, "ID", "FullName", stocktransfer.EncodedBy);
            ViewBag.ReceivedBy = new SelectList(empList, "ID", "FullName", stocktransfer.ReceivedBy);
            ViewBag.RequestedBy = new SelectList(empList, "ID", "FullName", stocktransfer.RequestedBy); ;
            ViewBag.ApprovedBy = new SelectList(empList, "ID", "FullName", stocktransfer.ApprovedBy); ;
            ViewBag.ReleasedBy = new SelectList(empList, "ID", "FullName", stocktransfer.ReleasedBy); ;
            ViewBag.CounterCheckedBy = new SelectList(empList, "ID", "FullName", stocktransfer.CounterCheckedBy); ;
            ViewBag.PostedBy = new SelectList(empList, "ID", "FullName", stocktransfer.PostedBy); ;
            ViewBag.ApprovedStatus = new SelectList(entity.ApprovalStatus, "ID", "Status", stocktransfer.ApprovedStatus);
            #endregion

            return View(stocktransfer);
        }

        // GET: StockTransfer/Delete/5
        [AccessChecker(Action = 3, ModuleID = 4)]
        public ActionResult Delete(int id = 0)
        {
            var stocktransfer = entity.StockTransfers.Find(id);

            if (stocktransfer == null)
            {
                return HttpNotFound();
            }

            return View(stocktransfer);
        }

        // POST: StockTransfer/Delete/5
        [AccessChecker(Action = 3, ModuleID = 4)]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id = 0)
        {
            var stocktransfer = entity.StockTransfers.Find(id);

            if (stocktransfer == null)
            {
                return HttpNotFound();
            }

            try
            {
                entity.StockTransfers.Remove(stocktransfer);
                entity.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
            }

            return View(stocktransfer);
        }

        // POST: StockTransfer/Approve/5
        [AccessChecker(Action = 5, ModuleID = 4)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Approve(int id)
        {
            try
            {
                // TODO: Add delete logic here
                //var pr = entity.StockTransfers.FirstOrDefault(r => r.ID == id && (r.RequestedBy == UserID || AcctType == 1 || AcctType == 4));
                var st = entity.StockTransfers.Find(id);
                if (st.StockTransferDetails.Count > 0)
                {
                    st.ApprovedStatus = 2;
                    st.IsSync = false;

                    entity.Entry(st).State = EntityState.Modified;
                    entity.SaveChanges();
                    var rd = st.Requisition.RequisitionDetails.Select(p => p.ItemCode).ToList();
                    var item = entity.Items.Where(i => rd.Contains(i.ID.ToString())).Select(i => i.Code);
                    var inv = entity.Inventories.Where(i => item.Contains(i.ItemCode) && i.LocationCode == st.Requisition.LocationID).ToList();
                    if (inv != null)
                    {
                        foreach (var _inv in inv)
                        {
                            var i = entity.Inventories.Find(_inv.ID);
                            i.Committed = invRepo.getCommited(_inv.ItemCode);
                            i.Ordered = invRepo.getPurchaseOrder(_inv.ItemCode);
                            i.InStock = invRepo.getInstocked(st.RequisitionID.Value, _inv.ItemCode);
                            i.Available = (i.InStock + i.Ordered) - i.Committed;

                            entity.Entry(i).State = EntityState.Modified;
                            entity.SaveChanges();
                        }

                    }

                   

                    return RedirectToAction("Index");
                }
            }
            catch
            {
            }
            return View();
        }

        // POST: StockTransfer/Denied/5
        [AccessChecker(Action = 5, ModuleID = 4)]
        [HttpPost]
        public ActionResult Denied(int id)
        {
            try
            {
                // TODO: Add delete logic here
                var st = entity.StockTransfers.Find(id);
                st.ApprovedStatus = 3;
                st.IsSync = false;

                entity.Entry(st).State = EntityState.Modified;
                entity.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: StockTransfer/ApprovedItems/5
        [AccessChecker(Action = 1, ModuleID = 4)]
        public ActionResult ApprovedItems(int id, int? page)
        {
            var items = entity.StockTransferDetails
                        .ToList()
                        .FindAll(rd => rd.StockTransferID == id && rd.AprovalStatusID == 2);

            ViewBag.STid = id;

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        // GET: StockTransfer/PendingItems/5
        [AccessChecker(Action = 1, ModuleID = 4)]
        public ActionResult PendingItems(int id, int? page)
        {
            int UserID = Convert.ToInt32(Session["sessionuid"]);
            int UserType = Convert.ToInt32(Session["usertype"]);

            var RequestedBy = entity.StockTransfers.FirstOrDefault(r => r.ID == id).RequestedBy;
            if (RequestedBy != UserID && UserType != 1 && UserType != 4)
            {
                return RedirectToAction("Index", "Home");
            }

            var items = entity.StockTransferDetails
                        .ToList()
                        .FindAll(rd => rd.StockTransferID == id && rd.AprovalStatusID == 1 && (rd.StockTransfer.RequestedBy == UserID || UserType == 1 || UserType == 4));

            //var items = entity.RequisitionDetails
            //            .ToList()
            //            .FindAll(rd => rd.RequisitionID == id && rd.AprovalStatusID == 1 && rd.Requisition.RequestedBy == UserID);

            ViewBag.STid = id;
            ViewBag.RequestedBy =
            ViewBag.UserID = UserID;
            ViewBag.AcctType = UserType;

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);
            return PartialView(items.ToPagedList(pageNumber, pageSize));
        }

        // GET: StockTransfer/DeniedItems/5
        [AccessChecker(Action = 1, ModuleID = 4)]
        public ActionResult DeniedItems(int id, int? page)
        {
            var items = entity.StockTransferDetails
                        .ToList()
                        .FindAll(rd => rd.StockTransferID == id && rd.AprovalStatusID == 3);

            ViewBag.STid = id;

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        // POST: StockTransfer/ApproveItem/5
        [AccessChecker(Action = 1, ModuleID = 4)]
        public ActionResult ApproveItem(int id, int itemID)
        {
            try
            {
                var item = entity.StockTransferDetails.Find(itemID);
                if (item != null)
                {
                    item.AprovalStatusID = 2;
                    item.IsSync = false;

                    var reqDetails = entity.RequisitionDetails.Find(item.RequisitionDetailID);
                    if (reqDetails != null)
                    {
                        reqDetails.Quantity = reqDetails.Quantity - item.Quantity;

                        entity.Entry(reqDetails).State = EntityState.Modified;
                    }

                    entity.Entry(item).State = EntityState.Modified;
                    entity.SaveChanges();

                    #region WAC                    
                    //var itm = entity.Items.Where(i => i.ID == item.RequisitionDetail.ItemID).FirstOrDefault();
                    //if (item.Quantity <= itm.Quantity)
                    //{
                    //    var itmDetail = from s in entity.ItemDetail
                    //                    where s.ItemID == itm.ID
                    //                    select s;

                    //    itm.Quantity -= item.Quantity;

                    //    //decimal qtyCost = 0;

                    //    //foreach (var detail in itmDetail)
                    //    //    qtyCost += Convert.ToDecimal(detail.Quantity * detail.Cost);

                    //    //itm.Price = Convert.ToDecimal((qtyCost) / itm.Quantity);

                    //    //WAC

                    //    entity.Entry(itm).State = EntityState.Modified;
                    //    entity.SaveChanges();
                    //}
                    #endregion
                }

               

            }
            catch
            {

            }
            return RedirectToAction("PendingItems", "StockTransfer", new { id = id });
        }

        // POST: StockTransfer/DenyItem/5
        [AccessChecker(Action = 1, ModuleID = 4)]
        public ActionResult DenyItem(int id, int itemID)
        {
            try
            {
                var item = entity.StockTransferDetails.Find(itemID);
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
            return RedirectToAction("PendingItems", "StockTransfer", new { id = id });
        }

        #region PARTIAL

        // GET: StockTransfer/AddItemPartial/5
        [AccessChecker(Action = 2, ModuleID = 4)]
        public ActionResult AddItemPartial(int id)
        {

            var recID = entity.StockTransfers.Find(id);
            int recIDs = Convert.ToInt32(recID.ReceivingID);
            var items = entity.ReceivingDetails
                        .ToList()
                        .FindAll(rd => rd.ReceivingID == recIDs && rd.AprovalStatusID == 2)
                        .Select(ed => new
                        {
                            ID = ed.ID,
                            Description = ed.RequisitionDetail.Item.Description
                        });

            var reqID = entity.StockTransfers.Find(id);
            int reqIDs = Convert.ToInt32(reqID.RequisitionID);
            var _items = entity.RequisitionDetails
                        .ToList()
                        .FindAll(rd => rd.RequisitionID == reqIDs && rd.AprovalStatusID == 2)
                        .Select(ed => new
                        {
                            ID = ed.ID,
                            Description = ed.Item.Description
                        });



            //ViewBag.STid = id;
            ViewBag.Post = recID.ReceivingID;
            ViewBag.ReceivingDetailID = new SelectList(items, "ID", "Description");
            ViewBag.RequisitionDetailID = new SelectList(_items, "ID", "Description");
            return PartialView();
        }

        // POST: StockTransfer/AddItemPartial/5
        [AccessChecker(Action = 2, ModuleID = 4)]
        [HttpPost]
        public ActionResult AddItemPartial(int id, StockTransferDetail stocktransferdetail)
        {
            try
            {

                stocktransferdetail.StockTransferID = id;
                stocktransferdetail.AprovalStatusID = 1; //submitted

                #region COMMENT
                //var receivingID = entity.StockTransfers.Find(stocktransferdetail.StockTransferID).ReceivingID;
                //var receivingdetailID = entity.ReceivingDetails.Find(receivingID).ID;

                //var items = entity.ReceivingDetails
                //            .ToList()
                //            .FindAll(rd => rd.ID == receivingdetailID && rd.AprovalStatusID == 2)
                //            .Select(ed => new
                //            {
                //                ID = ed.ID,
                //                Description = ed.RequisitionDetail.Item.Description
                //            }).FirstOrDefault();

                //stocktransferdetail.ReceivingDetailID = items.ID;


                //var reqID = entity.ReceivingDetails.Find(stocktransferdetail.ReceivingDetailID).RequisitionDetailID;
                //var itmID = entity.RequisitionDetails.Find(reqID).ItemID;


                //var com = entity.RequisitionDetails.Where(model => model.ItemID == itmID && model.AprovalStatusID == 2);
                //var committed = com.Sum(x => x.Quantity) ?? 0;
                //var pur = entity.RequisitionDetails.Where(model => model.Requisition.ReqTypeID == 1 && model.AprovalStatusID == 2 && model.ItemID == itmID);
                //var porder = pur.Sum(x => x.Quantity) ?? 0;

                //stocktransferdetail.Committed = committed;
                //stocktransferdetail.Ordered = porder;

                //stocktransferdetail.Available = (stocktransferdetail.InStock + stocktransferdetail.Ordered) - stocktransferdetail.Committed;
                #endregion

                var st = entity.StockTransferDetails.Where(s => s.StockTransferID == stocktransferdetail.StockTransferID && s.ReceivingDetailID == stocktransferdetail.ReceivingDetailID).ToList();
                
                if (st.Count() > 0)
                {
                    TempData["PartialError"] = "Item is already in the list.";
                }
                else
                {
                    stocktransferdetail.IsSync = false;

                    entity.StockTransferDetails.Add(stocktransferdetail);
                    entity.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                TempData["PartialError"] = "There's an error.";
            }

            //ViewBag.PRid = id;
            //ViewBag.ItemID = new SelectList(entity.Items, "ID", "Description", rd.ItemID);
            //ViewBag.AprovalStatusID = new SelectList(entity.ApprovalStatus, "ID", "Status", rd.AprovalStatusID);

            //return RedirectToAction("PendingItems", new { id = id });
            return RedirectToAction("Details", new { id = id });
        }



        // GET: StockTransfer/EditItemPartial/5
        [AccessChecker(Action = 2, ModuleID = 4)]
        public ActionResult EditItemPartial(int id)
        {
            var st = entity.StockTransferDetails.Find(id);

            ViewBag.AprovalStatusID = new SelectList(entity.ApprovalStatus, "ID", "Status", st.AprovalStatusID);

            return PartialView(st);
        }

        // POST: StockTransfer/EditItemPartial/5
        [AccessChecker(Action = 2, ModuleID = 4)]
        [HttpPost]
        public ActionResult EditItemPartial(int id, StockTransferDetail stocktransfer)
        {
            try
            {
                var sdetails = entity.StockTransferDetails.Find(stocktransfer.ID);

                int presentQty = Convert.ToInt32(stocktransfer.Quantity);
                int previousQty = Convert.ToInt32(sdetails.Quantity);

                if (sdetails.ReceivingDetailID != stocktransfer.ReceivingDetailID || sdetails.Quantity != stocktransfer.Quantity)
                {
                    stocktransfer.PreviousItemID = sdetails.ReceivingDetailID;
                    stocktransfer.PreviousQuantity = sdetails.Quantity;
                }

                sdetails.ReceivingDetailID = stocktransfer.ReceivingDetailID;
                sdetails.Quantity = stocktransfer.Quantity;
                sdetails.InStock = stocktransfer.InStock;
                sdetails.Remarks = stocktransfer.Remarks;
                sdetails.PreviousItemID = stocktransfer.PreviousItemID;
                sdetails.PreviousQuantity = stocktransfer.PreviousQuantity;

                if ((presentQty != previousQty) && stocktransfer.AprovalStatusID == 2)
                {
                    int itemID = Convert.ToInt32(entity.RequisitionDetails.Find(stocktransfer.RequisitionDetailID).ItemID);
                    var itm = entity.Items.Where(i => i.ID == itemID).FirstOrDefault();
                    #region WAC    

                    if (presentQty <= itm.Quantity)
                    {
                        var itmDetail = entity.ItemDetail.Where(i => i.ItemID == itm.ID).ToList();
                        var itemDetail = entity.ItemDetail.Find(itm.ID);

                        int prevSTItemQty = Convert.ToInt32(itm.Quantity + previousQty);

                        decimal qtyCost = 0;

                        foreach (var detail in itmDetail)
                            qtyCost += Convert.ToDecimal(detail.Quantity * detail.Cost);

                        itm.Quantity = prevSTItemQty - presentQty;
                        //double Price = Convert.ToDouble((qtyCost) / itm.Quantity);
                        //itm.Price = Convert.ToDecimal(Price);

                        //if (itmDetail.Count() > 1)
                        //{
                        //    //WAC             
                        //    int prevQty = Convert.ToInt32(itm.Quantity - itmDetails.Quantity);
                        //    itm.WeightedAverageCost = Convert.ToDecimal((itmDetails.Quantity * itmDetails.Cost) / prevQty);
                        //}
                        //else
                        //    itm.WeightedAverageCost = 0;
                    }
                    #endregion                    
                }

                sdetails.IsSync = false;
                entity.Entry(sdetails).CurrentValues.SetValues(stocktransfer);
                entity.SaveChanges();
            }
            catch (Exception ex)
            {
                TempData["PartialError"] = "There's an error.";
            }

            if (stocktransfer.AprovalStatusID == 1)
            {
                #region WAC    
                //var st = entity.StockTransferDetails.Where(s => s.ID == stocktransfer.ID).FirstOrDefault();
                //var req = entity.RequisitionDetails.Where(r => r.ID == stocktransfer.RequisitionDetailID).FirstOrDefault();
                //var itm = entity.Items.Where(i => i.ID == req.ItemID).FirstOrDefault();
                //if (st.Quantity <= itm.Quantity)
                //{
                //    var itmDetail = from s in entity.ItemDetail
                //                    where s.ItemID == itm.ID
                //                    select s;

                //    itm.Quantity += st.Quantity;

                //    decimal qtyCost = 0;

                //    foreach (var detail in itmDetail)
                //        qtyCost += Convert.ToDecimal(detail.Quantity * detail.Cost);

                //    double WeightedAverageCost = Convert.ToDouble((qtyCost) / itm.Quantity);
                //    itm.WeightedAverageCost = Convert.ToDecimal(WeightedAverageCost);

                //    entity.Entry(itm).State = EntityState.Modified;
                //    entity.SaveChanges();
                //}
                #endregion
                //return RedirectToAction("PendingItems", new { id = stocktransfer.StockTransferID });
                return RedirectToAction("Details", new { id = stocktransfer.StockTransferID });
            }
            return RedirectToAction("ApprovedItems", new { id = stocktransfer.StockTransferID });
        }

        // GET: StockTransfer/DeleteItemPartial/5
        [AccessChecker(Action = 3, ModuleID = 4)]
        public ActionResult DeleteItemPartial(int id)
        {
            var st = entity.StockTransferDetails.Find(id);

            return PartialView(st);
        }

        // POST: StockTransfer/DeleteItemPartial/5
        [AccessChecker(Action = 3, ModuleID = 4)]
        [HttpPost, ActionName("DeleteItemPartial")]
        public ActionResult DeleteItemPartialConfirm(int id)
        {
            var st = entity.StockTransferDetails.Find(id);

            int? reqID = st.StockTransferID;
            try
            {
                entity.StockTransferDetails.Remove(st);
                entity.SaveChanges();
            }
            catch
            {
                TempData["PartialError"] = "There's an error.";
            }

            //return RedirectToAction("PendingItems", new { id = reqID });

            return RedirectToAction("Details", new { id = reqID });
        }
        #endregion

        // -Pearl
        [HttpPost]
        public ActionResult GetLocationCode(int locID)
        {
            var code = entity.Locations
                .Where(s => s.ID == locID)
                .Select(s => new { s.Code });

            return Json(code, JsonRequestBehavior.AllowGet);
        }

        [AccessChecker(Action = 1, ModuleID = 5)]
        public ActionResult GetRequisitionDetail(int id)
        {
            var rd = entity.RequisitionDetails
                    .Include(r => r.Item)
                    .Where(r => r.ID == id)
                    .Select(r => new
                    {
                        Quantity = r.Quantity,
                    })
                    .FirstOrDefault();

            return Json(rd, JsonRequestBehavior.AllowGet);
        }

        public ActionResult StockDetails(int id)
        {
            return View(entity.StockTransferDetails.Find(id));
        }

        public ActionResult DisplayComputations(int? recID)
        {
            var itmID = entity.ReceivingDetails.Find(recID);
            var itmsDesc = entity.Items.FirstOrDefault(x => x.ID == itmID.RequisitionDetail.ItemID).Description;
            var com = entity.RequisitionDetails.Where(model => model.Requisition.RequisitionTypeID == 4 && model.AprovalStatusID == 2 && model.ItemID == itmID.RequisitionDetail.ItemID);
            var pur = entity.RequisitionDetails.Where(model => model.Requisition.RequisitionTypeID == 1 && model.AprovalStatusID == 2 && model.ItemID == itmID.RequisitionDetail.ItemID);
            var instock = entity.Inventories.FirstOrDefault(x => x.Description == itmsDesc && x.LocationCode == itmID.Receiving.LocationID).InStock;
            var qty = entity.ReceivingDetails.FirstOrDefault(model => model.ID == recID).Quantity;


            var computations = entity.RequisitionDetails
                               .Where(x => x.ItemID == itmID.RequisitionDetail.ItemID && x.AprovalStatusID == 2)
                               .Select(x => new
                               {
                                   Committed = com.Sum(y => y.Quantity) ?? 0,
                                   Ordered = pur.Sum(z => z.Quantity) ?? 0,
                                   InStock = instock,
                                   Quantity = qty
                               })
                               .FirstOrDefault();

            return Json(computations, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DisplayComputation(int? reqID)
        {
            var type = new int[] { 2, 3, 4 };
            var itmID = entity.RequisitionDetails.Find(reqID);
            var itmsDesc = entity.Items.FirstOrDefault(x => x.ID == itmID.ItemID).Description;
            var com = entity.RequisitionDetails.Where(model => type.Contains(model.Requisition.RequisitionTypeID.Value) && model.AprovalStatusID == 2 && model.ItemID == itmID.ItemID);
            var pur = entity.RequisitionDetails.Where(model => model.Requisition.RequisitionTypeID == 1 && model.AprovalStatusID == 2 && model.ItemID == itmID.ItemID);
            var instock = entity.Inventories.FirstOrDefault(x => x.Description == itmsDesc && x.LocationCode == itmID.Requisition.LocationID).InStock;
            var qty = entity.RequisitionDetails.FirstOrDefault(model => model.ID == reqID).Quantity;

            var st = entity.StockTransfers.Where(p => p.ApprovedStatus == 1).Select(p => p.ID).ToList();
            var stdetails = entity.StockTransferDetails.Where(model => model.RequisitionDetail.ItemID == itmID.ItemID && model.AprovalStatusID == 2 && st.Contains(model.StockTransferID.Value));
            var _out = stdetails.Sum(x => x.Quantity);
            int _stdetails = Convert.ToInt32(_out);




            var computations = entity.RequisitionDetails
                               .Where(x => x.ItemID == itmID.ItemID && x.AprovalStatusID == 2)
                               .Select(x => new
                               {
                                   Committed = com.Sum(y => y.Quantity) ?? 0,
                                   Ordered = pur.Sum(z => z.Quantity) ?? 0,
                                   InStock = instock - _stdetails,
                                   Quantity = qty
                               })
                               .FirstOrDefault();

            return Json(computations, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}