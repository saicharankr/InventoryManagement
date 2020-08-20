using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using InventoryProject.Models;
using Microsoft.AspNetCore.Identity;

namespace InventoryProject.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<InventoryProject.Models.InventoryItemModel> InventoryItems { get; set; }

        public DbSet<InventoryProject.Models.BillInfoModel> BillInfo { get; set; }
    }
}