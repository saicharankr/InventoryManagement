using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryProject.Data;
using InventoryProject.Models;
using InventoryProject.Apis.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace InventoryProject.Apis
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryItemApiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InventoryItemApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/InventoryItemApi
        [HttpGet]
        [Route("InventoryItemApi")]
        public async Task<ActionResult<IEnumerable<InventoryItemModel>>> GetInventoryItems()
        {
            return await _context.InventoryItems.ToListAsync();
        }

        [HttpGet]
        [Authorize]
        [Route("users")]
        public ActionResult<IEnumerable<UsersViewModel>> GetUser(int id)
        {
            var result = _context.Users.ToList();

            List<UsersViewModel> List = new List<UsersViewModel>();

            foreach (var user in result)
            {
                UsersViewModel usersView = new UsersViewModel();
                usersView.username = user.NormalizedUserName;
                List.Add(usersView);
            }
            return List;
        }

        [HttpPost]
        [Route("additem")]
        public string PostUser([FromBody] InventoryItemsViewModel AddItem)
        {
            InventoryItemModel inventoryItemModel = new InventoryItemModel();
            inventoryItemModel.ItemName = AddItem.ItemName;
            inventoryItemModel.PurchaseDate = AddItem.PurchaseDate;
            inventoryItemModel.PurchaseHours = AddItem.PurchaseHours;
            inventoryItemModel.SerialNumber = AddItem.SerialNumber;

            _context.InventoryItems.Add(inventoryItemModel);
            return "ItemAdded ";
        }

        // GET: api/InventoryItemApi/5
        [HttpGet]
        [Route("InventoryItemApi/{id}")]
        public async Task<ActionResult<InventoryItemModel>> GetInventoryItemModel(int id)
        {
            var inventoryItemModel = await _context.InventoryItems.FindAsync(id);

            if (inventoryItemModel == null)
            {
                return NotFound();
            }

            return inventoryItemModel;
        }

        [HttpPost]
        [Route("InventoryItemApi/update")]
        public async Task<ActionResult<InventoryItemModel>> PostInventoryItemModel(InventoryItemModel inventoryItemModel)
        {
            _context.InventoryItems.Add(inventoryItemModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInventoryItemModel", new { id = inventoryItemModel.ItemID }, inventoryItemModel);
        }

        //// DELETE: api/InventoryItemApi/5
        [HttpDelete]
        [Route("InventoryItemApi/Delete")]
        public async Task<ActionResult<InventoryItemModel>> DeleteInventoryItemModel(int id)
        {
            var inventoryItemModel = await _context.InventoryItems.FindAsync(id);
            if (inventoryItemModel == null)
            {
                return NotFound();
            }

            _context.InventoryItems.Remove(inventoryItemModel);
            await _context.SaveChangesAsync();

            return inventoryItemModel;
        }

        private bool InventoryItemModelExists(int id)
        {
            return _context.InventoryItems.Any(e => e.ItemID == id);
        }
    }
}