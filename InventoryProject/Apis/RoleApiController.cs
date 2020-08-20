using InventoryProject.Apis.ViewModels;
using InventoryProject.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        [HttpGet]
        [Route("RoleApi/GetListOfUserRoles")]
        public ActionResult<IEnumerable<IdentityUserRole<string>>> GetListOfUserRoles()
        {
            var result = _context.UserRoles.ToList();

            return result;
        }

        [HttpPost]
        public async Task<string> AssignOrRemoveRole(AssignOrRemoveRoleViewModel Assign)
        {
            foreach (string userId in Assign.AddIds ?? new string[] { })
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    await _userManager.AddToRoleAsync(user, Assign.RoleName);
                }
            }
            foreach (string userId in Assign.DeleteIds ?? new string[] { })
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    await _userManager.RemoveFromRoleAsync(user, Assign.RoleName);
                }
            }
            return "operation Done";
        }

        [HttpDelete]
        public async Task<string> DeleteRole(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await _roleManager.DeleteAsync(role);
                return result.ToString();
            }
            else
            {
                return "No role found";
            }
        }
    }
}