using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryProject.Data;
using InventoryProject.Models;

namespace InventoryProject.Apis
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillInfoApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BillInfoApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/BillInfoModels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BillInfoModel>>> GetBillInfo()
        {
            return await _context.BillInfo.ToListAsync();
        }

        // GET: api/BillInfoModels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BillInfoModel>> GetBillInfoModel(int id)
        {
            var billInfoModel = await _context.BillInfo.FindAsync(id);

            if (billInfoModel == null)
            {
                return NotFound();
            }

            return billInfoModel;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBillInfoModel(int id, BillInfoModel billInfoModel)
        {
            if (id != billInfoModel.BillId)
            {
                return BadRequest();
            }

            _context.Entry(billInfoModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BillInfoModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<BillInfoModel>> PostBillInfoModel(BillInfoModel billInfoModel)
        {
            _context.BillInfo.Add(billInfoModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBillInfoModel", new { id = billInfoModel.BillId }, billInfoModel);
        }

        // DELETE: api/BillInfoModels/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<BillInfoModel>> DeleteBillInfoModel(int id)
        {
            var billInfoModel = await _context.BillInfo.FindAsync(id);
            if (billInfoModel == null)
            {
                return NotFound();
            }

            _context.BillInfo.Remove(billInfoModel);
            await _context.SaveChangesAsync();

            return billInfoModel;
        }

        private bool BillInfoModelExists(int id)
        {
            return _context.BillInfo.Any(e => e.BillId == id);
        }
    }
}