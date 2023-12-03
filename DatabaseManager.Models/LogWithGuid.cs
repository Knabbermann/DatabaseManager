using System.ComponentModel.DataAnnotations;

namespace DatabaseManager.Models
{
    public class LogWithGuid
    {
        [Key]
        public Guid Guid { get; set; } = Guid.NewGuid();
        [Required]
        public DateTime Timestamp { get; set; } = DateTime.Now;
        [Required]
        public string Model { get; set; }
        [Required]
        public int ModelId { get; set; }
        [Required]
        public Guid SessionGuid { get; set; }
    }
}
