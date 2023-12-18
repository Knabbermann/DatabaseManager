using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseManager.Models
{
    public interface IEntity
    {
        Guid Id { get; }
        public DateTime? GcRecord { get; set; }
        [NotMapped]
        public bool HasGcRecord => GcRecord != null;
    }
}
