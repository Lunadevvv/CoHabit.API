namespace CoHabit.API.Enitites
{
    public class Characteristic
    {
        public string CharId { get; set; } = string.Empty;
        public required string Title { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public virtual ICollection<User>? Users { get; set; } = new List<User>();
    }
}