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

        public ValidationError[] Errors { get; }

        public ValidationResultModel(ModelStateDictionary modelState)
        {
            ErrorType = "Invalid Object State";
            Errors = modelState.Keys
                .SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage)))
                .ToArray();
        }

        public ValidationResultModel(string errorType, ValidationError[] errors)
        {
            ErrorType = errorType;
            Errors = errors;
        }
    }
}
