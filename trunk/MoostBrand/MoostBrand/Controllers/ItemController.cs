﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MoostBrand.DAL;
using PagedList;
using System.Data.Entity;
using System.Configuration;
using System.IO;

namespace MoostBrand.Controllers
{
    public class ItemController : Controller
    {
        MoostBrandEntities entity = new MoostBrandEntities();

        private int ID;
        DateTime dt = DateTime.Now; //Robi

        //Robi
        private string CodeGenerator()
        {
        //Initiate objects & vars
        startR: Random random = new Random();
            string randomString = "";
            int randNumber = 0;

            //Loop 'length' times to generate a random number or character
            for (int i = 0; i < 8; i++)
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

            //randomString = "M-" + randomString;
            randomString = "M" + dt.ToString("yy") + "-" + randomString;
            var itm = entity.Items.ToList().FindAll(p => p.ItemID == randomString);
            if (itm.Count() > 0)
            {
                goto startR;
            }
            return randomString;
        }

        // GET: Items
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "code" : "";
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "desc" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;


            var items = from i in entity.Items
                        select i;

            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(i => i.Code.Contains(searchString)
                                       || i.Description.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "code":
                    items = items.OrderByDescending(i => i.Code);
                    break;
                case "desc":
                    items = items.OrderByDescending(i => i.Description);
                    break;
                default:
                    items = items.OrderBy(i => i.ID);
                    break;
            }

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        // GET: Item/Details/5
        public ActionResult Details(int id)
        {
            var item = entity.Items.Find(id);
            return View(item);
        }

        // GET: Item/Create
        public ActionResult Create()
        {
            //Robi
            var item = new Item();
            item.ItemID = CodeGenerator();

            ViewBag.Categories = entity.Categories.ToList();
            ViewBag.Colors = entity.Colors.ToList();
            ViewBag.Sizes = entity.Sizes.ToList();
            ViewBag.UOMS = entity.UnitOfMeasurements.ToList();
            ViewBag.Brands = entity.Brands.ToList();

            return View(item);
        }

        // POST: Item/Create
        [HttpPost]
        public ActionResult Create(int? selectedSubCatID, int? selectedSubCatTypeID, Item item)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    var itm = entity.Items.ToList().FindAll(b => b.Description == item.Description);

                    if (item.Proceed == false)
                    {
                        if (itm.Count() > 0)
                        {
                            ViewBag.Error = "Would you like to proceed in adding the same items?";
                            ViewBag.Categories = entity.Categories.ToList();
                            ViewBag.Colors = entity.Colors.ToList();
                            ViewBag.Sizes = entity.Sizes.ToList();
                            ViewBag.UOMS = entity.UnitOfMeasurements.ToList();
                            ViewBag.Brands = entity.Brands.ToList();
                            if (selectedSubCatID != null)
                                ViewData["DDLSubCat"] = new SelectList(entity.SubCategories.ToList(), "ID", "Description", selectedSubCatID);
                            else
                                ViewData["DDLSubCat"] = new SelectList(entity.SubCategories.ToList(), "ID", "Description");

                            if (selectedSubCatTypeID != null)
                                ViewData["DDLSubCatType"] = new SelectList(entity.SubCategories.ToList(), "ID", "Description", selectedSubCatTypeID);
                            else
                                ViewData["DDLSubCatType"] = new SelectList(entity.SubCategoriesTypes.ToList(), "ID", "Description");

                            return View(item);
                        }
                    }

                    if (item.Img != null)
                    {
                        string imagePath = ConfigurationManager.AppSettings["imagePath"];
                        string path = Path.Combine(Server.MapPath("~" + imagePath), "ID" + item.ItemID + ".jpg");
                        item.Img.SaveAs(path);

                        string baseUrl = string.Format("{0}://{1}", Request.Url.Scheme, Request.Url.Authority);
                        item.Image = baseUrl + imagePath + "/ID" + item.ItemID + ".jpg";
                    }

                    item.Quantity = 0;
                    item.LastUnitCost = 0;
                    item.WeightedAverageCost = 0;
                    item.Price = 0;

