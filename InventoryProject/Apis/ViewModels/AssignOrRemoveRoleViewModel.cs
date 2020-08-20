using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryProject.Apis.ViewModels
{
    public class AssignOrRemoveRoleViewModel
    {
        public string RoleName { get; set; }

        public string RoleId { get; set; }
        public string[] AddIds { get; set; }
        public string[] DeleteIds { get; set; }
    }
}