namespace JobPortal.Models
{
    public class JobApplication
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public int UserId { get; set; }
        public DateTime AppliedOn { get; set; }
    }
}
