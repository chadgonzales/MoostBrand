using MoostBrand.DAL;
using System;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using System.Data.Entity;
using System.Configuration;
using MoostBrand.Models;

namespace MoostBrand.Controllers
{
    [LoginChecker]
    public class StockAllocationController : Controller
    {
        MoostBrandEntities entity = new MoostBrandEntities();

        // GET: StockAllocation
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "batchno" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;


            var sas = from o in entity.StockAllocations
                      select o;

            if (!String.IsNullOrEmpty(searchString))
            {
                sas = sas.Where(o => o.BatchNumber.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "batchno":
                    sas = sas.OrderByDescending(o => o.BatchNumber);
                    break;
                default:
                    sas = sas.OrderBy(o => o.ID);
                    break;
            }

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);
            return View(sas.ToPagedList(pageNumber, pageSize));
        }

        // GET: StockAllocation/Details/5
        public ActionResult Details(int id = 0)
        {
            //var pr = entity.Requisitions.FirstOrDefault(r => r.ID == id && (r.RequestedBy == UserID || AcctType == 1 || AcctType == 4));
            var sa = entity.StockAllocations.Find(id);
            if (sa == null)
            {
                return HttpNotFound();
            }

            return View(sa);
        }

        // GET: StockAllocation/Create
        public ActionResult Create()
        {
            var sa = new StockAllocation();
            sa.SADate = DateTime.Now;

            #region DROPDOWNS
            ViewBag.ReceivingID = new SelectList(entity.Receivings.Where(r => r.ApprovalStatus == 2), "ID", "ReceivingID");
            #endregion

            return View(sa);
        }
        
        // POST: StockAllocation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StockAllocation sa)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var checkSA = entity.StockAllocations.FirstOrDefault(s => s.BatchNumber == sa.BatchNumber);
                    if (checkSA == null)
                    {
                        sa.IsSync = false;

                        entity.StockAllocations.Add(sa);
                        entity.SaveChanges();

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "The Batch number already exists.");
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "There's an error.");
                }
            }

            #region DROPDOWNS
            ViewBag.ReceivingID = new SelectList(entity.Receivings.Where(r => r.ApprovalStatus == 2), "ID", "ReceivingID", sa.ReceivingID);
            #endregion

            return View(sa);
        }

        // GET: StockAllocation/Edit
        public ActionResult Edit(int id = 0)
        {
            var sa = entity.StockAllocations.Find(id);

            #region DROPDOWNS
            ViewBag.ReceivingID = new SelectList(entity.Receivings.Where(r => r.ApprovalStatus == 2), "ID", "ReceivingID", sa.ReceivingID);
            #endregion

            return View(sa);
        }

        // POST: StockAllocation/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StockAllocation sa)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    sa.IsSync = false;

                    entity.Entry(sa).State = EntityState.Modified;
                    entity.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch
                {
                    ModelState.AddModelError("", "There's an error.");
                }
            }

            #region DROPDOWNS
            ViewBag.ReceivingID = new SelectList(entity.Receivings.Where(r => r.ApprovalStatus == 2), "ID", "ReceivingID", sa.ReceivingID);
            #endregion

            return View(sa);
        }

        // GET: StockAllocation/Delete/5
        public ActionResult Delete(int id)
        {
            //var pr = entity.Requisitions.FirstOrDefault(r => r.ID == id && (r.RequestedBy == UserID || AcctType == 1 || AcctType == 4));
            var allocation = entity.StockAllocations.FirstOrDefault(r => r.ID == id);
            if (allocation == null)
            {
                return HttpNotFound();
            }

            return View(allocation);
        }

        // POST: StockAllocation/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {
            var allocation = entity.StockAllocations.Find(id);

            try
            {
                entity.StockAllocations.Remove(allocation);
                entity.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
            }

            return View(allocation);
        }

        // GET: StockAllocation/Items/5
        public ActionResult Items(int id, int? page)
        {
            int UserID = Convert.ToInt32(Session["sessionuid"]);
            int UserType = Convert.ToInt32(Session["usertype"]);

            var sa = entity.StockAllocations.Find(id);

            var items = entity.StockAllocationDetails
                        .ToList()
                        .FindAll(rd => rd.StockAllocationID == id);

            ViewBag.STAid = id;
            ViewBag.ReceivedBy = sa.Receiving.ReceivedBy;
            ViewBag.UserID = UserID;
            ViewBag.AcctType = UserType;

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        #region PARTIAL

        // GET: StockAllocation/AddItemPartial/5
        public ActionResult AddItemPartial(int id)
        {
            int recID = entity.StockAllocations.Find(id).ReceivingID;
            var items = entity.ReceivingDetails
                        .ToList()
                        .FindAll(rd => rd.ReceivingID == recID && rd.AprovalStatusID == 2)
                        .Select(ed => new
                        {
                            ID = ed.ID,
                            Description = ed.StockTransferDetail.RequisitionDetail.Item.Description
                        });

            ViewBag.STAid = id;
            ViewBag.ReceivingDetailID = new SelectList(items, "ID", "Description");
            ViewBag.ContainerLocationID = new SelectList(entity.ContainerLocations, "ID", "Description");
            ViewBag.ContainerStorageID = new SelectList(entity.ContainerStorages, "ID", "Description");

            return PartialView();
        }

        // POST: StockAllocation/AddItemPartial/5
        [HttpPost]
        public ActionResult AddItemPartial(int id, StockAllocationDetail sad)
        {
            try
            {
                // TODO: Add insert logic here
                sad.StockAllocationID = id;
                sad.IsSync = false;

                entity.StockAllocationDetails.Add(sad);
                entity.SaveChanges();
            }
            catch
            {
                TempData["PartialError"] = "There's an error.";
            }
            
            return RedirectToAction("Items", new { id = id });
        }

        // GET: StockAllocation/EditItemPartial/5
        public ActionResult EditItemPartial(int id)
        {
            var sad = entity.StockAllocationDetails.Find(id);
            
            ViewBag.ContainerLocationID = new SelectList(entity.ContainerLocations, "ID", "Description", sad.ContainerLocationID);
            ViewBag.ContainerStorageID = new SelectList(entity.ContainerStorages, "ID", "Description", sad.ContainerStorageID);

            return PartialView(sad);
        }

        // POST: StockAllocation/EditItemPartial/5
        [HttpPost]
        public ActionResult EditItemPartial(int id, StockAllocationDetail rd)
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

            return RedirectToAction("PendingItems", new { id = rd.StockAllocationID });
        }

        // GET: StockAllocation/DeleteItemPartial/5
        public ActionResult DeleteItemPartial(int id)
        {
            var rd = entity.StockAllocationDetails.Find(id);

            return PartialView(rd);
        }

        // POST: Receiving/DeleteItemPartial/5
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

            return RedirectToAction("PendingItems", new { id = reqID });
        }

        #endregion
    }
}