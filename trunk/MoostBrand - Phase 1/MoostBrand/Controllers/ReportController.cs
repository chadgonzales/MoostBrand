using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MoostBrand.DAL;
using PagedList;
using System.Linq.Dynamic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Web.Mvc.Html;
using Microsoft.Reporting.WebForms;

namespace MoostBrand.Controllers
{
    public class ReportController : Controller
    {
        MoostBrandEntities entity = new MoostBrandEntities();
        InventoryRepository invRepo = new InventoryRepository();
        // GET: Report
        public ActionResult SummaryReport(string dateFrom, string dateTo, int? brand, int? category, int? vendor, int? location)
        {

            var affectedRows1 = entity.Database.ExecuteSqlCommand("spUpdate_Inventory");
            #region DROPDOWNS
            var loc = entity.Locations.Where(x => x.ID != 10)
                            .Select(x => new
                            {
                                ID = x.ID,
                                Description = x.Description
                            });

            ViewBag.Brand = new SelectList(entity.Items.Select(p=>p.Brand).Distinct(), "ID", "Description");
            ViewBag.Category = new SelectList(entity.Items.Select(p => p.Category).Distinct(), "ID", "Description");
            ViewBag.Vendor = new SelectList(entity.Vendors.OrderBy(v=>v.GeneralName), "ID", "GeneralName");
            ViewBag.Location = new SelectList(loc, "ID", "Description");
            #endregion
            DateTime dtDateFrom = DateTime.Now.Date;
            DateTime dtDateTo = DateTime.Now;

            string _sortbybrand = "Brand: ALL", _sortbycategory = "Category: ALL", _sortbyvendor = "Vendor: ALL", _sortbylocation = "Location: ALL";
            if (!String.IsNullOrEmpty(dateFrom))
            {
                dtDateFrom = Convert.ToDateTime(dateFrom).Date;
            }

            if (!String.IsNullOrEmpty(dateTo))
            {
                dtDateTo = Convert.ToDateTime(dateTo);
            }

            var _lst = entity.Inventories.ToList();

            if(brand!=null)
            {
              //  var items = entity.Items.Where(p => p.BrandID == brand).Select(p => p.Code);
                _lst= _lst.Where(p => p.Items.BrandID == brand).ToList();
                string _brand = entity.Brands.Find(brand).Description;
                _sortbybrand = "Brand:" + _brand;
            }

            if (category != null)
            {
               // string _category = entity.Categories.Find(category).Description;
                _lst =_lst.Where(p => p.Items.CategoryID == category).ToList();
                string _category = entity.Categories.Find(category).Description;
                _sortbycategory = "Category:"+ _category;
            }

            if (vendor != null)
            {
                //  var items = entity.Items.Where(p => p.VendorCoding == vendor).Select(p => p.Code);
                _lst = _lst.Where(p => p.Items.VendorCoding == vendor && p.Items.VendorCoding != null).ToList();
                string _vendor = entity.Vendors.Find(vendor).Name;
                _sortbyvendor = "Vendor:"+ _vendor;
            }

            if (location != null)
            {
                _lst=_lst.Where(p => p.LocationCode == location).ToList();
                string _location = entity.Locations.Find(location).Description;
                _sortbylocation = "Location:"+_location;
              
            }

            var sl = entity.StockLedgers.Where(p => p.Date >= dtDateFrom).ToList();

            var lstInventory1 = (from p in sl.Where(p => p.Date <= dtDateTo.AddDays(1)).ToList()
                                 group p by new { p.InventoryID, p.Type } into g
                                 select new
                                 {
                                     ItemId = g.Key,
                                     InQty = g.Sum(p=>p.InQty),
                                     OutQty = g.Sum(p => p.OutQty),
                                     Variance = g.Sum(p => p.Variance)
                                 }).ToList();


            var lstInventory2 = (from p in sl.Where(p => p.Date <= dtDateTo.AddDays(1)).ToList()
                                 group p by new { p.InventoryID } into g
                                 select new
                                 {
                                     ItemId = g.Key,
                                     BeginningInstock = g.OrderBy(p => p.Date).ThenBy(p => p.ID).FirstOrDefault().BeginningBalance
                                 }).ToList();



            var lstInventory3 = (from p in entity.RequisitionDetails.Where(r=>r.Requisition.ReqTypeID == 2 && r.Requisition.RequisitionTypeID == 4 && r.Requisition.Customer != null).ToList()
                                 group p by new { p.Item.Code, p.Requisition.LocationID } into g
                                 select new
                                 {
                                     ItemId = g.Key,
                                   
                                    ReservationName = string.Join("\n", g.Where(p => p.Requisition.RequestedDate.Date >= dtDateFrom && p.Requisition.RequestedDate.Date <= dtDateTo && p.Requisition.ApprovalStatus == 2).Select(p => p.Requisition.Customer).ToArray())
                                 }).ToList();

            var lstInventory = (from i in _lst
                                select new
                                {
                                    ItemCode = i.ItemCode != null ? i.ItemCode : " ",
                                    ItemPurchaseDesc = i.SalesDescription != null ? i.SalesDescription : " ",
                                    ItemSalesDesc = i.Description != null ? i.Description : " ",
                                    UOM = i.InventoryUoM != null ? i.InventoryUoM : " ",
                                    Location = i.Location.Description != null ? i.Location.Description : " ",
                                    ReOrderLevel = i.ReOrder != null ? i.ReOrder : 0,
                                    InQty = lstInventory1.FirstOrDefault(p => p.ItemId.InventoryID == i.ID && p.ItemId.Type.ToLower() == "stock in") != null ? lstInventory1.FirstOrDefault(p => p.ItemId.InventoryID == i.ID && p.ItemId.Type.ToLower() == "stock in").InQty : 0,
                                    OutQty = lstInventory1.FirstOrDefault(p => p.ItemId.InventoryID == i.ID && p.ItemId.Type.ToLower() == "stock out") != null ? lstInventory1.FirstOrDefault(p => p.ItemId.InventoryID == i.ID && p.ItemId.Type.ToLower() == "stock out").OutQty : 0, //invRepo.getTotalStockTranfer(i.ItemCode,i.LocationCode.Value, dtDateFrom, dtDateTo),
                                    AdjustedQty = lstInventory1.FirstOrDefault(p => p.ItemId.InventoryID == i.ID && p.ItemId.Type.ToLower() == "variance") != null ? lstInventory1.FirstOrDefault(p => p.ItemId.InventoryID == i.ID && p.ItemId.Type.ToLower() == "variance").Variance : 0,
                                    BegInstock = lstInventory2.FirstOrDefault(p => p.ItemId.InventoryID == i.ID) != null ? lstInventory2.FirstOrDefault(p => p.ItemId.InventoryID == i.ID).BeginningInstock : 0,//invRepo.getTotalVariance(i.ID,i.LocationCode.Value, dtDateFrom, dtDateTo),
                                    CommittedQty = i.Committed != null || i.Committed > 0 ? i.Committed : 0,
                                    TotalOrder = i.Ordered != null ? i.Ordered : 0,
                                    ReservationName = lstInventory3.FirstOrDefault(p => p.ItemId.Code == i.ItemCode && p.ItemId.LocationID == i.LocationCode) != null ? lstInventory3.FirstOrDefault(p => p.ItemId.Code == i.ItemCode && p.ItemId.LocationID == i.LocationCode).ReservationName : " ",
                                    QOH = 0,
                                    PcsPerBox = i.Items.Quantity,
                                    Instock = i.InStock != null || i.InStock > 0 ? i.InStock : 0


                                }).ToList();

            var _lstInventory = (from i in lstInventory
                                 select new
                                {
                                    ItemCode = i.ItemCode,
                                    ItemPurchaseDesc = i.ItemPurchaseDesc,
                                    ItemSalesDesc = i.ItemSalesDesc,
                                    UOM = i.UOM,
                                    Location = i.Location,
                                    ReOrderLevel = i.ReOrderLevel,
                                    InQty = i.InQty,
                                    OutQty = i.OutQty,
                                    AdjustedQty = i.AdjustedQty,
                                    CommittedQty = i.CommittedQty,
                                    TotalOrder = i.TotalOrder,
                                    ReservationName = i.ReservationName,
                                    QOH = dtDateTo.Date.ToString() == DateTime.Now.Date.ToString() ? i.Instock : i.BegInstock + (i.InQty - i.OutQty) + i.AdjustedQty, //i.Instock,
                                    PcsPerBox = i.PcsPerBox
                                  

                                 }).ToList();



            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;

            ReportDataSource _rds = new ReportDataSource();
            _rds.Name = "dsInventory";
            _rds.Value = _lstInventory.Distinct().OrderBy(o => o.Location).ThenBy(o => o.ItemSalesDesc).Select(p => p);

            reportViewer.KeepSessionAlive = false;
            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Views\Report\rdlc\Summary.rdlc";

            List<ReportParameter> _parameter = new List<ReportParameter>();
            _parameter.Add(new ReportParameter("DateRange", dtDateFrom.ToString("MMMM dd, yyyy") + " - " + dtDateTo.ToString("MMMM dd, yyyy")));
            _parameter.Add(new ReportParameter("DateGenerated",DateTime.Now.ToString("h:mm tt")));
            _parameter.Add(new ReportParameter("SortByBrand", _sortbybrand));
            _parameter.Add(new ReportParameter("SortByCategory", _sortbycategory));
            _parameter.Add(new ReportParameter("SortByVendor", _sortbyvendor));
            _parameter.Add(new ReportParameter("SortByLocation", _sortbylocation));

            reportViewer.LocalReport.DataSources.Add(_rds);
            reportViewer.LocalReport.Refresh();
            reportViewer.LocalReport.SetParameters(_parameter);

            ViewBag.ReportViewer = reportViewer;

            ViewBag.DateFrom = dtDateFrom.ToString("MM/dd/yyyy");
            ViewBag.DateTo = dtDateTo.ToString("MM/dd/yyyy");

          //  ViewBag.CompanyName = companyRepo.GetById(Sessions.CompanyId.Value).Name;

            return View();
        }

