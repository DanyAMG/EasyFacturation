using EasyFacturation.Ressources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFacturation.AppServices.DTOs
{
    internal class AppOwnerCreateDTO
    {
        [MaxLength(100)]
        public string LastName { get; set; }

        [MaxLength(100)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        public string CompanyName { get; set; }

        [Required]
        [MaxLength(100)]
        public string CompanyNumber { get; set; }

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
    }
}
