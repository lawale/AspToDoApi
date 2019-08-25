using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoApp.Errors.Validation
{
    public class ValidationResultModel
    {
        public string ErrorType { get; }

        public List<ValidationError> Errors { get; }

        public ValidationResultModel(ModelStateDictionary modelState)
        {
            ErrorType = "Model Validation";
            Errors = modelState.Keys
                .SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage)))
                .ToList();
        }

        public ValidationResultModel(string errorType, IEnumerable<ValidationError> errors)
        {
            ErrorType = errorType;
            Errors = errors as List<ValidationError>;
        }
    }
}
