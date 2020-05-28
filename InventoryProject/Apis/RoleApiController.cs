using InventoryProject.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace InventoryProject.Apis
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleApiController : Controller
    {
        private readonly ApplicationDbContext _context;
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<IdentityUser> _userManager;

        public RoleApiController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [Authorize]
        [HttpGet]
        [Route("RoleApi/GetListOfRoles")]
        public ActionResult<IEnumerable<IdentityRole>> GetListOfRoles()
        {
            var result = _roleManager.Roles.ToList();

            return result;
        }

        [HttpPost]
        [Route("RoleApi/CreateRole")]
        public async Task<string> CreatRole(string rolename)
        {
            var rows = _context.Roles.Count();
            IdentityRole role = new IdentityRole();
            role.Id = Math.Pow(2, rows).ToString();
            role.Name = rolename;
            await _roleManager.CreateAsync(role);

            return "RoleCreated";
        }
    }
}