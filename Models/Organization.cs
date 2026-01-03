namespace JobPortal.Models
{
    public class Organization
    {
        public int OrganizationId { get; set; }

        public string OrganizationName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public string? Website { get; set; }  

        public ICollection<Job> Jobs { get; set; }

    }
}
