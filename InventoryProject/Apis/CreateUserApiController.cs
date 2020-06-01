using InventoryProject.Apis.ViewModels;
using InventoryProject.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace InventoryProject.Apis
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateUserApiController : Controller
    {
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;

        public CreateUserApiController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, IEmailSender emailSender)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [Route("api/CreateUser")]
        [HttpPost]
        public async Task<string> CreateUser([FromBody]CreateUserViewModel User)
        {
            var user = new IdentityUser { UserName = User.EmailId, Email = User.EmailId };
            var result = await _userManager.CreateAsync(user, User.Password);

            if (result.Succeeded)
            {
                int tempId = 0;
                foreach (var role1 in User.AssignRole)
                {
                    var role = _roleManager.FindByIdAsync(role1).Result;
                    tempId += Int32.Parse(role.Id);
                    await _userManager.AddToRoleAsync(user, role.Name);
                }

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId = user.Id, code = code },
                    protocol: Request.Scheme);

                await _emailSender.SendEmailAsync(User.EmailId, "Confirm your email",
                                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
            }
            return "User Created";
        }
    }
}