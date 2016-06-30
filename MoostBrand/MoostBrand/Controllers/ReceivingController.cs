using MoostBrand.DAL;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Configuration;
using PagedList;

namespace MoostBrand.Controllers
{
    public class ReceivingController : Controller
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
            else if (r.ReceivingTypeID == 2 || r.ReceivingTypeID == 3 || r.ReceivingTypeID == 3) //BR or WR or OR
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
        #endregion

        public ActionResult GenerateRefNumber(string id)
        {
            return Json(Generator(id), JsonRequestBehavior.AllowGet);
        }

        // GET: Receiving
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
            return View(rrs.ToPagedList(pageNumber, pageSize));
        }


        // GET: Receiving/GetStockTransfers/5
        public ActionResult GetStockTransfers(int id) //returns Json
        {
            var sts = entity.StockTransfers
                     .Include(st => st.Requisition)
                     .ToList()
                     .FindAll(st => st.Requisition.RequisitionTypeID == id)
                     .Select(st => new
                     {
                         ID = st.ID,
                         TransferID = st.TransferID
                     });

            return Json(sts, JsonRequestBehavior.AllowGet);
        }


        // GET: Receiving/GetRequisition/5
        public ActionResult GetRequisition(int id) //returns Json
        {
            var req = entity.StockTransfers
                     .Include(st => st.Requisition)
                     .Where(st => st.ID == id)
                     .Select(st => new
                     {
                         ID = st.RequisitionID,
                         RefNumber = st.Requisition.RefNumber,
                         RequestedBy = st.Requisition.Employee1.FirstName + " " + st.Requisition.Employee1.LastName,
                         Destination = st.Requisition.Location1.Description,
                         SourceLoc = st.Requisition.Location.Description,
                         Vendor = st.Requisition.Vendor.Name,
                         VendorCode = st.Requisition.Vendor.Code,
                         VendorContact = st.Requisition.Vendor.Attn,
                         CustName = st.Requisition.Customer,
                         ShipmentType = st.Requisition.ShipmentType.Type,
                         Invoice = st.Requisition.InvoiceNumber
                     })
                     .FirstOrDefault();

            return Json(req, JsonRequestBehavior.AllowGet);
        }

        // GET: Receiving/Create/
        public ActionResult Create()
        {
            var receiving = new Receiving();
            receiving.ReceivingDate = DateTime.Now;
            receiving.PONumber = POGenerator();

            #region DROPDOWNS
            ViewBag.ReceivingTypeID = new SelectList(entity.ReceivingTypes, "ID", "Type");
            ViewBag.LocationID = new SelectList(entity.Locations, "ID", "Description");
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

                        if(newR != null)
                        {
                            entity.Receivings.Add(newR);
                            entity.SaveChanges();

                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "The Receiving ID already exists.");
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "There's an error.");
                }
            }

            #region DROPDOWNS
            ViewBag.ReceivingTypeID = new SelectList(entity.ReceivingTypes, "ID", "Type", receiving.ReceivingTypeID);
            ViewBag.LocationID = new SelectList(entity.Locations, "ID", "Description", receiving.LocationID);
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
        public ActionResult Edit(int id = 0)
        {
            var receiving = entity.Receivings.Find(id);

            if(receiving != null)
            {
                return HttpNotFound();
            }

            #region DROPDOWNS
            
            ViewBag.LocationID = new SelectList(entity.Locations, "ID", "Description", receiving.LocationID);
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
        



        // GET: Receiving/Items/5
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

        // GET: Receiving/GetRequisitionDetail/5
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
    }
}