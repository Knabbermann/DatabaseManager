using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseManager.Models
{
    public class Payment : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        [Required]
        public DateTime PaymentDate { get; set; }
        [Required]
        [StringLength(50)]
        public string PaymentMethod { get; set; } // e.g. "Credit Card", "PayPal"
        public virtual Customer Customer { get; set; }
        public DateTime? GcRecord { get; set; }
    }
}
