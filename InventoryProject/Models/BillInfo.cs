using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryProject.Models
{
    public class BillInfo
    {
        [Key]
        public int BillId { get; set; }

        [Display(Name = "Bill Number")]
        public int BillNumber { get; set; }

        [NotMapped]
        [Display(Name = "Upload Bill")]
        public IFormFile UploadBill { get; set; }

        public string BillName { get; set; }

        [Display(Name = "Number of Items in bill")]
        public int NumberOfItems { get; set; }

        [NotMapped]
        public List<string> Items { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Bill Date")]
        public DateTime BillDate { get; set; }

        [Display(Name = "Purchased By")]
        public string PurchasedBy { get; set; }

        [Display(Name = "Approved By")]
        public string ApprovedBy { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}