                    entity.Items.Add(item);
                    entity.SaveChanges();
                    return RedirectToAction("Index");

                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "There's an error!");
                }
            }

            ViewBag.Categories = entity.Categories.ToList();
            ViewBag.Colors = entity.Colors.ToList();
            ViewBag.Sizes = entity.Sizes.ToList();
            ViewBag.UOMS = entity.UnitOfMeasurements.ToList();
            ViewBag.Brands = entity.Brands.ToList();

            return View(item);
        }


        //[HttpPost]
        //public JsonResult IsUniq(string code, string description)
        //{
        //    bool status;
        //    var itm = entity.Items.ToList().FindAll(b => b.Code == code && b.Description == description);

        //    if (itm.Count() > 0)
        //        status = false;
        //    else
        //        status = true;

        //    return new JsonResult { Data = new { status = status } };
        //}

        // GET: Item/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.Categories = entity.Categories.ToList();
            ViewBag.Colors = entity.Colors.ToList();
            ViewBag.Sizes = entity.Sizes.ToList();
            ViewBag.UOMS = entity.UnitOfMeasurements.ToList();
            ViewBag.Brands = entity.Brands.ToList();

            var item = entity.Items.Find(id);
            return View(item);
        }

        // POST: Item/Edit/5
        [HttpPost]
        public ActionResult Edit(Item item)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (item.Img != null)
                    {
                        string imagePath = ConfigurationManager.AppSettings["imagePath"];
                        string path = Path.Combine(Server.MapPath("~" + imagePath), "ID" + item.ItemID + ".jpg");
                        item.Img.SaveAs(path);

                        string baseUrl = string.Format("{0}://{1}", Request.Url.Scheme, Request.Url.Authority);
                        item.Image = baseUrl + imagePath + "/ID" + item.ItemID + ".jpg";
                    }

                    entity.Entry(item).State = EntityState.Modified;
                    entity.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    ModelState.AddModelError("", "Fill all fields");
                }
            }
            ViewBag.Categories = entity.Categories.ToList();
            ViewBag.Colors = entity.Colors.ToList();
            ViewBag.Sizes = entity.Sizes.ToList();
            ViewBag.UOMS = entity.UnitOfMeasurements.ToList();
            ViewBag.Brands = entity.Brands.ToList();
            return View(item);
        }

        // GET: Item/Delete/5
        public ActionResult Delete(int id = 0)
        {
            var item = entity.Items.Find(id);
            return View(item);
        }

        // POST: Item/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id = 0)
        {
            try
            {
                var item = entity.Items.Find(id);

                try
                {
                    entity.Items.Remove(item);
                    entity.SaveChanges();
                }
                catch { }
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult ItemViews(int id)
        {
            ViewBag.ItemID = id;
            var itemDetails = entity.ItemDetail.Where(x => x.ItemID == id);
            if (itemDetails != null)
                return View(itemDetails.ToList());
            else
                return View();
        }

        public ActionResult ItemDetails(int id)
        {
            var item = entity.ItemDetail.Find(id);
            return View(item);
        }

        public ActionResult AddItem(int id)
        {
            var itemDetails = new ItemDetails();
            itemDetails.ItemID = id;
            itemDetails.Date = DateTime.Now;
            return View(itemDetails);
        }

        [HttpPost]
        public ActionResult AddItem(ItemDetails itmDetails)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var item = entity.Items.Where(i => i.ID == itmDetails.ItemID).FirstOrDefault();
                    if (item.Quantity == null)
                        item.Quantity = 0;
                    item.Price = 0;
                    item.LastUnitCost = 0;
                    item.WeightedAverageCost = 0;
                    item.ReOrderLevel = 0;

                    entity.ItemDetail.Add(itmDetails);
                    entity.SaveChanges();

                    #region WAC
                    var itmDetail = from s in entity.ItemDetail
                                    where s.ItemID == itmDetails.ItemID
                                    select s;

                    item.Quantity += Convert.ToInt32(itmDetails.Quantity);

                    //Price
                    decimal qtyCost = 0;
                    foreach (var detail in itmDetail)
                        qtyCost += Convert.ToDecimal(detail.Quantity * detail.Cost);

                    double Price = Convert.ToDouble((qtyCost) / item.Quantity);
                    item.Price = Convert.ToDecimal(Price);

                    if (itmDetail.Count() > 1)
                    {
                        //WAC             
                        int prevQty = Convert.ToInt32(item.Quantity - itmDetails.Quantity);
                        item.WeightedAverageCost = Convert.ToDecimal((itmDetails.Quantity * itmDetails.Cost) / prevQty);
                    }

                    item.LastUnitCost = itmDetails.Cost;
                    #endregion

                    entity.Entry(item).State = EntityState.Modified;
                    entity.SaveChanges();

                    return RedirectToAction("ItemViews", new { id = itmDetails.ItemID });
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "There's an error");
                }
            }
            return View(itmDetails);
        }

        public ActionResult EditItem(int id)
        {
            var itm = entity.ItemDetail.Find(id);
            return View(itm);
        }

        [HttpPost]
        public ActionResult EditItem(ItemDetails itmDetails, int ItemID)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var idetails = entity.ItemDetail.Find(itmDetails.ID);

                    int presentQty = Convert.ToInt32(itmDetails.Quantity);
                    int previousQty = Convert.ToInt32(idetails.Quantity);

                    if (presentQty != previousQty)
                    {
                        int itemID = Convert.ToInt32(entity.ItemDetail.Find(itmDetails.ItemID));
                        var itm = entity.Items.Where(i => i.ID == itmDetails.ItemID).FirstOrDefault();
                        #region WAC    

                        if (presentQty >= itm.Quantity)
                        {
                            var itmDetail = entity.ItemDetail.Where(i => i.ItemID == itm.ID).ToList();
                            var itemDetail = entity.ItemDetail.Find(itm.ID);

                            int prevSTItemQty = Convert.ToInt32(itm.Quantity + previousQty);

                            itm.Quantity = prevSTItemQty - presentQty;

                            //Price
                            decimal qtyCost = 0;
                            foreach (var detail in itmDetail)
                                /*
                                 */
                                qtyCost += Convert.ToDecimal(detail.Quantity * detail.Cost);

                            double Price = Convert.ToDouble((qtyCost) / itm.Quantity);
                            itm.Price = Convert.ToDecimal(Price);

                            if (itmDetail.Count() > 1)
                            {
                                //WAC             
                                int prevQty = Convert.ToInt32(itm.Quantity - itmDetails.Quantity);
                                itm.WeightedAverageCost = Convert.ToDecimal((itmDetails.Quantity * itmDetails.Cost) / prevQty);
                            }
                            itm.LastUnitCost = itmDetails.Cost;

                        }
                        else
                        {
                            var itmDetail = entity.ItemDetail.Where(i => i.ItemID == itm.ID).ToList();
                            var itemDetail = entity.ItemDetail.Find(itm.ID);

                            int prevItmQty = Convert.ToInt32(itm.Quantity - previousQty);
                            itm.Quantity = prevItmQty + presentQty;

                            //Price
                            decimal qtyCost = 0;
                            foreach (var detail in itmDetail)
                                qtyCost += Convert.ToDecimal(detail.Quantity * detail.Cost);

                            double Price = Convert.ToDouble((qtyCost) / itm.Quantity);
                            itm.Price = Convert.ToDecimal(Price);

                            if (itmDetail.Count() > 1)
                            {
                                //WAC             
                                int prevQty = Convert.ToInt32(itm.Quantity - itmDetails.Quantity);
                                itm.WeightedAverageCost = Convert.ToDecimal((itmDetails.Quantity * itmDetails.Cost) / prevQty);
                            }
                            itm.LastUnitCost = itmDetails.Cost;
                        }
                        #endregion
                    }

                    entity.Entry(idetails).CurrentValues.SetValues(itmDetails);
                    entity.SaveChanges();

                    return RedirectToAction("ItemViews", new { id = ItemID });
                }
                catch
                {
                    ModelState.AddModelError("", "Fill all fields");
                }
            }
            return View(itmDetails);
        }

        public ActionResult DeleteItem(int id)
        {
            var itm = entity.ItemDetail.Find(id);
            return View(itm);
        }

        [HttpPost, ActionName("DeleteItem")]
        public ActionResult DeleteItemConfirmed(int id, int ItemID)
        {
            try
            {
                var itmDtl = entity.ItemDetail.Find(id);
                try
                {
                    entity.ItemDetail.Remove(itmDtl);
                    entity.SaveChanges();

                    var item = entity.Items.Where(i => i.ID == ItemID).FirstOrDefault();

                    #region WAC
                    var itmDetail = from s in entity.ItemDetail
                                    where s.ItemID == itmDtl.ItemID
                                    select s;

                    int Qty = Convert.ToInt32(item.Quantity - itmDtl.Quantity);
                    item.Quantity = Convert.ToInt32(Qty);

                    int WAC = Convert.ToInt32(item.WeightedAverageCost - itmDtl.WeightedAverageCost);
                    item.WeightedAverageCost = Convert.ToInt32(WAC);

                    item.LastUnitCost = itmDtl.Cost;

                    entity.Entry(item).State = EntityState.Modified;
                    entity.SaveChanges();
                    #endregion
                }
                catch { }
                return RedirectToAction("ItemViews", new { id = ItemID });
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult GetSubCategory(int categoryID)
        {

            var objsubcat = entity.SubCategories
                    .Where(s => s.CategoryID == categoryID)
                    .Select(s => new { s.ID, s.Description })
                    .OrderBy(s => s.Description);

            return Json(objsubcat, JsonRequestBehavior.AllowGet);
        }

        //Robi
        [HttpPost]
        public ActionResult GetSubCategoriesTypes(int subcategoryID)
        {
            var objsubcattypes = entity.SubCategoriesTypes
                .Where(s => s.SubCategoriesID == subcategoryID)
                .Select(s => new { s.ID, s.Description })
                .OrderBy(s => s.Description);

            return Json(objsubcattypes, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetItemById(int itemID)
        {
            var item = entity.Items.FirstOrDefault(i => i.ID == itemID);

            return Json(
                new
                {
                    ID = item.ID,
                    Category = item.CategoryID,
                    SubCategory = item.SubCategoryID

                },
                JsonRequestBehavior.AllowGet);
        }
    }
}