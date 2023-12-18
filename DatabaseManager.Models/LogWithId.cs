using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseManager.Models
{
    public class LogWithId
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public DateTime Timestamp { get; set; } = DateTime.Now;
        [Required]
        public string Model { get; set; }
        [Required]
        public Guid ModelId { get; set; }
        [Required]
        public Guid SessionGuid { get; set; }
    }
}