        public ActionResult InventoryReport(int? brand, int? category, int? vendor, int? location)
        {

            var affectedRows1 = entity.Database.ExecuteSqlCommand("spUpdate_Inventory");
            #region DROPDOWNS
            var loc = entity.Locations.Where(x => x.ID != 10)
                            .Select(x => new
                            {
                                ID = x.ID,
                                Description = x.Description
                            });

            ViewBag.Brand = new SelectList(entity.Items.Select(p => p.Brand).Distinct(), "ID", "Description");
            ViewBag.Category = new SelectList(entity.Items.Select(p => p.Category).Distinct(), "ID", "Description");
            ViewBag.Vendor = new SelectList(entity.Vendors.OrderBy(v => v.GeneralName), "ID", "GeneralName");
            ViewBag.Location = new SelectList(loc, "ID", "Description");
            #endregion
            DateTime dtDateFrom = DateTime.Now.Date;
            DateTime dtDateTo = DateTime.Now;

            string _sortbybrand = "Brand: ALL", _sortbycategory = "Category: ALL", _sortbyvendor = "Vendor: ALL", _sortbylocation = "Location: ALL";

            var _lst = entity.Inventories.ToList();

            if (brand != null)
            {
                //  var items = entity.Items.Where(p => p.BrandID == brand).Select(p => p.Code);
                _lst = _lst.Where(p => p.Items.BrandID == brand).ToList();
                string _brand = entity.Brands.Find(brand).Description;
                _sortbybrand = "Brand:" + _brand;
            }

            if (category != null)
            {
                // string _category = entity.Categories.Find(category).Description;
                _lst = _lst.Where(p => p.Items.CategoryID == category).ToList();
                string _category = entity.Categories.Find(category).Description;
                _sortbycategory = "Category:" + _category;
            }

            if (vendor != null)
            {
                //  var items = entity.Items.Where(p => p.VendorCoding == vendor).Select(p => p.Code);
                _lst = _lst.Where(p => p.Items.VendorCoding == vendor && p.Items.VendorCoding != null).ToList();
                string _vendor = entity.Vendors.Find(vendor).Name;
                _sortbyvendor = "Vendor:" + _vendor;
            }

            if (location != null)
            {
                _lst = _lst.Where(p => p.LocationCode == location).ToList();
                string _location = entity.Locations.Find(location).Description;
                _sortbylocation = "Location:" + _location;

            }

            var sl = entity.StockLedgers.Where(p => p.Date >= dtDateFrom).ToList();

            var lstInventory = (from i in _lst
                                select new
                                {
                                    ItemCode = i.ItemCode != null ? i.ItemCode : " ",
                                    ItemPurchaseDesc = i.SalesDescription != null ? i.SalesDescription : " ",
                                    ItemSalesDesc = i.Description != null ? i.Description : " ",
                                    UOM = i.InventoryUoM != null ? i.InventoryUoM : " ",
                                    Location = i.Location.Description != null ? i.Location.Description : " ",
                                    ReOrderLevel = i.ReOrder != null ? i.ReOrder : 0,
                                    InQty = 0,
                                    OutQty = 0,
                                    AdjustedQty = 0,
                                    CommittedQty = i.Committed != null || i.Committed > 0 ? i.Committed : 0,
                                    TotalOrder = i.Ordered != null ? i.Ordered : 0,
                                    ReservationName = " ",
                                    QOH = 0,
                                    PcsPerBox = i.Items.Quantity,
                                    Instock =i.InStock != null ? i.InStock : 0


                                }).ToList();

            var _lstInventory = (from i in lstInventory
                                 select new
                                 {
                                     ItemCode = i.ItemCode,
                                     ItemPurchaseDesc = i.ItemPurchaseDesc,
                                     ItemSalesDesc = i.ItemSalesDesc,
                                     UOM = i.UOM,
                                     Location = i.Location,
                                     ReOrderLevel = i.ReOrderLevel,
                                     InQty = i.InQty,
                                     OutQty = i.OutQty,
                                     AdjustedQty = i.AdjustedQty,
                                     CommittedQty = i.CommittedQty,
                                     TotalOrder = i.TotalOrder,
                                     ReservationName = i.ReservationName,
                                     QOH = i.Instock,
                                     PcsPerBox = i.PcsPerBox


                                 }).ToList();



            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;

            ReportDataSource _rds = new ReportDataSource();
            _rds.Name = "dsInventory";
            _rds.Value = _lstInventory.Distinct().OrderBy(o => o.Location).ThenBy(o => o.ItemSalesDesc).Select(p => p);

            reportViewer.KeepSessionAlive = false;
            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Views\Report\rdlc\Inventory.rdlc";

            List<ReportParameter> _parameter = new List<ReportParameter>();
            _parameter.Add(new ReportParameter("DateRange", ""));
            _parameter.Add(new ReportParameter("DateGenerated", DateTime.Now.ToString("MM/dd/yyyy h:mm tt")));
            _parameter.Add(new ReportParameter("SortByBrand", _sortbybrand));
            _parameter.Add(new ReportParameter("SortByCategory", _sortbycategory));
            _parameter.Add(new ReportParameter("SortByVendor", _sortbyvendor));
            _parameter.Add(new ReportParameter("SortByLocation", _sortbylocation));

            reportViewer.LocalReport.DataSources.Add(_rds);
            reportViewer.LocalReport.Refresh();
            reportViewer.LocalReport.SetParameters(_parameter);

            ViewBag.ReportViewer = reportViewer;

            ViewBag.DateFrom = dtDateFrom.ToString("MM/dd/yyyy");
            ViewBag.DateTo = dtDateTo.ToString("MM/dd/yyyy");

            //  ViewBag.CompanyName = companyRepo.GetById(Sessions.CompanyId.Value).Name;

            return View();
        }

