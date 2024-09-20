using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ResturantManagementSystem.Data;
using ResturantManagementSystem.ViewModels;

namespace ResturantManagementSystem.Helpers
{
    public class SelectListsHelper
    {
        private readonly ApplicationDbContext context;
        public SelectListsHelper(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<List<SelectListItem>> GetIngredientsLists()
        {
            var ingredients = await context.Ingredients.OrderBy(x => x.Name).ToListAsync();
            var ingredientsSelectItemList = new List<SelectListItem>();

            foreach (var ingredient in ingredients)
            {
                ingredientsSelectItemList.Add(new SelectListItem
                {
                    Value = ingredient.Id.ToString(),
                    Text = ingredient.Name,
                });
            }

            return ingredientsSelectItemList;
        }
        public async Task<List<SelectListItem>> GetFoodsList()
        {
            var foods = await context.Foods.OrderBy(x => x.Name).ToListAsync();
            var foodsSelectItemList = new List<SelectListItem>();

            foreach (var food in foods)
            {
                foodsSelectItemList.Add(new SelectListItem
                {
                    Value = food.Id.ToString(),
                    Text = food.Name,
                });
            }
            
            return foodsSelectItemList;
        }
        public async Task<List<SelectListItem>> GetChefsList()
        {
            var chefs = await context.Chefs
                .Include(x => x.User)
                .OrderBy(x => x.User.UserName)
                .ToListAsync();
            var chefsSelectItemList = new List<SelectListItem>();

            foreach (var chef in chefs)
            {
                chefsSelectItemList.Add(new SelectListItem
                {
                    Value = chef.Id.ToString(),
                    Text = chef.User?.UserName,
                });
            }

            return chefsSelectItemList;
        }
        public async Task<FoodCreateVM> GetFoodCreateVMWithList()
        {
            var ingredientsList = await GetIngredientsLists();

            var viewModel = new FoodCreateVM
            {
                Ingredients = ingredientsList,
            };

            return viewModel;
        }
        public async Task<FoodUpdateVM> GetFoodUpdateVMWithList()
        {
            var ingredientsList = await GetIngredientsLists();

            var viewModel = new FoodUpdateVM
            {
                Ingredients = ingredientsList,
            };

            return viewModel;
        }

        public async Task<OrderCreateStep1VM> GetOrderCreateVMWithFoodsAndChefsList()
        {
            var foodsList = await GetFoodsList();
            var chefsList = await GetChefsList();

            var viewModel = new OrderCreateStep1VM
            {
                Foods = foodsList,
                Chefs = chefsList,  
            };

            return viewModel;
        }

        public async Task<OrderCreateStep1VM> GetOrderCreateVMWithFoodsAndChefsLisst()
        {
            var foodsList = await GetFoodsList();
            var chefsList = await GetChefsList();

            var viewModel = new OrderCreateStep1VM
            {
                Foods = foodsList,
                Chefs = chefsList,  
            };

            return viewModel;
        }

		public async Task<List<SelectListItem>> GetRolesList()
		{
            var roles = await context.Roles.ToListAsync();
            var rolesList = new List<SelectListItem>();
            foreach (var role in roles)
            {
                var listItem = new SelectListItem
                {
                    Value = role.Name,
                    Text = role.Name
                };
                rolesList.Add(listItem);

			}
			return rolesList;
		}
    }
}
