﻿using InventoryProject.Data;
using InventoryProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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
        public async Task<IActionResult> Index()
        {
            return View(await _context.InventoryItems.ToListAsync());
        }

        // GET: InventoryItems/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: InventoryItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: InventoryItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemID,ItemName,PurchaseDate,PurchaseHours,History,BillInfo,Status,CreatedAt,ItemImage,UserGroup,Category,AssignedTo")] InventoryItems inventoryItems)
        {
            if (ModelState.IsValid)
            {
                //Save image to wwwroot/image
                string wwwRootPath = webHostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(inventoryItems.ItemImage.FileName);
                string extension = Path.GetExtension(inventoryItems.ItemImage.FileName);
                inventoryItems.ImageName = fileName = inventoryItems.ItemID + DateTime.Now.ToString("_yyyyMMdd_HHmmss") + extension;
                string path = Path.Combine(wwwRootPath + "/Images/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await inventoryItems.ItemImage.CopyToAsync(fileStream);
                }

                inventoryItems.CreatedAt = DateTime.UtcNow;
                _context.Add(inventoryItems);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(inventoryItems);
        }

        // GET: InventoryItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
        public async Task<IActionResult> Edit(int id, [Bind("ItemID,ItemName,PurchaseDate,PurchaseHours,History,BillInfo,Status,UserGroup,Category,AssignedTo")] InventoryItems inventoryItems)
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
                return RedirectToAction(nameof(Index));
            }
            return View(inventoryItems);
        }

        // GET: InventoryItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inventoryItems = await _context.InventoryItems.FindAsync(id);
            _context.InventoryItems.Remove(inventoryItems);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InventoryItemsExists(int id)
        {
            return _context.InventoryItems.Any(e => e.ItemID == id);
        }
    }
}