        public ActionResult ReservationReport(string dateFrom, string dateTo, int? brand, int? category, int? vendor, int? location)
        {

            var affectedRows1 = entity.Database.ExecuteSqlCommand("spUpdate_Inventory");
            #region DROPDOWNS
            var loc = entity.Locations.Where(x => x.ID != 10)
                            .Select(x => new
                            {
                                ID = x.ID,
                                Description = x.Description
                            });

            ViewBag.Brand = new SelectList(entity.Items.Select(p => p.Brand).Distinct(), "ID", "Description");
            ViewBag.Category = new SelectList(entity.Items.Select(p => p.Category).Distinct(), "ID", "Description");
            ViewBag.Vendor = new SelectList(entity.Vendors.OrderBy(v => v.GeneralName), "ID", "GeneralName");
            ViewBag.Location = new SelectList(loc, "ID", "Description");
            #endregion
            DateTime dtDateFrom = DateTime.Now.Date;
            DateTime dtDateTo = DateTime.Now;

            string _sortbybrand = "Brand: ALL", _sortbycategory = "Category: ALL", _sortbyvendor = "Vendor: ALL", _sortbylocation = "Location: ALL";
            if (!String.IsNullOrEmpty(dateFrom))
            {
                dtDateFrom = Convert.ToDateTime(dateFrom);
            }

            if (!String.IsNullOrEmpty(dateTo))
            {
                dtDateTo = Convert.ToDateTime(dateTo);
            }

            var _lst = entity.RequisitionDetails.Where(r=> r.Requisition.ReqTypeID == 2 && r.Requisition.RequisitionTypeID == 4 && r.Requisition.Customer != null).ToList();

            if (brand != null)
            {
                //  var items = entity.Items.Where(p => p.BrandID == brand).Select(p => p.Code);
                _lst = _lst.Where(p => p.Item.BrandID == brand).ToList();
                string _brand = entity.Brands.Find(brand).Description;
                _sortbybrand = "Brand:" + _brand;
            }

            if (category != null)
            {
                // string _category = entity.Categories.Find(category).Description;
                _lst = _lst.Where(p => p.Item.CategoryID == category).ToList();
                string _category = entity.Categories.Find(category).Description;
                _sortbycategory = "Category:" + _category;
            }

            if (vendor != null)
            {
                //  var items = entity.Items.Where(p => p.VendorCoding == vendor).Select(p => p.Code);
                _lst = _lst.Where(p => p.Item.VendorCoding == vendor && p.Item.VendorCoding != null).ToList();
                string _vendor = entity.Vendors.Find(vendor).Name;
                _sortbyvendor = "Vendor:" + _vendor;
            }

            if (location != null)
            {
                _lst = _lst.Where(p => p.Requisition.LocationID == location).ToList();
                string _location = entity.Locations.Find(location).Description;
                _sortbylocation = "Location:" + _location;

            }



            var lstReservation = (from i in _lst.Where(p=> p.Requisition.RequestedDate.Date >= dtDateFrom && p.Requisition.RequestedDate.Date <= dtDateTo).ToList()
                                select new
                                {
                                    DateCreated = i.Requisition.RequestedDate != null ? i.Requisition.RequestedDate.ToString() : " ",
                                    orderBydate = i.Requisition.RequestedDate,
                                    RefNumber = i.Requisition.RefNumber != null ? i.Requisition.RefNumber : " ",
                                    ItemCode = i.Item.Code != null ? i.Item.Code : " ",
                                    ItemSalesDesc = i.Item.DescriptionPurchase != null ? i.Item.DescriptionPurchase : " ",
                                    ReservationQty = i.ReferenceQuantity != null ? i.ReferenceQuantity : 0,
                                    QtyAfterReservation = " ",
                                    UOM = i.Item.UnitOfMeasurement.Description != null ? i.Item.UnitOfMeasurement.Description : " ",
                                    CustomerName = i.Requisition.Customer != null ? i.Requisition.Customer : " ", //invRepo.getTotalStockTranfer(i.ItemCode,i.LocationCode.Value, dtDateFrom, dtDateTo),
                                    DateNeeded = i.Requisition.DateRequired != null ? i.Requisition.DateRequired.ToString() : " ",//invRepo.getTotalVariance(i.ID,i.LocationCode.Value, dtDateFrom, dtDateTo),
                                    Status = i.Requisition.ReservationType.Type != null ? i.Requisition.ReservationType.Type : " "
                                  


                                }).ToList();

           


            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;

            ReportDataSource _rds = new ReportDataSource();
            _rds.Name = "dsReservation";
            _rds.Value = lstReservation.OrderBy(p => p.orderBydate);

            reportViewer.KeepSessionAlive = false;
            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Views\Report\rdlc\Reservation.rdlc";

            List<ReportParameter> _parameter = new List<ReportParameter>();
            _parameter.Add(new ReportParameter("DateRange", dtDateFrom.ToString("MMMM dd, yyyy") + " - " + dtDateTo.ToString("MMMM dd, yyyy")));
            _parameter.Add(new ReportParameter("SortByBrand", _sortbybrand));
            _parameter.Add(new ReportParameter("SortByCategory", _sortbycategory));
            _parameter.Add(new ReportParameter("SortByVendor", _sortbyvendor));
            _parameter.Add(new ReportParameter("SortByLocation", _sortbylocation));

            reportViewer.LocalReport.DataSources.Add(_rds);
            reportViewer.LocalReport.Refresh();
            reportViewer.LocalReport.SetParameters(_parameter);

            ViewBag.ReportViewer = reportViewer;

            ViewBag.DateFrom = dtDateFrom.ToString("MM/dd/yyyy");
            ViewBag.DateTo = dtDateTo.ToString("MM/dd/yyyy");

            //  ViewBag.CompanyName = companyRepo.GetById(Sessions.CompanyId.Value).Name;

            return View();
        }

