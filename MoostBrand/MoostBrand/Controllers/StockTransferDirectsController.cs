using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MoostBrand.DAL;
using System.Configuration;
using PagedList;
using MoostBrand.Models;

namespace MoostBrand.Controllers
{
    public class StockTransferDirectsController : Controller
    {
        private MoostBrandEntities entity = new MoostBrandEntities();

        #region Action
        // GET: StockTransferDirects
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
            var sts = from o in entity.StockTransfers.Where(p => p.StockTransferTypeID == 4)
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

        [AccessChecker(Action = 1, ModuleID = 4)]
        public ActionResult Details(int? page, int id = 0)
        {
            var stocktransfer = entity.StockTransfers.Find(id);

            if (stocktransfer == null)
            {
                return HttpNotFound();
            }
            ViewBag.Page = page;
            return View(stocktransfer);
        }

        // GET: StockTransferDirects/Create
        public ActionResult Create(string id)
        {
            var stDirect = new StockTransfer();
            stDirect.STDAte = DateTime.Now;
            stDirect.TransferID = "STR " + Generator(id);

            #region DROPDOWNS
            var items = entity.StockTransferDetails.Where(r => r.RequisitionDetailID != null )
                .ToList()
                .FindAll(r => r.AprovalStatusID == 2)
                .Select(rd => new
                {
                    ID = rd.RequisitionDetail.Item.DescriptionPurchase, //rd.ID,
                    Description = rd.RequisitionDetail.Item.DescriptionPurchase
                });

            var empList = new SelectList((from s in entity.Employees
                                          select new
                                          {
                                              ID = s.ID,
                                              FullName = s.FirstName + " " + s.LastName
                                          }), "ID", "FullName");

            var loc = entity.Locations.Where(x => x.ID != 10)
            .Select(x => new
            {
                ID = x.ID,
                Description = x.Description
            });


            ViewBag.ItemID = new SelectList(items.Where(p=>p.ID == ""), "ID", "Description");
            ViewBag.LocationID = new SelectList(loc, "ID", "Description");
            ViewBag.DestinationID = new SelectList(loc, "ID", "Description");
            ViewBag.ReceivedBy = empList;
            ViewBag.RequestedBy = empList;
            ViewBag.ApprovedBy = empList;
            ViewBag.ReleasedBy = empList;
            ViewBag.CounterCheckedBy = empList;
            ViewBag.PostedBy = empList;
            ViewBag.EncodedBy = empList;
            ViewBag.ApprovedStatus = new SelectList(entity.ApprovalStatus, "ID", "Status");
            #endregion

            return View(stDirect);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StockTransfer stockTransferDirect)
        {

            //string itemname = entity.Items.Find(stockTransferDirect.ItemID).Description;

            var stDirect = entity.StockTransfers.Where(s => s.TransferID == stockTransferDirect.TransferID).ToList();

            if (stDirect.Count() > 0)
            {
                ModelState.AddModelError("", "Stock Transfer ID already exists");
            }
            else
            {
                #region Helper
                foreach (Helper h in stockTransferDirect.Helpers.ToList())
                {
                    try
                    {
                        if (h.DeletedHelper == 1)
                        {
                            stockTransferDirect.Helpers.Remove(h);
                        }

                    }
                    catch
                    {
                        ModelState.AddModelError(string.Empty, "There's an error");
                    }
                }
                #endregion
                #region Operator
                foreach (Operator o in stockTransferDirect.Operators.ToList())
                {
                    try
                    {
                        if (o.DeletedOperator == 1)
                        {
                            stockTransferDirect.Operators.Remove(o);
                        }
                    }
                    catch
                    {
                        ModelState.AddModelError(string.Empty, "There's an error");
                    }
                }
                #endregion
                if (stockTransferDirect.LocationID == stockTransferDirect.DestinationID)
                {
                    ModelState.AddModelError("", "Source location should not be the same with the destination.");
                }
                else
                {

                    //stockTransferDirect.ItemName = itemname;
                    stockTransferDirect.ApprovedStatus = 1;
                    stockTransferDirect.IsSync = false;
                    stockTransferDirect.StockTransferTypeID = 4;

                    entity.StockTransfers.Add(stockTransferDirect);
                    entity.SaveChanges();
                    return RedirectToAction("Index");
                }
            

            }

            #region DROPDOWNS
            var items = entity.StockTransferDetails.Where(r => r.RequisitionDetailID != null)
              .ToList()
              .FindAll(r => r.AprovalStatusID == 2)
              .Select(rd => new
              {
                    ID = rd.RequisitionDetail.Item.DescriptionPurchase, //rd.ID,
                    Description = rd.RequisitionDetail.Item.DescriptionPurchase
              });

            var empList = from s in entity.Employees
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

            ViewBag.ItemID = new SelectList(items, "ID", "Description");
            ViewBag.LocationID = new SelectList(loc, "ID", "Description", stockTransferDirect.LocationID);
            ViewBag.DestinationID = new SelectList(loc, "ID", "Description", stockTransferDirect.DestinationID);
            ViewBag.ReceivedBy = new SelectList(empList, "ID", "FullName", stockTransferDirect.ReceivedBy);
            ViewBag.RequestedBy = new SelectList(empList, "ID", "FullName", stockTransferDirect.RequestedBy);
            ViewBag.ApprovedBy = new SelectList(empList, "ID", "FullName", stockTransferDirect.ApprovedBy);
            ViewBag.ReleasedBy = new SelectList(empList, "ID", "FullName", stockTransferDirect.ReleasedBy);
            ViewBag.CounterCheckedBy = new SelectList(empList, "ID", "FullName", stockTransferDirect.CounterCheckedBy);
            ViewBag.PostedBy = new SelectList(empList, "ID", "FullName", stockTransferDirect.PostedBy);
            ViewBag.EncodedBy = new SelectList(empList, "ID", "FullName", stockTransferDirect.EncodedBy);
            ViewBag.ApprovedStatus = new SelectList(entity.ApprovalStatus, "ID", "Status", stockTransferDirect.ApprovedStatus);
            #endregion

            return View(stockTransferDirect);
        }

