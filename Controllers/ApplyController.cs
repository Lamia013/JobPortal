using Microsoft.AspNetCore.Mvc;
using JobPortal.Models;
using JobPortal.Data;

namespace JobPortal.Controllers
{
    public class ApplyController : Controller
    {
        private readonly JobPortalContext _context;
        private readonly IWebHostEnvironment _env;

        public ApplyController(JobPortalContext context,
                               IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        

    public IActionResult Notifications()
    {
        var email = HttpContext.Session.GetString("UserName");

        var list = _context.Notifications
            .Where(x => x.Email == email)
            .OrderByDescending(x => x.CreatedAt)
            .ToList();

        return View(list);
    }

        // ===== GET =====
        [HttpGet]
        public IActionResult Apply()
        {
            return View();
        }

        // ===== POST: APPLICANT SUBMIT =====
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply(Apply model)
        {
           

            if (model.ResumeFile == null || model.CoverLetterFile == null)
            {
                ViewBag.Error = "Resume and Cover Letter are required";
                return View(model);
            }

            string folder = Path.Combine(
                _env.WebRootPath, "uploads/applications");

            Directory.CreateDirectory(folder);

            // ===== RESUME =====
            string resumeName = Guid.NewGuid() + "_" + model.ResumeFile.FileName;
            string resumePath = Path.Combine(folder, resumeName);

            using (var fs = new FileStream(resumePath, FileMode.Create))
            {
                await model.ResumeFile.CopyToAsync(fs);
            }

            using (var ms = new MemoryStream())
            {
                await model.ResumeFile.CopyToAsync(ms);
                model.ResumeData = ms.ToArray();
            }

            model.ResumeFileName = model.ResumeFile.FileName;
            model.ResumePath = "uploads/applications/" + resumeName;

            // ===== COVER LETTER =====
            string coverName = Guid.NewGuid() + "_" + model.CoverLetterFile.FileName;
            string coverPath = Path.Combine(folder, coverName);

            using (var fs = new FileStream(coverPath, FileMode.Create))
            {
                await model.CoverLetterFile.CopyToAsync(fs);
            }

            using (var ms = new MemoryStream())
            {
                await model.CoverLetterFile.CopyToAsync(ms);
                model.CoverLetterData = ms.ToArray();
            }

            model.CoverLetterFileName = model.CoverLetterFile.FileName;
            model.CoverLetterPath = "uploads/applications/" + coverName;

            // ===== SAVE TO DB =====
            _context.ApplyForms.Add(model);
            await _context.SaveChangesAsync();

           
            TempData["SuccessMessage"] = "Your application has been submitted successfully!";

            return RedirectToAction("Dashboard", "Applicant");

        }
    }
}