        public ActionResult StockLedgerReport(string dateFrom, string dateTo,string itemcode, string itemdesc, int? location)
        {

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

            var loc = entity.Locations.Where(x => x.ID != 10)
                          .Select(x => new
                          {
                              ID = x.ID,
                              Description = x.Description
                          });

            ViewBag.ItemCode = new SelectList(entity.Inventories.Select(x => x.Items).Distinct(), "Code", "Code");
            ViewBag.ItemDesc = new SelectList(entity.Inventories.Select(x => x.Items).Distinct(), "Code", "Description");
            ViewBag.Location = new SelectList(loc, "ID", "Description");


            string _sortbycode = "Item Code: ALL", _sortbydesc = "Item Description: ALL", _sortbyLocation = "Location: ALL";


            var _lst = entity.StockLedgers.ToList();

            if (!String.IsNullOrEmpty(itemcode))
            {

                _lst = _lst.Where(p => p.Inventories.ItemCode.Trim() == itemcode.Trim()).ToList();
                string _Code = entity.Items.FirstOrDefault(p => p.Code == itemcode).Code;
                _sortbycode = "Item Code:" + _Code;
            }

            if (!String.IsNullOrEmpty(itemdesc))
            {

                _lst = _lst.Where(p => p.Inventories.ItemCode.Trim() == itemdesc.Trim()).ToList();
                string _desc = entity.Items.FirstOrDefault(p => p.Code.Trim() == itemdesc.Trim()).Description;
                _sortbydesc = "Item Description:" + _desc;
            }

            if (location != null)
            {
                _lst = _lst.Where(p => p.Inventories.LocationCode == location).ToList();
                string _location = entity.Locations.Find(location).Description;
                _sortbyLocation = "Location:" + _location;

            }

            var lstStockLedger = (from i in _lst.Where(p => p.Date.Value.Date >= dtDateFrom && p.Date.Value.Date <= dtDateTo).ToList()
                                  select new
                                {
                                    ItemCode = i.Inventories.ItemCode != null ? i.Inventories.ItemCode : " ",
                                    ItemDesc = i.Inventories.Items.Description != null ? i.Inventories.Items.Description : " ",
                                    Type = i.Type != null ? i.Type : " ",
                                    ReferenceNo = i.ReferenceNo != null ? i.ReferenceNo : " ",
                                    InQty = i.InQty != null ? i.InQty : 0,
                                    OutQty = i.OutQty != null ? i.OutQty : 0,
                                    RemainingBalance = i.RemainingBalance != null ? i.RemainingBalance : 0,
                                    BeginningBalance = i.BeginningBalance != null ? i.BeginningBalance : 0,
                                    Variance = i.Variance != null ? i.Variance : 0,
                                    Date = i.Date != null ? i.Date.Value.ToString() : "",
                                    orderBydate = i.Date.Value,
                                    Location = i.Inventories.LocationCode != null ? i.Inventories.Location.Description : " "

                                }).ToList();


            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;

            ReportDataSource _rds = new ReportDataSource();
            _rds.Name = "dsStockLedger";
            _rds.Value = lstStockLedger.Distinct().OrderBy(o => o.Location).ThenBy(o => o.orderBydate).ToList();

            reportViewer.KeepSessionAlive = false;
            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Views\Report\rdlc\StockLedger.rdlc";

            List<ReportParameter> _parameter = new List<ReportParameter>();
            _parameter.Add(new ReportParameter("DateRange", dtDateFrom.ToString("MMMM dd, yyyy") + " - " + dtDateTo.ToString("MMMM dd, yyyy")));
            _parameter.Add(new ReportParameter("SortByCode", _sortbycode));
            _parameter.Add(new ReportParameter("SortByDesc", _sortbydesc));
            _parameter.Add(new ReportParameter("SortByLoc", _sortbyLocation));
            reportViewer.LocalReport.DataSources.Add(_rds);
            reportViewer.LocalReport.Refresh();
            reportViewer.LocalReport.SetParameters(_parameter);

            ViewBag.ReportViewer = reportViewer;

            ViewBag.DateFrom = dtDateFrom.ToString("MM/dd/yyyy");
            ViewBag.DateTo = dtDateTo.ToString("MM/dd/yyyy");

            return View();
        }

        public ActionResult UnserveReport(string dateFrom, string dateTo, int? brand, int? category, int? vendor, int? location, string itemcode, string itemdesc)//
        {
            var affectedRows1 = entity.Database.ExecuteSqlCommand("spUpdate_Inventory");
            #region DROPDOWNS
            var loc = entity.Locations.Where(x => x.ID != 10)
                            .Select(x => new
                            {
                                ID = x.ID,
                                Description = x.Description
                            });

            ViewBag.Brand = new SelectList(entity.Items.Select(p => p.Brand).Distinct(), "ID", "Description");
            ViewBag.Category = new SelectList(entity.Items.Select(p => p.Category).Distinct(), "ID", "Description");
            ViewBag.Vendor = new SelectList(entity.Vendors.OrderBy(v => v.GeneralName), "ID", "GeneralName");
            ViewBag.Location = new SelectList(loc, "ID", "Description");
            ViewBag.ItemCode = new SelectList(entity.Inventories.Select(x => x.Items).Distinct(), "Code", "Code");
            ViewBag.ItemDesc = new SelectList(entity.Inventories.Select(x => x.Items).Distinct(), "Code", "Description");
            #endregion
            DateTime dtDateFrom = DateTime.Now.Date;
            DateTime dtDateTo = DateTime.Now;

            string _sortbybrand = "Brand: ALL", _sortbycategory = "Category: ALL", _sortbyvendor = "Vendor: ALL", _sortbylocation = "Location: ALL",
                   _sortbycode = "Item Code: ALL", _sortbydesc= "Item Description: ALL";

            if (!String.IsNullOrEmpty(dateFrom))
            {
                dtDateFrom = Convert.ToDateTime(dateFrom).Date;
            }

            if (!String.IsNullOrEmpty(dateTo))
            {
                dtDateTo = Convert.ToDateTime(dateTo);
            }

            var _lst = entity.RequisitionDetails.Where(r=>r.Requisition.ReqTypeID == 1 && r.Quantity > 0).ToList();

            _lst = _lst.Where(r => r.Requisition.RequestedDate >= dtDateFrom && r.Requisition.RequestedDate <= dtDateTo.AddDays(1)).ToList();

            if (!String.IsNullOrEmpty(itemcode))
            {

                _lst = _lst.Where(p => p.Item.Code.Trim() == itemcode.Trim()).ToList();
                string _Code = entity.Items.FirstOrDefault(p => p.Code == itemcode).Code;
                _sortbycode = "Item Code:" + _Code;
            }

            if (!String.IsNullOrEmpty(itemdesc))
            {

                _lst = _lst.Where(p => p.Item.Code.Trim() == itemdesc.Trim()).ToList();
                string _desc = entity.Items.FirstOrDefault(p => p.Code.Trim() == itemdesc.Trim()).Description;
                _sortbydesc = "Item Description:" + _desc;
            }

            if (brand != null)
            {               
                _lst = _lst.Where(p => p.Item.BrandID == brand).ToList();
                string _brand = entity.Brands.Find(brand).Description;
                _sortbybrand = "Brand:" + _brand;
            }

            if (category != null)
            {
                _lst = _lst.Where(p => p.Item.CategoryID == category).ToList();
                string _category = entity.Categories.Find(category).Description;
                _sortbycategory = "Category:" + _category;
            }

            if (vendor != null)
            {
                _lst = _lst.Where(p => p.Item.VendorCoding == vendor && p.Item.VendorCoding != null).ToList();
                string _vendor = entity.Vendors.Find(vendor).Name;
                _sortbyvendor = "Vendor:" + _vendor;
            }

            if (location != null)
            {
                _lst = _lst.Where(p => p.Requisition.LocationID == location).ToList();
                string _location = entity.Locations.Find(location).Description;
                _sortbylocation = "Location:" + _location;

            }

           
            var lstUnserve = (from i in _lst
                                select new
                                {
                                    DateRequested = i.Requisition.RequestedDate.ToString(),
                                    orderByDate = i.Requisition.RequestedDate,
                                    PO = i.Requisition.PONumber != null ? i.Requisition.PONumber : "" ,
                                    ItemCode = i.Item.Code,
                                    ItemPurDesc =  i.Item.Description,
                                    ItemSalesDesc = i.Item.DescriptionPurchase,
                                    UOM =  i.Item.UnitOfMeasurement.Description,
                                    Quantity = i.Quantity,
                                    Location = i.Requisition.Location.Description,
                                    ReOrderLevel =  entity.Inventories.FirstOrDefault(inv=>inv.ItemID == i.ItemID && inv.LocationCode == i.Requisition.LocationID) != null ?
                                                    (entity.Inventories.FirstOrDefault(inv => inv.ItemID == i.ItemID && inv.LocationCode == i.Requisition.LocationID).ReOrder != null ?
                                                    entity.Inventories.FirstOrDefault(inv => inv.ItemID == i.ItemID && inv.LocationCode == i.Requisition.LocationID).ReOrder : 0) : 0 , //from inventory
                                    StatusOfOrder = ""

                                }).ToList();

   

            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;

            ReportDataSource _rds = new ReportDataSource();
            _rds.Name = "dsUnserve";
            _rds.Value = lstUnserve.OrderBy(p => p.orderByDate);

            reportViewer.KeepSessionAlive = false;
            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Views\Report\rdlc\Unserve.rdlc";

            List<ReportParameter> _parameter = new List<ReportParameter>();
            _parameter.Add(new ReportParameter("DateRange", dtDateFrom.ToString("MMMM dd, yyyy") + " - " + dtDateTo.ToString("MMMM dd, yyyy")));
            _parameter.Add(new ReportParameter("DateGenerated", DateTime.Now.ToString("h:mm tt")));
            _parameter.Add(new ReportParameter("SortByBrand", _sortbybrand));
            _parameter.Add(new ReportParameter("SortByCategory", _sortbycategory));
            _parameter.Add(new ReportParameter("SortByVendor", _sortbyvendor));
            _parameter.Add(new ReportParameter("SortByLocation", _sortbylocation));
            _parameter.Add(new ReportParameter("SortByItemCode", _sortbycode));
            _parameter.Add(new ReportParameter("SortByItemDesc", _sortbydesc));

            reportViewer.LocalReport.DataSources.Add(_rds);
            reportViewer.LocalReport.Refresh();
            reportViewer.LocalReport.SetParameters(_parameter);

            ViewBag.ReportViewer = reportViewer;

            ViewBag.DateFrom = dtDateFrom.ToString("MM/dd/yyyy");
            ViewBag.DateTo = dtDateTo.ToString("MM/dd/yyyy");

            return View();
        }

