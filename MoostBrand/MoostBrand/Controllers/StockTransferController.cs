using MoostBrand.DAL;
using System;
using System.Linq;
using PagedList;
using System.Web.Mvc;
using System.Configuration;
using System.Data.Entity;

namespace MoostBrand.Controllers
{
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

        // GET: StockTransfer/Edit/5
        [HttpPost]
        public ActionResult Edit(StockTransfer stocktransfer)
        {
            if (ModelState.IsValid)
            {
                try
                {
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

        // GET: StockTransfer/Items/5
        public ActionResult Items(int id, int? page)
        {
            int reqID = entity.StockTransfers.Find(id).RequisitionID;
            var items = entity.RequisitionDetails
                        .ToList()
                        .FindAll(rd => rd.RequisitionID == reqID && rd.AprovalStatusID == 2);

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }
    }
}