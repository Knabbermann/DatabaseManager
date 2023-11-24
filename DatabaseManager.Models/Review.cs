using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseManager.Models
{
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        [Required]
        [StringLength(1000)]
        public string Content { get; set; }
        [Range(1, 5)]
        public int Rating { get; set; }
        public DateTime ReviewDate { get; set; }
        public virtual Customer Customer { get; set; }
        public DateTime GcRecord { get; set; }
    }
}