        public ActionResult StockReceivingReport(string dateFrom, string dateTo, int? vendor, int? location)
        {
            var affectedRows1 = entity.Database.ExecuteSqlCommand("spUpdate_Inventory");
            #region DROPDOWNS
            var loc = entity.Locations.Where(x => x.ID != 10)
                            .Select(x => new
                            {
                                ID = x.ID,
                                Description = x.Description
                            });

           
            ViewBag.Vendor = new SelectList(entity.Vendors.OrderBy(v => v.GeneralName), "ID", "GeneralName");
            ViewBag.Location = new SelectList(loc, "ID", "Description");
            #endregion
            DateTime dtDateFrom = DateTime.Now.Date;
            DateTime dtDateTo = DateTime.Now;

            string _sortbyvendor = "Vendor: ALL", _sortbylocation = "Location: ALL";
            if (!String.IsNullOrEmpty(dateFrom))
            {
                dtDateFrom = Convert.ToDateTime(dateFrom).Date;
            }

            if (!String.IsNullOrEmpty(dateTo))
            {
                dtDateTo = Convert.ToDateTime(dateTo);
            }

            var _lst = entity.ReceivingDetails.ToList();

            _lst = _lst.Where(r => r.Receiving.ReceivingDate >= dtDateFrom && r.Receiving.ReceivingDate <= dtDateTo.AddDays(1)).ToList();


            if (vendor != null)
            {
                _lst = _lst.Where(p => p.RequisitionDetail.Item.VendorCoding == vendor && p.RequisitionDetail.Item.VendorCoding != null).ToList();
                string _vendor = entity.Vendors.Find(vendor).Name;
                _sortbyvendor = "Vendor:" + _vendor;
            }

            if (location != null)
            {
               
                string _location = entity.Locations.Find(location).Description;
                _lst = _lst.Where(p => p.Receiving.GetLocation == _location).ToList();
                _sortbylocation = "Location:" + _location;

            }


            var lstStockReceiving = (from i in _lst
                              select new
                              {
                                  DateCreated = i.Receiving.Requisition.RequestedDate.ToString(),
                                  orderByDate = i.Receiving.Requisition.RequestedDate,
                                  SRR = i.Receiving.ReceivingID != null ? i.Receiving.ReceivingID : "",
                                  PO = i.RequisitionDetail.Requisition.PONumber,
                                  ItemCode = i.RequisitionDetail.Item.Code,
                                  ItemPurDesc = i.RequisitionDetail.Item.Description,
                                  ItemSalesDesc = i.RequisitionDetail.Item.DescriptionPurchase,
                                  UOM = i.RequisitionDetail.Item.UnitOfMeasurement.Description,
                                  Quantity = i.ReferenceQuantity,
                                  Location = i.Receiving.GetLocation,
                                  ReceivedBy = i.Receiving.Employee2.FirstName+' '+i.Receiving.Employee2.LastName,
                                  CreatedBy = i.Receiving.Requisition.Employee1.FirstName +' ' + i.Receiving.Requisition.Employee1.LastName

                              }).ToList();



            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;

            ReportDataSource _rds = new ReportDataSource();
            _rds.Name = "dsStockReceiving";
            _rds.Value = lstStockReceiving.OrderBy(p => p.orderByDate);

            reportViewer.KeepSessionAlive = false;
            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Views\Report\rdlc\StockReceiving.rdlc";

            List<ReportParameter> _parameter = new List<ReportParameter>();
            _parameter.Add(new ReportParameter("DateRange", dtDateFrom.ToString("MMMM dd, yyyy") + " - " + dtDateTo.ToString("MMMM dd, yyyy")));
            _parameter.Add(new ReportParameter("DateGenerated", DateTime.Now.ToString("h:mm tt")));
            _parameter.Add(new ReportParameter("SortByVendor", _sortbyvendor));
            _parameter.Add(new ReportParameter("SortByLocation", _sortbylocation));

            reportViewer.LocalReport.DataSources.Add(_rds);
            reportViewer.LocalReport.Refresh();
            reportViewer.LocalReport.SetParameters(_parameter);

            ViewBag.ReportViewer = reportViewer;

            ViewBag.DateFrom = dtDateFrom.ToString("MM/dd/yyyy");
            ViewBag.DateTo = dtDateTo.ToString("MM/dd/yyyy");

            return View();
        }

