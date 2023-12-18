using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseManager.Models
{
    public class Customer : IEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        [StringLength(50)]
        public string? MiddleName { get; set; }
        [Required]
        [StringLength(100)]
        public string Email { get; set; }
        [StringLength(100)]
        public string? SecondaryEmail { get; set; }
        [StringLength(15)]
        public string? PhoneNumber { get; set; }
        [StringLength(15)]
        public string? SecondaryPhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [StringLength(10)]
        public string? Gender { get; set; }
        [StringLength(50)]
        public string? Nationality { get; set; }
        [StringLength(100)]
        public string? AddressLine1 { get; set; }
        [StringLength(100)]
        public string? AddressLine2 { get; set; }
        [StringLength(50)]
        public string? City { get; set; }
        [StringLength(50)]
        public string? State { get; set; }
        [StringLength(50)]
        public string? Country { get; set; }
        [StringLength(5)]
        public string? PostalCode { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastOnline { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        [StringLength(50)]
        public string? PreferredLanguage { get; set; }
        [StringLength(50)]
        public string? PreferredPaymentMethod { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public DateTime? GcRecord { get; set; }
        [NotMapped]
        public bool HasGcRecord => GcRecord != null;
    }
}