        // GET: StockTransfer/AddItemPartial/5
        [AccessChecker(Action = 2, ModuleID = 4)]
        public ActionResult AddItemPartial(int id)
        {

            var recID = entity.StockTransfers.Find(id);
            var items = entity.Inventories
                        .ToList()
                        .FindAll(rd => rd.LocationCode == recID.LocationID && rd.InStock > 0) 
                        .Select(ed => new
                        {
                            ID = ed.ID,
                            Description = ed.Description
                        }).OrderBy(p=>p.Description);

     
            ViewBag.InventoryID = new SelectList(items, "ID", "Description");

            return PartialView();
        }

      
        [AccessChecker(Action = 2, ModuleID = 4)]
        [HttpPost]
        public ActionResult AddItemPartial(int id, StockTransferDetail stocktransferdetail)
        {
            try
            {

                stocktransferdetail.StockTransferID = id;
                stocktransferdetail.AprovalStatusID = 1;

                #region COMMENT
              
                #endregion

                var st1 = entity.StockTransferDetails.Where(s => s.StockTransferID == stocktransferdetail.StockTransferID && s.InventoryID == stocktransferdetail.InventoryID).ToList();


                if (stocktransferdetail.InventoryID != null)
                {
                    if (st1.Count() >= 1)
                    {
                        TempData["PartialError"] = "Item is already in the list.";
                    }
                    else
                    {
                        stocktransferdetail.IsSync = false;
                        stocktransferdetail.ReferenceQuantity = stocktransferdetail.Quantity;
                        entity.StockTransferDetails.Add(stocktransferdetail);
                        entity.SaveChanges();
                    }
                }
               

            }
            catch (Exception ex)
            {
                TempData["PartialError"] = "There's an error.";
            }

           
            return RedirectToAction("Details", new { id = id });
        }

        // GET: StockTransferDirects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var stockTransferDirect = entity.StockTransfers.Find(id);
            if (stockTransferDirect == null)
            {
                return HttpNotFound();
            }
            
