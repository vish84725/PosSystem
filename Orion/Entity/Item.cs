using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Entity
{
    public class Item
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string Batch { get; set; }
        public int GroupId { get; set; }
        public PrimaryGroup Group { get; set; }
        public int SecondaryGroupId { get; set; }
        public SecondaryGroup SecondaryGroup { get; set; }
        public int ThirdGroupId { get; set; }
        public ThirdGroup ThirdGroup { get; set; }
        public string Barcode { get; set; }
        public int WharehousId { get; set; }
        public string PhotoFileName { get; set; }
    }
}
