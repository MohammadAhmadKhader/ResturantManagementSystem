using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ResturantManagementSystem.Data;
using ResturantManagementSystem.Models;
using ResturantManagementSystem.Repositories;
using ResturantManagementSystem.ViewModels;

namespace ResturantManagementSystem.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class ChefsController : Controller
    {
        private readonly UnitOfWork unitOfWork;
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IMapper mapper;
        public ChefsController(IMapper mapper, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.unitOfWork = new UnitOfWork(context);
            this.mapper = mapper;
        }

        [HttpGet("/chefs")]
        public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] int limit = 9)
        {
            var result = await unitOfWork.chefsRepository.GetAll(page, limit, "User");
            var mappedChefs = mapper.Map<IEnumerable<Chef>, IEnumerable<ChefVM>>(result.list);

            return View("Index", (page, limit, result.count, mappedChefs.ToList()));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ChefCreateVM chefCreateVM)
        {
            if (!ModelState.IsValid)
            {
                return View(chefCreateVM);
            }

            var user = await userManager.FindByEmailAsync(chefCreateVM.Email);
            if (user is null)
            {
                ModelState.AddModelError("Email", "User with this email does not exist");
                return View(chefCreateVM);
            }
            var mappedChef = mapper.Map<ChefCreateVM, Chef>(chefCreateVM);

            mappedChef.UserId = user.Id;
            await unitOfWork.chefsRepository.CreateAsync(mappedChef);
            var addCount = await unitOfWork.SaveChangesAsync();
            if (addCount == 0)
            {
                ModelState.AddModelError("Error", "Something went wrong during the save operation please try again later");
                return View(chefCreateVM);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update([FromRoute] int Id)
        {
            var chef = await unitOfWork.chefsRepository.GetChefDetailsById(Id);
            if(chef is null)
            {
                return NotFound();
            }
            var mappedChef = mapper.Map<Chef, ChefCreateVM>(chef);

			return View(mappedChef);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ChefCreateVM chefCreateVM)
        {
            if (!ModelState.IsValid)
            {
                return View(chefCreateVM);
            }

            var chef = await unitOfWork.chefsRepository.GetChefDetailsById(chefCreateVM.Id);
            if (chef is null)
            {
                return NotFound();
            }
            chef.Salary = chefCreateVM.Salary;

            if (chefCreateVM.Email != chef.User!.Email)
            {
                var newChef = await userManager.FindByEmailAsync(chefCreateVM.Email);
                if (newChef is null)
                {
                    ModelState.AddModelError("Email", "USer with this email does not exist");
                    return View(chefCreateVM);
                }
                chef.UserId = newChef.Id;
            }
            var updatedCount = await unitOfWork.SaveChangesAsync();
            if(updatedCount == 0)
            {
                return Json(new { success = false, Error = "Something went wrong during saving changes" });
            }

			return Json(new { success = true, redirectUrl = Url.Action("Index") });
		}

        [HttpDelete]
        public async Task<IActionResult> Delete([FromRoute] int Id)
        {
            var isDeleted = await unitOfWork.chefsRepository.Delete(Id);
            if (!isDeleted)
            {
                return NotFound();
            }
            var deletedCount = await unitOfWork.SaveChangesAsync();
            if(deletedCount == 0)
            {
                return Json(new { success = false, Error = "Something went wrong during saving process" });
            }

            return Json(new { success = true, redirectUrl = Url.Action("Index") });
        }
    }
}

