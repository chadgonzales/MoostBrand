using MoostBrand.DAL;
using System;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using System.Data.Entity;
using System.Configuration;

namespace MoostBrand.Controllers
{
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


        // GET: StockAllocation/Create
        public ActionResult Create()
        {
            var sa = new StockAllocation();
            sa.SADate = DateTime.Now;

            #region DROPDOWNS
            ViewBag.LocationID = new SelectList(entity.Locations, "ID", "Description");
            ViewBag.ReceivingID = new SelectList(entity.Receivings.Where(r => r.ApprovalStatus == 2), "ID", "ReceivingID");
            #endregion

            return View(sa);
        }


        // POST: StockAllocation/Create
        [HttpPost]
        public ActionResult Create(StockAllocation sa)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var checkSA = entity.StockAllocations.FirstOrDefault(s => s.BatchNumber == sa.BatchNumber);
                    if (checkSA == null)
                    {
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
            ViewBag.LocationID = new SelectList(entity.Locations, "ID", "Description", sa.LocationID);
            ViewBag.ReceivingID = new SelectList(entity.Receivings.Where(r => r.ApprovalStatus == 2), "ID", "ReceivingID", sa.ReceivingID);
            #endregion

            return View(sa);
        }
    }
}