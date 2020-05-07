using InventoryProject.Data;
using InventoryProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryProject.Controllers
{
    [Authorize]
    public class InventoryItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public InventoryItemsController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
        }

        // GET: InventoryItems
        public async Task<IActionResult> InventoryIndex()
        {
            return View(await _context.InventoryItems.ToListAsync());
        }

        // GET: InventoryItems/Details/5
        public async Task<IActionResult> ItemDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            await QrCode(id);
            var inventoryItems = await _context.InventoryItems
                .FirstOrDefaultAsync(m => m.ItemID == id);
            if (inventoryItems == null)
            {
                return NotFound();
            }
            return View(inventoryItems);
        }

        // GET: InventoryItems/Create
        public IActionResult AddItemToInventory()
        {
            return View();
        }

        // POST: InventoryItems/AddItemToInventory
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItemToInventory([Bind("ItemID,SerialNumber,ItemName,PurchaseDate,PurchaseHours,History,BillInfo,Status,CreatedAt,ItemImage,QrCodeName,UserGroup,Category,AssignedTo")] InventoryItemModel inventoryItems)
        {
            if (ModelState.IsValid)
            {
                //Save image to wwwroot/image
                string wwwRootPath = webHostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(inventoryItems.ItemImage.FileName);
                string extension = Path.GetExtension(inventoryItems.ItemImage.FileName);
                inventoryItems.ImageName = fileName = inventoryItems.SerialNumber + DateTime.Now.ToString("_yyyyMMdd_HHmmss") + extension;
                string path = Path.Combine(wwwRootPath + "/Images/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await inventoryItems.ItemImage.CopyToAsync(fileStream);
                }

                inventoryItems.CreatedAt = DateTime.UtcNow;
                inventoryItems.QrCodeName = inventoryItems.SerialNumber + "_" + inventoryItems.ItemName + ".jpg";
                _context.Add(inventoryItems);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(InventoryIndex));
            }
            return View(inventoryItems);
        }

        // GET: InventoryItems/Edit/5
        public async Task<IActionResult> EditItem(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventoryItems = await _context.InventoryItems.FindAsync(id);
            if (inventoryItems == null)
            {
                return NotFound();
            }
            return View(inventoryItems);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditItem(int id, [Bind("ItemID,ItemName,SerialNumber,PurchaseDate,PurchaseHours,History,BillInfo,Status,UserGroup,Category,AssignedTo")] InventoryItemModel inventoryItems)
        {
            if (id != inventoryItems.ItemID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inventoryItems);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventoryItemsExists(inventoryItems.ItemID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(InventoryIndex));
            }
            return View(inventoryItems);
        }

        // GET: InventoryItems/Delete/5
        public async Task<IActionResult> DeleteItem(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventoryItems = await _context.InventoryItems
                .FirstOrDefaultAsync(m => m.ItemID == id);
            if (inventoryItems == null)
            {
                return NotFound();
            }

            return View(inventoryItems);
        }

        // POST: InventoryItems/Delete/5
        [HttpPost, ActionName("DeleteItem")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inventoryItems = await _context.InventoryItems.FindAsync(id);
            _context.InventoryItems.Remove(inventoryItems);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(InventoryIndex));
        }

        private bool InventoryItemsExists(int id)
        {
            return _context.InventoryItems.Any(e => e.ItemID == id);
        }

        public async Task<IActionResult> QrCode(int? id)
        {
            var info = await _context.InventoryItems.FirstOrDefaultAsync(m => m.ItemID == id);

            string qrinfo = "Serial Number :- " + info.SerialNumber + " ItemName:-" + info.ItemName;
            QRCodeGenerator qrGenerator = new QRCodeGenerator();

            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrinfo, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            var bitmapBytes = BitmapToBytes(qrCodeImage);//Convert bitmap into a byte array
            string wwwRootPath = webHostEnvironment.WebRootPath;
            string filename = info.QrCodeName;
            string path = Path.Combine(wwwRootPath + "/Qrcodes/", filename);

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                qrCodeImage.Save(fileStream, System.Drawing.Imaging.ImageFormat.Png);
            }

            return View("ItemDetails"); //Return as file result
        }

        // This method is for converting bitmap into a byte array
        private byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
    }
}