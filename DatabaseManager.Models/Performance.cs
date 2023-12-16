using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseManager.Models
{
    public class Performance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required] 
        public DateTime RunAt { get; set; } = DateTime.Now;
        [Required]
        public int Runs { get; set; }
        [Required]
        public string Table { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public double AverageSeconds { get; set; }
    }
}
