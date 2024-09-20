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
    public class IngredientsController : Controller
    {
        private readonly UnitOfWork unitOfWork;
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IMapper mapper;
        public IngredientsController(IMapper mapper, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.unitOfWork = new UnitOfWork(context);
            this.mapper = mapper;
        }

        [HttpGet("/ingredients")]
        public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] int limit = 9)
        {
            var result = await unitOfWork.ingredientsRepository.GetAll(page, limit);
            var mappedIngredrients = mapper.Map<IEnumerable<Ingredient>, IEnumerable<IngredientVM>>(result.list);

            return View("Index", (page, limit, result.count, mappedIngredrients.ToList()));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(IngredientVM ingredientsVM)
        {
            if (!ModelState.IsValid)
            {
                return View(ingredientsVM);
            }

            var mappedIngredient = mapper.Map<IngredientVM, Ingredient>(ingredientsVM);
            await unitOfWork.ingredientsRepository.CreateAsync(mappedIngredient);

            var addCount = await unitOfWork.SaveChangesAsync();
            if (addCount == 0)
            {
                ModelState.AddModelError("Error", "Something went wrong during the save operation please try again later");
                return View(ingredientsVM);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update([FromRoute] int Id)
        {
            var ingredient = await unitOfWork.ingredientsRepository.GetById(Id);
            if (ingredient is null)
            {
                return NotFound();
            }
            var mappedIngredient = mapper.Map<Ingredient, IngredientVM>(ingredient);

            return View(mappedIngredient);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] IngredientVM ingredientVM)
        {
            if (!ModelState.IsValid)
            {
                return View(ingredientVM);
            }

            var ingredient = await unitOfWork.ingredientsRepository.GetById(ingredientVM.Id);
            if (ingredient is null)
            {
                return NotFound();
            }

            ingredient.Name = ingredientVM.Name;
            ingredient.Description = ingredientVM.Description;

            var updatedCount = await unitOfWork.SaveChangesAsync();
            if (updatedCount == 0)
            {
                return Json(new { success = false, Error = "Something went wrong during saving changes" });
            }

            return Json(new { success = true, redirectUrl = Url.Action("Index") });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromRoute] int Id)
        {
            var isDeleted = await unitOfWork.ingredientsRepository.Delete(Id);
            if (!isDeleted)
            {
                return NotFound();
            }
            var deletedCount = await unitOfWork.SaveChangesAsync();
            if (deletedCount == 0)
            {
                return Json(new { success = false, Error = "Something went wrong during saving process" });
            }

            return Json(new { success = true, redirectUrl = Url.Action("Index") });
        }
    }
}
