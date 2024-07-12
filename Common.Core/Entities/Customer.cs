namespace Common.Core.Entities
{
    public class Customer : BaseEntity
    {
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool IsVerified { get; set; }
        public int LGAId { get; set; }
        public LGA? LGA { get; set; }
    }
}
