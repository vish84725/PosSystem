using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orion.Entity;

namespace Orion.Interfaces
{
    public interface IStockService
    {
        Item GetItemByBarcode(string barcode);
        Item GetItemByItemId(int itemId);
        Stock GetStockById(int groupId, int secondaryGroupId, int thirdGroupId);
        List<PrimaryGroup> GetAllGroups();
        List<SecondaryGroup> GetAllSecondaryGroups();
        List<ThirdGroup> GetAllThirdGroups();
        double GetVAT();
        List<Sale> GetSales(int? itemId, int? stockId, int? salesId);
        bool UpdateSale(double qty, double price, double cost, double vat, string itemId, string salesId, int stockId);
        bool AddSale(Sale sale);
        bool UpdateStock(int groupId, int secondaryGroupId, int thirdGroupId,int wharehouseId, double quantity);
    }
}
