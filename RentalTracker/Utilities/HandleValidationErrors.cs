using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace RentalTracker.Utilities
{
    public static class HandleValidationErrors
    {
        public static void AddErrorsToModel(Controller controller, IEnumerable<ValidationResult> validationResults)
        {
            foreach (var validationResult in validationResults)
            {
                foreach (var memberName in validationResult.MemberNames)
                {
                    controller.ModelState.AddModelError(memberName, validationResult.ErrorMessage);
                }
            }
        }

        public static void AddExceptionError(Controller controller, Exception ex)
        {
            Exception inner = ex;
            string message = String.Empty;
            while (inner != null)
            {
                message = inner.Message;
                inner = inner.InnerException;
            }
            controller.ModelState.AddModelError(String.Empty, message);
        }

    }
}