using MoostBrand.DAL;
using MoostBrand.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoostBrand.Controllers
{
    [LoginChecker]
    public class StockAdjustmentController : Controller
    {
        MoostBrandEntities entity = new MoostBrandEntities();

        // GET: StockAdjustment
        [AccessChecker(Action = 1, ModuleID = 8)]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "type" : "";
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "trans" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var rrs = from o in entity.StockAdjustments
                      select o;

            if (!String.IsNullOrEmpty(searchString))
            {
                rrs = rrs.Where(o => o.ReturnType.Type.Contains(searchString)
                                  || o.TransactionType.Type.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "type":
                    rrs = rrs.OrderByDescending(o => o.ReturnType.Type);
                    break;
                case "trans":
                    rrs = rrs.OrderByDescending(o => o.TransactionType.Type);
                    break;
                default:
                    rrs = rrs.OrderBy(o => o.ID);
                    break;
            }

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);
            return View(rrs.ToPagedList(pageNumber, pageSize));
        }

        // GET: StockAdjustment/Details/5
        [AccessChecker(Action = 1, ModuleID = 8)]
        public ActionResult Details(int id = 0)
        {
            //var pr = entity.Requisitions.FirstOrDefault(r => r.ID == id && (r.RequestedBy == UserID || AcctType == 1 || AcctType == 4));
            var r = entity.StockAdjustments.Find(id);
            if (r == null)
            {
                return HttpNotFound();
            }

            return View(r);
        }

        // GET: StockAdjustment/Create/
        [AccessChecker(Action = 2, ModuleID = 8)]
        public ActionResult Create()
        {
            var adjust = new StockAdjustment();
            adjust.Date = DateTime.Now;

            #region DROPDOWNS
            var employees = from s in entity.Employees
                            select new
                            {
                                ID = s.ID,
                                FullName = s.FirstName + " " + s.LastName
                            };
            ViewBag.PreparedBy = new SelectList(employees, "ID", "FullName");
            ViewBag.AdjustedBy = new SelectList(employees, "ID", "FullName");
            ViewBag.TransactionTypeID = new SelectList(entity.TransactionTypes, "ID", "Type");
            ViewBag.ReturnTypeID = new SelectList(entity.ReturnTypes, "ID", "Type");
            ViewBag.ApprovedBy = new SelectList(employees, "ID", "FullName");
            ViewBag.PostedBy = new SelectList(employees, "ID", "FullName");
            #endregion

            return View(adjust);
        }

        // POST: StockAdjustment/Create/
        [AccessChecker(Action = 2, ModuleID = 8)]
        [HttpPost]
        public ActionResult Create(StockAdjustment adjust)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    adjust.ApprovalStatus = 1;
                    adjust.IsSync = false;

                    entity.StockAdjustments.Add(adjust);
                    entity.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "There's an error");
                }
            }
            #region DROPDOWNS
            var employees = from s in entity.Employees
                            select new
                            {
                                ID = s.ID,
                                FullName = s.FirstName + " " + s.LastName
                            };
            ViewBag.PreparedBy = new SelectList(employees, "ID", "FullName", adjust.PreparedBy);
            ViewBag.AdjustedBy = new SelectList(employees, "ID", "FullName", adjust.AdjustedBy);
            ViewBag.TransactionTypeID = new SelectList(entity.TransactionTypes, "ID", "Type", adjust.TransactionTypeID);
            ViewBag.ReturnTypeID = new SelectList(entity.ReturnTypes, "ID", "Type", adjust.ReturnTypeID);
            ViewBag.ApprovedBy = new SelectList(employees, "ID", "FullName", adjust.ApprovedBy);
            ViewBag.PostedBy = new SelectList(employees, "ID", "FullName", adjust.PostedBy); 
            #endregion

            return View(adjust);
        }

        // GET: StockAdjustment/Edit/5
        [AccessChecker(Action = 2, ModuleID = 8)]
        public ActionResult Edit(int id = 0)
        {
            var adjust = entity.StockAdjustments.Find(id);

            if (adjust == null)
            {
                return HttpNotFound();
            }

            if (adjust.ApprovalStatus == 1)
            {
                #region DROPDOWNS
                var employees = from s in entity.Employees
                                select new
                                {
                                    ID = s.ID,
                                    FullName = s.FirstName + " " + s.LastName
                                };
                ViewBag.PreparedBy = new SelectList(employees, "ID", "FullName", adjust.PreparedBy);
                ViewBag.AdjustedBy = new SelectList(employees, "ID", "FullName", adjust.AdjustedBy);
                ViewBag.TransactionTypeID = new SelectList(entity.TransactionTypes, "ID", "Type", adjust.TransactionTypeID);
                ViewBag.ReturnTypeID = new SelectList(entity.ReturnTypes, "ID", "Type", adjust.ReturnTypeID);
                ViewBag.ApprovedBy = new SelectList(employees, "ID", "FullName", adjust.ApprovedBy);
                ViewBag.PostedBy = new SelectList(employees, "ID", "FullName", adjust.PostedBy); 
                #endregion

                return View(adjust);
            }

            return RedirectToAction("Details", new { id = id });
        }

        // POST: StockAdjustment/Edit/5
        [AccessChecker(Action = 2, ModuleID = 8)]
        [HttpPost]
        public ActionResult Edit(StockAdjustment adjust)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    adjust.IsSync = false;

                    entity.Entry(adjust).State = EntityState.Modified;
                    entity.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "There's an error");
                }
            }
            #region DROPDOWNS
            var employees = from s in entity.Employees
                            select new
                            {
                                ID = s.ID,
                                FullName = s.FirstName + " " + s.LastName
                            };
            ViewBag.PreparedBy = new SelectList(employees, "ID", "FullName", adjust.PreparedBy);
            ViewBag.AdjustedBy = new SelectList(employees, "ID", "FullName", adjust.AdjustedBy);
            ViewBag.TransactionTypeID = new SelectList(entity.TransactionTypes, "ID", "Type", adjust.TransactionTypeID);
            ViewBag.ReturnTypeID = new SelectList(entity.ReturnTypes, "ID", "Type", adjust.ReturnTypeID);
            ViewBag.ApprovedBy = new SelectList(employees, "ID", "FullName", adjust.ApprovedBy);
            ViewBag.PostedBy = new SelectList(employees, "ID", "FullName", adjust.PostedBy);
            #endregion

            return View(adjust);
        }

        // GET: StockAdjustment/Delete/5
        [AccessChecker(Action = 3, ModuleID = 8)]
        public ActionResult Delete(int id)
        {
            //var pr = entity.Requisitions.FirstOrDefault(r => r.ID == id && (r.RequestedBy == UserID || AcctType == 1 || AcctType == 4));
            var adjust = entity.StockAdjustments.FirstOrDefault(r => r.ID == id);
            if (adjust == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (adjust.ApprovalStatus == 1)
                {
                    return View(adjust);
                }
            }

            return RedirectToAction("Details", new { id = adjust.ID });
        }

        // POST: StockAdjustment/Delete/5
        [AccessChecker(Action = 3, ModuleID = 8)]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {
            var adjust = entity.StockAdjustments.Find(id);

            try
            {
                entity.StockAdjustments.Remove(adjust);
                entity.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
            }

            return View(adjust);
        }

        // POST: StockAdjustment/Approve/5
        [AccessChecker(Action = 5, ModuleID = 8)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Approve(int id)
        {
            try
            {
                // TODO: Add delete logic here
                //var pr = entity.Requisitions.FirstOrDefault(r => r.ID == id && (r.RequestedBy == UserID || AcctType == 1 || AcctType == 4));
                var adjust = entity.StockAdjustments.Find(id);
                if(adjust.StockAdjustmentDetails.Count > 0)
                {
                    adjust.ApprovalStatus = 2;
                    adjust.IsSync = false;

                    entity.Entry(adjust).State = EntityState.Modified;
                    entity.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            catch
            {
            }

            return View();
        }

        // POST: StockAdjustment/Denied/5
        [AccessChecker(Action = 5, ModuleID = 8)]
        [HttpPost]
        public ActionResult Denied(int id)
        {
            try
            {
                // TODO: Add delete logic here
                var adjust = entity.StockAdjustments.Find(id);
                adjust.ApprovalStatus = 3;
                adjust.IsSync = false;

                entity.Entry(adjust).State = EntityState.Modified;
                entity.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //GET: StockAdjustment/Items/5
        [AccessChecker(Action = 1, ModuleID = 8)]
        public ActionResult Items(int id, int? page)
        {
            int UserID = Convert.ToInt32(Session["sessionuid"]);
            int UserType = Convert.ToInt32(Session["usertype"]);

            var items = entity.StockAdjustmentDetails
                        .ToList()
                        .FindAll(rd => rd.StockAdjustmentID == id);

            ViewBag.STTAid = id;

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        #region PARTIAL

        // GET: StockAdjustment/AddItemPartial/5
        [AccessChecker(Action = 1, ModuleID = 8)]
        public ActionResult AddItemPartial(int id)
        {
            var ret = entity.StockAdjustments.Find(id);

            if (ret.TransactionTypeID == 1)
            {
                var items = entity.StockTransferDetails
                      .ToList()
                      .FindAll(rd => rd.AprovalStatusID == 2 && rd.StockTransfer.Receiving.ReceivingTypeID == ret.ReturnTypeID)
                      .Select(ed => new
                      {
                          ID = ed.ID,
                          Description = ed.ReceivingDetail.RequisitionDetail.Item.Description
                      });
                ViewBag.StockTransferDetailID = new SelectList(items, "ID", "Description");               
                               
            }
            else
            {
                var items = entity.ReceivingDetails
                        .ToList()
                        .FindAll(rd => rd.AprovalStatusID == 2 && rd.Receiving.ReceivingTypeID == ret.ReturnTypeID)
                        .Select(ed => new
                        {
                            ID = ed.ID,
                            Description = ed.RequisitionDetail.Item.Description
                        });
                ViewBag.ReceivingDetailID = new SelectList(items, "ID", "Description");
            }

            ViewBag.STTAid = id;

            ViewBag.ReasonForAdjustmentID = new SelectList(entity.ReasonForAdjustments, "ID", "Reason");
            ViewBag.TransType = ret.TransactionTypeID;

            return PartialView();
        }

        // POST: StockAdjustment/AddItemPartial/5
        [AccessChecker(Action = 1, ModuleID = 8)]
        [HttpPost]
        public ActionResult AddItemPartial(int id, StockAdjustmentDetail rd)
        {
            try
            {
                var found = false;

                var adj = entity.StockAdjustments.Find(id);

                rd.StockAdjustmentID = id;

                if (adj.TransactionTypeID == 1)
                {
                    var rd1 = entity.ReturnedItems.Where(r => r.ReturnID == rd.StockAdjustmentID && r.StockTransferDetailID == rd.StockTransferDetailID).ToList();

                    if (rd1.Count() > 0)
                    {
                        TempData["PartialError"] = "Item is already in the list.";
                        found = true;
                    }
                }
                else
                {
                    var rd1 = entity.ReturnedItems.Where(r => r.ReturnID == rd.StockAdjustmentID && r.ReceivingDetailID == rd.ReceivingDetailID).ToList();

                    if (rd1.Count() > 0)
                    {
                        TempData["PartialError"] = "Item is already in the list.";
                        found = true;
                    }
                }

                if (!found)
                {
                    rd.IsSync = false;

                    entity.StockAdjustmentDetails.Add(rd);
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

            return RedirectToAction("Items", new { id = id });
        }

        // GET: StockAdjustment/EditItemPartial/5
        [AccessChecker(Action = 1, ModuleID = 8)]
        public ActionResult EditItemPartial(int id)
        {
            var sa = entity.StockAdjustmentDetails.Find(id);

            ViewBag.ReasonForAdjustmentID = new SelectList(entity.ReasonForAdjustments, "ID", "Reason", sa.ReasonForAdjustmentID);
            return PartialView(sa);
        }

        // POST: StockAdjustment/EditItemPartial/5
        [AccessChecker(Action = 1, ModuleID = 8)]
        [HttpPost]
        public ActionResult EditItemPartial(int id, StockAdjustmentDetail rd)
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

            return RedirectToAction("Items", new { id = rd.StockAdjustmentID });
        }

        // GET: StockAdjustment/DeleteItemPartial/5
        [AccessChecker(Action = 1, ModuleID = 8)]
        public ActionResult DeleteItemPartial(int id)
        {
            var sa = entity.StockAdjustmentDetails.Find(id);

            return PartialView(sa);
        }

        // POST: StockAdjustment/DeleteItemPartial/5
        [AccessChecker(Action = 1, ModuleID = 8)]
        [HttpPost, ActionName("DeleteItemPartial")]
        public ActionResult DeleteItemPartialConfirm(int id)
        {
            var sa = entity.StockAdjustmentDetails.Find(id);

            int? reqID = sa.StockAdjustmentID;
            try
            {
                entity.StockAdjustmentDetails.Remove(sa);
                entity.SaveChanges();
            }
            catch
            {
                TempData["PartialError"] = "There's an error.";
            }

            return RedirectToAction("Items", new { id = reqID });
        }

        #endregion
    }
}