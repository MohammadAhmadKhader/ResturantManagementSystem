namespace ResturantManagementSystem.Helpers
{
    public class UsersHelper
    {
        public static (bool isChanged, List<string> addedRoles, List<string> removedRoles) GetRolesChanges(List<string> userRolesBefore, List<string> userRolesAfter)
        {
            bool isChanged = false;
            var addedRoles = new List<string>();
            var removedRoles = new List<string>();

            foreach (var role in userRolesBefore)
            {
                if (!userRolesAfter.Contains(role))
                {
                    removedRoles.Add(role);
                    isChanged = true;
                }
            }

            foreach (var role in userRolesAfter)
            {
                if (!userRolesBefore.Contains(role))
                {
                    addedRoles.Add(role);
                    isChanged = true;
                }
            }

            return (isChanged, addedRoles, removedRoles);
        }
    }
}
