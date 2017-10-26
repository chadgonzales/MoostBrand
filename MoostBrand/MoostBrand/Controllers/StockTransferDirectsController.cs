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

namespace MoostBrand.Controllers
{
    public class StockTransferDirectsController : Controller
    {
        private MoostBrandEntities entity = new MoostBrandEntities();

        #region Action
        // GET: StockTransferDirects
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            var items = entity.StockTransferDetails
                            .ToList()
                            .FindAll(r => r.AprovalStatusID == 2)
                            .Select(rd => new
                            {
                                ID = rd.ID,
                                Description = rd.RequisitionDetail.Item.Description
                            });

            ViewBag.ItemName = items;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "type" : "";
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "reqno" : "";

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
            var prs = from g in entity.StockTransferDirects
                         select g;

            if (!String.IsNullOrEmpty(searchString))
            {
                prs = prs.Where(o => o.TransferID.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "type":
                    prs = prs.OrderByDescending(o => o.TransferID);
                    break;
                default:
                    prs = prs.OrderBy(o => o.ID);
                    break;
            }

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);

            //if (user.LocationID != 10)
            //{
            //    prs = prs.Where(x => x.LocationID == locID);
            //    return View(prs.ToPagedList(pageNumber, pageSize));
            //}
            //else
            //    return View(prs.ToPagedList(pageNumber, pageSize));

            return View(prs.ToPagedList(pageNumber, pageSize));
        }

        // GET: StockTransferDirects/Details/5
        public ActionResult Details(int? id)
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

        // GET: StockTransferDirects/Create
        public ActionResult Create(string id)
        {
            var stDirect = new StockTransferDirect();
            stDirect.STDate = DateTime.Now;
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


            ViewBag.ItemID = new SelectList(items, "ID", "Description");
            ViewBag.LocationID = new SelectList(loc, "ID", "Description");
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
        public ActionResult Create(StockTransferDirect stockTransferDirect)
        {

            string itemname = entity.Items.Find(stockTransferDirect.ItemID).Description;

            var stDirect = entity.StockTransferDirects.Where(s => s.TransferID == stockTransferDirect.TransferID).ToList();

            if (stDirect.Count() > 0)
            {
                ModelState.AddModelError("", "Stock Transfer ID already exists");
            }
            else
            {
                #region Helper
                foreach (StockTransferHelper h in stockTransferDirect.StockTransferHelpers.ToList())
                {
                    try
                    {
                        if (h.DeletedHelper == 1)
                        {
                            stockTransferDirect.StockTransferHelpers.Remove(h);
                        }

                    }
                    catch
                    {
                        ModelState.AddModelError(string.Empty, "There's an error");
                    }
                }
                #endregion
                #region Operator
                foreach (StockTransferOperator o in stockTransferDirect.StockTransferOperators.ToList())
                {
                    try
                    {
                        if (o.DeletedOperator == 1)
                        {
                            stockTransferDirect.StockTransferOperators.Remove(o);
                        }
                    }
                    catch
                    {
                        ModelState.AddModelError(string.Empty, "There's an error");
                    }
                }
                #endregion

                stockTransferDirect.ItemName = itemname;
                stockTransferDirect.ApprovedStatus = 1;
                stockTransferDirect.IsSync = false;

                entity.StockTransferDirects.Add(stockTransferDirect);
                entity.SaveChanges();
                return RedirectToAction("Index");
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

        // GET: StockTransferDirects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var stockTransferDirect = entity.StockTransferDirects.Find(id);
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


                ViewBag.ItemID = new SelectList(items, "ID", "Description", stockTransferDirect.ItemID);
                ViewBag.LocationID = new SelectList(loc, "ID", "Description", stockTransferDirect.LocationID);
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
        public ActionResult Edit(StockTransferDirect stockTransferDirect)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    stockTransferDirect.IsSync = false;

                    #region Helper
                    foreach (StockTransferHelper helper in stockTransferDirect.StockTransferHelpers.ToList())
                    {
                        try
                        {
                            if (helper.DeletedHelper == 1 && helper.ID != 0)
                            {
                                entity.Entry(helper).State = EntityState.Deleted;
                                entity.SaveChanges();
                            }
                            else if (helper.ID != stockTransferDirect.ID)
                            {
                                helper.ID = stockTransferDirect.ID;

                                if (helper.Name != null && helper.Name.Trim() != string.Empty)
                                {
                                    entity.Entry(helper).State = EntityState.Added;
                                    entity.SaveChanges();
                                }
                            }
                            else if (helper.ID != 0)
                            {
                                if (helper.Name != null && helper.Name.Trim() != string.Empty)
                                {
                                    entity.Entry(helper).State = EntityState.Modified;
                                    entity.SaveChanges();
                                }
                            }
                        }
                        catch { }
                        stockTransferDirect.StockTransferHelpers.Remove(helper);
                    }
                    #endregion

                    #region Operator
                    foreach (StockTransferOperator operators in stockTransferDirect.StockTransferOperators.ToList())
                    {
                        try
                        {
                            if (operators.DeletedOperator == 1 && operators.ID != 0)
                            {
                                entity.Entry(operators).State = EntityState.Deleted;
                                entity.SaveChanges();
                            }
                            else if (operators.ID != stockTransferDirect.ID)
                            {
                                operators.ID = stockTransferDirect.ID;

                                if (operators.Name != null && operators.Name.Trim() != string.Empty)
                                {
                                    entity.Entry(operators).State = EntityState.Added;
                                    entity.SaveChanges();
                                }
                            }
                            else if (operators.ID != 0)
                            {
                                if (operators.Name != null && operators.Name.Trim() != string.Empty)
                                {
                                    entity.Entry(operators).State = EntityState.Modified;
                                    entity.SaveChanges();
                                }
                            }
                        }
                        catch { }
                        stockTransferDirect.StockTransferOperators.Remove(operators);
                    }
                    #endregion
                    string itemname = entity.Items.Find(stockTransferDirect.ItemID).Description;
                    stockTransferDirect.ItemName = itemname;
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


            ViewBag.ItemID = new SelectList(items, "ID", "Description", stockTransferDirect.ItemID);
            ViewBag.LocationID = new SelectList(loc, "ID", "Description", stockTransferDirect.LocationID);
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

        #region ApprovalStatus

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Approve(int id)
        {
            try
            {
           
                var st = entity.StockTransferDirects.Find(id);
                var Dst =st.ItemID;
                var rq = entity.Inventories.Where(r => r.Items.ID == Dst &&  r.LocationCode == st.LocationID).FirstOrDefault();
                var itm = rq.ID;

                var items = entity.Inventories.Find(itm);
                int qty = Convert.ToInt32(items.InStock) - Convert.ToInt32(st.Quantity);

                Inventory Invnt = entity.Inventories.Find(itm);
                Invnt.InStock = qty;
                Invnt.Available = (Invnt.InStock + Invnt.Ordered) - Invnt.Committed;

                st.ApprovedStatus = 2;
                st.IsSync = false;

                entity.Entry(st).State = EntityState.Modified;
                entity.SaveChanges();

                return RedirectToAction("Index");

            }
            catch
            {
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

            var st = entity.Inventories.Where(s => s.LocationCode == loc.ID
                                                       && s.InStock> 0)
                     .Select(r => new
                     {
                         ID = r.Items.ID,
                         ItemName = r.Items.Description
                     });


            

            return Json(st, JsonRequestBehavior.AllowGet);




        }

        #endregion
    }
}
