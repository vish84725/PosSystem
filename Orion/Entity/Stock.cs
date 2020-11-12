using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Entity
{
    public class Stock
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public int SecondaryGroupId { get; set; }
        public int ThirdGroupId { get; set; }
        public int Quantity { get; set; }
        public int WharehousId { get; set; }
        public int ShelfId { get; set; }
        public string ExpiryDate { get; set; }
        public string Expiry { get; set; }
        public string UnitOfMeaure { get; set; }
        public double Cost { get; set; }
        public double Price { get; set; }
        public double ReorderPoint { get; set; }
        public string VatApplicable { get; set; }
    }
}