            if(stockTransferDirect.ApprovedStatus == 1)
            {

                #region DROPDOWNS
                var items = entity.Items
                    .ToList()
                    .Select(rd => new
                    {
                        ID = rd.ID,
                        Description = rd.Description
                    });

                var empList = from s in entity.Employees
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


                //ViewBag.ItemID = new SelectList(items, "ID", "Description", stockTransferDirect.ItemID);
                ViewBag.LocationID = new SelectList(loc, "ID", "Description", stockTransferDirect.LocationID);
                ViewBag.DestinationID = new SelectList(loc.Where(p=>p.ID != stockTransferDirect.LocationID), "ID", "Description", stockTransferDirect.DestinationID);
                ViewBag.ReceivedBy = new SelectList(empList, "ID", "FullName", stockTransferDirect.ReceivedBy);
                ViewBag.RequestedBy = new SelectList(empList, "ID", "FullName", stockTransferDirect.RequestedBy);
                ViewBag.ApprovedBy = new SelectList(empList, "ID", "FullName", stockTransferDirect.ApprovedBy);
                ViewBag.ReleasedBy = new SelectList(empList, "ID", "FullName", stockTransferDirect.ReleasedBy);
                ViewBag.CounterCheckedBy = new SelectList(empList, "ID", "FullName", stockTransferDirect.CounterCheckedBy);
                ViewBag.PostedBy = new SelectList(empList, "ID", "FullName", stockTransferDirect.PostedBy);
                ViewBag.EncodedBy = new SelectList(empList, "ID", "FullName", stockTransferDirect.EncodedBy);
                ViewBag.ApprovedStatus = new SelectList(entity.ApprovalStatus, "ID", "Status", stockTransferDirect.ApprovedStatus);
                #endregion

            }

            return View(stockTransferDirect);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StockTransfer stockTransferDirect)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    stockTransferDirect.IsSync = false;

