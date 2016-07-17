using MoostBrand.DAL;
using System;
using System.Linq;
using PagedList;
using System.Web.Mvc;
using System.Configuration;
using System.Data.Entity;
using MoostBrand.Models;

namespace MoostBrand.Controllers
{
    [LoginChecker]
    public class StockTransferController : Controller
    {
        MoostBrandEntities entity = new MoostBrandEntities();

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

        // GET: StockTransfer
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

            return View(sts.ToPagedList(pageNumber, pageSize));
        }

        // GET: StockTransfer/Details/5
        public ActionResult Details(int id = 0)
        {
            var stocktransfer = entity.StockTransfers.Find(id);

            if (stocktransfer == null)
            {
                return HttpNotFound();
            }

            return View(stocktransfer);
        }

        // GET: StockTransfer/Create/
        public ActionResult Create()
        {
            var stocktransfer = new StockTransfer();
            stocktransfer.TransferID = Generator("STR");
            stocktransfer.STDAte = DateTime.Now;

            #region DROPDOWNS
            ViewBag.RequisitionID = new SelectList(entity.Requisitions.ToList().FindAll(r => r.ApprovalStatus == 2), "ID", "RefNumber");
            ViewBag.LocationID = new SelectList(entity.Locations, "ID", "Description");
            var empList = new SelectList((from s in entity.Employees
                                          select new
                                          {
                                              ID = s.ID,
                                              FullName = s.FirstName + " " + s.LastName
                                          }), "ID", "FullName");
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

        [HttpPost]
        public ActionResult Create(StockTransfer stocktransfer)
        {
            try
            {
                var st = entity.StockTransfers.Where(s => s.TransferID == stocktransfer.TransferID).ToList();

                if (st.Count() > 0)
                {
                    ModelState.AddModelError("", "Stock Transfer ID already exists");
                }
                else
                {
                    stocktransfer.ApprovedStatus = 1;
                    stocktransfer.ApprovedBy = Convert.ToInt32(Session["sessionuid"]);
                    stocktransfer.IsSync = false;

                    entity.StockTransfers.Add(stocktransfer);
                    entity.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            catch
            {
                ModelState.AddModelError("", "There's an error.");
            }

            #region DROPDOWNS
            ViewBag.RequisitionID = new SelectList(entity.Requisitions.ToList().FindAll(r => r.ApprovalStatus == 2), "ID", "RefNumber");
            ViewBag.LocationID = new SelectList(entity.Locations, "ID", "Description", stocktransfer.LocationID);
            var empList = from s in entity.Employees
                          select new
                          {
                              ID = s.ID,
                              FullName = s.FirstName + " " + s.LastName
                          };

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

        // GET: StockTransfer/Edit/5
        public ActionResult Edit(int id = 0)
        {
            var stocktransfer = entity.StockTransfers.Find(id);
            
            if(stocktransfer == null)
            {
                return HttpNotFound();
            }

            if(stocktransfer.ApprovedStatus == 1)
            {
                #region DROPDOWNS
                ViewBag.LocationID = new SelectList(entity.Locations, "ID", "Description", stocktransfer.LocationID);
                var empList = from s in entity.Employees
                              select new
                              {
                                  ID = s.ID,
                                  FullName = s.FirstName + " " + s.LastName
                              };

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
        [HttpPost]
        public ActionResult Edit(StockTransfer stocktransfer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    stocktransfer.IsSync = false;

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
            ViewBag.LocationID = new SelectList(entity.Locations, "ID", "Description", stocktransfer.LocationID);
            var empList = from s in entity.Employees
                          select new
                          {
                              ID = s.ID,
                              FullName = s.FirstName + " " + s.LastName
                          };

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Approve(int id)
        {
            try
            {
                // TODO: Add delete logic here
                //var pr = entity.StockTransfers.FirstOrDefault(r => r.ID == id && (r.RequestedBy == UserID || AcctType == 1 || AcctType == 4));
                var st = entity.StockTransfers.Find(id);
                st.ApprovedStatus = 2;
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

        // POST: StockTransfer/Denied/5
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
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        // GET: StockTransfer/DeniedItems/5
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
        public ActionResult AddItemPartial(int id)
        {
            int reqID = entity.StockTransfers.Find(id).RequisitionID;
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

        // POST: StockTransfer/AddItemPartial/5
        [HttpPost]
        public ActionResult AddItemPartial(int id, StockTransferDetail stocktransfer)
        {
            try
            {
                stocktransfer.StockTransferID = id;
                stocktransfer.AprovalStatusID = 1; //submitted

                var st = entity.StockTransferDetails.Where(s => s.StockTransferID == stocktransfer.StockTransferID && s.RequisitionDetailID == stocktransfer.RequisitionDetailID).ToList();

                if (st.Count() > 0)
                {
                    TempData["PartialError"] = "Item is already in the list.";
                }
                else
                {
                    stocktransfer.IsSync = false;

                    entity.StockTransferDetails.Add(stocktransfer);
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

        // GET: StockTransfer/EditItemPartial/5
        public ActionResult EditItemPartial(int id)
        {
            var st = entity.StockTransferDetails.Find(id);

            ViewBag.AprovalStatusID = new SelectList(entity.ApprovalStatus, "ID", "Status", st.AprovalStatusID);

            return PartialView(st);
        }

        // POST: StockTransfer/EditItemPartial/5
        [HttpPost]
        public ActionResult EditItemPartial(int id, StockTransferDetail stocktransfer)
        {
            try
            {
                stocktransfer.IsSync = false;

                entity.Entry(stocktransfer).State = EntityState.Modified;
                entity.SaveChanges();
            }
            catch
            {
                TempData["PartialError"] = "There's an error.";
            }

            return RedirectToAction("PendingItems", new { id = stocktransfer.StockTransferID });
        }

        // GET: StockTransfer/DeleteItemPartial/5
        public ActionResult DeleteItemPartial(int id)
        {
            var st = entity.StockTransferDetails.Find(id);

            return PartialView(st);
        }

        // POST: StockTransfer/DeleteItemPartial/5
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

            return RedirectToAction("PendingItems", new { id = reqID });
        }
        #endregion
    }
}