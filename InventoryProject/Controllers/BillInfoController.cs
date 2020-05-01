using InventoryProject.Data;
using InventoryProject.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
        public async Task<IActionResult> Index()
        {
            return View(await _context.BillInfo.ToListAsync());
        }

        // GET: BillInfo/Details/5
        public async Task<IActionResult> Details(int? id)
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
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BillId,BillNumber,UploadBill,BillName,NumberOfItems,Items,BillDate,PurchasedBy,ApprovedBy")] BillInfo billInfo)
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
                return RedirectToAction(nameof(Index));
            }
            return View(billInfo);
        }

        // GET: BillInfo/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BillId,BillNumber,Upload Bill,NumberOfItems,Items,BillDate,PurchasedBy,ApprovedBy")] BillInfo billInfo)
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
                return RedirectToAction(nameof(Index));
            }
            return View(billInfo);
        }

        // GET: BillInfo/Delete/5
        public async Task<IActionResult> Delete(int? id)
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
            return RedirectToAction(nameof(Index));
        }

        private bool BillInfoExists(int id)
        {
            return _context.BillInfo.Any(e => e.BillId == id);
        }
    }
}