        public ActionResult NonMovingItemsReport(string dateFrom, string dateTo, int? brand, int? category, int? vendor, int? location)
        {

            var affectedRows1 = entity.Database.ExecuteSqlCommand("spUpdate_Inventory");
            #region DROPDOWNS
            var loc = entity.Locations.Where(x => x.ID != 10)
                            .Select(x => new
                            {
                                ID = x.ID,
                                Description = x.Description
                            });

            ViewBag.Brand = new SelectList(entity.Items.Select(p => p.Brand).Distinct(), "ID", "Description");
            ViewBag.Category = new SelectList(entity.Items.Select(p => p.Category).Distinct(), "ID", "Description");
            ViewBag.Vendor = new SelectList(entity.Vendors.OrderBy(v => v.GeneralName), "ID", "GeneralName");
            ViewBag.Location = new SelectList(loc, "ID", "Description");
            #endregion
            DateTime dtDateFrom = DateTime.Now.Date;
            DateTime dtDateTo = DateTime.Now;

            string _sortbybrand = "Brand: ALL", _sortbycategory = "Category: ALL", _sortbyvendor = "Vendor: ALL", _sortbylocation = "Location: ALL";
            if (!String.IsNullOrEmpty(dateFrom))
            {
                dtDateFrom = Convert.ToDateTime(dateFrom).Date;
            }

            if (!String.IsNullOrEmpty(dateTo))
            {
                dtDateTo = Convert.ToDateTime(dateTo);
            }

            var _lst = entity.Inventories.ToList();

            if (brand != null)
            {
                _lst = _lst.Where(p => p.Items.BrandID == brand).ToList();
                string _brand = entity.Brands.Find(brand).Description;
                _sortbybrand = "Brand:" + _brand;
            }

            if (category != null)
            {
                _lst = _lst.Where(p => p.Items.CategoryID == category).ToList();
                string _category = entity.Categories.Find(category).Description;
                _sortbycategory = "Category:" + _category;
            }

            if (vendor != null)
            {
                _lst = _lst.Where(p => p.Items.VendorCoding == vendor && p.Items.VendorCoding != null).ToList();
                string _vendor = entity.Vendors.Find(vendor).Name;
                _sortbyvendor = "Vendor:" + _vendor;
            }

            if (location != null)
            {
                _lst = _lst.Where(p => p.LocationCode == location).ToList();
                string _location = entity.Locations.Find(location).Description;
                _sortbylocation = "Location:" + _location;

            }

            var sl = entity.StockLedgers.Where(p => p.Date >= dtDateFrom).ToList();

            var lstInventory1 = (from p in sl.Where(p => p.Date <= dtDateTo.AddDays(1)).ToList()
                                 group p by new { p.InventoryID, p.Type } into g
                                 select new
                                 {
                                     ItemId = g.Key,
                                     InQty = g.Sum(p => p.InQty),
                                     OutQty = g.Sum(p => p.OutQty),
                                     Variance = g.Sum(p => p.Variance)
                                 }).ToList();


            var lstInventory3 = (from p in entity.RequisitionDetails.Where(r => r.Requisition.ReqTypeID == 2 && r.Requisition.RequisitionTypeID == 4 && r.Requisition.Customer != null).ToList()
                                 group p by new { p.Item.Code, p.Requisition.LocationID } into g
                                 select new
                                 {
                                     ItemId = g.Key,

                                     ReservationName = string.Join("\n", g.Where(p => p.Requisition.RequestedDate.Date >= dtDateFrom && p.Requisition.RequestedDate.Date <= dtDateTo && p.Requisition.ApprovalStatus == 2).Select(p => p.Requisition.Customer).ToArray())
                                 }).ToList();

            var lstInventory = (from i in _lst
                                select new
                                {
                                    ItemCode = i.ItemCode != null ? i.ItemCode : " ",
                                    ItemPurchaseDesc = i.SalesDescription != null ? i.SalesDescription : " ",
                                    ItemSalesDesc = i.Description != null ? i.Description : " ",
                                    UOM = i.InventoryUoM != null ? i.InventoryUoM : " ",
                                    Location = i.Location.Description != null ? i.Location.Description : " ",
                                    ReOrderLevel = i.ReOrder != null ? i.ReOrder : 0,
                                    InQty = lstInventory1.FirstOrDefault(p => p.ItemId.InventoryID == i.ID && p.ItemId.Type.ToLower() == "stock in") != null ? lstInventory1.FirstOrDefault(p => p.ItemId.InventoryID == i.ID && p.ItemId.Type.ToLower() == "stock in").InQty : 0,
                                    OutQty = lstInventory1.FirstOrDefault(p => p.ItemId.InventoryID == i.ID && p.ItemId.Type.ToLower() == "stock out") != null ? lstInventory1.FirstOrDefault(p => p.ItemId.InventoryID == i.ID && p.ItemId.Type.ToLower() == "stock out").OutQty : 0, //invRepo.getTotalStockTranfer(i.ItemCode,i.LocationCode.Value, dtDateFrom, dtDateTo),
                                    AdjustedQty = lstInventory1.FirstOrDefault(p => p.ItemId.InventoryID == i.ID && p.ItemId.Type.ToLower() == "variance") != null ? lstInventory1.FirstOrDefault(p => p.ItemId.InventoryID == i.ID && p.ItemId.Type.ToLower() == "variance").Variance : 0,//invRepo.getTotalVariance(i.ID,i.LocationCode.Value, dtDateFrom, dtDateTo),
                                    CommittedQty = i.Committed != null || i.Committed > 0 ? i.Committed : 0,
                                    TotalOrder = i.Ordered != null ? i.Ordered : 0,
                                    ReservationName = lstInventory3.FirstOrDefault(p => p.ItemId.Code == i.ItemCode && p.ItemId.LocationID == i.LocationCode) != null ? lstInventory3.FirstOrDefault(p => p.ItemId.Code == i.ItemCode && p.ItemId.LocationID == i.LocationCode).ReservationName : " ",
                                    QOH = 0,
                                    PcsPerBox = i.Items.Quantity,
                                    Instock = i.InStock != null ? i.InStock : 0,


                                }).ToList();

            var _lstInventory = (from i in lstInventory
                                 select new
                                 {
                                     ItemCode = i.ItemCode,
                                     ItemPurchaseDesc = i.ItemPurchaseDesc,
                                     ItemSalesDesc = i.ItemSalesDesc,
                                     UOM = i.UOM,
                                     Location = i.Location,
                                     ReOrderLevel = i.ReOrderLevel,
                                     InQty = i.InQty,
                                     OutQty = i.OutQty,
                                     AdjustedQty = i.AdjustedQty,
                                     CommittedQty = i.CommittedQty,
                                     TotalOrder = i.TotalOrder,
                                     ReservationName = i.ReservationName,
                                     QOH = i.Instock, // - i.OutQty - i.CommittedQty + i.AdjustedQty
                                     PcsPerBox = i.PcsPerBox


                                 }).ToList();



            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;

            ReportDataSource _rds = new ReportDataSource();
            _rds.Name = "dsNonMovingItem";
            _rds.Value = _lstInventory.OrderBy(p => p.ItemSalesDesc);

            reportViewer.KeepSessionAlive = false;
            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Views\Report\rdlc\Inventory.rdlc";

            List<ReportParameter> _parameter = new List<ReportParameter>();
            _parameter.Add(new ReportParameter("DateRange", dtDateFrom.ToString("MMMM dd, yyyy") + " - " + dtDateTo.ToString("MMMM dd, yyyy")));
            _parameter.Add(new ReportParameter("DateGenerated", DateTime.Now.ToString("h:mm tt")));
            _parameter.Add(new ReportParameter("SortByBrand", _sortbybrand));
            _parameter.Add(new ReportParameter("SortByCategory", _sortbycategory));
            _parameter.Add(new ReportParameter("SortByVendor", _sortbyvendor));
            _parameter.Add(new ReportParameter("SortByLocation", _sortbylocation));

            reportViewer.LocalReport.DataSources.Add(_rds);
            reportViewer.LocalReport.Refresh();
            reportViewer.LocalReport.SetParameters(_parameter);

            ViewBag.ReportViewer = reportViewer;

            ViewBag.DateFrom = dtDateFrom.ToString("MM/dd/yyyy");
            ViewBag.DateTo = dtDateTo.ToString("MM/dd/yyyy");

            //  ViewBag.CompanyName = companyRepo.GetById(Sessions.CompanyId.Value).Name;

            return View();
        }

