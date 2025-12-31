using Microsoft.AspNetCore.Mvc;
using JobPortal.Data;
using JobPortal.Models;
using JobPortal.Helpers;
using System.Linq;

namespace JobPortal.Controllers
{
    public class AccountController : Controller
    {
        private readonly JobPortalDbContext _context;

        public AccountController(JobPortalDbContext context)
        {
            _context = context;
        }

        // =========================
        // LOGIN (GET)
        // =========================
        public IActionResult Login()
        {
            return View();
        }

        // =========================
        // LOGIN (POST)
        // =========================
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Invalid login data";
                return View();
            }

            // ---------- ADMIN LOGIN ----------
            LoginValidator adminCheck = (email, password) =>
                email == "admin@jobportal.com" && password == "Admin123";

            if (model.Role == "Admin" && adminCheck(model.Email, model.Password))
            {
                HttpContext.Session.SetString("Role", "Admin");
                return RedirectToAction("Index", "Jobs");
            }

            // ---------- USER LOGIN ----------
            if (model.Role == "User")
            {
                var user = _context.Users
                    .FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

                if (user != null)
                {
                    HttpContext.Session.SetInt32("UserId", user.Id);
                    HttpContext.Session.SetString("Role", "User");
                    return RedirectToAction("Index", "Jobs");
                }
            }

            // ---------- ORGANIZATION LOGIN ----------
            if (model.Role == "Organization")
            {
                var org = _context.Employers
                    .FirstOrDefault(o => o.Email == model.Email && o.Password == model.Password);

                if (org != null)
                {
                    HttpContext.Session.SetString("Role", "Organization");
                    return RedirectToAction("Index", "Jobs");
                }
            }

            ViewBag.Error = "Invalid Email, Password or Role";
            return View();
        }

        // =========================
        // USER REGISTER
        // =========================
        public IActionResult UserRegister()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UserRegister(User user)
        {
            if (!ModelState.IsValid)
                return View(user);

            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }

        // =========================
        // ORGANIZATION REGISTER
        // =========================
        public IActionResult OrganizationRegister()
        {
            return View();
        }

        [HttpPost]
        public IActionResult OrganizationRegister(Employer org)
        {
            if (!ModelState.IsValid)
                return View(org);

            _context.Employers.Add(org);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }

        // =========================
        // LOGOUT
        // =========================
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Jobs");
        }
    }
}
