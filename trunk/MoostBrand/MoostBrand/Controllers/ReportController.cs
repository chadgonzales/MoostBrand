using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MoostBrand.DAL;
using PagedList;
using System.Linq.Dynamic;
using System.Data.Entity;
using Microsoft.Reporting.WebForms;
using System.Data.Entity.Core.Objects;
using System.Web.Mvc.Html;



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
            ViewBag.Vendor = new SelectList(entity.Vendors, "ID", "GeneralName");
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


            var lstInventory1 = (from p in entity.StockTransferDetails.ToList()
                                 group p by new { p.RequisitionDetail.Item.Code, p.StockTransfer.Requisition.LocationID } into g
                                 select new
                                 {
                                     ItemId = g.Key,

                                     OutQty = g.Where(p => p.StockTransfer.STDAte.Date >= dtDateFrom && p.StockTransfer.STDAte.Date <= dtDateTo && p.AprovalStatusID == 2).Sum(p => p.Quantity)
                                 }).ToList();

            var lstInventory2 = (from p in entity.StockAdjustmentDetails.ToList()
                                 group p by new { p.ItemID } into g
                                 select new
                                 {
                                     ItemId = g.Key,

                                     AdjustedQty = g.Where(p => p.StockAdjustment.ErrorDate.Date >= dtDateFrom && p.StockAdjustment.ErrorDate.Date <= dtDateTo && p.StockAdjustment.ApprovalStatus == 2).Sum(p => p.Variance)
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
                                        InQty = i.InStock != null ? i.InStock :0,
                                        OutQty = lstInventory1.FirstOrDefault(p => p.ItemId.Code == i.ItemCode && p.ItemId.LocationID == i.LocationCode) != null ? lstInventory1.FirstOrDefault(p => p.ItemId.Code == i.ItemCode && p.ItemId.LocationID == i.LocationCode).OutQty : 0, //invRepo.getTotalStockTranfer(i.ItemCode,i.LocationCode.Value, dtDateFrom, dtDateTo),
                                        AdjustedQty = lstInventory2.FirstOrDefault(p=>p.ItemId.ItemID == i.ID) != null ? lstInventory2.FirstOrDefault(p => p.ItemId.ItemID == i.ID).AdjustedQty : 0,//invRepo.getTotalVariance(i.ID,i.LocationCode.Value, dtDateFrom, dtDateTo),
                                        CommittedQty = i.Committed != null ? i.Committed :0,
                                        TotalOrder = i.Ordered != null ? i.Ordered :0,                                                                                       
                                        ReservationName = lstInventory3.FirstOrDefault(p => p.ItemId.Code == i.ItemCode && p.ItemId.LocationID == i.LocationCode) != null ? lstInventory3.FirstOrDefault(p => p.ItemId.Code == i.ItemCode && p.ItemId.LocationID == i.LocationCode).ReservationName : " ",
                                        QOH =0,
                                        PcsPerBox = i.Items.Quantity


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
                                    QOH = i.InQty - i.OutQty - i.CommittedQty + i.AdjustedQty,
                                    PcsPerBox = i.PcsPerBox
                                  

                                 }).ToList();



            //  if(brand)



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
    }
}