using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseManager.Models
{
    public class Order : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
        [Required]
        [StringLength(50)]
        public string Status { get; set; } // e.g. "Pending", "Shipped", "Delivered"
        public virtual Customer Customer { get; set; }
        public DateTime? GcRecord { get; set; }
    }
}
