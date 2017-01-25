using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentalTracker.Utilities
{
    public static class HandleValidationErrors
    {
        public static void AddErrorsToModel(Controller controller, IEnumerable<DbEntityValidationResult> entityValidationErrors)
        {
            foreach (var validationErrors in entityValidationErrors)
            {
                foreach (var validationError in validationErrors.ValidationErrors)
                {
                    controller.ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                }
            }
        }
    }
}