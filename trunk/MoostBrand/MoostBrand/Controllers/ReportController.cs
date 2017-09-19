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

            #region DROPDOWNS
            var loc = entity.Locations.Where(x => x.ID != 10)
                            .Select(x => new
                            {
                                ID = x.ID,
                                Description = x.Description
                            });

            ViewBag.Brand = new SelectList(entity.Brands, "ID", "Description");
            ViewBag.Category = new SelectList(entity.Categories, "ID", "Description");
            ViewBag.Vendor = new SelectList(entity.Vendors, "ID", "GeneralName");
            ViewBag.Location = new SelectList(loc, "ID", "Description");
            #endregion
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

            var _lst = entity.Inventories.ToList();

            if(brand!=null)
            {
                var items = entity.Items.Where(p => p.BrandID == brand).Select(p => p.Code);
                _lst= _lst.Where(p => items.Contains(p.ItemCode)).ToList();
            }

            if (category != null)
            {
                string _category = entity.Categories.Find(category).Description;
                _lst =_lst.Where(p => p.Category == _category).ToList();
            }

            if (vendor != null)
            {
                var items = entity.Items.Where(p => p.VendorCoding == vendor).Select(p => p.Code);
                _lst= _lst.Where(p => items.Contains(p.ItemCode)).ToList();
            }

            if (location != null)
            {
                _lst=_lst.Where(p => p.LocationCode == location).ToList();
            }

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
                                        OutQty = invRepo.getTotalStockTranfer(i.ItemCode,i.LocationCode.Value, dtDateFrom, dtDateTo),
                                        AdjustedQty = invRepo.getTotalVariance(i.ID,i.LocationCode.Value, dtDateFrom, dtDateTo),
                                        CommittedQty = i.Committed != null ? i.Committed :0,
                                        TotalOrder = i.Ordered != null ? i.Ordered :0,
                                        ReservationName = "",
                                        QOH = "",


                                    }).ToList();

          //  if(brand)

     

       
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;

            ReportDataSource _rds = new ReportDataSource();
            _rds.Name = "dsInventory";
            _rds.Value = lstInventory.OrderBy(p => p.ItemSalesDesc);

            reportViewer.KeepSessionAlive = false;
            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Views\Report\rdlc\Inventory.rdlc";

            List<ReportParameter> _parameter = new List<ReportParameter>();
            _parameter.Add(new ReportParameter("DateRange", dtDateFrom.ToString("MMMM dd, yyyy") + " - " + dtDateTo.ToString("MMMM dd, yyyy")));
            //_parameter.Add(new ReportParameter("CompanyName", companyRepo.GetById(Sessions.CompanyId.Value).Name));
            //_parameter.Add(new ReportParameter("TotalMaterials", totalMaterials.ToString()));
            //_parameter.Add(new ReportParameter("TotalTires", totalTires.ToString()));
            //_parameter.Add(new ReportParameter("TotalBatteries", totalBatteries.ToString()));

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