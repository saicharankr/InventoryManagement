using InventoryProject.Data;
using InventoryProject.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryProject.Controllers
{
    public class BillInfoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public BillInfoController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
        }

        // GET: BillInfo
        public async Task<IActionResult> BillIndex()
        {
            return View(await _context.BillInfo.ToListAsync());
        }

        // GET: BillInfo/Details/5
        public async Task<IActionResult> BillDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billInfo = await _context.BillInfo
                .FirstOrDefaultAsync(m => m.BillId == id);
            if (billInfo == null)
            {
                return NotFound();
            }

            return View(billInfo);
        }

        // GET: BillInfo/Create
        public IActionResult AddBillToDb()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBillToDb([Bind("BillId,BillNumber,UploadBill,BillName,NumberOfItems,Items,BillDate,PurchasedBy,ApprovedBy")] BillInfoModel billInfo)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = webHostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(billInfo.UploadBill.FileName);
                string extension = Path.GetExtension(billInfo.UploadBill.FileName);
                billInfo.BillName = fileName = billInfo.BillNumber + DateTime.Now.ToString("_yyyyMMdd_HHmmss") + extension;
                string path = Path.Combine(wwwRootPath + "/Images/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await billInfo.UploadBill.CopyToAsync(fileStream);
                }

                billInfo.CreatedAt = DateTime.UtcNow;
                _context.Add(billInfo);
                await _context.SaveChangesAsync();

                foreach (InventoryItemModel item in billInfo.InventoryItems)
                {
                    item.CreatedAt = DateTime.UtcNow;
                    item.QrCodeName = item.SerialNumber + "_" + item.ItemName + ".jpg";
                    _context.InventoryItems.Add(item);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(BillIndex));
            }
            return View(billInfo);
        }

        // GET: BillInfo/Edit/5
        public async Task<IActionResult> BillEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billInfo = await _context.BillInfo.FindAsync(id);
            if (billInfo == null)
            {
                return NotFound();
            }
            return View(billInfo);
        }

        // POST: BillInfo/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BillEdit(int id, [Bind("BillId,BillNumber,Upload Bill,NumberOfItems,Items,BillDate,PurchasedBy,ApprovedBy")] BillInfoModel billInfo)
        {
            if (id != billInfo.BillId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(billInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BillInfoExists(billInfo.BillId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(BillIndex));
            }
            return View(billInfo);
        }

        // GET: BillInfo/Delete/5
        public async Task<IActionResult> DeleteBill(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billInfo = await _context.BillInfo
                .FirstOrDefaultAsync(m => m.BillId == id);
            if (billInfo == null)
            {
                return NotFound();
            }

            return View(billInfo);
        }

        // POST: BillInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var billInfo = await _context.BillInfo.FindAsync(id);
            _context.BillInfo.Remove(billInfo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(BillIndex));
        }

        private bool BillInfoExists(int id)
        {
            return _context.BillInfo.Any(e => e.BillId == id);
        }
    }
}