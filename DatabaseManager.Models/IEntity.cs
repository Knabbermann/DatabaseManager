namespace DatabaseManager.Models
{
    public interface IEntity
    {
        int Id { get; }
        public DateTime? GcRecord { get; set; }
    }
}
