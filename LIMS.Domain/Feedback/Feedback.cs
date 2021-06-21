namespace LIMS.Domain.Feedback
{
    public class Feedback : BaseEntity
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Category { get; set; }
        public string Message { get; set; }
    }
}
