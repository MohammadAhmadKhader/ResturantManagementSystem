using Microsoft.AspNetCore.Identity;

namespace ResturantManagementSystem.Helpers
{
    public class RolesHelper
    {
        public static async Task<string?> UpdateRole(RoleManager<IdentityRole> rolemanager, IdentityRole roleToUpdate)
        {
            try
            {
                var updatedResult = await rolemanager.UpdateAsync(roleToUpdate);
                if (!updatedResult.Succeeded)
                {
                    var firstError = updatedResult.Errors.First();
                    var message = $"Code: {firstError.Code}, Description: {firstError.Description}";
                    return message;
                }

                return null;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return error.Message;
            }
        }

    }
}
