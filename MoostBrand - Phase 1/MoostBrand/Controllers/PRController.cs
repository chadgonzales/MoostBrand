using System;
using System.Linq;
using System.Web.Mvc;
using MoostBrand.DAL;
using PagedList;
using System.Data.Entity;
using System.Configuration;
using MoostBrand.Models;
using System.Web.Routing;
using System.Collections.Generic;
using MoostBrand.Helpers;
using System.Data.Entity.Validation;
using System.Linq.Dynamic;

namespace MoostBrand.Controllers
{
    [LoginChecker]
    public class PRController : Controller
    {
        MoostBrandEntities entity = new MoostBrandEntities();
        RequisitionDetailsRepository reqDetailRepo = new RequisitionDetailsRepository();
        InventoryRepository invRepo = new InventoryRepository();

        int UserID = 0;
        int UserType = 0;
        int ModuleID = 3;

        #region COMMENT



        #endregion


        #region PRIVATE METHODS
        private string Generator(string prefix)
        {
            startR: var cref = entity.Requisitions.Where(r => r.RefNumber.Contains(prefix)).Count();

            string refnum = string.Format(prefix + "-{0:000000}", cref);

            var pr = entity.Requisitions.ToList().FindAll(p => p.RefNumber == refnum);
            if (pr.Count() > 0)
            {
                goto startR;
            }


            return refnum;
            ////Initiate objects & vars
            //startR: Random random = new Random();
            //string randomString = "";
            //int randNumber = 0;

            ////Loop ‘length’ times to generate a random number or character
            //for (int i = 0; i < 6; i++)
            //{
            //    if (i == 0)
            //    {
            //        start: randNumber = random.Next(0, 9); //int {0-9}
            //        if (randNumber == 0)
            //            goto start;
            //    }
            //    else
            //    {
            //        randNumber = random.Next(0, 9);
            //    }
            //    //append random char or digit to random string
            //    randomString = randomString + randNumber.ToString();
            //}

            //randomString = prefix + "-" + randomString;
            //var pr = entity.Requisitions.ToList().FindAll(p => p.RefNumber == randomString);
            //if (pr.Count() > 0)
            //{
            //    goto startR;
            //}

            ////return the random string
            //return randomString;
        }

        private Requisition SetNull(Requisition pr)
        {
            if (pr.RequisitionTypeID == 1) //PR
            {
                pr.Destination = null;
                pr.PlateNumber = null;
                pr.Others = null;
                pr.TimeDeparted = null;
                pr.TimeArrived = null;
                pr.Driver = null;
                pr.Customer = null;
                pr.ReservationTypeID = null;
                pr.ReservedBy = null;
                pr.PaymentStatusID = null;
                pr.InvoiceNumber = null;
                pr.AuthorizedPerson = null;
                pr.ValidatedBy = null;
                pr.ShipmentTypeID = null;
                pr.DropShipID = null;
                pr.Driver = null;
            }
            else if (pr.RequisitionTypeID == 2 || pr.RequisitionTypeID == 3 || pr.RequisitionTypeID == 5) //BR or WR or OR
            {
                pr.VendorID = null;
                pr.Customer = null;
                pr.ReservationTypeID = null;
                pr.ReservedBy = null;
                pr.PaymentStatusID = null;
                pr.InvoiceNumber = null;
                pr.AuthorizedPerson = null;
                pr.ValidatedBy = null;
                pr.ShipmentTypeID = null;
                pr.DropShipID = null;
            }
            else if (pr.RequisitionTypeID == 4) //CR
            {
                pr.VendorID = null;
                pr.Destination = null;
                pr.PlateNumber = null;
                pr.Others = null;
                pr.TimeDeparted = null;
                pr.TimeArrived = null;
                if (pr.ShipmentTypeID == 2)
                {
                    pr.DropShipID = null;
                }
            }
            else
            {
                return null;
            }
            return pr;
        }

        private void AccessChecker(int action)
        {
            int UserID = Convert.ToInt32(Session["sessionuid"]);

            var access = entity.UserAccesses.FirstOrDefault(u => u.EmployeeID == UserID && u.ModuleID == ModuleID);

            if(action == 1) //CanView
            {
                if (!access.CanView)
                {
                    RedirectToAction("Index", "Home");
                }
            }
            else if(action == 2)//CanEdit
            {
                if (!access.CanEdit)
                {
                    RedirectToAction("Index", "Home");
                }
            }
            else if (action == 3)//CanDelete
            {
                if (!access.CanDelete)
                {
                    RedirectToAction("Index", "Home");
                }
            }
            else if (action == 4)//CanRequest
            {
                if (!access.CanRequest)
                {
                    RedirectToAction("Index", "Home");
                }
            }
            else if (action == 5)//Can Approve/Deny
            {
                if (!access.CanDecide)
                {
                    RedirectToAction("Index", "Home");
                }
            }
        }

        public int getCommited(int itemID)
        {
            var requi = entity.RequisitionDetails.FirstOrDefault(x => x.AprovalStatusID == 2);
            int c = 0;
            if (requi != null)
            {
                var lstReqDetail = new List<RequisitionDetail>();

                lstReqDetail = entity.RequisitionDetails.Where(x => x.Requisition.RequisitionTypeID == 4 && x.AprovalStatusID == 2 && x.ItemID == itemID && x.Requisition.LocationID == requi.Requisition.LocationID).ToList();

                c = lstReqDetail.Sum(x => x.Quantity) ?? 0;
            }
            return c;

        }
        public int getPurchaseOrder(int itemID)
        {
            var requi = entity.RequisitionDetails.FirstOrDefault(x => x.AprovalStatusID == 2);
            int po = 0;
            if (requi != null)
            {
                var lstReqDetail = new List<RequisitionDetail>();

                lstReqDetail = entity.RequisitionDetails.Where(x => x.Requisition.RequisitionTypeID == 1 && x.AprovalStatusID == 2 && x.ItemID == itemID && x.Requisition.LocationID == requi.Requisition.LocationID).ToList();

                po = lstReqDetail.Sum(x => x.Quantity) ?? 0;
            }
            return po;
        }
        public int getInstocked(string description)
        {
            var requi = entity.Requisitions.FirstOrDefault(x => x.RequisitionTypeID == 4 || x.RequisitionTypeID == 1);
            int getIS = 0;
            var query = entity.Inventories.FirstOrDefault(x => x.Description == description && x.LocationCode == requi.LocationID);
            if (query != null)
            {
                getIS = Convert.ToInt32(query.InStock);
            }
            return getIS;
        }
        #endregion

