using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResturantManagementSystem.Data;
using ResturantManagementSystem.ViewModels;

namespace ResturantManagementSystem.Controllers
{
    [Authorize(Roles ="SuperAdmin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public RolesController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        [HttpGet("/roles")]
        public async Task<IActionResult> Index()
        {
            var roles = await roleManager.Roles.ToListAsync();
            var rolesVM = roles.Select(role => new RoleVM
            {
                Id = role.Id,
                Name = role.Name!
            }).ToList();

            return View("Index", rolesVM);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleVM rolevm)
        {
            if (!ModelState.IsValid)
            {
                return View(rolevm);
            }
            var result = await roleManager.CreateAsync(new IdentityRole(rolevm.Name));

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update([FromRoute] string Id)
        {
            if (Id is null)
            {
                return NotFound();
            }

            var roleToEdit = await roleManager.FindByIdAsync(Id);
            var roleVM = new RoleVM()
            {
                Id= roleToEdit?.Id,
                Name= roleToEdit.Name!,
            };

            if (roleToEdit is null)
            {
                return NotFound();
            }
            return View("Update", roleVM);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] RoleVM rolevm)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Validation failed");
                return View(rolevm);
            }

            var roleToEdit = await roleManager.FindByIdAsync(rolevm.Id);
            if(roleToEdit is null)
            {
                return NotFound();
            }
            
            roleToEdit!.Name = rolevm.Name;

            var result = await roleManager.UpdateAsync(roleToEdit);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(rolevm);
            }
            
            return Json(new { success = true, redirectUrl = Url.Action("Index") });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromRoute] string Id)
        {
            Console.WriteLine("Worked ",Id);
            var role = await roleManager.FindByIdAsync(Id);
            if(role is null)
            {
                return NotFound(Id);
            }

            var deletRole = await roleManager.DeleteAsync(role);
            if (!deletRole.Succeeded)
            {
                ModelState.AddModelError("Error", "Something went wrong please try again later");
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
