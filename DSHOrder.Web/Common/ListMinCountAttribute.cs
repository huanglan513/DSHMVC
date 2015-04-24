using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DSHOrder.Web.Common
{
    public class ListMinCountAttribute : ValidationAttribute
    {
        //protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        //{
        //    if (value == null)
        //    {
        //        return ValidationResult.Success;
        //    }

        //    int convertedValue;
        //    try
        //    {
        //        IEnumerable<T> list = value as IEnumerable<T>;
        //        convertedValue = Convert.ToInt32(value);
        //    }
        //    catch (FormatException)
        //    {
        //        return new ValidationResult(Resource1.ConversionError);
        //    }

        //    if (convertedValue % 2 == 0)
        //    {
        //        return ValidationResult.Success;
        //    }
        //    else
        //    {
        //        return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        //    }
        //}
    }
}