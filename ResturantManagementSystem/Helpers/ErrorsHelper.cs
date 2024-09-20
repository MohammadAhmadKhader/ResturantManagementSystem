using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ResturantManagementSystem.Helpers
{
    public class ErrorsHelper
    {
        public static List<string> GetModelStateErrMessage(ModelStateDictionary ModelState)
        {
            var errors = new List<string>();
            foreach (var key in ModelState.Keys)
            {
                var state = ModelState[key];
                if (state.Errors.Count > 0)
                {
                    foreach (var error in state.Errors)
                    {
                        errors.Add($"{error.ErrorMessage}");
                    }
                }
            }
            return errors;
        }
		public static string CaptureUserManagerError(IEnumerable<IdentityError> errs)
		{
			var errorsAsString = string.Join(", ", errs.Select(x => x.Description));
			return errorsAsString;
		}

	}
}
