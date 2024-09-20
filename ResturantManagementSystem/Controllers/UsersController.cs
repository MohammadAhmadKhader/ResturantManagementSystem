using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResturantManagementSystem.Data;
using ResturantManagementSystem.Repositories;
using ResturantManagementSystem.ViewModels;
using System;
using ResturantManagementSystem.Interfaces.IRepository;
using Microsoft.AspNetCore.Identity.UI.V5.Pages.Internal;
using ResturantManagementSystem.Helpers;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Authorization;

namespace ResturantManagementSystem.Controllers
{
    [Authorize(Roles ="SuperAdmin")]
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IUsersRepository usersRepository;
		private readonly SignInManager<IdentityUser> signInManager;
		private readonly RoleManager<IdentityRole> roleManager;
        private readonly UnitOfWork unitOfWork;

		public UsersController(UserManager<IdentityUser> userManager, ApplicationDbContext context, IMapper mapper, IUsersRepository usersRepository, 
            SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.context = context;
            this.usersRepository = usersRepository;
			this.signInManager = signInManager;
			this.roleManager = roleManager;
			this.mapper = mapper;
            this.unitOfWork = new UnitOfWork(context);
        }

        [HttpGet("/users")]
        public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] int limit = 9)
        {
            Console.WriteLine(333333);
            var result = await usersRepository.GetAll(page, limit);
            var rmsUsersList = mapper.Map<IEnumerable<IdentityUser>, IEnumerable<RmsUserVM>>(result.list);

            return View((page,limit,result.count, rmsUsersList.ToList()));
        }

        [HttpGet("/users/{Id}")]
		public async Task<IActionResult> Details(string Id)
		{
			var user = await userManager.FindByIdAsync(Id);
			if (user is null)
			{
				return NotFound();
			}
            var userVM = mapper.Map<IdentityUser, RmsUserVM>(user);
            var userRoles = await userManager.GetRolesAsync(user);
            userVM.Roles = userRoles.ToList();

            return View(userVM);
		}

		[HttpGet("/users/update-role/{Id}")]
		public async Task<IActionResult> UpdateRoles(string Id)
		{
            var user = await userManager.FindByIdAsync(Id);
			if (user is null)
			{
				return NotFound();
			}

			var userVM = mapper.Map<IdentityUser, RmsUserUpdateRolesVM>(user);
            var userRoles = await userManager.GetRolesAsync(user);
            userVM.SelectedRoles = userRoles.ToList();

            var selectListsHelper = new SelectListsHelper(context);
            userVM.Roles = await selectListsHelper.GetRolesList();

			return View(userVM);
		}

		[HttpPut("/users/update-role/{Id}")]
		public async Task<IActionResult> UpdateRoles(RmsUserUpdateRolesVM RmsUserVM)
		{
            if (!ModelState.IsValid)
            {
                var errors = ErrorsHelper.GetModelStateErrMessage(ModelState);
                return Json(new { success = false, errors = errors });
            }
            var user = await userManager.FindByIdAsync(RmsUserVM.Id);
            if(user == null)
            {
                return Json(new { success = false, errors = new List<string> { $"user with id: {RmsUserVM.Id} was not found"} });
            }

            var rolesListBeforeChange = await userManager.GetRolesAsync(user);
            var (isChanged,addedRoles,removedRoles) = UsersHelper.GetRolesChanges(rolesListBeforeChange.ToList() , RmsUserVM.SelectedRoles!);
            if (!isChanged)
            {
                return Json(new { success = false, errors = new List<string> { $"roles were not chnaged" } });
            }

            using var transaction = await unitOfWork.StartTransactionAsync();
            try
            { 
                foreach (var addedRole in addedRoles)
                {
                    await userManager.AddToRoleAsync(user, addedRole);
                }

                foreach (var removedRole in removedRoles)
                {
                    await userManager.RemoveFromRoleAsync(user, removedRole);
                }

                await transaction.CommitAsync();
            }catch(Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine(ex.Message);
                return Json(new { success = false, errors = new List<string> { $"An error has occured during tranasaction, error: {ex.Message}" } });
            }

            return Json(new { success = true, redirectUrl = Url.Action("Index") });
        }

		[HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(string Id)
        {
            var user = await userManager.FindByIdAsync(Id);
            if(user is null)
            {
                return NotFound();
            }

            var deleteResult = await userManager.DeleteAsync(user);
			if (!deleteResult.Succeeded)
			{
				ModelState.AddModelError("Error", "Something went wrong please try again later");
				return RedirectToAction("Index");
			}

			return RedirectToAction("Index");
        }

        
    }
}
