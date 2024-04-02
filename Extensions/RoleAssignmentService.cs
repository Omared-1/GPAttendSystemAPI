using GPAttendSystemAPI.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace GPAttendSystemAPI.Extensions
{
    public class RoleAssignmentService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleAssignmentService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        private async Task AssignRoleToUser(AppUser DrEmanTest, string DataMining)
        {
            if (!await _roleManager.RoleExistsAsync(DataMining))
            {
                // Create the role if it doesn't exists
                await _roleManager.CreateAsync(new IdentityRole(DataMining));
            }

            // Assign the role to the user
            await _userManager.AddToRoleAsync(DrEmanTest, DataMining);
        }
    }
}