                    #region Helper
                    foreach (Helper helper in stockTransferDirect.Helpers.ToList())
                    {
                        try
                        {
                            if (helper.DeletedHelper == 1 && helper.HelperID != 0)
                            {
                                entity.Entry(helper).State = EntityState.Deleted;
                                entity.SaveChanges();
                            }
                            else if (helper.HelperID != stockTransferDirect.ID)
                            {
                                helper.HelperID = stockTransferDirect.ID;

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
                        stockTransferDirect.Helpers.Remove(helper);
                    }
                    #endregion

                    #region Operator
                    foreach (Operator operators in stockTransferDirect.Operators.ToList())
                    {
                        try
                        {
                            if (operators.DeletedOperator == 1 && operators.OperatorID != 0)
                            {
                                entity.Entry(operators).State = EntityState.Deleted;
                                entity.SaveChanges();
                            }
                            else if (operators.OperatorID != stockTransferDirect.ID)
                            {
                                operators.OperatorID = stockTransferDirect.ID;

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
                        stockTransferDirect.Operators.Remove(operators);
                    }
                    #endregion
                    //string itemname = entity.Items.Find(stockTransferDirect.ItemID).Description;
                    //stockTransferDirect.ItemName = itemname;
                    entity.Entry(stockTransferDirect).State = EntityState.Modified;
                    entity.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    ModelState.AddModelError("", "There's an error.");
                }
            }

            #region DROPDOWNS
            var items = entity.Items
                .ToList()
                .Select(rd => new
                {
                    ID = rd.ID,
                    Description = rd.Description
                });

            var empList = from s in entity.Employees
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


            //ViewBag.ItemID = new SelectList(items, "ID", "Description", stockTransferDirect.ItemID);
            ViewBag.LocationID = new SelectList(loc, "ID", "Description", stockTransferDirect.LocationID);
            ViewBag.DestinationID = new SelectList(loc.Where(p=>p.ID != stockTransferDirect.LocationID), "ID", "Description", stockTransferDirect.DestinationID);
            ViewBag.ReceivedBy = new SelectList(empList, "ID", "FullName", stockTransferDirect.ReceivedBy);
            ViewBag.RequestedBy = new SelectList(empList, "ID", "FullName", stockTransferDirect.RequestedBy);
            ViewBag.ApprovedBy = new SelectList(empList, "ID", "FullName", stockTransferDirect.ApprovedBy);
            ViewBag.ReleasedBy = new SelectList(empList, "ID", "FullName", stockTransferDirect.ReleasedBy);
            ViewBag.CounterCheckedBy = new SelectList(empList, "ID", "FullName", stockTransferDirect.CounterCheckedBy);
            ViewBag.PostedBy = new SelectList(empList, "ID", "FullName", stockTransferDirect.PostedBy);
            ViewBag.EncodedBy = new SelectList(empList, "ID", "FullName", stockTransferDirect.EncodedBy);
            ViewBag.ApprovedStatus = new SelectList(entity.ApprovalStatus, "ID", "Status", stockTransferDirect.ApprovedStatus);
            #endregion

            return View(stockTransferDirect);
        }

        [AccessChecker(Action = 2, ModuleID = 4)]
        public ActionResult EditItemPartial(int id)
        {
            var st = entity.StockTransferDetails.Find(id);

            ViewBag.AprovalStatusID = new SelectList(entity.ApprovalStatus, "ID", "Status", st.AprovalStatusID);
            
             ViewBag.Qty = entity.Inventories.FirstOrDefault(model => model.ID == st.InventoryID).InStock;
         

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

                if (sdetails.InventoryID != stocktransfer.InventoryID)
                {
                    stocktransfer.PreviousItemID = sdetails.InventoryID;
                    stocktransfer.PreviousQuantity = sdetails.Quantity;
                }

                sdetails.InventoryID = stocktransfer.InventoryID;
                sdetails.Quantity = stocktransfer.Quantity;
                sdetails.ReferenceQuantity = stocktransfer.Quantity;
                sdetails.InStock = stocktransfer.InStock;
                sdetails.Remarks = stocktransfer.Remarks;
                sdetails.PreviousItemID = stocktransfer.PreviousItemID;
                sdetails.PreviousQuantity = stocktransfer.PreviousQuantity;

                if ((presentQty != previousQty) && stocktransfer.AprovalStatusID == 2)
                {
                    int itemID = Convert.ToInt32(entity.Inventories.Find(stocktransfer.InventoryID).ItemID);
                    var itm = entity.Items.Where(i => i.ID == itemID).FirstOrDefault();
                    #region WAC    

                    if (presentQty <= itm.Quantity)
                    {
                        //var itmDetail = entity.ItemDetail.Where(i => i.ItemID == itm.ID).ToList();
                        //var itemDetail = entity.ItemDetail.Find(itm.ID);

                        int prevSTItemQty = Convert.ToInt32(itm.Quantity + previousQty);

                        decimal qtyCost = 0;

                        //foreach (var detail in itmDetail)
                        //    qtyCost += Convert.ToDecimal(detail.Quantity * detail.Cost);

                        itm.Quantity = prevSTItemQty - presentQty;
                        
                    }
                    #endregion                    
                }

                sdetails.IsSync = false;
                entity.Entry(sdetails).CurrentValues.SetValues(stocktransfer);
                entity.SaveChanges();

                var _st = entity.StockTransferDetails.Find(stocktransfer.ID);
                _st.ReferenceQuantity = stocktransfer.Quantity;
                entity.SaveChanges();
            }
            catch (Exception ex)
            {
                TempData["PartialError"] = "There's an error.";
            }

            if (stocktransfer.AprovalStatusID == 1)
            {
                #region WAC    
                
                #endregion
                return RedirectToAction("Details", new { id = id });
            }
            return RedirectToAction("ApprovedItems", new { id = stocktransfer.StockTransferID });
        }


