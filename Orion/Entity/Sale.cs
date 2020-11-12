using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Entity
{
    public class Sale
    {
        public int Id { get; set; }
        public int SalesId { get; set; }
        public DateTime SalesDate { get; set; }
        public int ItemId { get; set; }
        public int StockId { get; set; }
        public double Quantity { get; set; }
        public float Price { get; set; }
        public float TotalPrice { get; set; }
        public float Cost { get; set; }
        public float TotalCost { get; set; }
        public float Vat { get; set; }
        public float TotalVat { get; set; }
        public string ExprDate { get; set; }
        public string Terminal { get; set; }
    }
}
