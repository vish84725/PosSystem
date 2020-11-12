using Orion.Entity;
using Orion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Services
{
    public class StockService : IStockService
    {
        public bool AddSale(Sale sale)
        {
            try
            {
                clsUtility.ExecuteSQLQuery(@" INSERT INTO Sales(SALES_ID,Sales_Date,ITEM_ID,QTY,Price,TotalPrice,Cost,TotalCost,Vat,TotalVat,ExprDate,STOCK_ID, Terminal) VALUES 
                                                                   ( '" + sale.SalesId + @"' ,'" + 
                                                                          sale.SalesDate.ToString("yyyy-MM-dd") + @"' , '" + 
                                                                          sale.ItemId + @"', '" + 
                                                                          sale.Quantity + @"', '" + 
                                                                          sale.Price + @"','" + 
                                                                          sale.TotalPrice + @"','" + 
                                                                          sale.Cost + @"','" + 
                                                                          sale.TotalCost + @"','" + 
                                                                          sale.Vat + @"','" + 
                                                                          sale.TotalVat + @"', '" +
                                                                          sale.ExprDate + @"', '" +
                                                                          sale.StockId + @"', 'POS') ");
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<PrimaryGroup> GetAllGroups()
        {
            try
            {
                List<PrimaryGroup> primaryGroups = new List<PrimaryGroup>();
                clsUtility.ExecuteSQLQuery("SELECT * from ItemGroup");
                if (clsUtility.sqlDT.Rows.Count > 0)
                {
                    for(int i = 0; i < clsUtility.sqlDT.Rows.Count; i++)
                    {
                        primaryGroups.Add(new PrimaryGroup()
                        {
                            GroupName = clsUtility.sqlDT.Rows[i]["GROUP_NAME"].ToString(),
                            Id = Convert.ToInt32(clsUtility.sqlDT.Rows[i]["GROUP_ID"]),
                        });
                    }
                }
                return primaryGroups;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<SecondaryGroup> GetAllSecondaryGroups()
        {
            try
            {
                List<SecondaryGroup> secondaryGroups = new List<SecondaryGroup>();
                clsUtility.ExecuteSQLQuery("SELECT * from ItemSecondoryGroup");
                if (clsUtility.sqlDT.Rows.Count > 0)
                {
                    for (int i = 0; i < clsUtility.sqlDT.Rows.Count; i++)
                    {
                        secondaryGroups.Add(new SecondaryGroup()
                        {
                            SecondaryGroupName = clsUtility.sqlDT.Rows[i]["SECONDARY_GROUP_NAME"].ToString(),
                            Id = Convert.ToInt32(clsUtility.sqlDT.Rows[i]["SECONDARY_GROUP_ID"]),
                            GroupId = Convert.ToInt32(clsUtility.sqlDT.Rows[i]["GROUP_ID"]),
                        });
                    }
                }
                return secondaryGroups;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<ThirdGroup> GetAllThirdGroups()
        {
            try
            {
                List<ThirdGroup> thirdGroups = new List<ThirdGroup>();
                clsUtility.ExecuteSQLQuery("SELECT * from ItemThirdGroup");
                if (clsUtility.sqlDT.Rows.Count > 0)
                {
                    for (int i = 0; i < clsUtility.sqlDT.Rows.Count; i++)
                    {
                        thirdGroups.Add(new ThirdGroup()
                        {
                            ThirdGroupName = clsUtility.sqlDT.Rows[i]["THIRD_GROUP_NAME"].ToString(),
                            Id = Convert.ToInt32(clsUtility.sqlDT.Rows[i]["THIRD_GROUP_ID"]),
                            SecondaryGroupId = Convert.ToInt32(clsUtility.sqlDT.Rows[i]["SECONDARY_GROUP_ID"]),
                        });
                    }
                }
                return thirdGroups;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Item GetItemByBarcode(string barcode)
        {
            try
            {
                clsUtility.ExecuteSQLQuery("SELECT * FROM ItemInformation WHERE  (Barcode LIKE '%" + barcode + "%')   ");
                if (clsUtility.sqlDT.Rows.Count > 0)
                {
                    return new Item()
                    {
                        Id = Convert.ToInt32(clsUtility.sqlDT.Rows[0]["ITEM_ID"]),
                        Barcode = clsUtility.sqlDT.Rows[0]["Barcode"].ToString(),
                        Batch = clsUtility.sqlDT.Rows[0]["Batch"].ToString(),
                        GroupId = Convert.ToInt32(clsUtility.sqlDT.Rows[0]["GROUP_ID"]),
                        SecondaryGroupId = Convert.ToInt32(clsUtility.sqlDT.Rows[0]["SECONDARY_GROUP_ID"]),
                        ThirdGroupId = Convert.ToInt32(clsUtility.sqlDT.Rows[0]["THIRD_GROUP_ID"]),
                        ItemName = clsUtility.sqlDT.Rows[0]["ItemName"].ToString(),
                        PhotoFileName = clsUtility.sqlDT.Rows[0]["PhotoFileName"].ToString(),
                        WharehousId = Convert.ToInt32(clsUtility.sqlDT.Rows[0]["WarehouseID"]),
                    };
                }
                return null;
            }
            catch(Exception)
            {
                return null;
            }
        }

        public Item GetItemByItemId(int itemId)
        {
            try
            {
                clsUtility.ExecuteSQLQuery("SELECT * FROM ItemInformation WHERE ITEM_ID = " + itemId);
                if (clsUtility.sqlDT.Rows.Count > 0)
                {
                    return new Item()
                    {
                        Id = Convert.ToInt32(clsUtility.sqlDT.Rows[0]["ITEM_ID"]),
                        Barcode = clsUtility.sqlDT.Rows[0]["Barcode"].ToString(),
                        Batch = clsUtility.sqlDT.Rows[0]["Batch"].ToString(),
                        GroupId = Convert.ToInt32(clsUtility.sqlDT.Rows[0]["GROUP_ID"]),
                        SecondaryGroupId = Convert.ToInt32(clsUtility.sqlDT.Rows[0]["SECONDARY_GROUP_ID"]),
                        ThirdGroupId = Convert.ToInt32(clsUtility.sqlDT.Rows[0]["THIRD_GROUP_ID"]),
                        ItemName = clsUtility.sqlDT.Rows[0]["ItemName"].ToString(),
                        PhotoFileName = clsUtility.sqlDT.Rows[0]["PhotoFileName"].ToString(),
                        WharehousId = Convert.ToInt32(clsUtility.sqlDT.Rows[0]["WarehouseID"]),
                    };
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<Sale> GetSales(int? itemId, int? stockId, int? salesId)
        {
            try
            {
                List<Sale> sales = new List<Sale>();
                if(itemId != null && stockId != null && salesId != null)
                {
                    clsUtility.ExecuteSQLQuery(" SELECT * FROM Sales  WHERE ITEM_ID = '" + itemId + "' AND SALES_ID = '" + salesId + "' AND STOCK_ID= " + stockId);
                }
                else
                {
                    clsUtility.ExecuteSQLQuery(" SELECT * FROM Sales");
                }
   
                if (clsUtility.sqlDT.Rows.Count > 0)
                {
                    for (int i = 0; i < clsUtility.sqlDT.Rows.Count; i++)
                    {
                        sales.Add(new Sale()
                        {
                            Id = Convert.ToInt32(clsUtility.sqlDT.Rows[i]["ID"]),
                            SalesId = Convert.ToInt32(clsUtility.sqlDT.Rows[i]["SALES_ID"]),
                            SalesDate = DateTime.Parse(clsUtility.sqlDT.Rows[i]["Sales_Date"].ToString()),
                            ItemId = Convert.ToInt32(clsUtility.sqlDT.Rows[i]["ITEM_ID"]),
                            StockId = Convert.ToInt32(clsUtility.sqlDT.Rows[i]["STOCK_ID"]),
                            Quantity = Convert.ToDouble(clsUtility.sqlDT.Rows[i]["QTY"]),
                            Price = Convert.ToSingle(clsUtility.sqlDT.Rows[i]["Price"]),
                            TotalPrice = Convert.ToSingle(clsUtility.sqlDT.Rows[i]["TotalPrice"]),
                            Cost = Convert.ToSingle(clsUtility.sqlDT.Rows[i]["Cost"]),
                            TotalCost = Convert.ToSingle(clsUtility.sqlDT.Rows[i]["TotalCost"]),
                            Vat = Convert.ToSingle(clsUtility.sqlDT.Rows[i]["Vat"]),
                            TotalVat = Convert.ToSingle(clsUtility.sqlDT.Rows[i]["TotalVat"]),
                            ExprDate = clsUtility.sqlDT.Rows[i]["ExprDate"].ToString(),
                            Terminal = clsUtility.sqlDT.Rows[i]["Terminal"].ToString(),
                        });
                    }
                }
                return sales;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Stock GetStockById(int groupId, int secondaryGroupId, int thirdGroupId)
        {
            try
            {
                clsUtility.ExecuteSQLQuery(@" SELECT * FROM  Stock  WHERE GROUP_ID ='" + groupId + @"' AND 
                                                                    SECONDARY_GROUP_ID ='" + secondaryGroupId + @"' AND 
                                                                    THIRD_GROUP_ID = '" + thirdGroupId + "'");
                if (clsUtility.sqlDT.Rows.Count > 0)
                {
                    return new Stock()
                    {
                        Id = Convert.ToInt32(clsUtility.sqlDT.Rows[0]["STOCK_ID"]),
                        Quantity = Convert.ToInt32(clsUtility.sqlDT.Rows[0]["Quantity"]),
                        ExpiryDate = clsUtility.sqlDT.Rows[0]["ExpiryDate"].ToString(),
                        Expiry = clsUtility.sqlDT.Rows[0]["Expiry"].ToString(),
                        ShelfId = Convert.ToInt32(clsUtility.sqlDT.Rows[0]["SHELF_ID"]),
                        GroupId = Convert.ToInt32(clsUtility.sqlDT.Rows[0]["GROUP_ID"]),
                        SecondaryGroupId = Convert.ToInt32(clsUtility.sqlDT.Rows[0]["SECONDARY_GROUP_ID"]),
                        ThirdGroupId = Convert.ToInt32(clsUtility.sqlDT.Rows[0]["THIRD_GROUP_ID"]),
                        UnitOfMeaure = clsUtility.sqlDT.Rows[0]["UnitOfMeasure"].ToString(),
                        Price = Convert.ToDouble(clsUtility.sqlDT.Rows[0]["Price"]),
                        Cost = Convert.ToDouble(clsUtility.sqlDT.Rows[0]["Cost"]),
                        VatApplicable = clsUtility.sqlDT.Rows[0]["VAT_Applicable"].ToString(),
                        WharehousId = Convert.ToInt32(clsUtility.sqlDT.Rows[0]["WarehouseID"]),
                        ReorderPoint = Convert.ToDouble(clsUtility.sqlDT.Rows[0]["ReorderPoint"])
                    };
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public double GetVAT()
        {
            try
            {
                clsUtility.ExecuteSQLQuery("SELECT * FROM Vat");
                if (clsUtility.sqlDT.Rows.Count > 0)
                {
                    return Convert.ToDouble(clsUtility.sqlDT.Rows[0]["Vat"]);
                }
                else
                {
                    return 0;
                }
            }
            catch(Exception)
            {
                return 0;
            }
 
        }

        public bool UpdateSale(double qty, double price, double cost, double vat, string itemId, string salesId, int stockId)
        {
            try
            {
                clsUtility.ExecuteSQLQuery(" UPDATE  Sales SET QTY = QTY + '" + qty + "' ,TotalPrice= TotalPrice +'" + qty * price + "' ,TotalCost= TotalCost +'" + qty * cost + "', TotalVat= TotalVat + '" + qty * vat + "' " +
                                                   " WHERE ITEM_ID = '" + itemId + "' AND SALES_ID = '" + salesId + "' AND STOCK_ID = '"+ stockId + "'");
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public bool UpdateStock(int groupId, int secondaryGroupId, int thirdGroupId, int wharehouseId, double quantity )
        {
            try
            {
                clsUtility.ExecuteSQLQuery(" UPDATE   Stock SET Quantity = Quantity - '" + quantity + "' WHERE GROUP_ID ='" + groupId + "' AND SECONDARY_GROUP_ID='" + secondaryGroupId + "' AND THIRD_GROUP_ID='" + thirdGroupId + "' AND WarehouseID='" + wharehouseId + "'  ");
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
