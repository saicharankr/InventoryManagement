using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryProject.Models
{
    public class BillInfoModel
    {
        [Key]
        public int BillId { get; set; }

        [Display(Name = "Bill Number")]
        public int BillNumber { get; set; }

        [NotMapped]
        [Display(Name = "Upload Bill")]
        public IFormFile UploadBill { get; set; }

        public string BillName { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Bill Date")]
        public DateTime BillDate { get; set; }

        [Display(Name = "Purchased By")]
        public string PurchasedBy { get; set; }

        [Display(Name = "Approved By")]
        public string ApprovedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        [NotMapped]
        public List<InventoryItemModel> InventoryItems { get; set; } = new List<InventoryItemModel>();
    }
}