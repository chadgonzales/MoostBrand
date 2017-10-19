using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemsEncoder
{
    class Program
    {
        public class ItemDTO
        {
            public Item item { get; set; }
        }

        public class VendorDTO
        {
            public Vendor vendor { get; set; }
        }

        public class InventoryDTO
        {
            public Inventory inventory { get; set; }
        }

        static void Main(string[] args)
        {
            MoostBrandEntities entity = new MoostBrandEntities();

            List<ItemDTO> lstItemDTO = new List<ItemDTO>();

            List<InventoryDTO> lstInventoryDTO = new List<InventoryDTO>();

            List<VendorDTO> lstVendorDTO = new List<VendorDTO>();

            const Int32 BufferSize = 128;
            using (var fileStream = File.OpenRead(@"C:\reorder1.csv"))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                String line;
                int lineCnt = 1;
                while ((line = streamReader.ReadLine()) != null)
                {
                    if (lineCnt >= 1)
                    {
                        string[] strLine = line.Split(',');

                        if (true) {
                            Item item = new Item();
                            Inventory inventory = new Inventory();
                            //Vendor vendor = new Vendor();

                            //string txtBrand = Convert.ToString(strLine[5]);
                            //Brand brand = entity.Brands.FirstOrDefault(p => p.Code == txtBrand);
                            //if (brand == null)
                            //{
                            //    brand = new Brand
                            //    {
                            //        Code = txtBrand,
                            //        Description = txtBrand
                            //    };
                            //    entity.Brands.Add(brand);
                            //    entity.SaveChanges();
                            //}
                            //string Code = Convert.ToString(strLine[0]).Trim();
                            //Item _item = entity.Items.FirstOrDefault(p => p.Code.Trim() == Code);
                            //if (_item != null)
                            //{
                                string txtCategory = Convert.ToString(strLine[2]);
                                Category category = entity.Categories.FirstOrDefault(p => p.Code == txtCategory);
                                if (category == null)
                                {
                                    category = new Category
                                    {
                                        Code = txtCategory,
                                        Description = txtCategory
                                    };
                                    entity.Categories.Add(category);
                                    entity.SaveChanges();
                                }



                                string txtSubCategory = Convert.ToString(strLine[3]);
                                SubCategory subCategory = entity.SubCategories.FirstOrDefault(p => p.Code == txtSubCategory);
                                if (subCategory == null)
                                {
                                    subCategory = new SubCategory
                                    {
                                        CategoryID = category.ID,
                                        Code = txtSubCategory,
                                        Description = txtSubCategory
                                    };
                                    entity.SubCategories.Add(subCategory);
                                    entity.SaveChanges();
                                }

                                //string txtUOM = Convert.ToString(strLine[5]);
                                //UnitOfMeasurement uom = entity.UnitOfMeasurements.FirstOrDefault(p => p.Description == txtUOM);
                                //if (uom == null)
                                //{
                                //    uom = new UnitOfMeasurement
                                //    {
                                //        Code = txtUOM,
                                //        Description = txtUOM
                                //    };
                                //    entity.UnitOfMeasurements.Add(uom);
                                //    entity.SaveChanges();
                                //}

                                //int locid = 0;
                                //string txtLocation = Convert.ToString(strLine[2]);
                                //Location location = entity.Locations.FirstOrDefault(p => p.Description.Trim() == txtLocation.Trim());
                                //if (location != null)
                                //{
                                //    locid = location.ID;
                                //}


                                //string dec = Convert.ToString(strLine[1]);
                                //if (dec == "")
                                //{
                                //    dec = "0";
                                //}
                                //decimal d = Convert.ToDecimal(dec); ;
                                //int wholenumber = (int)Math.Floor(d);

                                //decimal _decimal = d - wholenumber;

                                //string txtItemCode = Convert.ToString(strLine[0]);
                                //Item item = entity.Items.FirstOrDefault(p => p.Code.Trim() == txtItemCode.Trim());

                                //if (item != null && item.UnitOfMeasurementID != null && item.Quantity != null && locid != 0)
                                //{

                                //    inventory.ItemCode = item.Code;
                                //    inventory.InventoryUoM = item.UnitOfMeasurement.Description;
                                //    inventory.Description = item.DescriptionPurchase;
                                //    inventory.LocationCode = locid;
                                //    inventory.InventoryStatus = 2;
                                //    inventory.POSBarCode = item.Barcode;
                                //    inventory.Year = item.Year;
                                //    inventory.Category = item.Category.Description;

                                //    //if (item.UnitOfMeasurementID == 6)
                                //    //{
                                //    //    inventory.InStock = (int)((wholenumber * item.Quantity) + (_decimal * item.Quantity));
                                //    //}
                                //    //else if (item.UnitOfMeasurementID == 4)
                                //    //{
                                //    //    inventory.InStock = wholenumber;
                                //    //}

                                //    inventory.InStock = wholenumber;

                                //}
                                //else
                                //{
                                //    Console.WriteLine(txtItemCode);
                                //}


                                //item.Year ="2017";
                                //item.Barcode = Convert.ToString(strLine[1]);
                                //item.Code = Convert.ToString(strLine[0]);
                                //item.Description = Convert.ToString(strLine[2]);
                                //item.DescriptionPurchase = Convert.ToString(strLine[3]);
                                //  item.Quantity = Convert.ToInt32(strLine[5]);


                                //  item.UnitOfMeasurementID = 6;//uom.ID;
                                //item.BrandID = brand.ID;

                                //////statusu

                                //item.CategoryID = category.ID;
                                //item.SubCategoryID = subCategory.ID;

                                //string txtLeadTime = Convert.ToString(strLine[5]).Trim();
                                //item.LeadTime = txtLeadTime != "" ? Convert.ToInt32(txtLeadTime) : 0;



                                //if (_item.UnitOfMeasurementID == 4)
                                //{
                                //    string txtDailyAve = Convert.ToString(strLine[7]).Trim();
                                //    item.DailyAverageUsage = txtDailyAve != "" ? Convert.ToInt32(txtDailyAve) : 0;
                                //}
                                //else if (_item.UnitOfMeasurementID == 6)
                                //{
                                //    string txtDailyAve = Convert.ToString(strLine[6]).Trim();
                                //    item.DailyAverageUsage = txtDailyAve != "" ? Convert.ToInt32(txtDailyAve) : 0;
                                //}
                                //else
                                //{ item.DailyAverageUsage = 0; }

                                //item.ReOrderLevel = item.LeadTime * item.DailyAverageUsage;
                                ////item.Status
                                //string pcs = Convert.ToString(strLine[4]);
                                //try
                                //{
                                //    item.Quantity = pcs != "" ? Convert.ToInt32(pcs) : 0;

                                //}
                                //catch (Exception)
                                //{
                                //    item.Quantity = 0;
                                //}

                                //vendor.Code = Convert.ToString(strLine[0]).Trim();
                                //vendor.Name = Convert.ToString(strLine[1]);
                                //vendor.GeneralName = Convert.ToString(strLine[2]);
                         //   }
                            //ItemDTO itemDTO = new ItemDTO();
                            //itemDTO.item = item;

                            //lstItemDTO.Add(itemDTO);

                            //InventoryDTO invDTO = new InventoryDTO();
                            //invDTO.inventory = inventory;

                            //lstInventoryDTO.Add(invDTO);

                            //VendorDTO venDTO = new VendorDTO();
                            //venDTO.vendor = vendor;

                            //lstVendorDTO.Add(venDTO);
                        }
                    }

                    lineCnt++;
                }
            }

            //  entity.Items.AddRange(lstItemDTO.Select(p => p.item));

            //entity.Inventories.AddRange(lstInventoryDTO.Select(p => p.inventory));

            //entity.Vendors.AddRange(lstVendorDTO.Select(p => p.vendor));

            //foreach (var _item in lstItemDTO.Select(p => p.item))
            //{
            //    bool exist = entity.Items.Count(p => p.Code.Trim() == _item.Code.Trim() && p.ReOrderLevel == null) > 0;

            //    try
            //    {
            //        if (exist)
            //        {
            //            int i = entity.Items.FirstOrDefault(p => p.Code.Trim() == _item.Code.Trim() && p.ReOrderLevel == null).ID;
            //            Item item = entity.Items.Find(i);
            //            item.SubCategoryID = _item.SubCategoryID;
            //            item.Barcode = _item.Barcode;
            //            item.LeadTime = _item.LeadTime;
            //            item.DailyAverageUsage = _item.DailyAverageUsage;
            //            item.ReOrderLevel = _item.ReOrderLevel;

            //        }
            //        else
            //        {
            //          //  Console.WriteLine(_item.Code.Trim());

            //        }
            //        entity.SaveChanges();
            //    }
            //    catch (Exception e) { e.ToString(); }
            //}

            //foreach (var _item in lstInventoryDTO.Select(p => p.inventory))
            //{
            //    bool exist = entity.Inventories.Count(p => p.Description.Trim() == _item.Description.Trim()
            //                                             && p.LocationCode == _item.LocationCode
            //                                             && p.ItemCode.Trim() == _item.ItemCode.Trim()) > 0;


            //    try
            //    {
            //        if (exist)
            //        {
            //            int i = entity.Inventories.FirstOrDefault(p => p.Description.Trim() == _item.Description.Trim()
            //                                             && p.LocationCode == _item.LocationCode
            //                                             && p.ItemCode == _item.ItemCode).ID;

            //            Inventory inventory = entity.Inventories.Find(i);
            //            //item.DescriptionPurchase = _item.DescriptionPurchase;
            //            inventory.InStock = _item.InStock;
            //          //  inventory.Category = _item.Category;
            //            // item.UnitOfMeasurementID = _item.UnitOfMeasurementID;


            //        }
            //        else
            //        {
            //            entity.Inventories.Add(_item);

            //        }
            //        entity.SaveChanges();
            //    }
            //    catch (Exception e) { e.ToString(); }

            //}


            entity.SaveChanges();

                Console.WriteLine("Done!");

                Console.ReadLine();
            }
    }
}
