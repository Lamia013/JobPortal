using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

public class JobsController : Controller
{
    private readonly JobPortalContext _context;

    public JobsController(JobPortalContext context)
    {
        _context = context;
    }

    // Display all jobs
    public IActionResult Index()
    {
        var jobs = _context.Jobs.Include(j => j.Organization).ToList();
        return View(jobs);
    }

    // Display job details
    public IActionResult Details(int id)
    {
        var job = _context.Jobs.Include(j => j.Organization).FirstOrDefault(j => j.JobId == id);
        if (job == null) 
        {
            return NotFound();
        }

        // Split tags into a list if stored as comma-separated string
        var tags = string.IsNullOrEmpty(job.Tags) ? new List<string>() : job.Tags.Split(',').ToList();

        // Pass tags to ViewBag
        ViewBag.Tags = tags;

        return View(job);
    }
    // GET: show the form
public IActionResult Create()
{
    ViewBag.Organizations = new SelectList(_context.Organizations, "OrganizationId", "CompanyName");
    return View();
}

// POST: handle form submission
[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult Create(Job job)
{
    if (ModelState.IsValid)
    {
        job.CreateDate = DateTime.Now;
        _context.Jobs.Add(job);
        _context.SaveChanges();
        return RedirectToAction("Index", "Jobs");
    }
    ViewBag.Organizations = new SelectList(_context.Organizations, "OrganizationId", "CompanyName", job.OrganizationId);
    return View(job);
}

}

// Mim Mony.....................

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using JobPortal.Data;
using JobPortal.Models;
using System;
using System.Linq;
using System.Collections.Generic;

namespace JobPortal.Controllers
{
    public class JobsController : Controller
    {
        private readonly JobPortalDbContext _context;

        public JobsController(JobPortalDbContext context)
        {
            _context = context;
        }

        // ==========================
        // ABOUT US PAGE
        // ==========================
        public IActionResult About()
        {
            return View();
        }

        // ==========================
        // ADD JOB (ORG / ADMIN)
        // ==========================
        public IActionResult Create()
        {
            var role = HttpContext.Session.GetString("Role");

            if (role != "Admin" && role != "Organization")
                return RedirectToAction("Index");

            return View();
        }

        [HttpPost]
        public IActionResult Create(Job job)
        {
            var role = HttpContext.Session.GetString("Role");

            if (role != "Admin" && role != "Organization")
                return RedirectToAction("Index");

            if (!ModelState.IsValid)
                return View(job);

            job.PostedDate = DateTime.Now;
            _context.Jobs.Add(job);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // ==========================
        // JOB LIST + FILTER
        // ==========================
        public IActionResult Index(string? keyword, string[]? types)
        {
            var jobs = _context.Jobs.AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                jobs = jobs.Where(j => j.Title.Contains(keyword));
            }

            if (types != null && types.Any())
            {
                jobs = jobs.Where(j => types.Contains(j.JobType));
            }

            return View(jobs.ToList());
        }

        // ==========================
        // JOB DETAILS
        // ==========================
        public IActionResult Details(int id)
        {
            var job = _context.Jobs.FirstOrDefault(j => j.Id == id);
            if (job == null)
                return NotFound();

            return View(job);
        }

        // ==========================
        // APPLY (LOGIN REQUIRED)
        // ==========================
        public IActionResult Apply(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            return RedirectToAction("ApplyConfirm", new { jobId = id });
        }

        // ==========================
        // APPLY CONFIRM
        // ==========================
        [HttpPost, HttpGet]
        public IActionResult ApplyConfirm(int jobId)
        {
            try
            {
                int? userId = HttpContext.Session.GetInt32("UserId");
                if (userId == null)
                    return RedirectToAction("Login", "Account");

                JobApplication application = new JobApplication
                {
                    JobId = jobId,
                    UserId = userId.Value,
                    AppliedOn = DateTime.Now
                };

                _context.JobApplications.Add(application);
                _context.SaveChanges();

                TempData["Success"] = "Job applied successfully!";
                return RedirectToAction("Details", new { id = jobId });
            }
            catch
            {
                return View("Error");
            }
        }
    }
}
