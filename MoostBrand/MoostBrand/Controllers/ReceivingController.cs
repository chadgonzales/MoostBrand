﻿using MoostBrand.DAL;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Configuration;
using PagedList;
using MoostBrand.Models;
using System.IO;
using System.Collections.Generic;

namespace MoostBrand.Controllers
{
    [LoginChecker]
    public class ReceivingController : Controller
    {
        MoostBrandEntities entity = new MoostBrandEntities();
        RequisitionDetailsRepository reqDetailRepo = new RequisitionDetailsRepository();

        private int ID;

        #region METHODS

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
            var r = entity.Receivings.ToList().FindAll(p => p.ReceivingID == randomString);
            if (r.Count() > 0)
            {
                goto startR;
            }

            //return the random string
            return randomString;
        }

        private string POGenerator()
        {
        //Initiate objects & vars
        startR: Random random = new Random();
            string randomString = "";
            int randNumber = 0;

            //Loop ‘length’ times to generate a random number or character
            for (int i = 0; i < 5; i++)
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

            randomString = "V-" + randomString;
            var r = entity.Receivings.ToList().FindAll(p => p.PONumber == randomString);
            if (r.Count() > 0)
            {
                goto startR;
            }

            //return the random string
            return randomString;
        }

        private Receiving SetNull(Receiving r)
        {
            if (r.ReceivingTypeID == 1) //PO
            {
                r.EncodedBy = null;
                r.CheckedBy = null;
            }
            else if (r.ReceivingTypeID == 2 || r.ReceivingTypeID == 3 || r.ReceivingTypeID == 5) //BR or WR or OR
            {
                r.VesselNumber = null;
                r.VoyageNumber = null;
                r.VanNumber = null;
            }
            else if (r.ReceivingTypeID == 4) //CR
            {
                r.EncodedBy = null;
                r.CheckedBy = null;
                r.VesselNumber = null;
                r.VoyageNumber = null;
                r.VanNumber = null;
            }
            else
            {
                return null;
            }
            return r;
        }

