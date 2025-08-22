using EasyFacturation.Domain.Models;
using EasyFacturation.Ressources;
using System.ComponentModel.DataAnnotations;

namespace EasyFacturation.AppServices.DTOs
{
    public class ClientUpdateDTO
    {
            [Required]
            public Guid Id { get; set; }

            public Client.ClientTitle? Title { get; set; }

            [MaxLength(100)]
            public string? LastName { get; set; }

            [MaxLength(100)]
            public string? FirstName { get; set; }

            [MaxLength(100)]
            public string? CompanyName { get; set; }

            [MaxLength(10)]
            public string? StreetNumber { get; set; }

            [MaxLength(100)]
            public string? StreetName { get; set; }

            [MaxLength(100)]
            public string? AdressLine1 { get; set; }

            [MaxLength(100)]
            public string? AdressLine2 { get; set; }

            [MaxLength(100)]
            public string? City { get; set; }

            [MaxLength(10)]
            public string? ZipCode { get; set; }

            [MaxLength(20)]
            [Phone(ErrorMessageResourceType = typeof(ValidationMessage),
                ErrorMessageResourceName = "InvalidPhoneNumber")]
            public string? Phone { get; set; }

            [MaxLength(100)]
            [EmailAddress(ErrorMessageResourceType = typeof(ValidationMessage),
                ErrorMessageResourceName = "InvalidEmailAdress")]
            public string? Email { get; set; }
    }
}

