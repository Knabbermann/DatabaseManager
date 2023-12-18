﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseManager.Models
{
    public class Payment : IEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        [ForeignKey("Customer")]
        public Guid CustomerId { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        [Required]
        public DateTime PaymentDate { get; set; }
        [Required]
        [StringLength(50)]
        public string PaymentMethod { get; set; }
        public virtual Customer Customer { get; set; }
        public DateTime? GcRecord { get; set; }
        [NotMapped]
        public bool HasGcRecord => GcRecord != null;
    }
}
