using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using JobPortal.Services;
using JobPortal.Data;     
using JobPortal.Models;   
public class OrganizationController : Controller
{
    private readonly JobPortalContext _context;
    private readonly EmailService _email; // CHANGED: inject email service if you want notifications

    public OrganizationController(JobPortalContext context, EmailService email) // CHANGED: added email service
    {
        _context = context;
        _email = email;
    }

    public IActionResult OrgDash()
    {
        var orgId = HttpContext.Session.GetInt32("OrgId");
        if (orgId == null)
        {
            return RedirectToAction("Login", "Account"); // session expired or not logged in
        }


        ViewBag.Organizations = new SelectList(
            _context.Organizations.ToList(),
            "OrganizationId",
            "OrganizationName"
        );
        var jobs = _context.Jobs
            .Include(j => j.Organization)
            .Include(j => j.ApplyForms) // navigation property
            .Where(j => j.OrganizationId == orgId.Value)
            .ToList();
            
        ViewBag.OrganizationId = orgId.Value;
        return View(jobs); 
    }

    // CHANGED: Add UpdateStatus for org
    public IActionResult UpdateStatus(int id, string status, int orgId)
{
    
    var app = _context.ApplyForms.FirstOrDefault(a => a.Id == id);
    if (app == null)
        return RedirectToAction("OrgDash", new { id = orgId });

    // update status
    app.Status = status;

    // send email only
    _email.Send(
        app.Email,
        "Application Status Update",
        $"<h3>Your application is <b>{status}</b></h3>"
    );

    // save changes
    _context.SaveChanges();

    TempData["SuccessMessage"] = "Status updated & email sent!";
    return RedirectToAction("OrgDash", new { id = orgId });
}
public class DeleteJobRequest
{
    public int JobId { get; set; }
    public int OrgId { get; set; }
}

[HttpPost]
public IActionResult DeleteJob([FromBody] DeleteJobRequest req)
{
    var job = _context.Jobs
        .Include(j => j.Organization)
        .FirstOrDefault(j => j.JobId == req.JobId && j.OrganizationId == req.OrgId);

    if (job == null)
        return Json(new { success = false, message = "Job not found" });

    _context.Jobs.Remove(job);
    _context.SaveChanges();
    return Json(new { success = true, message = $"🗑️ Job '{job.Title}' deleted successfully!" });
}
}