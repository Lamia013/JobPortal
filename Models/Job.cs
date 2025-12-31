public class Job
{
    public int JobId { get; set; }

    public string? Title { get; set; }

    public int? Vacancy { get; set; }

    public string? Description { get; set; }

    public string? Qualification { get; set; }

    public string? Experience { get; set; }

    public string? Specialization { get; set; }

    public DateTime? LastDateToApply { get; set; }

    public double? Salary { get; set; }

    public string? JobType { get; set; }

    public int? OrganizationId { get; set; }

    public string? Address { get; set; }

    public string? Country { get; set; }

    public string? State { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual Organization? Organization { get; set; }

    public string? Tags { get; set; } // e.g., "React,TypeScript,Tailwind"
}

// Mim mony.......

using System.ComponentModel.DataAnnotations;

namespace JobPortal.Models
{
    public class Job
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = "";

        [Required]
        public string Company { get; set; } = "";

        [Required]
        public string Location { get; set; } = "";

        [Required]
        public string JobType { get; set; } = "";

        [Required]
        public decimal SalaryMin { get; set; }

        [Required]
        public decimal SalaryMax { get; set; }

        [Required]
        public string Description { get; set; } = "";

        public DateTime PostedDate { get; set; }
    }
}
