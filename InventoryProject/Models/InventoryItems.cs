using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryProject.Models
{
    public class InventoryItems
    {
        [Key]
        public int ItemID { get; set; }

        public string ItemName { get; set; }

        [DataType(DataType.Date)]
        public DateTime PurchaseDate { get; set; }

        [DataType(DataType.Time)]
        public DateTime PurchaseHours { get; set; }

        public string History { get; set; }

        public string BillInfo { get; set; }
    }
}