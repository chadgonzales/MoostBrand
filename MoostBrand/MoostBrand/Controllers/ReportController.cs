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
        public ActionResult InventoryReport(string dateFrom, string dateTo, int? brand, int? category, int? vendor, int? location)
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
                _lst= _lst.Where(p => p.Items.VendorCoding == vendor).ToList();
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
                                        AdjustedQty = lstInventory1.FirstOrDefault(p => p.ItemId.InventoryID == i.ID && p.ItemId.Type.ToLower() == "variance") != null ? lstInventory1.FirstOrDefault(p => p.ItemId.InventoryID == i.ID && p.ItemId.Type.ToLower() == "variance").Variance : 0,//invRepo.getTotalVariance(i.ID,i.LocationCode.Value, dtDateFrom, dtDateTo),
                                        CommittedQty = i.Committed != null || i.Committed > 0 ? i.Committed :0,
                                        TotalOrder = i.Ordered != null ? i.Ordered :0,                                                                                       
                                        ReservationName = lstInventory3.FirstOrDefault(p => p.ItemId.Code == i.ItemCode && p.ItemId.LocationID == i.LocationCode) != null ? lstInventory3.FirstOrDefault(p => p.ItemId.Code == i.ItemCode && p.ItemId.LocationID == i.LocationCode).ReservationName : " ",
                                        QOH =0,
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
            _rds.Name = "dsInventory";
            _rds.Value = _lstInventory.OrderBy(p => p.ItemSalesDesc);

            reportViewer.KeepSessionAlive = false;
            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Views\Report\rdlc\Inventory.rdlc";

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
                _lst = _lst.Where(p => p.Item.VendorCoding == vendor).ToList();
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
            _rds.Value = lstReservation.OrderBy(p => p.ItemSalesDesc);

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
                                    Date = i.Date != null ? i.Date.Value.ToString("MM/dd/yyyy") : "",
                                    Location = i.Inventories.LocationCode != null ? i.Inventories.Location.Description : " "

                                }).ToList();



            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;

            ReportDataSource _rds = new ReportDataSource();
            _rds.Name = "dsStockLedger";
            _rds.Value = lstStockLedger.OrderBy(c => c.Date).ThenBy(n => n.Type);

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
                _lst = _lst.Where(p => p.Item.VendorCoding == vendor).ToList();
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
                                    DateRequested = i.Requisition._RequestedDate,
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
            _rds.Value = lstUnserve.OrderBy(p => p.ItemSalesDesc);

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

            string  _sortbyvendor = "Vendor: ALL"; //_sortbylocation = "Location: ALL"
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
                _lst = _lst.Where(p => p.RequisitionDetail.Item.VendorCoding == vendor).ToList();
                string _vendor = entity.Vendors.Find(vendor).Name;
                _sortbyvendor = "Vendor:" + _vendor;
            }

            //if (location != null)
            //{
            //    _lst = _lst.Where(p => p.Receiving.LocationID == location).ToList();
            //    string _location = entity.Locations.Find(location).Description;
            //    _sortbylocation = "Location:" + _location;

            //}


            var lstStockReceiving = (from i in _lst
                              select new
                              {
                                  DateCreated = i.Receiving.Requisition._RequestedDate,
                                  SRR = i.Receiving.ReceivingID != null ? i.Receiving.ReceivingID : "",
                                  PO = "",
                                  ItemCode = i.RequisitionDetail.Item.Code,
                                  ItemPurDesc = i.RequisitionDetail.Item.Description,
                                  ItemSalesDesc = i.RequisitionDetail.Item.DescriptionPurchase,
                                  UOM = i.RequisitionDetail.Item.UnitOfMeasurement.Description,
                                  Quantity = i.Quantity,
                                  Location = i.Receiving.Location.Description,
                                  ReceivedBy = i.Receiving.Employee2.FirstName+' '+i.Receiving.Employee2.LastName,
                                  CreatedBy = i.Receiving.Requisition.Employee1.FirstName +' ' + i.Receiving.Requisition.Employee1.LastName

                              }).ToList();



            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;

            ReportDataSource _rds = new ReportDataSource();
            _rds.Name = "dsStockReceiving";
            _rds.Value = lstStockReceiving.OrderBy(p => p.ItemSalesDesc);

            reportViewer.KeepSessionAlive = false;
            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Views\Report\rdlc\StockReceiving.rdlc";

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
                _lst = _lst.Where(p => p.Items.VendorCoding == vendor).ToList();
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

            string _sortbyvendor = "Vendor: ALL"; //_sortbylocation = "Location: ALL"
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
                _lst = _lst.Where(p => p.RequisitionDetail.Item.VendorCoding == vendor).ToList();
                string _vendor = entity.Vendors.Find(vendor).Name;
                _sortbyvendor = "Vendor:" + _vendor;
            }

            //if (location != null)
            //{
            //    _lst = _lst.Where(p => p.Receiving.LocationID == location).ToList();
            //    string _location = entity.Locations.Find(location).Description;
            //    _sortbylocation = "Location:" + _location;

            //}


            var lstStockReceiving = (from i in _lst
                                     select new
                                     {
                                         DateCreated = i.Receiving.Requisition._RequestedDate,
                                         SRR = i.Receiving.ReceivingID != null ? i.Receiving.ReceivingID : "",
                                         PO = "",
                                         ItemCode = i.RequisitionDetail.Item.Code,
                                         ItemPurDesc = i.RequisitionDetail.Item.Description,
                                         ItemSalesDesc = i.RequisitionDetail.Item.DescriptionPurchase,
                                         UOM = i.RequisitionDetail.Item.UnitOfMeasurement.Description,
                                         Quantity = i.Quantity,
                                         Location = i.Receiving.Location.Description,
                                         ReceivedBy = i.Receiving.Employee2.FirstName + ' ' + i.Receiving.Employee2.LastName,
                                         CreatedBy = i.Receiving.Requisition.Employee1.FirstName + ' ' + i.Receiving.Requisition.Employee1.LastName

                                     }).ToList();



            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;

            ReportDataSource _rds = new ReportDataSource();
            _rds.Name = "dsStockTransfer";
            _rds.Value = lstStockReceiving.OrderBy(p => p.ItemSalesDesc);

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



    }
}