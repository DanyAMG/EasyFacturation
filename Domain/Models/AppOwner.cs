using EasyFacturation.Ressources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFacturation.Domain.Models
{
    public class AppOwner : IValidatableObject
    {
        [Key]
        public Guid Id { get; set; } //EF will generate automatically the value at the insertion
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string CompanyName { get; set; }
        public string CompanyNumber {  get; set; }
        public string TaxeNumber { get; set; }
        public string StreetNumber { get; set; }
        public string StreetName { get; set; }
        public string AdressLine1 { get; set; }
        public string AdressLine2 { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        /// <summary>
        /// Validates that either both first and last names are provided, or the company name is provided.
        /// </summary>
        /// <param name="validationContext">Context information about the validation operation.</param>
        /// <returns>Returns a validation error if neither a full name (first and last) nor a company name is provided.  
        /// Otherwise, the enumeration completes without errors.</returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //If the company name AND (First Name OR Last Name)
            if (string.IsNullOrWhiteSpace(CompanyName) &&
                (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName)))
            {
                yield return new ValidationResult(
                    ValidationMessage.ClientNameOrCompany,
                    new[] { nameof(FirstName), nameof(LastName), nameof(CompanyName) });
            }
            yield break;
        }
    }
}
