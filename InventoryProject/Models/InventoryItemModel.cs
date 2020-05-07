using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryProject.Models
{
    public class InventoryItemModel
    {
        [Key]
        public int ItemID { get; set; }

        public string SerialNumber { get; set; }

        public string ItemName { get; set; }

        [DataType(DataType.Date)]
        public DateTime PurchaseDate { get; set; }

        [DataType(DataType.Time)]
        public DateTime PurchaseHours { get; set; }

        public string History { get; set; }

        public string BillInfo { get; set; }

        public string Status { get; set; }

        public string Category { get; set; }

        public string UserGroup { get; set; }

        public string AssignedTo { get; set; }

        public DateTime CreatedAt { get; set; }

        [NotMapped]
        [Display(Name = "Item Image")]
        public IFormFile ItemImage { get; set; }

        public string QrCodeName { get; set; }

        public string ImageName { get; set; }
    }
}