using MoostBrand.DAL;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Configuration;
using PagedList;
using MoostBrand.Models;

namespace MoostBrand.Controllers
{
    [LoginChecker]
    public class ReturnController : Controller
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
            var r = entity.Returns.ToList().FindAll(p => p.ApprovalNumber == randomString);
            if (r.Count() > 0)
            {
                goto startR;
            }

            //return the random string
            return randomString;
        }

        #endregion

        // GET: Return/GetRefNumber/5
        public ActionResult GetRefNumber(int id)
        {
            if(id == 1)
            {
                var rd = entity.StockTransfers
                    .Where(r => r.ApprovedStatus == 2)
                    .Select(r => new
                    {
                        ID = r.ID,
                        RefNum = r.TransferID
                    })
                    .FirstOrDefault();

                return Json(rd, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var rd = entity.Receivings
                    .Where(r => r.ApprovalStatus == 2)
                    .Select(r => new
                    {
                        ID = r.ID,
                        RefNum = r.ReceivingID
                    })
                    .FirstOrDefault();

                return Json(rd, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Return
        [AccessChecker(Action = 1, ModuleID = 7)]
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

            var rrs = from o in entity.Returns
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

        // GET: Return/Details/5
        [AccessChecker(Action = 1, ModuleID = 7)]
        public ActionResult Details(int id = 0)
        {
            //var pr = entity.Requisitions.FirstOrDefault(r => r.ID == id && (r.RequestedBy == UserID || AcctType == 1 || AcctType == 4));
            var r = entity.Returns.Find(id);
            if (r == null)
            {
                return HttpNotFound();
            }

            return View(r);
        }

        // GET: Return/Create/
        [AccessChecker(Action = 2, ModuleID = 7)]
        public ActionResult Create()
        {
            var retrn = new Return();
            retrn.Date = DateTime.Now;

            #region DROPDOWNS
            ViewBag.TransactionTypeID = new SelectList(entity.TransactionTypes, "ID", "Type");
            ViewBag.ReturnTypeID = new SelectList(entity.ReturnTypes, "ID", "Type");
            #endregion

            return View(retrn);
        }

        // POST: Return/Create/
        [AccessChecker(Action = 2, ModuleID = 7)]
        [HttpPost]
        public ActionResult Create(Return retrn)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    retrn.ApprovalStatus = 1;
                    retrn.IsSync = false;

                    entity.Returns.Add(retrn);
                    entity.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "There's an error");
                }
            }
            #region DROPDOWNS
            ViewBag.TransactionTypeID = new SelectList(entity.TransactionTypes, "ID", "Type", retrn.TransactionTypeID);
            ViewBag.ReturnTypeID = new SelectList(entity.ReturnTypes, "ID", "Type", retrn.ReturnTypeID);
            #endregion

            return View(retrn);
        }

        // GET: Return/Edit/5
        [AccessChecker(Action = 2, ModuleID = 7)]
        public ActionResult Edit(int id = 0)
        {
            var retrn = entity.Returns.Find(id);

            if (retrn == null)
            {
                return HttpNotFound();
            }

            if(retrn.ApprovalStatus == 1)
            {
                #region DROPDOWNS
                ViewBag.TransactionTypeID = new SelectList(entity.TransactionTypes, "ID", "Type");
                ViewBag.ReturnTypeID = new SelectList(entity.ReturnTypes, "ID", "Type");
                #endregion

                return View(retrn);
            }

            return RedirectToAction("Details", new { id = id });
        }

        // POST: Return/Edit/5
        [AccessChecker(Action = 2, ModuleID = 7)]
        [HttpPost]
        public ActionResult Edit(Return retrn)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    retrn.IsSync = false;

                    entity.Entry(retrn).State = EntityState.Modified;
                    entity.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "There's an error");
                }
            }
            #region DROPDOWNS
            ViewBag.TransactionTypeID = new SelectList(entity.TransactionTypes, "ID", "Type", retrn.TransactionTypeID);
            ViewBag.ReturnTypeID = new SelectList(entity.ReturnTypes, "ID", "Type", retrn.ReturnTypeID);
            #endregion

            return View(retrn);
        }

        // GET: Return/Delete/5
        [AccessChecker(Action = 3, ModuleID = 7)]
        public ActionResult Delete(int id)
        {
            //var pr = entity.Requisitions.FirstOrDefault(r => r.ID == id && (r.RequestedBy == UserID || AcctType == 1 || AcctType == 4));
            var retrn = entity.Returns.FirstOrDefault(r => r.ID == id);
            if (retrn == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (retrn.ApprovalStatus == 1)
                {
                    return View(retrn);
                }
            }

            return RedirectToAction("Details", new { id = retrn.ID });
        }

        // POST: Return/Delete/5
        [AccessChecker(Action = 3, ModuleID = 7)]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {
            var retrn = entity.Returns.Find(id);

            try
            {
                entity.Returns.Remove(retrn);
                entity.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
            }

            return View(retrn);
        }

        // POST: Return/Approve/5
        [AccessChecker(Action = 5, ModuleID = 7)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Approve(int id)
        {
            try
            {
                // TODO: Add delete logic here
                //var pr = entity.Requisitions.FirstOrDefault(r => r.ID == id && (r.RequestedBy == UserID || AcctType == 1 || AcctType == 4));
                var retrn = entity.Returns.Find(id);
                if(retrn.ReturnedItems.Count > 0)
                {
                    retrn.ApprovalStatus = 2;
                    retrn.IsSync = false;

                    entity.Entry(retrn).State = EntityState.Modified;
                    entity.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            catch
            {
            }

            return View();
        }

        // POST: Return/Denied/5
        [AccessChecker(Action = 5, ModuleID = 7)]
        [HttpPost]
        public ActionResult Denied(int id)
        {
            try
            {
                // TODO: Add delete logic here
                var retrn = entity.Returns.Find(id);
                retrn.ApprovalStatus = 3;
                retrn.IsSync = false;

                entity.Entry(retrn).State = EntityState.Modified;
                entity.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //GET: Return/Items/5
        [AccessChecker(Action = 1, ModuleID = 7)]
        public ActionResult Items(int id, int? page)
        {
            int UserID = Convert.ToInt32(Session["sessionuid"]);
            int UserType = Convert.ToInt32(Session["usertype"]);

            var items = entity.ReturnedItems
                        .ToList()
                        .FindAll(rd => rd.ReturnID == id);

            ViewBag.RTid = id;

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        #region PARTIAL

        // GET: Return/AddItemPartial/5
        [AccessChecker(Action = 1, ModuleID = 7)]
        public ActionResult AddItemPartial(int id)
        {
            var ret = entity.Returns.Find(id);
            
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

            ViewBag.RTid = id;

            ViewBag.TransType = ret.TransactionTypeID;

            return PartialView();
        }

        // POST: Return/AddItemPartial/5
        [AccessChecker(Action = 1, ModuleID = 7)]
        [HttpPost]
        public ActionResult AddItemPartial(int id, ReturnedItem rd)
        {
            try
            {
                var found = false;

                var ret = entity.Returns.Find(id);

                rd.ReturnID = id;

                if (ret.TransactionTypeID == 1)
                {
                    var rd1 = entity.ReturnedItems.Where(r => r.ReturnID == rd.ReturnID && r.StockTransferDetailID == rd.StockTransferDetailID).ToList();

                    if (rd1.Count() > 0)
                    {
                        TempData["PartialError"] = "Item is already in the list.";
                        found = true;
                    }
                }
                else
                {
                    var rd1 = entity.ReturnedItems.Where(r => r.ReturnID == rd.ReturnID && r.ReceivingDetailID == rd.ReceivingDetailID).ToList();

                    if (rd1.Count() > 0)
                    {
                        TempData["PartialError"] = "Item is already in the list.";
                        found = true;
                    }
                }
                
                if (!found)
                {
                    rd.IsSync = false;

                    entity.ReturnedItems.Add(rd);
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

        // GET: Return/EditItemPartial/5
        [AccessChecker(Action = 1, ModuleID = 7)]
        public ActionResult EditItemPartial(int id)
        {
            var rd = entity.ReturnedItems.Find(id);
            
            return PartialView(rd);
        }

        // POST: Return/EditItemPartial/5
        [AccessChecker(Action = 1, ModuleID = 7)]
        [HttpPost]
        public ActionResult EditItemPartial(int id, ReturnedItem rd)
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

            return RedirectToAction("Items", new { id = rd.ReturnID });
        }

        // GET: Return/DeleteItemPartial/5
        [AccessChecker(Action = 1, ModuleID = 7)]
        public ActionResult DeleteItemPartial(int id)
        {
            var rd = entity.ReturnedItems.Find(id);

            return PartialView(rd);
        }

        // POST: Return/DeleteItemPartial/5
        [AccessChecker(Action = 1, ModuleID = 7)]
        [HttpPost, ActionName("DeleteItemPartial")]
        public ActionResult DeleteItemPartialConfirm(int id)
        {
            var rd = entity.ReturnedItems.Find(id);

            int? reqID = rd.ReturnID;
            try
            {
                entity.ReturnedItems.Remove(rd);
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