        public ActionResult GenerateRefNumber(string id)
        {
            return Json(Generator(id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRequisition(int id) //returns Json
        {
            var req = entity.Requisitions
                .Where(r => r.ID == id)
                .Select(r => new
                {
                    RefNumber = r.RefNumber,
                    RequestedBy = r.Employee1.FirstName + " " + r.Employee1.LastName,
                    Destination = r.Location1.Description,
                    SourceLoc = r.Location.Description,
                    Vendor = r.Vendor.Name,
                    VendorCode = r.Vendor.Code,
                    VendorContact = r.Vendor.Attn,
                    CustName = r.Customer,
                    ShipmentType = r.ShipmentType.Type,
                    Invoice = r.InvoiceNumber
                })
                .FirstOrDefault();

            return Json(req, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult getInstock(string Code)
        {
            //var instock = entity.Inventories.FirstOrDefault(x => x.ItemCode == Code);
            //int total;
            //if (instock != null)
            //{
            //    total = Convert.ToInt32(instock.InStock);
            //}
            //else
            //{
            //    total = 0;
            //}

            int requisitionId = Convert.ToInt32(Session["requisitionId"]);
            int total = reqDetailRepo.getInstocked(requisitionId, Code);

            return Json(total, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCommitted(int ItemID)
        {
            //Available = In Stock + Ordered – Committed

            int requisitionId = Convert.ToInt32(Session["requisitionId"]);
            int total = reqDetailRepo.getCommited(requisitionId, ItemID);


            //var num = ItemID;
            return Json(total, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult getCommit(int ItemID)
        {
            int requisitionId = Convert.ToInt32(Session["requisitionId"]);

            int total = reqDetailRepo.getCommited(requisitionId, ItemID);

            return Json(total, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult getPO(int ItemID)
        {
            int requisitionId = Convert.ToInt32(Session["requisitionId"]);

            var requi = entity.Requisitions.Find(requisitionId);

            int total = 0;
            if (requi != null)
            {
                var lstReqDetail = new List<RequisitionDetail>();

                lstReqDetail = entity.RequisitionDetails.Where(x => x.Requisition.RequisitionTypeID == 1 && x.AprovalStatusID == 2 && x.ItemID == ItemID && x.Requisition.LocationID == requi.LocationID).ToList();

                total = lstReqDetail.Sum(x => x.Quantity) ?? 0;
            }
            return Json(total, JsonRequestBehavior.AllowGet);
        }
        public int getCommited(int itemID)
        {
            int c = 0;
            var com = entity.RequisitionDetails.Where(model => model.ItemID == itemID && model.AprovalStatusID == 2 && model.Requisition.RequisitionTypeID == 4);
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
            var pur = entity.RequisitionDetails.Where(model => model.Requisition.RequisitionTypeID == 1 && model.AprovalStatusID == 2 && model.ItemID == itemID);
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
            var query = entity.Inventories.FirstOrDefault(x => x.Description == description);
            if (query != null)
            {
                getIS = Convert.ToInt32(query.InStock);
            }
            return getIS;
        }

        public ActionResult DisplayComputations(int? reqID)
        {
            var itmID = entity.RequisitionDetails.Find(reqID);
            var item = entity.Items.FirstOrDefault(p => p.ID == itmID.ItemID);
            var itmsDesc = entity.Items.FirstOrDefault(x => x.ID == itmID.ItemID).Description;
            //var com = entity.RequisitionDetails.Where(model => model.Requisition.RequisitionTypeID == 4 && model.AprovalStatusID == 2 && model.ItemID == itmID.ItemID);
            //var pur = entity.RequisitionDetails.Where(model => model.Requisition.RequisitionTypeID == 1 && model.AprovalStatusID == 2 && model.ItemID == itmID.ItemID);
            //var instock = entity.Inventories.FirstOrDefault(x => x.Description == itmsDesc && x.LocationCode == itmID.Requisition.LocationID).InStock;

            int requisitionId = Convert.ToInt32(Session["requisitionId"]);
            var requi = entity.Requisitions.Find(requisitionId);

            int com = reqDetailRepo.getCommited(requisitionId, item.ID);
            int pur = 0;
            if (requi != null)
            {
                var lstReqDetail = new List<RequisitionDetail>();

                lstReqDetail = entity.RequisitionDetails.Where(x => x.Requisition.RequisitionTypeID == 1 && x.AprovalStatusID == 2 && x.ItemID == item.ID && x.Requisition.LocationID == requi.LocationID).ToList();

                pur = lstReqDetail.Sum(x => x.Quantity) ?? 0;
            }
            int instock = reqDetailRepo.getInstocked(requisitionId, item.Code);

            var computations = entity.RequisitionDetails
                               .Where(x => x.ItemID == itmID.ItemID && x.AprovalStatusID == 2)
                               .Select(x => new
                               {
                                   //Committed = com.Sum(y => y.Quantity) ?? 0,
                                   //Ordered = pur.Sum(z => z.Quantity) ?? 0,
                                   //InStock = instock
                                   Committed = com,
                                   Ordered = pur,
                                   InStock = instock
                               })
                               .FirstOrDefault();

            return Json(computations, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getRequiLoc(int id)
        {
            var loc = entity.Locations.Find(id);
            var _requisitions = entity.Requisitions.Where(r => r.ApprovalStatus == 2 && r.LocationID == loc.ID)
                    .Select(r => new
                    {
                        ID = r.ID,
                        RefNumber = (r.RefNumber.Contains("PR")) ? "PO" + r.RefNumber.Substring(2) : r.RefNumber
                    });

            return Json(_requisitions, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region COMMENTS
        // GET: Receiving/GetStockTransfers/5
        //public ActionResult GetStockTransfers(int id) //returns Json
        //{
        //    var sts = entity.StockTransfers
        //             .Include(st => st.Requisition)
        //             .ToList()
        //             .FindAll(st => st.Requisition.RequisitionTypeID == id)
        //             .Select(st => new
        //             {
        //                 ID = st.ID,
        //                 TransferID = st.TransferID
        //             });

        //    return Json(sts, JsonRequestBehavior.AllowGet);
        //}


        //public ActionResult GetRequisition(int id) //returns Json
        //{
        //    //var req = entity.StockTransfers
        //    //         .Include(st => st.Requisition)
        //    //         .Where(st => st.ID == id)
        //    //         .Select(st => new
        //    //         {
        //    //             ID = st.RequisitionID,
        //    //             RefNumber = st.Requisition.RefNumber,
        //    //             RequestedBy = st.Requisition.Employee1.FirstName + " " + st.Requisition.Employee1.LastName,
        //    //             Destination = st.Requisition.Location1.Description,
        //    //             SourceLoc = st.Requisition.Location.Description,
        //    //             Vendor = st.Requisition.Vendor.Name,
        //    //             VendorCode = st.Requisition.Vendor.Code,
        //    //             VendorContact = st.Requisition.Vendor.Attn,
        //    //             CustName = st.Requisition.Customer,
        //    //             ShipmentType = st.Requisition.ShipmentType.Type,
        //    //             Invoice = st.Requisition.InvoiceNumber
        //    //         })
        //    //         .FirstOrDefault();
        //    return Json(req, JsonRequestBehavior.AllowGet);
        //}

        #endregion

        #region ACTIONS
        // GET: Receiving
        [AccessChecker(Action = 1, ModuleID = 5)]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "type" : "";
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "recno" : "";

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

            var rrs = from o in entity.Receivings
                      select o;

            if (!String.IsNullOrEmpty(searchString))
            {
                rrs = rrs.Where(o => o.ReceivingType.Type.Contains(searchString)
                                       || o.ReceivingID.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "type":
                    rrs = rrs.OrderByDescending(o => o.ReceivingType.Type);
                    break;
                case "recno":
                    rrs = rrs.OrderByDescending(o => o.ReceivingID);
                    break;
                default:
                    rrs = rrs.OrderBy(o => o.ID);
                    break;
            }

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);

            //if (user.LocationID != 10)
            //{
            //    rrs = rrs.Where(x => x.LocationID == locID);
            //    return View(rrs.ToPagedList(pageNumber, pageSize));
            //}
            //else
            //    return View(rrs.ToPagedList(pageNumber, pageSize));

            return View(rrs.ToPagedList(pageNumber, pageSize));
        }

        // GET: Receiving/Details/5
        [AccessChecker(Action = 1, ModuleID = 5)]
        public ActionResult Details(int id = 0)
        {
            Receiving receiving = entity.Receivings.Find(id);
            Session["requisitionId"] = receiving.RequisitionID;

            //var pr = entity.Requisitions.FirstOrDefault(r => r.ID == id && (r.RequestedBy == UserID || AcctType == 1 || AcctType == 4));
            var r = entity.Receivings.Find(id);
            if (r == null)
            {
                return HttpNotFound();
            }

            return View(r);
        }


        // GET: Receiving/Create/
        [AccessChecker(Action = 2, ModuleID = 5)]
        public ActionResult Create()
        {
            var receiving = new Receiving();
            receiving.ReceivingDate = DateTime.Now;
            receiving.PONumber = POGenerator();

            int userID = Convert.ToInt32(Session["userID"]);

            #region DROPDOWNS
            var loc = entity.Locations.Where(x => x.ID != 10)
                .Select(x => new
                {
                    ID = x.ID,
                    Description = x.Description
                });

            //var _requisitions = entity.Requisitions.Where(r => r.ApprovalStatus == 2)
            //                    .Select(r => new
            //                    {
            //                        ID = r.ID,
            //                        RefNumber = (r.RefNumber.Contains("PR")) ? "PO" + r.RefNumber.Substring(2) : r.RefNumber
            //                    });

            //ViewBag.RequisitionID = new SelectList(_requisitions, "ID", "RefNumber");
            ViewBag.ReceivingTypeID = new SelectList(entity.ReceivingTypes, "ID", "Type");
            ViewBag.LocationID = new SelectList(loc, "ID", "Description");
            var empList = new SelectList((from s in entity.Employees
                                          select new
                                          {
                                              ID = s.ID,
                                              FullName = s.FirstName + " " + s.LastName
                                          }), "ID", "FullName");

            ViewBag.EncodedBy = empList;
            ViewBag.CheckedBy = empList;
            ViewBag.ReceivedBy = empList;
            ViewBag.ApprovalStatus = new SelectList(entity.ApprovalStatus, "ID", "Status");
            ViewBag.ApprovedBy = empList;
            #endregion

            return View(receiving);
        }

        // Post: Receiving/Create/
        [AccessChecker(Action = 2, ModuleID = 5)]
        [HttpPost]
        public ActionResult Create(Receiving receiving)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var rec = entity.Receivings.FirstOrDefault(r => r.ReceivingID == receiving.ReceivingID);

                    if (rec == null)
                    {
                        var newR = SetNull(receiving);

                        if (newR != null)
                        {
                            newR.ApprovalStatus = 1;
                            newR.ApprovedBy = Convert.ToInt32(Session["sessionuid"]);
                            newR.IsSync = false;

                            if (receiving.Img != null)
                            {
                                string imagePath = ConfigurationManager.AppSettings["imagePath"];
                                string path = Path.Combine(Server.MapPath("~" + imagePath), "ID" + receiving.ReceivingID + ".jpg");
                                newR.Img.SaveAs(path);

                                string baseUrl = string.Format("{0}://{1}", Request.Url.Scheme, Request.Url.Authority);
                                newR.Image = baseUrl + imagePath + "/ID" + receiving.ReceivingID + ".jpg";
                            }

                            entity.Receivings.Add(newR);
                            entity.SaveChanges();

                            //return RedirectToAction("Index");

                            return RedirectToAction("Details", new { id = receiving.ID });
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "The Receiving ID already exists.");
                    }
                }
                catch(Exception e)
                {
                    ModelState.AddModelError("", "There's an error.");
                }
            }
            else
            {
                string messages = string.Join("; ", ModelState.Values
                                                        .SelectMany(x => x.Errors)
                                                        .Select(x => x.ErrorMessage));
            }

            #region DROPDOWNS
            var loc = entity.Locations.Where(x => x.ID != 10)
                    .Select(x => new
                    {
                        ID = x.ID,
                        Description = x.Description
                    });

            //ViewBag.RequisitionID = new SelectList(entity.Requisitions.ToList().FindAll(r => r.ApprovalStatus == 2), "ID", "RefNumber");
            ViewBag.ReceivingTypeID = new SelectList(entity.ReceivingTypes, "ID", "Type", receiving.ReceivingTypeID);
            ViewBag.LocationID = new SelectList(loc, "ID", "Description", receiving.LocationID);
            var empList = from s in entity.Employees
                          select new
                          {
                              ID = s.ID,
                              FullName = s.FirstName + " " + s.LastName
                          };
            //new SelectList((), "ID", "FullName");
            ViewBag.EncodedBy = new SelectList(empList, "ID", "FullName", receiving.EncodedBy);
            ViewBag.CheckedBy = new SelectList(empList, "ID", "FullName", receiving.CheckedBy);
            ViewBag.ReceivedBy = new SelectList(empList, "ID", "FullName", receiving.ReceivedBy);
            ViewBag.ApprovalStatus = new SelectList(entity.ApprovalStatus, "ID", "Status", receiving.ApprovalStatus);
            ViewBag.ApprovedBy = new SelectList(empList, "ID", "FullName", receiving.ApprovedBy);
            #endregion

            return View(receiving);
        }

        // GET: Receiving/Edit/
        [AccessChecker(Action = 2, ModuleID = 5)]
        public ActionResult Edit(int id = 0)
        {
            //var receiving = entity.Receivings.Find(id);
            var receiving = entity.Receivings.FirstOrDefault(r => r.ID == id);

            if (receiving == null)
            {
                return HttpNotFound();
            }

            if (receiving.ApprovalStatus == 1)
            {
                #region DROPDOWNS
                var loc = entity.Locations.Where(x => x.ID != 10)
                        .Select(x => new
                        {
                            ID = x.ID,
                            Description = x.Description
                        });

                ViewBag.RequisitionID = new SelectList(entity.Requisitions.ToList().FindAll(r => r.ApprovalStatus == 2), "ID", "RefNumber", receiving.RequisitionID);
                ViewBag.ReceivingTypeID = new SelectList(entity.ReceivingTypes, "ID", "Type", receiving.ReceivingTypeID);
                ViewBag.LocationID = new SelectList(loc, "ID", "Description", receiving.LocationID);
                //ViewBag.StockTransferID = new SelectList(entity.StockTransfers, "ID", "TransferID", receiving.StockTransferID);
                var empList = from s in entity.Employees
                              select new
                              {
                                  ID = s.ID,
                                  FullName = s.FirstName + " " + s.LastName
                              };
                //new SelectList((), "ID", "FullName");
                ViewBag.EncodedBy = new SelectList(empList, "ID", "FullName", receiving.EncodedBy);
                ViewBag.CheckedBy = new SelectList(empList, "ID", "FullName", receiving.CheckedBy);
                ViewBag.ReceivedBy = new SelectList(empList, "ID", "FullName", receiving.ReceivedBy);
                ViewBag.ApprovalStatus = new SelectList(entity.ApprovalStatus, "ID", "Status", receiving.ApprovalStatus);
                ViewBag.ApprovedBy = new SelectList(empList, "ID", "FullName", receiving.ApprovedBy);
                #endregion

                return View(receiving);
            }

            return RedirectToAction("Details", new { id = id });
        }

        // POST: Receiving/Edit/5
        [AccessChecker(Action = 2, ModuleID = 5)]
        [HttpPost]
        public ActionResult Edit(Receiving receiving)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //var r = entity.Requisitions.FirstOrDefault(r1 => r1.ID == pr.ID && (r1.RequestedBy == UserID || AcctType == 1 || AcctType == 4)).ApprovalStatus;
                    var r = entity.Receivings.FirstOrDefault(r1 => r1.ID == receiving.ID);
                    receiving.ApprovalStatus = 1;
                    if (r.ApprovalStatus == 1)
                    {
                        if (receiving.Img != null)
                        {
                            string imagePath = ConfigurationManager.AppSettings["imagePath"];
                            string path = Path.Combine(Server.MapPath("~" + imagePath), "ID" + receiving.ReceivingID + ".jpg");
                            receiving.Img.SaveAs(path);

                            string baseUrl = string.Format("{0}://{1}", Request.Url.Scheme, Request.Url.Authority);
                            receiving.Image = baseUrl + imagePath + "/ID" + receiving.ReceivingID + ".jpg";
                        }

                        var newReceiving = SetNull(receiving);
                        entity.Entry(r).CurrentValues.SetValues(newReceiving);
                        entity.SaveChanges();

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return RedirectToAction("Details", new { id = receiving.ID });
                    }
                }
                catch(Exception ex)
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

            ViewBag.RequisitionID = new SelectList(entity.Requisitions.ToList().FindAll(r => r.ApprovalStatus == 2), "ID", "RefNumber");
            ViewBag.ReceivingTypeID = new SelectList(entity.ReceivingTypes, "ID", "Type", receiving.ReceivingTypeID);
            ViewBag.LocationID = new SelectList(loc, "ID", "Description", receiving.LocationID);
            //ViewBag.StockTransferID = new SelectList(entity.StockTransfers, "ID", "TransferID", receiving.StockTransferID);
            var empList = from s in entity.Employees
                          select new
                          {
                              ID = s.ID,
                              FullName = s.FirstName + " " + s.LastName
                          };
            //new SelectList((), "ID", "FullName");
            ViewBag.EncodedBy = new SelectList(empList, "ID", "FullName", receiving.EncodedBy);
            ViewBag.CheckedBy = new SelectList(empList, "ID", "FullName", receiving.CheckedBy);
            ViewBag.ReceivedBy = new SelectList(empList, "ID", "FullName", receiving.ReceivedBy);
            ViewBag.ApprovalStatus = new SelectList(entity.ApprovalStatus, "ID", "Status", receiving.ApprovalStatus);
            ViewBag.ApprovedBy = new SelectList(empList, "ID", "FullName", receiving.ApprovedBy);
            #endregion

            return View(receiving);
        }

        // GET: Receiving/Delete/5
        [AccessChecker(Action = 3, ModuleID = 5)]
        public ActionResult Delete(int id)
        {
            //var pr = entity.Requisitions.FirstOrDefault(r => r.ID == id && (r.RequestedBy == UserID || AcctType == 1 || AcctType == 4));
            var receiving = entity.Receivings.FirstOrDefault(r => r.ID == id);
            if (receiving == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (receiving.ApprovalStatus == 1)
                {
                    return View(receiving);
                }
            }

            return RedirectToAction("Details", new { id = receiving.ID });
        }

        // POST: Receiving/Delete/5
        [AccessChecker(Action = 3, ModuleID = 5)]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {
            var receiving = entity.Receivings.Find(id);

            try
            {
                entity.Receivings.Remove(receiving);
                entity.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
            }

            return View(receiving);
        }

        // POST: Receiving/Approve/5
        [AccessChecker(Action = 5, ModuleID = 5)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Approve(int id)
        {
            try
            {
                // TODO: Add delete logic here
                //var pr = entity.Requisitions.FirstOrDefault(r => r.ID == id && (r.RequestedBy == UserID || AcctType == 1 || AcctType == 4));
                var receving = entity.Receivings.Find(id);
                if (receving.ReceivingDetails.Count() > 0)
                {
                    receving.ApprovalStatus = 2;
                    receving.IsSync = false;

                    entity.Entry(receving).State = EntityState.Modified;
                    entity.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            catch
            {
            }
            return View();
        }

        // POST: Receiving/Denied/5
        [AccessChecker(Action = 5, ModuleID = 5)]
        [HttpPost]
        public ActionResult Denied(int id)
        {
            try
            {
                // TODO: Add delete logic here
                var receiving = entity.Receivings.Find(id);
                receiving.ApprovalStatus = 3;
                receiving.IsSync = false;

                entity.Entry(receiving).State = EntityState.Modified;
                entity.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Receiving/Items/5
        [AccessChecker(Action = 1, ModuleID = 5)]
        public ActionResult Items(int id, int? page)
        {
            var items = entity.ReceivingDetails
                        .ToList()
                        .FindAll(rd => rd.ReceivingID == id);

            ViewBag.Rid = id;

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        // GET: Receiving/ApprovedItems/5
        [AccessChecker(Action = 1, ModuleID = 5)]
        public ActionResult ApprovedItems(int id, int? page)
        {
            var items = entity.ReceivingDetails
                        .ToList()
                        .FindAll(rd => rd.ReceivingID == id && rd.AprovalStatusID == 2);

            ViewBag.Rid = id;

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        // GET: Receiving/GetRequisitionDetail/5
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

        // GET: Receiving/PendingItems/5
        [AccessChecker(Action = 1, ModuleID = 5)]
        public ActionResult PendingItems(int id, int? page)
        {
            int UserID = Convert.ToInt32(Session["sessionuid"]);
            int UserType = Convert.ToInt32(Session["usertype"]);

            var ReceivedBy = entity.Receivings.FirstOrDefault(r => r.ID == id).ReceivedBy;
            if (ReceivedBy != UserID && UserType != 1 && UserType != 4)
            {
                return RedirectToAction("Index", "Home");
            }

            var items = entity.ReceivingDetails
                        .ToList()
                        .FindAll(rd => rd.ReceivingID == id && rd.AprovalStatusID == 1 && (rd.Receiving.ReceivedBy == UserID || UserType == 1 || UserType == 4));
            //var items = entity.RequisitionDetails
            //            .ToList()
            //            .FindAll(rd => rd.RequisitionID == id && rd.AprovalStatusID == 1 && rd.Requisition.RequestedBy == UserID);

            ViewBag.Rid = id;
            ViewBag.ReceivedBy =
            ViewBag.UserID = UserID;
            ViewBag.AcctType = UserType;

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);
            return PartialView(items.ToPagedList(pageNumber, pageSize));
        }

        // GET: Receiving/DeniedItems/5
        [AccessChecker(Action = 1, ModuleID = 5)]
        public ActionResult DeniedItems(int id, int? page)
        {
            var items = entity.ReceivingDetails
                        .ToList()
                        .FindAll(rd => rd.ReceivingID == id && rd.AprovalStatusID == 3);

            ViewBag.Rid = id;

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        // POST: Receiving/ApproveItem/5
        [AccessChecker(Action = 1, ModuleID = 5)]
        [HttpPost]
        public ActionResult ApproveItem(int id, int itemID)
        {
            try
            {
                var item = entity.ReceivingDetails.Find(itemID);
                if (item != null)
                {

                    //int com = getCommited(itemID);
                    //item.Committed = com + item.Quantity;

                    //int po = getPurchaseOrder(itemID);
                    //item.Ordered = po + item.Ordered;

                    ////Available = In Stock + Ordered – Committed
                    //int avail = (Convert.ToInt32(item.InStock) + Convert.ToInt32(item.Ordered)) - Convert.ToInt32(item.Committed);
                    //item.Available = avail;

                    int requisitionId = Convert.ToInt32(Session["requisitionId"]);
                    var reqDetail = entity.RequisitionDetails.Find(item.RequisitionDetailID);

                    item.InStock = reqDetailRepo.getInstocked(requisitionId, reqDetail.Item.Code) + item.Quantity;

                    item.AprovalStatusID = 2;
                    item.IsSync = false;

                    //entity.Entry(item).State = EntityState.Modified;
                    entity.SaveChanges();
                }
            }
            catch
            {

            }
            return RedirectToAction("PendingItems", "Receiving", new { id = id });
        }

        // POST: Receiving/DenyItem/5
        [AccessChecker(Action = 1, ModuleID = 5)]
        [HttpPost]
        public ActionResult DenyItem(int id, int itemID)
        {
            try
            {
                var item = entity.ReceivingDetails.Find(itemID);
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
            return RedirectToAction("PendingItems", "Receiving", new { id = id });
        }

        #endregion

        #region PARTIAL
        // GET: Receiving/AddItemPartial/5
        [AccessChecker(Action = 1, ModuleID = 5)]
        public ActionResult AddItemPartial(int id)
        {
            //int transferID = entity.Receivings.Find(id).StockTransferID;
            //var items = entity.StockTransferDetails
            //            .ToList()
            //            .FindAll(rd => rd.StockTransferID == transferID && rd.AprovalStatusID == 2)
            //            .Select(ed => new
            //            {
            //                ID = ed.ID,
            //                Description = ed.RequisitionDetail.Item.Description
            //            });

            //ViewBag.Rid = id;
            //ViewBag.StockTransferDetailID = new SelectList(items, "ID", "Description");


            int reqID = entity.Receivings.Find(id).RequisitionID;
            var items = entity.RequisitionDetails
                        .ToList()
                        .FindAll(rd => rd.RequisitionID == reqID && rd.AprovalStatusID == 2)
                        .Select(ed => new
                        {
                            ID = ed.ID,
                            Description = ed.Item.Description
                        });

            ViewBag.STid = id;
            ViewBag.RequisitionDetailID = new SelectList(items, "ID", "Description");

            return PartialView();
        }

        // POST: Receiving/AddItemPartial/5
        [AccessChecker(Action = 1, ModuleID = 5)]
        [HttpPost]
        public ActionResult AddItemPartial(int id, ReceivingDetail rd)
        {
            try
            {
                // TODO: Add insert logic here
                rd.ReceivingID = id;
                rd.AprovalStatusID = 1; //submitted

                //var rd1 = entity.ReceivingDetails.Where(r => r.ReceivingID == rd.ReceivingID && r.StockTransferDetailID == rd.StockTransferDetailID).ToList();

                var rd1 = entity.ReceivingDetails.Where(s => s.ReceivingID == rd.ReceivingID && s.RequisitionDetailID == rd.RequisitionDetailID).ToList();

                RequisitionDetailsRepository reqRepo = new RequisitionDetailsRepository();

                //int com = reqRepo.getCommited(rd.RequisitionDetail.RequisitionID, rd.RequisitionDetail.ItemID); //getCommited(Convert.ToInt32(rd.RequisitionDetailID));

                var requisitionDetail = entity.RequisitionDetails.Find(rd.RequisitionDetailID);
                rd.Committed = reqRepo.getCommited(requisitionDetail.RequisitionID, requisitionDetail.ItemID);

                //int por = getPurchaseOrder(Convert.ToInt32(rd.RequisitionDetailID));
                rd.Ordered = reqRepo.getPurchaseOrder(requisitionDetail.ItemID);

                rd.Available = (rd.InStock + rd.Ordered) - rd.Committed;

                if (rd1.Count() > 0)
                {
                    TempData["PartialError"] = "Item is already in the list.";
                }
                else
                {
                    rd.IsSync = false;

                    entity.ReceivingDetails.Add(rd);
                    entity.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                //throw;
                TempData["PartialError"] = "There's an error.";
            }

            //ViewBag.PRid = id;
            //ViewBag.ItemID = new SelectList(entity.Items, "ID", "Description", rd.ItemID);
            //ViewBag.AprovalStatusID = new SelectList(entity.ApprovalStatus, "ID", "Status", rd.AprovalStatusID);

            //return RedirectToAction("PendingItems", new { id = id });
            return RedirectToAction("Details", new { id = id });
        }

        // GET: Receiving/EditItemPartial/5
        [AccessChecker(Action = 1, ModuleID = 5)]
        public ActionResult EditItemPartial(int id)
        {
            var rd = entity.ReceivingDetails.Find(id);

            ViewBag.AprovalStatusID = new SelectList(entity.ApprovalStatus, "ID", "Status", rd.AprovalStatusID);

            return PartialView(rd);
        }

        // POST: Receiving/EditItemPartial/5
        [AccessChecker(Action = 1, ModuleID = 5)]
        [HttpPost]
        public ActionResult EditItemPartial(int id, ReceivingDetail rd)
        {
            try
            {
                rd.IsSync = false;
                var prvreceivingDetail = entity.ReceivingDetails.Find(rd.ID);
                if(prvreceivingDetail.RequisitionDetailID != rd.RequisitionDetailID || prvreceivingDetail.Quantity != rd.Quantity)
                {
                    rd.PreviousItemID = prvreceivingDetail.RequisitionDetailID;
                    rd.PreviousQuantity = prvreceivingDetail.Quantity;
                }

                prvreceivingDetail.RequisitionDetailID = rd.RequisitionDetailID;
                prvreceivingDetail.Quantity = rd.Quantity;
                prvreceivingDetail.InStock = rd.InStock;
                prvreceivingDetail.Remarks = rd.Remarks;
                prvreceivingDetail.PreviousItemID = rd.PreviousItemID;
                prvreceivingDetail.PreviousQuantity = rd.PreviousQuantity;
                prvreceivingDetail.IsSync = false;

                entity.Entry(prvreceivingDetail).CurrentValues.SetValues(rd);
                //entity.Entry(prvreceivingDetail).State = EntityState.Modified;
                entity.SaveChanges();
            }
            catch
            {
                TempData["PartialError"] = "There's an error.";
            }

            if (rd.AprovalStatusID == 1)
            {
                //return RedirectToAction("PendingItems", new { id = rd.ReceivingID });
                return RedirectToAction("Details", new { id = rd.ReceivingID });
            }
            return RedirectToAction("ApprovedItems", new { id = rd.ReceivingID });
        }

        // GET: Receiving/DeleteItemPartial/5
        [AccessChecker(Action = 1, ModuleID = 5)]
        public ActionResult DeleteItemPartial(int id)
        {
            var rd = entity.ReceivingDetails.Find(id);

            return PartialView(rd);
        }

        // POST: Receiving/DeleteItemPartial/5
        [AccessChecker(Action = 1, ModuleID = 5)]
        [HttpPost, ActionName("DeleteItemPartial")]
        public ActionResult DeleteItemPartialConfirm(int id)
        {
            var rd = entity.ReceivingDetails.Find(id);

            int? reqID = rd.ReceivingID;
            try
            {
                entity.ReceivingDetails.Remove(rd);
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

    }
}