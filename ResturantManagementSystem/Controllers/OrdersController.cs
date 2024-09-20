using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ResturantManagementSystem.Data;
using ResturantManagementSystem.Enums;
using ResturantManagementSystem.Helpers;
using ResturantManagementSystem.Models;
using ResturantManagementSystem.Repositories;
using ResturantManagementSystem.ViewModels;
using static System.Net.Mime.MediaTypeNames;

namespace ResturantManagementSystem.Controllers
{
    [Authorize(Roles ="SuperAdmin,Admin")]
	public class OrdersController : Controller
	{
        private readonly UnitOfWork unitOfWork;
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IMapper mapper;
        public OrdersController(IMapper mapper, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.unitOfWork = new UnitOfWork(context);
            this.mapper = mapper;
        }

        [HttpGet("/orders")]
        public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] int limit = 9)
        {
            var result = await unitOfWork.ordersRepository.GetAllIncluded(page, limit);
            var mappedOrders = mapper.Map<IEnumerable<Order>,IEnumerable<OrderVM>>(result.list);
           
            return View("Index", (page, limit, result.count, mappedOrders.ToList()));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var selectListsHelper = new SelectListsHelper(context);
            var viewModel = await selectListsHelper.GetOrderCreateVMWithFoodsAndChefsList();
            return View(viewModel);
        }

        [HttpPost("/orders/initiate")]
        public async Task<IActionResult> CreateStep2(OrderCreateStep1VM orderVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var selectListsHelper = new SelectListsHelper(context);
                    var viewModel = await selectListsHelper.GetOrderCreateVMWithFoodsAndChefsList();
                    return View(viewModel);
                }

                var foodsList = await unitOfWork.foodsRepository.GetFoodsByIdsAsync(orderVM.SelectedFoods);
                orderVM.Select = foodsList;

                var orderInit = new OrderCreateStep2VM
                {
                    ChefId = orderVM.ChefId,
                };
                foreach(var food in orderVM.Select)
                {
                    orderInit.Selections.Add(new FoodQuantityListItem
                    {
                        FoodId = food.Id,
                        FoodName = food.Name,
                        Quantity = 1,
                    });
                }
         
                return View("CreateStep2", orderInit);
                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return RedirectToAction("Index");
            }
            
        }

        [HttpPost("/orders/create")]
        public async Task<IActionResult> CreateFinal(OrderCreateStep2VM orderVM)
        {
            using var transaction = await unitOfWork.StartTransactionAsync();
            try
            { 
                if (!ModelState.IsValid)
                {
                    return View("CreateStep2", orderVM);
                }

                var order = new Order
                {
                    ChefId = orderVM.ChefId,
                    Status = OrderStatus.Pending
                };

                await unitOfWork.ordersRepository.CreateAsync(order);
				var addOrderCount = await unitOfWork.SaveChangesAsync();
                if (addOrderCount == 0)
                {
                    ModelState.AddModelError("Quantity", "Something went wrong during the save operation please try again later");
                    await transaction.RollbackAsync();
                    return View("CreateStep2", orderVM);
                }

                foreach (var FoodQtyItem in orderVM.Selections)
                {
                    var orderFood = new OrderFood
                    {
                        FoodId = FoodQtyItem.FoodId,
                        OrderId = order.Id,
                        Quantity = FoodQtyItem.Quantity
                    };
                    unitOfWork.ordersFoodsRepository.Create(orderFood);
                }
               
                var addOrderFoodsCount = await unitOfWork.SaveChangesAsync();
                if (addOrderFoodsCount == 0)
                {
                    ModelState.AddModelError("Quantity", "Something went wrong during the save operation please try again later");
                    await transaction.RollbackAsync();
                    return View("CreateStep2", orderVM);
                }

                await transaction.CommitAsync();
                return RedirectToAction("Index");

            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View("CreateStep2", orderVM);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update([FromRoute] int Id)
        {
            var order = await unitOfWork.ordersRepository.GetOrderByIdAllIncluded(Id);
            if (order is null)
            {
                return NotFound();
            }
            var orderVM = new OrderUpdateVM
            {
                Chef = order.Chef ?? null,
                ChefId = order.ChefId,
                Selections = order.OrderFoods.Select(of => new FoodQuantityListItem
                {
                    FoodId = of.FoodId,
                    FoodName = of.Food == null ? "Food Not Found" : of.Food.Name,
                    Quantity = of.Quantity
                }).ToList(),
                Status = Enum.GetName(order.Status)!,
                Id = order.Id,
            };

            return View(orderVM);
        }

        [HttpPut]
        public async Task<IActionResult> Update(OrderUpdateVM orderVM)
        {
            var order = await unitOfWork.ordersRepository.GetOrderByIdAllIncluded(orderVM.Id);
            if (order is null)
            {
                return NotFound();
            }

            var (isChanged, listOfChangedOrdersFoods) = OrdersHelper.GetOrdersFoodsChanges(order,orderVM);
            if (isChanged)
            {
                foreach(var changedOrderFood in listOfChangedOrdersFoods)
                {
                    unitOfWork.ordersFoodsRepository.UpdateByModel(changedOrderFood);
                }
            }

            try
            {
                order.Status = (OrderStatus) Enum.Parse(typeof(OrderStatus), orderVM.Status);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Status", "Status can be either completed or pending");
                Console.WriteLine(ex.Message);
                return View(orderVM);
            }

            await unitOfWork.SaveChangesAsync();

            return View("Index");
        }

        [HttpPatch]
        public async Task<IActionResult> Cancel([FromRoute] int Id)
        {
            var order = await unitOfWork.ordersRepository.GetById(Id);
            if (order == null)
            {
                return NotFound();
            }
            order.Status = OrderStatus.Cancelled;

            await unitOfWork.SaveChangesAsync();

            return Json(new { success = true, redirectUrl = Url.Action("Index") });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromRoute] int Id)
        {
            var isDeleted = await unitOfWork.ordersRepository.Delete(Id);
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
