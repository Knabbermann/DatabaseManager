using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseManager.Models
{
    public class Review : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        [Required]
        [StringLength(1000)]
        public string Content { get; set; }
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }
        [Required]
        public DateTime ReviewDate { get; set; }
        public virtual Customer Customer { get; set; }
        public DateTime? GcRecord { get; set; }
        [NotMapped]
        public bool HasGcRecord => GcRecord != null;
    }
}
