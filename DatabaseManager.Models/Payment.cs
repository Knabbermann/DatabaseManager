﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseManager.Models
{
    public class Payment : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        [Required]
        [StringLength(50)]
        public string PaymentMethod { get; set; } // e.g. "Credit Card", "PayPal"
        public virtual Customer Customer { get; set; }
        public DateTime GcRecord { get; set; }
    }
}
