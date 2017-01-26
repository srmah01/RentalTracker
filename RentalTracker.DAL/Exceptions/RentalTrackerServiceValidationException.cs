using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using System.Runtime.Serialization;

namespace RentalTracker.DAL.Exceptions
{
    public class RentalTrackerServiceValidationException : Exception
    {
        private IEnumerable<ValidationResult> validationResults;

        public RentalTrackerServiceValidationException()
        {
        }

        public RentalTrackerServiceValidationException(string message) : base(message)
        {
        }

        public RentalTrackerServiceValidationException(string message, IEnumerable<ValidationResult> validationResults) : base(message)
        {
            this.validationResults = validationResults;
        }

        public IEnumerable<ValidationResult> ValidationResults
        {
            get { return validationResults ?? new List<ValidationResult>(); }
        }
    }
}