        public ActionResult StockTransferReport(string dateFrom, string dateTo, int? vendor, int? location)
        {
            var affectedRows1 = entity.Database.ExecuteSqlCommand("spUpdate_Inventory");
            #region DROPDOWNS
            var loc = entity.Locations.Where(x => x.ID != 10)
                            .Select(x => new
                            {
                                ID = x.ID,
                                Description = x.Description
                            });


            ViewBag.Vendor = new SelectList(entity.Vendors.OrderBy(v => v.GeneralName), "ID", "GeneralName");
            ViewBag.Location = new SelectList(loc, "ID", "Description");
            #endregion
            DateTime dtDateFrom = DateTime.Now.Date;
            DateTime dtDateTo = DateTime.Now;

            string _sortbyvendor = "Vendor: ALL", _sortbylocation = "Location: ALL";
            if (!String.IsNullOrEmpty(dateFrom))
            {
                dtDateFrom = Convert.ToDateTime(dateFrom).Date;
            }

            if (!String.IsNullOrEmpty(dateTo))
            {
                dtDateTo = Convert.ToDateTime(dateTo);
            }

            var _lst = entity.StockTransferDetails.ToList();

            _lst = _lst.Where(r => r.StockTransfer.STDAte >= dtDateFrom && r.StockTransfer.STDAte <= dtDateTo.AddDays(1)).ToList();


            if (vendor != null)
            {
                try
                {
                    _lst = _lst.Where(p => (p.RequisitionDetail != null && p.RequisitionDetail.Item.VendorCoding == vendor && p.RequisitionDetail.Item.VendorCoding != null)
                           || (p.Inventories != null && p.Inventories.Items.VendorCoding == vendor && p.Inventories.Items.VendorCoding != null)).ToList();
                   
                }
                catch { }
                string _vendor = entity.Vendors.Find(vendor).Name;
                _sortbyvendor = "Vendor:" + _vendor;
            }

            if (location != null)
            {
                try
                {
                    _lst = _lst.Where(p => p.StockTransfer.Requisition.LocationID == location).ToList();                 
                }
                catch { }
                string _location = entity.Locations.Find(location).Description;
                _sortbylocation = "Location:" + _location;

            }


            var lstStockReceiving = (from i in _lst
                                     select new
                                     {

                                         DateCreated = i.StockTransfer.Requisition != null ? i.StockTransfer.Requisition.RequestedDate.ToString() : i.StockTransfer.STDAte.ToString(),
                                         orderbyDate = i.StockTransfer.Requisition != null ? i.StockTransfer.Requisition.RequestedDate : i.StockTransfer.STDAte,
                                         STR = i.StockTransfer.TransferID != null ? i.StockTransfer.TransferID : "",
                                         RequisitionID = i.RequisitionDetail != null ? i.RequisitionDetail.Requisition.RefNumber : "",
                                         ItemCode = i.RequisitionDetail != null? i.RequisitionDetail.Item.Code : "",
                                         ItemPurDesc = i.RequisitionDetail != null ? i.RequisitionDetail.Item.Description : "",
                                         ItemSalesDesc = i.RequisitionDetail != null ? i.RequisitionDetail.Item.DescriptionPurchase : "",
                                         UOM = i.RequisitionDetail !=null ? i.RequisitionDetail.Item.UnitOfMeasurement.Description : "",
                                         QtyOut = i.ReferenceQuantity,
                                         Location = i.StockTransfer.Requisition != null ? i.StockTransfer.Requisition.Location.Description : i.StockTransfer.Location.Description,
                                         ReceivedBy = i.StockTransfer.Employee3!=null ? i.StockTransfer.Employee3.FirstName + ' ' + i.StockTransfer.Employee3.LastName :"",
                                         CreatedBy = i.StockTransfer.Employee6 !=null ? i.StockTransfer.Employee6.FirstName + ' ' + i.StockTransfer.Employee6.LastName :""

                                     }).ToList();



            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;

            ReportDataSource _rds = new ReportDataSource();
            _rds.Name = "dsStockTransfer";
            _rds.Value = lstStockReceiving.OrderBy(p => p.orderbyDate);

            reportViewer.KeepSessionAlive = false;
            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Views\Report\rdlc\StockTransfer.rdlc";

            List<ReportParameter> _parameter = new List<ReportParameter>();
            _parameter.Add(new ReportParameter("DateRange", dtDateFrom.ToString("MMMM dd, yyyy") + " - " + dtDateTo.ToString("MMMM dd, yyyy")));
            _parameter.Add(new ReportParameter("DateGenerated", DateTime.Now.ToString("h:mm tt")));
            _parameter.Add(new ReportParameter("SortByVendor", _sortbyvendor));
            //_parameter.Add(new ReportParameter("SortByLocation", _sortbylocation));

            reportViewer.LocalReport.DataSources.Add(_rds);
            reportViewer.LocalReport.Refresh();
            reportViewer.LocalReport.SetParameters(_parameter);

            ViewBag.ReportViewer = reportViewer;

            ViewBag.DateFrom = dtDateFrom.ToString("MM/dd/yyyy");
            ViewBag.DateTo = dtDateTo.ToString("MM/dd/yyyy");

            return View();
        }

