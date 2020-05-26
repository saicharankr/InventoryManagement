using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryProject.Apis.ViewModels
{
    public class InventoryItemsViewModel
    {
        public string SerialNumber { get; set; }

        public string ItemName { get; set; }

        public DateTime PurchaseDate { get; set; }

        public DateTime PurchaseHours { get; set; }

        public string History { get; set; }

        public string BillInfo { get; set; }

        public string Status { get; set; }

        public string Category { get; set; }

        public string UserGroup { get; set; }

        public string AssignedTo { get; set; }

        //public IFormFile ItemImage { get; set; }

        //public string QrCodeName { get; set; }

        //public string ImageName { get; set; }
    }
}