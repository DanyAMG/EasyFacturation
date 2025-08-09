using EasyFacturation.Ressources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFacturation.Models
{
    public class AppOwner : IValidatableObject
    {
        [Key]
        public Guid Id { get; set; } //EF will generate automatically the value at the insertion

        [MaxLength(100)]
        public string LastName { get; set; }

        [MaxLength(100)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        public string CompanyName { get; set; }

        [Required]
        [MaxLength(100)]
        public string CompanyNumber {  get; set; }

        [Required]
        [MaxLength(100)]
        public string TaxeNumber { get; set; }

        [MaxLength(10)]
        public string StreetNumber { get; set; }

        [Required]
        [MaxLength(100)]
        public string StreetName { get; set; }

        [MaxLength(100)]
        public string AdressLine1 { get; set; }

        [MaxLength(100)]
        public string AdressLine2 { get; set; }

        [Required]
        [MaxLength(100)]
        public string City { get; set; }

        [Required]
        [MaxLength(10)]
        public string ZipCode { get; set; }

        [MaxLength(20)]
        [Phone(ErrorMessageResourceType = typeof(ValidationMessage),
            ErrorMessageResourceName = "InvalidPhoneNumber")]
        public string Phone { get; set; }

        [MaxLength(100)]
        [EmailAddress(ErrorMessageResourceType = typeof(ValidationMessage),
        ErrorMessageResourceName = "InvalidEmailAdress")]
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