        public ActionResult DiscrepancyReport(string dateFrom, string dateTo, int? vendor, int? location)
        {
            var affectedRows1 = entity.Database.ExecuteSqlCommand("spUpdate_Inventory");
            #region DROPDOWNS
            var loc = entity.Locations.Where(x => x.ID != 10)
                            .Select(x => new
                            {
                                ID = x.ID,
                                Description = x.Description
                            });


            ViewBag.Vendor = new SelectList(entity.Vendors.OrderBy(v => v.GeneralName), "ID", "GeneralName");
            ViewBag.Location = new SelectList(loc, "ID", "Description");
            #endregion
            DateTime dtDateFrom = DateTime.Now.Date;
            DateTime dtDateTo = DateTime.Now;

            string _sortbyvendor = "Vendor: ALL", _sortbylocation = "Location: ALL";
            if (!String.IsNullOrEmpty(dateFrom))
            {
                dtDateFrom = Convert.ToDateTime(dateFrom).Date;
            }

            if (!String.IsNullOrEmpty(dateTo))
            {
                dtDateTo = Convert.ToDateTime(dateTo);
            }

            var _lstReq = entity.RequisitionDetails.Where(p => p.Requisition.ApprovalStatus == 5).ToList();

            _lstReq = _lstReq.Where(r => r.Requisition.RequestedDate >= dtDateFrom && r.Requisition.RequestedDate <= dtDateTo).ToList();


            //if (vendor != null)
            //{
            //    try
            //    {
            //        _lst = _lst.Where(p => (p.RequisitionDetail != null && p.RequisitionDetail.Item.VendorCoding == vendor && p.RequisitionDetail.Item.VendorCoding != null)
            //               || (p.Inventories != null && p.Inventories.Items.VendorCoding == vendor && p.Inventories.Items.VendorCoding != null)).ToList();

            //    }
            //    catch { }
            //    string _vendor = entity.Vendors.Find(vendor).Name;
            //    _sortbyvendor = "Vendor:" + _vendor;
            //}

            //if (location != null)
            //{
            //    try
            //    {
            //        _lst = _lst.Where(p => p.StockTransfer.Requisition.LocationID == location).ToList();
            //    }
            //    catch { }
            //    string _location = entity.Locations.Find(location).Description;
            //    _sortbylocation = "Location:" + _location;

            //}


            var lstDiscrepancy1 = (from req in _lstReq.Where(r=>r.Requisition.ReqTypeID == 2)
                                   join st in entity.StockTransferDetails.Where(r => r.StockTransfer.ApprovedStatus == 2 && r.RequisitionDetailID != null) on new { a1 =req.ID, b1 = req.Requisition.ID } equals new { a1 = st.RequisitionDetailID.Value, b1 = st.StockTransfer.RequisitionID.Value }
                                   join rec in entity.ReceivingDetails.Where(r => r.Receiving.ApprovalStatus == 2 && r.RequisitionDetailID != null) on new { a1 = req.ID, b1 = req.Requisition.ID } equals new { a1 = rec.RequisitionDetailID.Value, b1 = rec.Receiving.RequisitionID.Value }
                                   select new
                                   {
                                       ItemCode = req.Item.Code,
                                       brand = req.Item.Brand.Description,
                                       itemsalesdescription = req.Item.Description,
                                       uom = req.Item.UnitOfMeasurement != null ? req.Item.UnitOfMeasurement.Description : "",

                                       reqtype = req.Requisition.ReqType.Type,
                                       dateRequested = req.Requisition.RequestedDate,//req.Requisition.RequestedDate.ToString("MM/dd/yyyy"),
                                       orderbydate = req.Requisition.RequestedDate,
                                       reqno = req.Requisition.RefNumber,
                                       requestedby = req.Requisition.Employee1 != null ? req.Requisition.Employee1.FirstName + " " + req.Requisition.Employee1.LastName : "",
                                       reqEncodedby = "",
                                       reqApprovedBy = req.Requisition.Employee != null ? req.Requisition.Employee.FirstName + " " + req.Requisition.Employee.LastName : "",
                                       reqQty = req.ReferenceQuantity.Value,


                                       dateReceived = rec != null ? rec.Receiving.ReceivingDate : DateTime.Now,//rec.Receiving.ReceivingDate.ToString("MM/dd/yyyy"),
                                       recno = rec != null ? rec.Receiving.ReceivingID : "",
                                       recQty = rec != null ? rec.ReferenceQuantity.Value : 0,
                                       receivedby = rec != null ? rec.Receiving.Employee2 != null ? rec.Receiving.Employee2.FirstName + " " + rec.Receiving.Employee2.LastName : "" : "",
                                       recEncodedby = rec != null ? rec.Receiving.Employee != null ? rec.Receiving.Employee.FirstName + " " + rec.Receiving.Employee.LastName : "":"",
                                       recApprovedBy = rec != null ? rec.Receiving.Employee3 != null ? rec.Receiving.Employee3.FirstName + " " + rec.Receiving.Employee3.LastName : "":"",

                                       dateTransferred = st != null? st.StockTransfer.STDAte : DateTime.Now,// st.StockTransfer.STDAte.ToString("MM/dd/yyyy"),
                                       stNo = st != null ? st.StockTransfer.TransferID : "",
                                       stQty = st != null ? st.ReferenceQuantity.Value: 0,     
                                       releasedby = st != null ? st.StockTransfer.Employee4 != null ? st.StockTransfer.Employee4.FirstName + " " + st.StockTransfer.Employee4.LastName : "":"",
                                       stEncodedby = st != null ? st.StockTransfer.Employee6 != null ? st.StockTransfer.Employee6.FirstName + " " + st.StockTransfer.Employee6.LastName : "":"",
                                       stApprovedBy = st != null ? st.StockTransfer.Employee != null ? st.StockTransfer.Employee.FirstName + " " + st.StockTransfer.Employee.LastName : "":"",

                                       discrepancy = rec != null ? st.ReferenceQuantity - rec.ReferenceQuantity:0,
                                       location = req.Requisition.Location.Description,
                                       remarks =  ""


                                   }).Where(r=>r.discrepancy > 0).ToList();

            var lstDiscrepancy2 = (from req in _lstReq.Where(r => r.Requisition.ReqTypeID ==1)
                                   join rec in entity.ReceivingDetails.Where(r=>r.Receiving.ApprovalStatus == 2 && r.Receiving.RequisitionID != null) on new { a1 = req.ID, b1 = req.Requisition.ID } equals new { a1 = rec.RequisitionDetailID.Value, b1 = rec.Receiving.RequisitionID.Value }
                                   select new
                                   {
                                       ItemCode = req.Item.Code,
                                       brand = req.Item.Brand.Description,
                                       itemsalesdescription = req.Item.Description,
                                       uom = req.Item.UnitOfMeasurement != null ? req.Item.UnitOfMeasurement.Description : "",

                                       reqtype = req.Requisition.ReqType.Type,
                                       dateRequested = req.Requisition.RequestedDate,// req.Requisition.RequestedDate.ToString("MM/dd/yyyy"),
                                       orderbydate = req.Requisition.RequestedDate,
                                       reqno = req.Requisition.RefNumber,
                                       requestedby = req.Requisition.Employee1 != null ? req.Requisition.Employee1.FirstName + " " + req.Requisition.Employee1.LastName : "",
                                       reqEncodedby = "",
                                       reqApprovedBy = req.Requisition.Employee != null ? req.Requisition.Employee.FirstName + " " + req.Requisition.Employee.LastName : "",
                                       reqQty = req.ReferenceQuantity.Value,


                                       dateReceived = rec.Receiving.ReceivingDate,//rec.Receiving.ReceivingDate.ToString("MM/dd/yyyy"),
                                       recno = rec.Receiving.ReceivingID,
                                       recQty = rec.ReferenceQuantity.Value,
                                       receivedby = rec.Receiving.Employee2 != null ? rec.Receiving.Employee2.FirstName + " " + rec.Receiving.Employee2.LastName : "",
                                       recEncodedby = rec.Receiving.Employee != null ? rec.Receiving.Employee.FirstName + " " + rec.Receiving.Employee.LastName : "",     
                                       recApprovedBy = rec.Receiving.Employee3 != null ? rec.Receiving.Employee3.FirstName + " " + rec.Receiving.Employee3.LastName : "",
                                       
                                       dateTransferred =DateTime.Now,
                                       stNo ="",
                                       stQty =0,
                                       releasedby = "",
                                       stEncodedby = "",
                                       stApprovedBy ="",

                                       discrepancy = req.ReferenceQuantity - rec.ReferenceQuantity,
                                       location = req.Requisition.Location.Description,
                                       remarks = ""


                                   }).Where(r => r.discrepancy > 0).ToList();

            var lstDiscrepancy3 = (from p in lstDiscrepancy1 select p).Union(from q in lstDiscrepancy2 select q);

            var lstDiscrepancy4 = (from i in lstDiscrepancy3
                                 select new
                                 {
                                     ItemCode = i.ItemCode,
                                     brand = i.brand,
                                     itemsalesdescription = i.itemsalesdescription,
                                     uom =i.uom,

                                     reqtype = i.reqtype,
                                     dateRequested = i.dateRequested.ToString("MM/dd/yyyy"),// req.Requisition.RequestedDate.ToString("MM/dd/yyyy"),
                                     orderbydate =i.orderbydate,
                                     reqno =i.reqno,
                                     requestedby = i.requestedby,
                                     reqEncodedby = i.reqEncodedby,
                                     reqApprovedBy = i.reqApprovedBy,
                                     reqQty = i.reqQty,


                                     dateReceived = i.recQty > 0 ? i.dateReceived.ToString("MM/dd/yyyy") : "",//rec.Receiving.ReceivingDate.ToString("MM/dd/yyyy"),
                                     recno = i.recno,
                                     recQty = i.recQty,
                                     receivedby = i.receivedby,
                                     recEncodedby = i.recEncodedby,
                                     recApprovedBy =i.recApprovedBy,

                                     dateTransferred = i.stQty > 0 ? i.dateTransferred.ToString("MM/dd/yyyy") : "",
                                     stNo = i.stNo,
                                     stQty = i.stQty,
                                     releasedby = i.releasedby,
                                     stEncodedby = i.stEncodedby,
                                     stApprovedBy = i.stApprovedBy,

                                     discrepancy = i.discrepancy,
                                     location = i.location,
                                     remarks = ""

                                 }).ToList();


            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;

            ReportDataSource _rds = new ReportDataSource();
            _rds.Name = "dsDiscrepancy";
            _rds.Value = lstDiscrepancy3.OrderBy(p => p.orderbydate);

            reportViewer.KeepSessionAlive = false;
            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Views\Report\rdlc\Discrepancy.rdlc";

            List<ReportParameter> _parameter = new List<ReportParameter>();
            _parameter.Add(new ReportParameter("DateRange", dtDateFrom.ToString("MMMM dd, yyyy") + " - " + dtDateTo.ToString("MMMM dd, yyyy")));

            reportViewer.LocalReport.DataSources.Add(_rds);
            reportViewer.LocalReport.Refresh();
            reportViewer.LocalReport.SetParameters(_parameter);

            ViewBag.ReportViewer = reportViewer;

            ViewBag.DateFrom = dtDateFrom.ToString("MM/dd/yyyy");
            ViewBag.DateTo = dtDateTo.ToString("MM/dd/yyyy");

            return View();
        }


    }
}