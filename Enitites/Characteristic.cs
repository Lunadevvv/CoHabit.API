namespace CoHabit.API.Enitites
{
    public class Characteristic
    {
        public Guid CharId { get; set; }
        public required string Title { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public virtual ICollection<User>? Users { get; set; }
    }
}