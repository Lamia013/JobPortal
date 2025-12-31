public class Organization
{
    public int OrganizationId { get; set; }
    public string CompanyName { get; set; }
    public string CompanyImage { get; set; }
    public string Website { get; set; }
    public string Email { get; set; }
    public virtual ICollection<Job> Jobs { get; set; }
}

//mim ............

using System;

namespace JobPortal.Models
{
    // CHANGE class name from Organization to Employer
    public class Employer
    {
        public int Id { get; set; }
        public string OrganizationName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
