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
        StockAdjustmentRepository stockadRepo = new StockAdjustmentRepository();

        // GET: StockAdjustment
        [AccessCheckerForDisablingButtons(ModuleID = 8)]
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
                rrs = rrs.Where(o => o.No.Contains(searchString)
                                  || o.TransactionType.Type.Contains(searchString));
            }

            switch (sortOrder)
            { 
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
            adjust.ErrorDate = DateTime.Now;

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

            ViewBag.LocationID = new SelectList(loc, "ID", "Description","");
            ViewBag.PreparedBy = new SelectList(employees, "ID", "FullName","");
            ViewBag.AdjustedBy = new SelectList(employees, "ID", "FullName","");
            ViewBag.TransactionTypeID = new SelectList(entity.TransactionTypes, "ID", "Type","");
            ViewBag.ApprovedBy = new SelectList(employees, "ID", "FullName","");
            ViewBag.PostedBy = new SelectList(employees, "ID", "FullName","");
            ViewBag.Date = DateTime.Now.ToString("MMM/dd/yyyy");
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
                string _type = "";
                if (adjust.TransactionTypeID == 1)
                    _type = "s";
                else if (adjust.TransactionTypeID == 2)
                    _type = "r";
                else if (adjust.TransactionTypeID == 3)
                    _type = "v";

                try
                {   adjust.No = stockadRepo.GeneratePoNumber(_type);
                    adjust.ApprovalStatus = 1;
                    adjust.IsSync = false;
                   // adjust.ErrorDate = DateTime.Now;

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


            var loc = entity.Locations.Where(x => x.ID != 10)
                        .Select(x => new
                        {
                            ID = x.ID,
                            Description = x.Description
                        });

            ViewBag.LocationID = new SelectList(loc, "ID", "Description","");
            ViewBag.PreparedBy = new SelectList(employees, "ID", "FullName","");
            ViewBag.AdjustedBy = new SelectList(employees, "ID", "FullName","");
            ViewBag.TransactionTypeID = new SelectList(entity.TransactionTypes, "ID", "Type","");
            ViewBag.ApprovedBy = new SelectList(employees, "ID", "FullName","");
            ViewBag.PostedBy = new SelectList(employees, "ID", "FullName","");
            ViewBag.Date = DateTime.Now.ToString("MMM/dd/yyyy");
            ViewBag.PostedDate = DateTime.Now.ToString("MMM/dd/yyyy");
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

                var loc = entity.Locations.Where(x => x.ID != 10)
                       .Select(x => new
                       {
                           ID = x.ID,
                           Description = x.Description
                       });

                ViewBag.LocationID = new SelectList(loc, "ID", "Description", adjust.LocationID);
                ViewBag.PreparedBy = new SelectList(employees, "ID", "FullName", adjust.PreparedBy);
                ViewBag.AdjustedBy = new SelectList(employees, "ID", "FullName", adjust.AdjustedBy);
                ViewBag.TransactionTypeID = new SelectList(entity.TransactionTypes, "ID", "Type",adjust.TransactionTypeID);
                ViewBag.ApprovedBy = new SelectList(employees, "ID", "FullName", adjust.ApprovedBy);
                ViewBag.PostedBy = new SelectList(employees, "ID", "FullName", adjust.PostedBy);
                ViewBag.Date = DateTime.Now.ToString("MMM/dd/yyyy");
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


            var loc = entity.Locations.Where(x => x.ID != 10)
                        .Select(x => new
                        {
                            ID = x.ID,
                            Description = x.Description
                        });

            ViewBag.LocationID = new SelectList(loc, "ID", "Description", adjust.LocationID);
            ViewBag.PreparedBy = new SelectList(employees, "ID", "FullName", adjust.PreparedBy);
            ViewBag.AdjustedBy = new SelectList(employees, "ID", "FullName", adjust.AdjustedBy);
            ViewBag.TransactionTypeID = new SelectList(entity.TransactionTypes, "ID", "Type", adjust.TransactionTypeID);
            ViewBag.ApprovedBy = new SelectList(employees, "ID", "FullName", adjust.ApprovedBy);
            ViewBag.PostedBy = new SelectList(employees, "ID", "FullName", adjust.PostedBy);
            ViewBag.Date = DateTime.Now.ToString("MMM/dd/yyyy");
            ViewBag.PostedDate = DateTime.Now.ToString("MMM/dd/yyyy");
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

                if (adjust.ApprovalStatus == 1)
                {
                    if (adjust.StockAdjustmentDetails.Count > 0)
                    {
                        adjust.ApprovalStatus = 2;
                        adjust.IsSync = false;

                        entity.Entry(adjust).State = EntityState.Modified;
                        entity.SaveChanges();


                        var inv = entity.StockAdjustmentDetails.Where(p => p.StockAdjustmentID == id).ToList();
                        if (inv != null)
                        {
                            foreach (var _inv in inv)
                            {
                                var i = entity.Inventories.Find(_inv.ItemID);
                                i.InStock = _inv.NewQuantity;
                                entity.Entry(i).State = EntityState.Modified;
                                entity.SaveChanges();

                                StockLedger _stockledger = new StockLedger();
                                _stockledger.InventoryID = i.ID;
                                _stockledger.Type = "Variance";
                                _stockledger.Variance = _inv.Variance;
                                _stockledger.ReferenceNo = adjust.No;
                                _stockledger.BeginningBalance = _inv.OldQuantity;
                                _stockledger.RemainingBalance = _inv.NewQuantity;
                                _stockledger.Date = DateTime.Now;

                                entity.StockLedgers.Add(_stockledger);
                                entity.SaveChanges();
                            }

                        }

                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    TempData["Error"] = "No Items to Approve";
                }
            }
            catch
            {
                TempData["Error"] = "No Items to Approve";
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
                            .Where(p=>p.StockAdjustmentID == id)
                            .ToList();
           

            ViewBag.STTAid = id;

            
            try
            {
                Session["locationID"] = entity.StockAdjustments.Find(id).LocationID;
            }
            catch { }
            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        #region PARTIAL

        public class StDetails
        {
            public int ID { get; set; }
            public string Description { get; set; }
        }

        // GET: StockAdjustment/AddItemPartial/5
        [AccessChecker(Action = 1, ModuleID = 8)]
        public ActionResult AddItemPartial(int id)
        {
            var ret = entity.StockAdjustments.Find(id);
        
            ViewBag.STTAid = id;


            var items = entity.Inventories.Where(x => x.LocationCode == ret.LocationID)
                        .Select(x => new
                        {
                            ID = x.ID,
                            Description = x.Description
                        });

            ViewBag.ItemID = new SelectList(items, "ID", "Description");
            ViewBag.TransType = ret.TransactionTypeID;

            return PartialView();
        }

        // POST: StockAdjustment/AddItemPartial/5
        [AccessChecker(Action = 1, ModuleID = 8)]
        [HttpPost]
        public ActionResult AddItemPartial(int id, StockAdjustmentDetail rd)
        {
            //try
            //{
           
                var found = false;

                var adj = entity.StockAdjustments.Find(id);

                rd.StockAdjustmentID = id;

                if (!found)
                {
                    rd.ID = 0;
                    rd.OldQuantity = stockadRepo.GetInventoryQuantity(rd.ItemID);
                    //rd.Variance = rd.Variance;//rd.NewQuantity - rd.OldQuantity;
                    rd.NewQuantity = rd.OldQuantity + rd.Variance;
                    rd.IsSync = false;
                    rd.StockAdjustmentID = id;
                 

                    entity.StockAdjustmentDetails.Add(rd);
                    entity.SaveChanges();
                }
         //   }
            //catch(Exception e)
            //{
            //    e.ToString();
            //    TempData["PartialError"] = "There's an error.";
            //}

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

            return PartialView(sa);
        }

        // POST: StockAdjustment/EditItemPartial/5
        [AccessChecker(Action = 1, ModuleID = 8)]
        [HttpPost]
        public ActionResult EditItemPartial(int id, StockAdjustmentDetail rd)
        {
            try
            {
                rd.OldQuantity = stockadRepo.GetInventoryQuantity(rd.ItemID);
                rd.NewQuantity = rd.OldQuantity + rd.Variance;
                // rd.Variance = rd.NewQuantity - rd.OldQuantity;
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

        public JsonResult GetNumber(int? TransType)
        {
            string _type = "";
            if (TransType == 1)
                _type = "s";
            else if (TransType == 2)
                _type = "r";
            else if (TransType == 3)
                _type = "v";

            string number = stockadRepo.GeneratePoNumber(_type);

            return Json(number);
        }

        public JsonResult GetOldQuantity(int ItemID)
        {

            int number = stockadRepo.GetInventoryQuantity(ItemID);

            return Json(number);
        }

        [HttpPost]
        public JsonResult GetCategories(string name)
        {
            var categories = entity.Categories.Where(x => x.Description.Contains(name))
                            .Select(x => new
                            {
                                ID = x.Description,
                                Name = x.Description
                            });
            return Json(categories, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetItems(string catID, string name)
        {
            int loc = Convert.ToInt32(Session["locationID"]);
            var items = entity.Inventories.Where(x => x.LocationCode == loc 
                                                      && (x.Category == catID || x.Description.Contains(name)))
                        .Select(x => new
                        {
                            ID = x.ID,
                            Code = x.ItemCode,
                            Name = x.Description,
                            Category = x.Category
                        });

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetItemCode(string catID, string name)
        {
            int loc = Convert.ToInt32(Session["locationID"]);
            var items = entity.Inventories.Where(x => x.LocationCode == loc
                                                      && (x.Category == catID || x.ItemCode.Contains(name)))
                        .Select(x => new
                        {
                            ID = x.ID,
                            Code = x.ItemCode,
                            Name = x.Description,
                            Category = x.Category
                        });

            return Json(items, JsonRequestBehavior.AllowGet);
        }


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
                    var sa = entity.StockAdjustments.Find(id);
                    sa.ApprovalStatus = 3;
                    sa.Comments = Reason;
                    entity.Entry(sa).State = EntityState.Modified;

                    entity.SaveChanges();

                    //var stdetails = entity.StockTransferDetails.Where(i => i.StockTransferID == id).ToList();
                    //if (stdetails != null)
                    //{
                    //    foreach (var _st in stdetails)
                    //    {
                    //        var _stdetails = entity.StockTransferDetails.Find(_st.ID);
                    //        _stdetails.AprovalStatusID = 3;

                    //        entity.Entry(stdetails).State = EntityState.Modified;
                    //        entity.SaveChanges();
                    //    }

                    //}

                }
                else
                {
                    TempData["Error"] = "Reason is required";
                }
            }
            catch { TempData["Error"] = "There's an error"; }

            return RedirectToAction("Index");
        }


                    

        #endregion
    }
}