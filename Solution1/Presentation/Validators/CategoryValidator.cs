using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Repositories;
using Humanizer.Localisation;

namespace Presentation.Validators
{
    public class CategoryValidator:ValidationAttribute
    {
        public string GetErrorMessage() =>
            $"Category chosen doesn't fall within acceptable range";

         protected override ValidationResult? IsValid(
            object? value, ValidationContext validationContext)
        {
            //value => the categoryfk chosen by the user

            CategoriesRepository _categoriesRepository = (CategoriesRepository) validationContext.GetService(typeof(CategoriesRepository));

            var minValueOfCategoryId = _categoriesRepository.GetCategories().Min(x => x.Id);
            var maxValueOfCategoryId = _categoriesRepository.GetCategories().Max(x => x.Id);

            if (value != null)
            {
                if ((int)value >= minValueOfCategoryId && (int)value <= maxValueOfCategoryId)
                {
                    return ValidationResult.Success;
                }
            }
            
            return new ValidationResult(GetErrorMessage());

            //var movie = (Movie)validationContext.ObjectInstance;
            //var releaseYear = ((DateTime)value!).Year;

            //if (movie.Genre == Genre.Classic && releaseYear > Year)
            //{
            //    return new ValidationResult(GetErrorMessage());
            //}

            return ValidationResult.Success;
        } 
    }
}
