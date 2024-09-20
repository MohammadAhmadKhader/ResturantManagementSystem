using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ResturantManagementSystem.Data;
using ResturantManagementSystem.Helpers;
using ResturantManagementSystem.Models;
using ResturantManagementSystem.Repositories;
using ResturantManagementSystem.ViewModels;
using static System.Net.Mime.MediaTypeNames;

namespace ResturantManagementSystem.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class FoodsController : Controller
	{
        private readonly UnitOfWork unitOfWork;
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IMapper mapper;
        public FoodsController(IMapper mapper, ApplicationDbContext context, UserManager<IdentityUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            this.userManager = userManager;
            this.unitOfWork = new UnitOfWork(context);
            this.webHostEnvironment = webHostEnvironment;
            this.mapper = mapper;
        }

        [HttpGet("/foods")]
        public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] int limit = 9)
        {
            var result = await unitOfWork.foodsRepository.GetAllIncluded(page, limit);
            var mappedFood = mapper.Map<IEnumerable<Food>,IEnumerable<FoodVM>>(result.list);
            
            return View("Index", (page, limit, result.count, mappedFood.ToList()));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var selectListsHelper = new SelectListsHelper(context);
            var viewModel = await selectListsHelper.GetFoodCreateVMWithList();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(FoodCreateVM foodVM)
        {
            if (!ModelState.IsValid)
            {
                var selectListsHelper = new SelectListsHelper(context);
                var viewModel = await selectListsHelper.GetFoodCreateVMWithList();
                return View(viewModel);
            }

            using var transaction = await unitOfWork.StartTransactionAsync();
            try
            {
                var mappedFood = mapper.Map<FoodCreateVM, Food>(foodVM);
                var imageName = ImagesHelper.UploadImage(webHostEnvironment, foodVM.ImageFile, ImagesHelper.FoodsFolderName);
                if (imageName is null)
                {
                    ModelState.AddModelError("ImageFile", "Something went wrong during image uploading, please try again later!");
                    var selectListsHelper = new SelectListsHelper(context);
                    var viewModel = await selectListsHelper.GetFoodCreateVMWithList();
                    await transaction.RollbackAsync();
                    return View(viewModel);
                }
                mappedFood.ImageUrl = imageName;

                await unitOfWork.foodsRepository.CreateAsync(mappedFood);
                var addFoodCount = await unitOfWork.SaveChangesAsync();
                if (addFoodCount == 0)
                {
                    ModelState.AddModelError("Error", "Something went wrong during the save operation please try again later");
                    var selectListsHelper = new SelectListsHelper(context);
                    var viewModel = await selectListsHelper.GetFoodCreateVMWithList();
                    await transaction.RollbackAsync();
                    return View(viewModel);
                }
                
                var ingredientsCount = foodVM.SelectedIngredients.Count();
                if (ingredientsCount != 0)
                {
                    foreach (var ingredientId in foodVM.SelectedIngredients)
                    {
                        var foodIngredient = new FoodIngredient
                        {
                            FoodId = mappedFood.Id,
                            IngredientId = ingredientId
                        };
                        mappedFood.FoodIngredients.Add(foodIngredient);
                    }
                }
                else
                {
                    ModelState.AddModelError("SelectedIngredients", "At least one ingredient is required");
                    var selectListsHelper = new SelectListsHelper(context);
                    var viewModel = await selectListsHelper.GetFoodCreateVMWithList();
                    await transaction.RollbackAsync();
                    return View(viewModel);
                }

                await transaction.CommitAsync();
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                await transaction.RollbackAsync();
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update([FromRoute] int Id)
        {
            var food = await unitOfWork.foodsRepository.GetByIdIncluded(Id);
            if (food is null)
            {
                return NotFound();
            }
            var selectListsHelper = new SelectListsHelper(context);
            var viewModel = await selectListsHelper.GetFoodUpdateVMWithList();
            viewModel.Name = food.Name;
            viewModel.Id = food.Id;
            viewModel.ImageUrl = food.ImageUrl;
            viewModel.SelectedIngredients = food.FoodIngredients.Select(fi => fi.Ingredient!.Id).ToList();

            return View(viewModel);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] FoodUpdateVM foodVM)
        {
            using var transaction = await unitOfWork.StartTransactionAsync();
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ErrorsHelper.GetModelStateErrMessage(ModelState);
                    return Json(new { success = false, errors = errors });
                }

                var food = await unitOfWork.foodsRepository.GetByIdIncluded(foodVM.Id);
                if(food == null)
                {
                    return Json(new { success = false, errors = new List<string>() { "Food was not found" } });
                }

                if(foodVM.Name != null)
                {
                    food.Name = foodVM.Name;
                }
                
                List<int> IngredsIds = food.FoodIngredients.Select(x => x.IngredientId).ToList();
                var (isUnchanged,listOfAddedIngredsIds,listOfRemovedIngredsIds) = FoodsHelper.GetIngredientsChanges(IngredsIds , foodVM);

                if (!isUnchanged)
                {
                    foreach (var addedIngredId in listOfAddedIngredsIds)
                    {
                        var foodIngredient = new FoodIngredient
                        {
                            FoodId = food.Id,
                            IngredientId = addedIngredId
                        };
                        
                        await unitOfWork.foodsIngredientsRepository.CreateAsync(foodIngredient);
                    }

                    foreach (var removedIngredsId in listOfRemovedIngredsIds)
                    {
                        var foodIngredient = food.FoodIngredients
                            .FirstOrDefault(fi => fi.IngredientId == removedIngredsId);
                        
                        if (foodIngredient != null)
                        {
                            unitOfWork.foodsIngredientsRepository.Remove(foodIngredient);
                        }
                    }
                }

                if (foodVM.ImageFile != null && foodVM.ImageFile.Length > 0 && foodVM.ImageUrl != null)
                {                    
                    var imageName = ImagesHelper.EditImage(foodVM.ImageUrl,ImagesHelper.FoodsFolderName,webHostEnvironment,
                        foodVM.ImageFile, ImagesHelper.FoodsFolderName);
                    if(imageName == null)
                    {
                        return Json(new { success = false, errors = new List<string>() { "Old image or new image was not found" } });
                    }

                    food.ImageUrl = imageName;
                }

                var changesCount = await unitOfWork.SaveChangesAsync();
                if(changesCount == 0)
                {
                    return Json(new { success = false, errors = new List<string>() { "Nothing was changed" } });
                }
                await transaction.CommitAsync();

                return Json(new { success = true, redirectUrl = Url.Action("Index") });

            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                await transaction.RollbackAsync();
                return Json(new { success = false, errors = new List<string>() { ex.Message } });
            }
            
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromRoute] int Id)
        {
            var isDeleted = await unitOfWork.foodsRepository.Delete(Id);
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
