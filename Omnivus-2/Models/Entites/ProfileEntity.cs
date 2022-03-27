using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Omnivus_2.Models.Entites
{
    public class ProfileEntity
    {
        [Key]
        //[Column(TypeName = "nvarchar(450)")]
        public int Id { get; set; } 

        [Required]
        [PersonalData]
        [Column(TypeName = "nvarchar(50)")]
        public string FirstName { get; set; }
        [Required]
        [PersonalData]
        [Column(TypeName = "nvarchar(50)")]
        public string LastName { get; set; }

        
        [Required]
        [PersonalData]
        [Column(TypeName = "nvarchar(50)")]
        public string StreetName { get; set; } = "";

        [Required]
        [PersonalData]
        [Column(TypeName = "char(6)")]
        public string PostalCode { get; set; } = "";

        [Required]
        [PersonalData]
        [Column(TypeName = "nvarchar(50)")]
        public string City { get; set; } = "";

        [Required]
        [PersonalData]
        [Column(TypeName = "nvarchar(50)")]
        public string Country { get; set; } = "Sverige";

        public string UserId { get; set; }
        public IdentityUser User { get; set; }

    }
}
