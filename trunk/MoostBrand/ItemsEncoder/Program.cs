using System;
using System.Collections.Generic;
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

        static void Main(string[] args)
        {
            MoostBrandEntities entity = new MoostBrandEntities();

            List<ItemDTO> lstItemDTO = new List<ItemDTO>();
            
            const Int32 BufferSize = 128;
            using (var fileStream = File.OpenRead(@"C:\mb_inventory.csv"))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                String line;
                int lineCnt = 1;
                while ((line = streamReader.ReadLine()) != null)
                {
                    if (lineCnt > 3)
                    {
                        string[] strLine = line.Split(',');

                        if (true) {
                            Item item = new Item();

                            string txtBrand = Convert.ToString(strLine[5]);
                            Brand brand = entity.Brands.FirstOrDefault(p => p.Code == txtBrand);
                            if (brand == null)
                            {
                                brand = new Brand
                                {
                                    Code = txtBrand,
                                    Description = txtBrand
                                };
                                entity.Brands.Add(brand);
                                entity.SaveChanges();
                            }

                            string txtCategory = Convert.ToString(strLine[7]);
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


                            string txtSubCategory = Convert.ToString(strLine[8]);
                            SubCategory subCategory = entity.SubCategories.FirstOrDefault(p => p.Code == txtSubCategory);
                            if (subCategory == null)
                            {
                                subCategory = new SubCategory
                                {
                                    Code = txtSubCategory,
                                    Description = txtSubCategory
                                };
                                entity.SubCategories.Add(subCategory);
                                entity.SaveChanges();
                            }

                            item.Year = Convert.ToString(strLine[0]).Trim();
                            item.Barcode = Convert.ToString(strLine[1]);
                            item.Code = Convert.ToString(strLine[2]);
                            item.Description = Convert.ToString(strLine[3]);

                            item.BrandID = brand.ID;

                            //status

                            item.CategoryID = category.ID;
                            item.SubCategoryID = subCategory.ID;

                            string txtLeadTime = Convert.ToString(strLine[19]).Trim();
                            item.LeadTime = txtLeadTime != "" ? Convert.ToInt32(txtLeadTime) : 0;

                            //item.Status
                            string pcs = Convert.ToString(strLine[21]);
                            try
                            {
                                item.Quantity = pcs != "" ? Convert.ToInt32(pcs) : 0;

                            }
                            catch (Exception)
                            {
                                item.Quantity = 0;
                            }
                            

                            ItemDTO itemDTO = new ItemDTO();
                            itemDTO.item = item;

                            lstItemDTO.Add(itemDTO);
                        }
                    }

                    lineCnt++;
                }
            }

            entity.Items.AddRange(lstItemDTO.Select(p => p.item));
            entity.SaveChanges();

            Console.WriteLine("Done!");

            Console.ReadLine();
        }
    }
}