        #region JSON
        public ActionResult GenerateRefNumber(string id)
        {
            return Json(Generator(id), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetRequisitionType(int id)
        {
            var reqtype = entity.RequisitionTypes.Where(x => x.ReqTypeID == id)
                            .Select(x => new
                            {
                                ID = x.ID,
                                Type = x.Type
                            });
            return Json(reqtype, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetVendors(string name)
        {
            //x.Name.Contains(name) ||
            var vendors = entity.Vendors.Where(x => x.GeneralName.Contains(name))
                            .Select(x => new
                            {
                                ID = x.ID,
                                Name = x.GeneralName
                            });
            return Json(vendors, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCategories(string name)
        {
            var categories = entity.Categories.Where(x => x.Description.Contains(name))
                            .Select(x => new
                            {
                                ID = x.ID,
                                Name = x.Description
                            });
            return Json(categories, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetItems(int catID, string name, int vendorid)
        {
            if (vendorid != 0)
            {
                var items = entity.Items.Where(x => x.CategoryID == catID || x.Description.Contains(name) && x.VendorCoding == vendorid && x.ItemStatus == 1)
                 .Select(x => new {
                     ID = x.ID,
                     Code = x.Code,
                     Vendors = x.VendorCoding,
                     Name = x.DescriptionPurchase,
                     PURName = x.Description,
                     UOM = x.UnitOfMeasurement.Description,
                     Category = x.Category.Description
                 });
                return Json(items, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var items = entity.Items.Where(x => x.CategoryID == catID || x.Description.Contains(name) && x.ItemStatus == 1)
                 .Select(x => new {
                     ID = x.ID,
                     Code = x.Code,
                     Vendors = x.VendorCoding,
                     Name = x.DescriptionPurchase,
                     PURName = x.Description,
                     UOM = x.UnitOfMeasurement.Description,
                     Category = x.Category.Description
                 });
                return Json(items, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetItemCode(int catID, string name, int vendorid)
        {
            if (vendorid != 0)
            {
                var items = entity.Items.Where(x => x.CategoryID == catID || x.Code.Contains(name) && x.VendorCoding == vendorid && x.ItemStatus == 1)
                 .Select(x => new {
                     ID = x.ID,
                     Code = x.Code,
                     Vendors = x.VendorCoding,
                     Name = x.DescriptionPurchase,
                     PURName = x.Description,
                     UOM = x.UnitOfMeasurement.Description,
                     Category = x.Category.Description
                 });
                return Json(items, JsonRequestBehavior.AllowGet);

            }
            else
            {
                var items = entity.Items.Where(x => x.CategoryID == catID || x.Code.Contains(name) && x.ItemStatus == 1)
                 .Select(x => new {
                     ID = x.ID,
                     Code = x.Code,
                     Vendors = x.VendorCoding,
                     Name = x.DescriptionPurchase,
                     PURName = x.Description,
                     UOM = x.UnitOfMeasurement.Description,
                     Category = x.Category.Description
                 });
                return Json(items, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult getInstock(int id, string Code, int ItemID)
        {
            int total = (reqDetailRepo.getInstocked(id, Code ) - reqDetailRepo.getStockTranfer(ItemID));

            return Json(total, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult getCommit(int id, int ItemID)
        {
            int total = reqDetailRepo.getCommited(id, ItemID);

            return Json(total, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult getPO(int id, int ItemID)
        {
            var requi = entity.Requisitions.Find(id);
            

            int total = reqDetailRepo.getPurchaseOrder(requi.LocationID.Value,ItemID);
          
            return Json(total, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Actions
        // GET: PR
        [AccessCheckerForDisablingButtons(ModuleID = 3)]
        [AccessChecker(Action = 1, ModuleID = 3)]
        public ActionResult Index(string sortOrder,string sortColumn, string currentFilter, string nextpage, string searchString, string dateFrom, string dateTo, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            //ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "type" : "";
            //ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "reqno" : "";
            //ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            ViewBag.CurrentSort = sortColumn;

            if (nextpage == null)
                sortOrder = sortOrder == "asc" ? "desc" : "asc";


            ViewBag.SortOrder = sortOrder;
            DateTime dtDateFrom = DateTime.Now.Date;
            DateTime dtDateTo = DateTime.Now;

            if (!String.IsNullOrEmpty(dateFrom))
            {
                dtDateFrom = Convert.ToDateTime(dateFrom);
            }

            if (!String.IsNullOrEmpty(dateTo))
            {
                dtDateTo = Convert.ToDateTime(dateTo);
            }

            //if (!String.IsNullOrEmpty(filterFrom))
            //{
            //    dtDateFrom = Convert.ToDateTime(filterFrom);
            //}

            //if (!String.IsNullOrEmpty(filterTo))
            //{
            //    dtDateTo = Convert.ToDateTime(filterTo);
            //}



            //if (searchString != null || dateFrom !=null && dateTo != null)
            //{
            //    page = 1;
            //}
            //else
            //{
            //    searchString = currentFilter;
            //    dateFrom = filterFrom;
            //    dateTo = filterTo;
            //}

            ViewBag.FilterFrom = dateFrom;
            ViewBag.FilterTo = dateTo;
            ViewBag.CurrentFilter = searchString;
            
            //int locID = Convert.ToInt32(Session["locationID"]);
            //int UserID = Convert.ToInt32(Session["userID"]);

            //var user = entity.Users.FirstOrDefault(x => x.ID == UserID);
           // var prs = entity.Requisitions.Where(x => x.Status == false && ( x.ID != 4 && x.ID != 5)); //active
            IQueryable<Requisition> prs = reqDetailRepo.List().Where(x => x.Status == false && (x.ID != 4 && x.ID != 5));

            if (!String.IsNullOrEmpty(searchString))
            {
                prs = prs.Where(o => o.RequisitionType.Type.Contains(searchString)
                                       || o.RefNumber.Contains(searchString));
            }

            //DateTime dtDateFrom = DateTime.Now.Date;
            //DateTime dtDateTo = DateTime.Now;

            //if (!String.IsNullOrEmpty(dateFrom) || !String.IsNullOrEmpty(dateTo))
            //{
            //    dtDateFrom = Convert.ToDateTime(dateFrom);
            //    dtDateTo = Convert.ToDateTime(dateTo);
            prs = prs.Where(p => DbFunctions.TruncateTime(p.RequestedDate) >= dtDateFrom && DbFunctions.TruncateTime(p.RequestedDate) <= dtDateTo);
            //}


            //switch (sortOrder)
            //{
            //    case "type":
            //        prs = prs.OrderByDescending(o => o.RequisitionType.Type);
            //        break;
            //    case "reqno":
            //        prs = prs.OrderByDescending(o => o.RefNumber);
            //        break;
            //    case "Date":
            //        prs = prs.OrderBy(o => o.RequestedDate);
            //        break;
            //    case "date_desc":
            //        prs = prs.OrderByDescending(o => o.RequestedDate);
            //        break;
            //    default:
            //        prs = prs.OrderByDescending(o => o.ID);
            //        break;
            //}

            if (String.IsNullOrEmpty(sortColumn))
            {
                prs = prs.OrderBy(p => p.ID);
            }
            else
            {
                prs = prs.OrderBy(sortColumn + " " + sortOrder);
            }

            ViewBag.DateFrom = dtDateFrom.ToString("MM/dd/yyyy");
            ViewBag.DateTo = dtDateTo.ToString("MM/dd/yyyy");

            int pageSize = 10;
            int pageNumber = (page ?? 1);
           return View(prs.ToPagedList(pageNumber, pageSize));
            //int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            //int pageNumber = (page ?? 1);

            ////if (user.LocationID != 10)
            ////{
            ////    prs = prs.Where(x => x.LocationID == locID);
            ////    return View(prs.ToPagedList(pageNumber, pageSize));
            ////}
            ////else
            ////    return View(prs.ToPagedList(pageNumber, pageSize));

            //return View(prs.OrderBy(p => p.ID).ToPagedList(pageNumber, pageSize));
        }

        // GET: PR/Details/5
        [AccessChecker(Action = 1, ModuleID = 3)]
        public ActionResult Details(int? id,int? page)
        {

            Session["requisitionId"] = id;

            var req = new Req
            {
                Requisitions = entity.Requisitions.Find(id)
            }; 

            var reqdetails = entity.RequisitionDetails
                     .Where(e => e.RequisitionID == id)
                     .Select(x => new ReqDetails
                     {
                         RequisitionDetails = x
                     }).ToList();

            if (req == null)
            {
                return HttpNotFound();
            }
            ViewBag.isApproved = req.Requisitions.ApprovalStatus;
            ViewBag.Page = page;
            Session["ReqTypeID"] = req.Requisitions.ReqTypeID.ToString();
            ViewBag.ReqTypeID =   req.Requisitions.ReqTypeID.ToString();
    
            var pager = new Pager(reqdetails.Count(),page);

            var viewModel = new requsitionModel
            {
                requisition = req,
                requisitiondetails = reqdetails.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: PR/Create
        [AccessChecker(Action = 2, ModuleID = 3)]
        public ActionResult Create()
        {
            var pr = new Requisition();
            pr.RequestedDate = DateTime.Now;

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

            
            ViewBag.PaymentStatusID = new SelectList(entity.PaymentStatus, "ID", "Status");
            ViewBag.ReqTypeID = new SelectList(entity.ReqTypes, "ID", "Type");
            ViewBag.RequisitionTypeID = new SelectList(entity.RequisitionTypes.Where(p=> p.ID!= 4 && p.ID !=5), "ID", "Type");
            ViewBag.RequestedBy = new SelectList(employees, "ID", "FullName");
            ViewBag.LocationID = new SelectList(loc, "ID", "Description");
            //ViewBag.VendorID = new SelectList(entity.Vendors, "ID", "Name");
            ViewBag.ReservationTypeID = new SelectList(entity.ReservationTypes, "ID", "Type");
            ViewBag.ShipmentTypeID = new SelectList(entity.ShipmentTypes, "ID", "Type");
            ViewBag.DropShipID = new SelectList(entity.DropShipTypes, "ID", "Type");
            ViewBag.ReservedBy = new SelectList(employees, "ID", "FullName");
            ViewBag.ValidatedBy = new SelectList(employees, "ID", "FullName");
            ViewBag.Destination = new SelectList(loc, "ID", "Description");
            ViewBag.ApprovalStatus = new SelectList(entity.ApprovalStatus, "ID", "Status");
            ViewBag.ApprovedBy = new SelectList(employees, "ID", "FullName");
            ViewBag.EncodedBy = new SelectList(employees,"ID","FullName");
            #endregion

            return View(pr);
        }

        // POST: PR/Create
        [AccessChecker(Action = 2, ModuleID = 3)]
        [HttpPost]
        public ActionResult Create(Requisition pr)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    pr.ApprovalStatus = 1; //submitted
                    pr.Status = false;

                    var checkPR = entity.Requisitions.Where(r => r.RefNumber == pr.RefNumber);

                    if (checkPR.Count() > 0)
                    {
                        ModelState.AddModelError("", "The ref number already exists.");
                    }
                    else
                    {
                        var newPR = SetNull(pr);

                        if (pr.LocationID == pr.Destination)
                        {
                            ModelState.AddModelError("", "Source location should not be the same with the destination.");
                        }
                        else
                        {
                            //if (pr.Proceed == false)
                            //{
                            if (newPR != null)
                            {
                                newPR.IsSync = false;
                                //newPR.RequestedDate = DateTime.Now;
                                //ViewBag.Error = "Would you like to proceed?";

                                entity.Requisitions.Add(newPR);
                                entity.SaveChanges();

                                //return RedirectToAction("Index");

                                return RedirectToAction("Details", new { id = pr.ID });
                            }
                            //}
                        }
                    }
                }
                catch(Exception e)
                {
                    e.ToString();
                    ModelState.AddModelError("", "There's an error.");
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

            ViewBag.PaymentStatusID = new SelectList(entity.PaymentStatus, "ID", "Status");
            ViewBag.ReqTypeID = new SelectList(entity.ReqTypes, "ID", "Type", pr.ReqTypeID);
            ViewBag.RequisitionTypeID = new SelectList(entity.RequisitionTypes.Where(p => p.ID != 4 && p.ID != 5), "ID", "Type", pr.RequisitionTypeID);
            ViewBag.RequestedBy = new SelectList(employees, "ID", "FullName", pr.RequestedBy);
            ViewBag.LocationID = new SelectList(loc, "ID", "Description", pr.LocationID);
            //ViewBag.VendorID = new SelectList(entity.Vendors, "ID", "Name", pr.VendorID);
            ViewBag.ReservationTypeID = new SelectList(entity.ReservationTypes, "ID", "Type", pr.ReservationTypeID);
            ViewBag.ShipmentTypeID = new SelectList(entity.ShipmentTypes, "ID", "Type", pr.ShipmentTypeID);
            ViewBag.DropShipID = new SelectList(entity.DropShipTypes, "ID", "Type", pr.DropShipID);
            ViewBag.ReservedBy = new SelectList(employees, "ID", "FullName", pr.ReservedBy);
            ViewBag.ValidatedBy = new SelectList(employees, "ID", "FullName", pr.ValidatedBy);
            ViewBag.Destination = new SelectList(entity.Locations, "ID", "Description", pr.Destination);
            ViewBag.ApprovalStatus = new SelectList(entity.ApprovalStatus, "ID", "Status", pr.ApprovalStatus);
            ViewBag.ApprovedBy = new SelectList(employees, "ID", "FullName", pr.ApprovedBy);
            ViewBag.EncodedBy = new SelectList(employees,"ID","FullName",pr.EncodedBy);

            if(pr.VendorID != null)
            {
                ViewBag.VendorName = entity.Vendors.FirstOrDefault(x => x.ID == pr.VendorID).Name;              
            }

            #endregion

            return View(pr);
        }

        // GET: PR/Edit/5
        [AccessChecker(Action = 2, ModuleID = 3)]
        public ActionResult Edit(int id)
        {
            var pr = entity.Requisitions.FirstOrDefault(r => r.ID == id);
            if (pr.ApprovalStatus == 1)
            {
                #region DROPDOWNS

                var employees = from s in entity.Employees
                                select new
                                {
                                    ID = s.ID,
                                    FullName = s.FirstName + " " + s.LastName
                                };
                ViewBag.RequisitionTypeID = new SelectList(entity.RequisitionTypes.Where(p => p.ID != 4 && p.ID != 5), "ID", "Type", pr.RequisitionTypeID);
                ViewBag.RequestedBy = new SelectList(employees, "ID", "FullName", pr.RequestedBy);
                ViewBag.LocationID = new SelectList(entity.Locations, "ID", "Description", pr.LocationID);
                ViewBag.VendorID = pr.VendorID;
                ViewBag.ReservationTypeID = new SelectList(entity.ReservationTypes, "ID", "Type", pr.ReservationTypeID);
                ViewBag.ShipmentTypeID = new SelectList(entity.ShipmentTypes, "ID", "Type", pr.ShipmentTypeID);
                ViewBag.DropShipID = new SelectList(entity.DropShipTypes, "ID", "Type", pr.DropShipID);
                ViewBag.ReservedBy = new SelectList(employees, "ID", "FullName", pr.ReservedBy);
                ViewBag.ValidatedBy = new SelectList(employees, "ID", "FullName", pr.ValidatedBy);
                ViewBag.Destination = new SelectList(entity.Locations, "ID", "Description", pr.Destination);
                ViewBag.ApprovalStatus = new SelectList(entity.ApprovalStatus, "ID", "Status", pr.ApprovalStatus);
                ViewBag.ApprovedBy = new SelectList(employees, "ID", "FullName", pr.ApprovedBy);
                ViewBag.PaymentStatusID = new SelectList(entity.PaymentStatus, "ID", "Status", pr.PaymentStatusID);
                ViewBag.EncodedBy = new SelectList(employees,"ID","FullName",pr.EncodedBy);
                if (pr.VendorID != null)
                {
                    ViewBag.VendorName = entity.Vendors.FirstOrDefault(x => x.ID == pr.VendorID).Name;
                }
                #endregion

                return View(pr);
            }

            return RedirectToAction("Details", new { id = id });
        }

        // POST: PR/Edit/5
        [AccessChecker(Action = 2, ModuleID = 3)]
        [HttpPost]
        public ActionResult Edit(Requisition pr)
        {
            if (pr.VendorID == null)
            {
                pr.VendorID = entity.Requisitions.Find(pr.ID).VendorID;
            }
            if (ModelState.IsValid)
            {
                try
                {
                    
                    var r = entity.Requisitions.FirstOrDefault(r1 => r1.ID == pr.ID);
                    if(r.ApprovalStatus == 1)
                    {
                        pr.IsSync = false;
                        pr.Status = false;
                        pr.ReqTypeID = r.ReqTypeID;
                        var newPR = SetNull(pr);
                        newPR.ApprovalStatus = r.ApprovalStatus;
                      //  newPR.RequestedDate = r.RequestedDate;
                        entity.Entry(r).CurrentValues.SetValues(newPR);
                        entity.SaveChanges();

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return RedirectToAction("Details", new { id = pr.ID });
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    // Retrieve the error messages as a list of strings.
                    var errorMessages = ex.EntityValidationErrors
                            .SelectMany(x => x.ValidationErrors)
                            .Select(x => x.ErrorMessage);

                    // Join the list to a single string.
                    var fullErrorMessage = string.Join("; ", errorMessages);

                    // Combine the original exception message with the new one.
                    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                    // Throw a new DbEntityValidationException with the improved exception message.
                    throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                }
            }

            #region DROPDOWNS
            var employees = from s in entity.Employees
                            select new
                            {
                                ID = s.ID,
                                FullName = s.FirstName + " " + s.LastName
                            };

            
            ViewBag.RequisitionTypeID = new SelectList(entity.RequisitionTypes.Where(p => p.ID != 4 && p.ID != 5), "ID", "Type", pr.RequisitionTypeID);
            ViewBag.RequestedBy = new SelectList(employees, "ID", "FullName", pr.RequestedBy);
            ViewBag.LocationID = new SelectList(entity.Locations, "ID", "Description", pr.LocationID);
            ViewBag.VendorID = pr.VendorID;
            ViewBag.ReservationTypeID = new SelectList(entity.ReservationTypes, "ID", "Type", pr.ReservationTypeID);
            ViewBag.ShipmentTypeID = new SelectList(entity.ShipmentTypes, "ID", "Type", pr.ShipmentTypeID);
            ViewBag.DropShipID = new SelectList(entity.DropShipTypes, "ID", "Type", pr.DropShipID);
            ViewBag.ReservedBy = new SelectList(employees, "ID", "FullName", pr.ReservedBy);
            ViewBag.ValidatedBy = new SelectList(employees, "ID", "FullName", pr.ValidatedBy);
            ViewBag.Destination = new SelectList(entity.Locations, "ID", "Description", pr.Destination);
            ViewBag.ApprovalStatus = new SelectList(entity.ApprovalStatus, "ID", "Status", pr.ApprovalStatus);
            ViewBag.ApprovedBy = new SelectList(employees, "ID", "FullName", pr.ApprovedBy);
            ViewBag.PaymentStatusID = new SelectList(entity.PaymentStatus, "ID", "Status", pr.PaymentStatusID);
            ViewBag.EncodedBy = new SelectList(employees,"ID","FullName",pr.EncodedBy);
            if (pr.VendorID != null)
            {
                ViewBag.VendorName = entity.Vendors.FirstOrDefault(x => x.ID == pr.VendorID).Name;
            }
            #endregion

            return View(pr);
        }

        // GET: PR/Delete/5
        [AccessChecker(Action = 3, ModuleID = 3)]
        public ActionResult Delete(int id)
        {
            
            var pr = entity.Requisitions.FirstOrDefault(r => r.ID == id);
            if(pr == null)
            {
                return HttpNotFound();
            }
            else
            {
                if(pr.ApprovalStatus == 1)
                {
                    return View(pr);
                }
            }
            
            return RedirectToAction("Details", new { id = pr.ID });
        }

        // POST: PR/Delete/5
        [AccessChecker(Action = 3, ModuleID = 3)]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {
            try
            {
                var pr = entity.Requisitions.Find(id);
                pr.Status = true;
                entity.Entry(pr).State = EntityState.Modified;
                entity.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
            }

            return RedirectToAction("Delete", new { id });
        }

        // POST: PR/Approve/5
        [AccessChecker(Action = 5, ModuleID = 3)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Approve(int id)
        {
            try
            {
                int approve = 0;
                // TODO: Add delete logic here
              
                var pr = entity.Requisitions.Find(id);

                if(pr.RequisitionDetails.Count() > 0)
                {
                    foreach ( var _details in pr.RequisitionDetails)
                    {
                        if (_details.AprovalStatusID != 1)
                        {
                            approve++;
                        }
                    }

                    if (pr.ApprovalStatus == 1)
                    {
                        if (approve == pr.RequisitionDetails.Count())
                        {

                            pr.ApprovalStatus = 2;
                            pr.IsSync = false;

                            entity.Entry(pr).State = EntityState.Modified;
                            entity.SaveChanges();

                            var rd = pr.RequisitionDetails.Select(p => p.ItemID).ToList();
                            var item = entity.Items.Where(i => rd.Contains(i.ID)).Select(i => i.Code);
                            var inv = entity.Inventories.Where(i => item.Contains(i.ItemCode) && i.LocationCode == pr.LocationID).ToList();
                            if (inv != null)
                            {
                                foreach (var _inv in inv)
                                {
                                    var i = entity.Inventories.Find(_inv.ID);
                                    int _qty = pr.RequisitionDetails.FirstOrDefault(p => p.Item.Code == _inv.ItemCode && p.RequisitionID == id).Quantity.Value;
                                    if (pr.ReqTypeID == 2)
                                    {
                                        i.Committed = i.Committed + _qty; //invRepo.getCommited(_inv.ItemCode,pr.LocationID.Value);
                                    }
                                    else
                                    {
                                        i.Ordered = i.Ordered + _qty; //invRepo.getPurchaseOrder(_inv.ItemCode, pr.LocationID.Value);
                                    }

                                    i.InStock = invRepo.getInstocked(pr.ID, _inv.ItemCode);
                                    i.Available = (i.InStock + i.Ordered) - i.Committed;

                                    entity.Entry(i).State = EntityState.Modified;
                                    entity.SaveChanges();

                                }

                            }

                            var invitems = entity.Items.Where(i => rd.Contains(i.ID)).ToList();

                            foreach (var _item in invitems)
                            {
                                int _qty = pr.RequisitionDetails.FirstOrDefault(p => p.Item.Code == _item.Code && p.RequisitionID == id).Quantity.Value;
                                var inv1 = entity.Inventories.Where(i => i.ItemCode == _item.Code && i.LocationCode == pr.LocationID).ToList();
                                if (inv1.Count == 0)
                                {
                                    Inventory inventory = new Inventory();
                                    inventory.Year = _item.Year;
                                    inventory.ItemCode = _item.Code;
                                    inventory.POSBarCode = _item.Barcode;
                                    inventory.Description = _item.DescriptionPurchase;
                                    inventory.SalesDescription = _item.Description;
                                    inventory.Category = _item.Category.Description;
                                    inventory.InventoryUoM = _item.UnitOfMeasurement.Description;
                                    inventory.InventoryStatus = 2;
                                    inventory.LocationCode = pr.LocationID;
                                    if (pr.ReqTypeID == 2)
                                    {
                                        inventory.Committed = _qty; //invRepo.getCommited(_inv.ItemCode,pr.LocationID.Value);
                                        inventory.Ordered = 0;
                                    }
                                    else
                                    {
                                        inventory.Ordered = _qty; //invRepo.getPurchaseOrder(_inv.ItemCode, pr.LocationID.Value);
                                        inventory.Committed = 0;
                                    }
                                    inventory.InStock = 0;
                                    inventory.Available = (inventory.InStock + inventory.Ordered) - inventory.Committed;
                                    inventory.ItemID = _item.ID;

                                    entity.Inventories.Add(inventory);
                                    entity.SaveChanges();
                                }
                            }
                            return RedirectToAction("Index");
                        }

                        else
                        {
                            TempData["Error"] = "Not all items are approved";
                            //ModelState.AddModelError(string.Empty, "Not all items are approved");
                        }
                    }
                }
                else
                {
                    TempData["Error"] = "There's no item";
                    //ModelState.AddModelError(string.Empty, "There's no item");
                }
            }
            catch (Exception e)
            {

               string c = e.ToString();
            }
           
            return View();
        }

        // POST: PR/Approve/5
        [AccessChecker(Action = 5, ModuleID = 3)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Revise(int id)
        {
            try
            {
                // TODO: Add delete logic here
                //var pr = entity.Requisitions.FirstOrDefault(r => r.ID == id && (r.RequestedBy == UserID || AcctType == 1 || AcctType == 4));
                var pr = entity.Requisitions.Find(id);

                if (pr.RequisitionDetails.Count() > 0)
                {
                    pr.ApprovalStatus = 4;
                    pr.IsSync = false;

                    entity.Entry(pr).State = EntityState.Modified;
                    entity.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "There's no item");
                }
            }
            catch
            {
            }
            return View();
        }

        // POST: PR/Denied/5
        [AccessChecker(Action = 5, ModuleID = 3)]
        [HttpPost]
        public ActionResult Denied(int id)
        {
            try
            {
                // TODO: Add delete logic here
                var pr = entity.Requisitions.Find(id);
                pr.ApprovalStatus = 3;
                pr.IsSync = false;

                entity.Entry(pr).State = EntityState.Modified;
                entity.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [AccessChecker(Action = 5, ModuleID = 3)]
        [HttpPost]
        public ActionResult ForceClosed(int id)
        {
            try
            {
                // TODO: Add delete logic here
                var pr = entity.Requisitions.Find(id);
                pr.ApprovalStatus = 5;
                pr.IsSync = false;

                entity.Entry(pr).State = EntityState.Modified;
                entity.SaveChanges();

                var rd = pr.RequisitionDetails.Where(p=>p.Quantity > 0).Select(p => p.ItemID).ToList();
                var item = entity.Items.Where(i => rd.Contains(i.ID)).Select(i => i.Code);
                var inv = entity.Inventories.Where(i => item.Contains(i.ItemCode) && i.LocationCode == pr.LocationID).ToList();
                if (inv != null)
                {
                    foreach (var _inv in inv)
                    {
                        var i = entity.Inventories.Find(_inv.ID);
                        i.Committed = i.Committed - invRepo.getCommited_ForceClose(id,_inv.ItemID.Value);
                        i.Ordered = i.Ordered - invRepo.getPurchaseOrder_ForceClose(id, _inv.ItemID.Value);
                        i.InStock = invRepo.getInstocked(pr.ID, _inv.ItemCode);
                        i.Available = (i.InStock + i.Ordered) - i.Committed;

                        entity.Entry(i).State = EntityState.Modified;
                        entity.SaveChanges();

                    }

                }
                   

             return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: PR/Items/5
        [AccessChecker(Action = 1, ModuleID = 3)]
        public ActionResult Items(int id, int? page)
        {
            var items = entity.RequisitionDetails
                        .ToList()
                        .FindAll(rd => rd.RequisitionID == id);

            ViewBag.PRid = id;

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        // GET: PR/ApprovedItems/5
        [AccessChecker(Action = 1, ModuleID = 3)]
        public ActionResult ApprovedItems(int id, int? page)
        {
            var items = entity.RequisitionDetails
                        .ToList()
                        .FindAll(rd => rd.RequisitionID == id && rd.AprovalStatusID == 2);

            ViewBag.PRid = id;

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        // GET: PR/PendingItems/5
        [AccessChecker(Action = 1, ModuleID = 3)]
        public ActionResult PendingItems(int id, int? page)
        {
            int UserID = Convert.ToInt32(Session["sessionuid"]);
            int UserType = Convert.ToInt32(Session["usertype"]);
            ViewBag.ReqTypeID =Session["ReqTypeID"].ToString();

            var requisition = entity.Requisitions.FirstOrDefault(r => r.ID == id);
            //if(requisition.RequestedBy != UserID && UserType != 1 && UserType != 4)
            //{
            //    return RedirectToAction("Index", "Home");
            //}

            var items = entity.RequisitionDetails
                        .ToList()
                        .FindAll(rd => rd.RequisitionID == id && rd.AprovalStatusID == 1);

            var reqdetails = entity.RequisitionDetails.FirstOrDefault(p => p.RequisitionID == id && p.AprovalStatusID == 2);
            //var items = entity.RequisitionDetails
            //            .ToList()
            //            .FindAll(rd => rd.RequisitionID == id && rd.AprovalStatusID == 1 && rd.Requisition.RequestedBy == UserID);

            ViewBag.PRid = id;
            try
            {
                ViewBag.Approved = reqdetails.AprovalStatusID.ToString();
            }
            catch { ViewBag.Approved = 1; }
            // ViewBag.RequestedBy = 
            ViewBag.UserID = UserID;
            ViewBag.AcctType = UserType;
            ViewBag.IsApproved = requisition.ApprovalStatus;

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);
            return PartialView(items.ToPagedList(pageNumber, pageSize));
        }

        // GET: PR/DeniedItems/5
        [AccessChecker(Action = 1, ModuleID = 3)]
        public ActionResult DeniedItems(int id, int? page)
        {
            var items = entity.RequisitionDetails
                        .ToList()
                        .FindAll(rd => rd.RequisitionID == id && rd.AprovalStatusID == 3);

            ViewBag.PRid = id;

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        // POST: PR/ApproveItem/5
        [AccessChecker(Action = 1, ModuleID = 3)]
        public ActionResult ApproveItem(int id, int itemID)
        {
            try
            {
                var item = entity.RequisitionDetails.Find(itemID);
                var inventory = entity.Inventories.FirstOrDefault(x => x.Description == item.Item.Description);

                var quantity = item.Quantity;

                if(item != null)
                {
                    item.AprovalStatusID = 2;
                    item.IsSync = false;
                    if (item.Requisition.ReqTypeID != 2)
                    {
                        item.Committed = Convert.ToInt32(getCommited(item.ItemID) + item.Committed);
                    }
                    item.Ordered = getPurchaseOrder(itemID) + item.Quantity;
                    int avail = (Convert.ToInt32(item.InStock) + Convert.ToInt32(item.Ordered)) - Convert.ToInt32(item.Committed);
                    item.Available = avail;
                    //item.InStock -= item.Quantity;
                    entity.Entry(item).State = EntityState.Modified;
                    //entity.SaveChanges();
                }
                //if (inventory != null)
                //{
                //    if (item.Requisition.ReqTypeID == 1)
                //    {
                //        inventory.Ordered = Convert.ToInt32(getPurchaseOrder(item.ItemID) + item.Ordered);
                //    }
                //    inventory.Committed = inventory.Committed.Value + quantity;
                //    entity.Entry(inventory).State = EntityState.Modified;
                //}
                entity.SaveChanges();
            }
            catch
            {

            }
            return RedirectToAction("PendingItems", "PR", new { id = id });
        }

        // POST: PR/DenyItem/5
        [AccessChecker(Action = 1, ModuleID = 3)]
        public ActionResult DenyItem(int id, int itemID)
        {
            try
            {
                var item = entity.RequisitionDetails.Find(itemID);
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
            return RedirectToAction("PendingItems", "PR", new { id = id });
        }

        #endregion

        #region PARTIAL

        // GET: PR/DenyItemPartial/5
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
                    var req = entity.Requisitions.Find(id);
                    req.ApprovalStatus = 3;
                    req.Remarks = Reason;
                    entity.Entry(req).State = EntityState.Modified;

                    entity.SaveChanges();


                    var reqdetails = entity.RequisitionDetails.Where(i => i.RequisitionID == id).ToList();
                    if (reqdetails != null)
                    {
                        foreach (var _rd in reqdetails)
                        {
                            var rd = entity.RequisitionDetails.Find(_rd.ID);
                            rd.AprovalStatusID = 3;

                            entity.Entry(rd).State = EntityState.Modified;
                            entity.SaveChanges();
                        }

                    }

                }
                else
                {
                    TempData["Error"] = "Reason is required";
                }
            }
            catch { TempData["Error"] = "There's an error"; }

            return RedirectToAction("Index");
        }


        // GET: PR/AddItemPartial/5
        public ActionResult AddItemPartial(int id)
        {
            var _rd = entity.Requisitions.Find(id);

            ViewBag.ReqTypeID = _rd.ReqTypeID.Value.ToString();
            ViewBag.PRid = id;
            ViewBag.ItemID = new SelectList(entity.Items, "ID", "Description");
            ViewBag.VendorID = _rd.VendorID != null ? _rd.VendorID : 0;

            return PartialView();
        }

        // POST: PR/AddItemPartial/5
        [HttpPost]
        public ActionResult AddItemPartial(int id, RequisitionDetail rd)
        {
            try
            {
                var req = entity.Requisitions.Find(id);
                var itm = entity.Items.FirstOrDefault(x => x.ID == rd.ItemID);
                var desc = itm.Description;
                // TODO: Add insert logic here
                rd.RequisitionID = id;
                rd.AprovalStatusID = 1; //submitted

                /*rd.Committed = getCommited(rd.ItemID) + rd.Quantity;*/ // COMMENT COMMENT dnagdgan ko + quantity kasi wala syang nkkuha kasi dpa nag ssave yung quantity haha gets mo
                rd.ReferenceQuantity = rd.Quantity;
                //rd.Committed = reqDetailRepo.getCommited(id, rd.ItemID); //getCommited(rd.ItemID);
                //
                //rd.Ordered = reqDetailRepo.getPurchaseOrder(req.LocationID.Value,rd.ItemID); //getPurchaseOrder(rd.ItemID); // di ko gets msyado ordered so ikaw na bahala haha
                //
                //rd.InStock = reqDetailRepo.getInstocked(id, desc); //getInstocked(desc);
                //
                //rd.Available = rd.InStock + rd.Ordered - rd.Committed;

                var rd1 = entity.RequisitionDetails.Where(r => r.RequisitionID == rd.RequisitionID && r.ItemID == rd.ItemID).ToList();

                if (rd1.Count() > 0)
                {
                    TempData["PartialError"] = "Item is already in the list.";
                }
                else
                {
                    rd.IsSync = false;
                    entity.RequisitionDetails.Add(rd);
                    entity.SaveChanges();
                }
            }
            catch(Exception e)
            {
                TempData["PartialError"] = "There's an error.";
            }

            //ViewBag.PRid = id;
            //ViewBag.ItemID = new SelectList(entity.Items, "ID", "Description", rd.ItemID);
            //ViewBag.AprovalStatusID = new SelectList(entity.ApprovalStatus, "ID", "Status", rd.AprovalStatusID);

            //return RedirectToAction("PendingItems", new { id = id });
            return RedirectToAction("Details", new { id = id });
        }

        // GET: PR/EditItemPartial/5
        public ActionResult EditItemPartial(int id)
        {
            var rd = entity.RequisitionDetails.Find(id);

            ViewBag.AprovalStatusID = new SelectList(entity.ApprovalStatus, "ID", "Status", rd.AprovalStatusID);
            ViewBag.ItemID = new SelectList(entity.Items, "ID", "Description", rd.ItemID);
            ViewBag.ItemCode = new SelectList(entity.Items, "ID", "Code", rd.ItemCode);

            return PartialView(rd);
        }

        // POST: PR/EditItemPartial/5
        [HttpPost]
        public ActionResult EditItemPartial(int id, RequisitionDetail rd)
        {
            try
            {
                //rd.IsSync = false;
                //Robi
                var prvrequiDetail = entity.RequisitionDetails.Find(rd.ID);
                var itm = entity.Items.Find(rd.ItemID);
                if (prvrequiDetail.ItemID != rd.ItemID || prvrequiDetail.Quantity != rd.Quantity)
                {
                    rd.PreviousItemID = prvrequiDetail.ItemID;
                    rd.PreviousQuantity = prvrequiDetail.Quantity;
                    //rd.Committed = reqDetailRepo.getCommited(id, rd.ItemID);
                    //rd.Ordered = reqDetailRepo.getPurchaseOrder(prvrequiDetail.Requisition.LocationID.Value, rd.ItemID);
                    //rd.InStock = reqDetailRepo.getInstocked(id, itm.Description);
                    //
                    //rd.Available = (rd.InStock + rd.Ordered) - rd.Committed;
                    rd.Remarks = rd.Remarks;
                }
                else
                {
                   // rd.Committed = reqDetailRepo.getCommited(id, rd.ItemID);
                   // rd.Ordered = reqDetailRepo.getPurchaseOrder(prvrequiDetail.Requisition.LocationID.Value, rd.ItemID);
                   // rd.InStock = reqDetailRepo.getInstocked(id, itm.Description);
                   // rd.Available = (rd.InStock + rd.Ordered) - rd.Committed;

                    rd.Remarks = rd.Remarks;
                    rd.PreviousQuantity = prvrequiDetail.Quantity;
                    rd.PreviousItemID = prvrequiDetail.PreviousItemID;
                }

                prvrequiDetail.ItemID = rd.ItemID;
                prvrequiDetail.Quantity = rd.Quantity;
                prvrequiDetail.ReferenceQuantity = rd.Quantity;
                prvrequiDetail.InStock = rd.InStock;
                prvrequiDetail.Ordered = rd.Ordered;
                prvrequiDetail.Committed = rd.Committed;
                prvrequiDetail.Available = rd.Available;
                prvrequiDetail.Remarks = rd.Remarks;
                prvrequiDetail.PreviousItemID = rd.PreviousItemID;
                prvrequiDetail.PreviousQuantity = rd.PreviousQuantity;

                prvrequiDetail.IsSync = false;
                entity.Entry(prvrequiDetail).CurrentValues.SetValues(rd);
                entity.SaveChanges();

                var _req = entity.RequisitionDetails.Find(rd.ID);
                _req.ReferenceQuantity = rd.Quantity;
                entity.SaveChanges();

            }
            catch
            {
                //throw;
                TempData["PartialError"] = "There's an error.";
            }

            if (rd.AprovalStatusID == 1)
            {
                //return RedirectToAction("PendingItems", new { id = rd.RequisitionID });
                return RedirectToAction("Details", new { id = rd.RequisitionID });
            }
            return RedirectToAction("ApprovedItems", new { id = rd.RequisitionID });
        }

        // GET: PR/DeleteItemPartial/5
        public ActionResult DeleteItemPartial(int id)
        {
            var rd = entity.RequisitionDetails.Find(id);

            return PartialView(rd);
        }

        // POST: PR/DeleteItemPartial/5
        [HttpPost, ActionName("DeleteItemPartial")]
        public ActionResult DeleteItemPartialConfirm(int id)
        {
            var rd = entity.RequisitionDetails.Find(id);

            int? reqID = rd.RequisitionID;
            try
            {
                entity.RequisitionDetails.Remove(rd);
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
