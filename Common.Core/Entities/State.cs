namespace Common.Core.Entities
{
    public class State : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<LGA> LGAs { get; set; } = Array.Empty<LGA>();
    }
}
