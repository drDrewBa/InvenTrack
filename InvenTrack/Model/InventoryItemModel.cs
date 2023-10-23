using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenTrack.Model
{
    public class InventoryItemModel
    {
        public int ID { get; set; }
        public string Product { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
    }
}
