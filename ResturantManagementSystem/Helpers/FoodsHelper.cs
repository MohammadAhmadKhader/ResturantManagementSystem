using ResturantManagementSystem.Models;
using ResturantManagementSystem.ViewModels;

namespace ResturantManagementSystem.Helpers
{
    public class FoodsHelper
    {
        public static (bool IsUnChanged, List<int> ListOfAddedIngredsIds, List<int> ListOfRemovedIngredsIds) GetIngredientsChanges(List<int> foodIngredsIds, FoodUpdateVM foodVM)
        {
            List<int> AddedIngredsIds = new List<int>();
            List<int> RemovedIngredsIds = new List<int>();
            var originalIngredsIdsDict = new Dictionary<int, int>();
            var foodVMIngredsIdsDict = new Dictionary<int, int>();
            // creating dictionary with original Ingreds ids before change
            foreach (var ingredsId in foodIngredsIds)
            {
                originalIngredsIdsDict.Add(ingredsId, ingredsId);
            }

            // creating dictionary with ingrds ids from the view model
            foreach (var ingredsId in foodVM.SelectedIngredients)
            {
                foodVMIngredsIdsDict.Add(ingredsId, ingredsId);
            }

            // checking if after update is the same or not
            bool IsUnChanged = true;
            // checking the removed IngredsIds
            foreach(var IngredId in foodVM.SelectedIngredients)
            {
                var isExistingInOriginalList = originalIngredsIdsDict.TryGetValue(IngredId, out int parsedIngredId);
                if (!isExistingInOriginalList)
                {
                    AddedIngredsIds.Add(IngredId);
                    IsUnChanged = false;
                    Console.WriteLine($"ID: {IngredId} ===> Added <======================");
                }
            }
            // checkinf the removed IngredsIds
            foreach (var IngredId in originalIngredsIdsDict.Keys)
            {
                if (!foodVM.SelectedIngredients.Contains(IngredId))
                {
                    RemovedIngredsIds.Add(IngredId);
                    IsUnChanged = false;
                    Console.WriteLine($"ID: {IngredId} ===> Removed <======================");
                }
            }

            return (IsUnChanged, AddedIngredsIds, RemovedIngredsIds);
        }
        //public static List<int> GetListOfUnChan
    }
}
