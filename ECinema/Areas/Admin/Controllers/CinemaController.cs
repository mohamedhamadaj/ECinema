using ECinema.DataAccess;
using ECinema.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECinema.Areas.Admin.Controllers
{
    public class CinemaController : Controller
    {
        ApplicationDbContext _context = new();
        public IActionResult Index()
        {
            var Cinemas = _context.Cinemas.AsNoTracking().AsQueryable();

            // Add Filter

            return View(Cinemas.Select(e => new
            {
                e.Id,
                e.Name,
                e.Description,
                e.Status,
            }).AsEnumerable());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Cinema Cinema, IFormFile img)
        {
            if (img is not null && img.Length > 0)
            {
                // Save Img in wwwroot
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName); // 30291jsfd4-210klsdf32-4vsfksgs.png
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    img.CopyTo(stream);
                }

                // Save Img in db
                Cinema.Img = fileName;
            }

            // Save Cinema in db
            _context.Cinemas.Add(Cinema);
            _context.SaveChanges();

            //Response.Cookies.Append("success-notification", "Add Cinema Successfully");
            TempData["success-notification"] = "Add Cinema Successfully";

            //return View(nameof(Index));
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var Cinema = _context.Cinemas.FirstOrDefault(e => e.Id == id);

            if (Cinema is null)
                return RedirectToAction("NotFoundPage", "Home");

            return View(Cinema);
        }

        [HttpPost]
        public IActionResult Edit(Cinema Cinema, IFormFile? img)
        {
            var CinemaInDb = _context.Cinemas.AsNoTracking().FirstOrDefault(e => e.Id == Cinema.Id);
            if (CinemaInDb is null)
                return RedirectToAction("NotFoundPage", "Home");

            if (img is not null)
            {
                if (img.Length > 0)
                {
                    // Save Img in wwwroot
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName); // 30291jsfd4-210klsdf32-4vsfksgs.png
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        img.CopyTo(stream);
                    }

                    // Remove old Img in wwwroot
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", CinemaInDb.Img);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }

                    // Save Img in db
                    Cinema.Img = fileName;
                }
            }
            else
            {
                Cinema.Img = CinemaInDb.Img;
            }

            _context.Cinemas.Update(Cinema);
            _context.SaveChanges();

            TempData["success-notification"] = "Update Cinema Successfully";

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var Cinema = _context.Cinemas.FirstOrDefault(e => e.Id == id);

            if (Cinema is null)
                return RedirectToAction("NotFoundPage", "Home");

            // Remove old Img in wwwroot
            var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", Cinema.Img);
            if (System.IO.File.Exists(oldPath))
            {
                System.IO.File.Delete(oldPath);
            }

            _context.Cinemas.Remove(Cinema);
            _context.SaveChanges();

            TempData["success-notification"] = "Delete Cinema Successfully";

            return RedirectToAction(nameof(Index));
        }
    }
}

