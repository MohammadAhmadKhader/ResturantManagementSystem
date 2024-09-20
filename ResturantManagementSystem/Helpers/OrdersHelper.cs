using ResturantManagementSystem.Models;
using ResturantManagementSystem.ViewModels;

namespace ResturantManagementSystem.Helpers
{
    public class OrdersHelper
    {
        public static (bool isChanged, List<OrderFood> orderFoodChanges) GetOrdersFoodsChanges(Order orderIncluded, OrderUpdateVM orderUpdateVM)
        {
            bool isChanged = false;
            var listOfChanges = new List<OrderFood>();
            foreach (var includedOrder in orderIncluded.OrderFoods)
            {
                foreach (var orderSelection in orderUpdateVM.Selections)
                {
                    if (includedOrder.FoodId == orderSelection.FoodId && includedOrder.Quantity != orderSelection.Quantity)
                    {
                        isChanged = true;
                        listOfChanges.Add(new OrderFood
                        {
                            FoodId = orderSelection.FoodId,
                            OrderId = includedOrder.Id,
                            Quantity = orderSelection.Quantity,
                        });
                    }
                }

            }

            return (isChanged, listOfChanges);
        }


        
    }
}
