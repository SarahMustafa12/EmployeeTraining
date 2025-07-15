using InternalTraining.Models;
using InternalTraining.Unit_of_Work;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InternalTraining.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserController(IUnitOfWork unitOfWork,UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }
        public async Task<IActionResult> Index(bool? isAdmin = false)
        {
            var allUsers = unitOfWork.ApplicationUsers.Get();
            var filteredUsers = new List<ApplicationUser>();

            foreach (var user in allUsers)
            {
                var roles = await userManager.GetRolesAsync(user);

                if (isAdmin == true)
                {
                    if (roles.Contains("Admin"))
                    {
                        filteredUsers.Add(user);
                    }
                }
                else
                {
                    filteredUsers.Add(user);
                }
            }

            return View(filteredUsers);
        }

        [HttpPost]
        public async Task<IActionResult> BlockUser(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.LockoutEnabled = true;
                user.LockoutEnd = DateTimeOffset.MaxValue;
                await userManager.UpdateAsync(user);
            }
            return RedirectToAction("Index", "User", new { area = "Admin" });
        }

        [HttpPost]
        public async Task<IActionResult> UnblockUser(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.LockoutEnd = null;
                await userManager.UpdateAsync(user);
            }
            return RedirectToAction("Index","User", new {area ="Admin"});
        }

    }
}