        // GET: StockTransferDirects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StockTransferDirect stockTransferDirect = entity.StockTransferDirects.Find(id);
            if (stockTransferDirect == null)
            {
                return HttpNotFound();
            }
            return View(stockTransferDirect);
        }

        // POST: StockTransferDirects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StockTransferDirect stockTransferDirect = entity.StockTransferDirects.Find(id);
            entity.StockTransferDirects.Remove(stockTransferDirect);
            entity.SaveChanges();
            return RedirectToAction("Index");
        }

        #endregion

        #region Methods
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                entity.Dispose();
            }
            base.Dispose(disposing);
        }

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

            var stocktransfer = entity.StockTransfers.FirstOrDefault(r => r.ID == id);
            //var RequestedBy = entity.StockTransfers.FirstOrDefault(r => r.ID == id).RequestedBy;
            //if (RequestedBy != UserID && UserType != 1 && UserType != 4)
            //{
            //    return RedirectToAction("Index", "Home");
            //}

            var items = entity.StockTransferDetails
                        .ToList()
                        .FindAll(rd => rd.StockTransferID == id && rd.AprovalStatusID == 1);

            var stdetails = entity.StockTransferDetails.FirstOrDefault(rd => rd.StockTransferID == id && rd.AprovalStatusID == 2);
            //var items = entity.RequisitionDetails
            //            .ToList()
            //            .FindAll(rd => rd.RequisitionID == id && rd.AprovalStatusID == 1 && rd.Requisition.RequestedBy == UserID);

            ViewBag.STid = id;
            try
            {
                ViewBag.Approved = stdetails.AprovalStatusID.ToString();
            }
            catch { ViewBag.Approved = 1; }
            // ViewBag.RequestedBy =
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

                    entity.Entry(item).State = EntityState.Modified;
                    entity.SaveChanges();

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


        #region ApprovalStatus

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Approve(int id)
        {
            int qty = 0;
            try
            {
                int approve = 0;
                var st = entity.StockTransfers.Find(id);

                if (st.StockTransferDetails.Count > 0)
                {

                    foreach (var _details in st.StockTransferDetails)
                    {
                        if (_details.AprovalStatusID != 1)
                        {
                            approve++;
                            qty += _details.Quantity.Value;
                        }
                    }

                    if (approve == st.StockTransferDetails.Count())
                    {

                        st.ApprovedStatus = 2;
                        st.IsSync = false;

                        entity.Entry(st).State = EntityState.Modified;
                        entity.SaveChanges();




                        foreach (var _inv in st.StockTransferDetails)
                        {

                            var i = entity.Inventories.Find(_inv.InventoryID);
                            i.InStock = Convert.ToInt32(i.InStock) - Convert.ToInt32(_inv.Quantity);
                            i.Available = (i.InStock + i.Ordered) - i.Committed;

                            entity.Entry(i).State = EntityState.Modified;
                            entity.SaveChanges();

                            StockLedger _stockledger = new StockLedger();
                            _stockledger.InventoryID = _inv.InventoryID.Value;
                            _stockledger.Type = "Stock Out";
                            _stockledger.OutQty = _inv.Quantity;
                            _stockledger.ReferenceNo = st.TransferID;
                            _stockledger.BeginningBalance = i.InStock + _stockledger.OutQty;
                            _stockledger.RemainingBalance = i.InStock;
                            _stockledger.Date = DateTime.Now;
                            //_stockledger.Inventories = 

                            entity.StockLedgers.Add(_stockledger);
                            entity.SaveChanges();
                        }

                    }

                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Not all items are approved");
                }

            

                return RedirectToAction("Index");

        }
            catch (Exception e)
            {
                e.Message.ToString();
            }
            return View();
        }
        
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

        public ActionResult getInventoryByLocation(int id)
        {
            var loc = entity.Locations.Find(id);

            var st = entity.Inventories.Where(s => s.LocationCode.Value == loc.ID && s.InStock.Value > 0)
                                        .Select(s=>s.Items)
                                        .OrderBy(s=>s.Description)
                     .Select(r => new
                     {
                         ID = r.ID,
                         ItemName = r.Description
                     });




            return Json(st.OrderBy(p => p.ItemName), JsonRequestBehavior.AllowGet);




        }

        #endregion

        public ActionResult DisplayComputations(int? recID)
        {
           

            var computations = entity.Inventories
                               .Where(x => x.ID == recID && x.InStock >  0)
                               .Select(x => new
                               {
                                   InStock = x.InStock,
                                   Quantity = x.InStock
                               })
                               .FirstOrDefault();

            return Json(computations, JsonRequestBehavior.AllowGet);
        }
    }
}
