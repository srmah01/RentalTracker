using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace RentalTracker.Utilities
{
    /// <summary>
    /// A utility class that adds validation errors returned by the DAL
    /// to the model state for display in the view. 
    /// </summary>
    public static class HandleValidationErrors
    {
        /// <summary>
        /// Adds a list of validation errors to the Model State of the given controller. 
        /// </summary>
        /// <param name="controller">The controller that recieved the errors.</param>
        /// <param name="validationResults">The collection of validation results.</param>
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

        /// <summary>
        /// Adds an error message, taken from an exception, to the Model State of the given controller. 
        /// </summary>
        /// <param name="controller">The controller that recieved the errors.</param>
        /// <param name="ex">The exception containing the error message.</param>
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