using ECinema.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECinema.ViewModels;
using ECinema.ViewModels.ECinema.ViewModels;
using ECinema.Models;

namespace ECinema.Areas.Admin.Controllers
{
    public class MovieController1 : Controller
    {
        ApplicationDbContext _context=new();
        public IActionResult Index(FilterMovieVM filterMovieVM, int page = 1)
        {
            
            var Movies = _context.Movies.AsNoTracking().AsQueryable();

            // Add Filter
            Movies = Movies.Include(e => e.Category).Include(e => e.Cinema);

            #region Filter Movie
            // Add Filter 
            if (filterMovieVM.Name is not null)
            {
                Movies = Movies.Where(e => e.Name.Contains(filterMovieVM.Name.Trim()));
                ViewBag.Name = filterMovieVM.Name;
            }

            if (filterMovieVM.Price is null)
            {
                Movies = Movies.Where(e => e.Price==filterMovieVM.Price);
                ViewBag.minPrice = filterMovieVM.Price;
            }

            if (filterMovieVM.CategoryId is not null)
            {
                Movies = Movies.Where(e => e.CategoryId == filterMovieVM.CategoryId);
                ViewBag.CategoryId = filterMovieVM.CategoryId;
            }

            if (filterMovieVM.CinemaId is not null)
            {
                Movies = Movies.Where(e => e.CinemaId == filterMovieVM.CinemaId);
                ViewBag.CinemaId = filterMovieVM.CinemaId;
            }

            

            // Categories
            var categories = _context.Categories;
            //ViewData["categories"] = categories.AsEnumerable();
            ViewBag.categories = categories.AsEnumerable();

            // Cinemas
            var Cinemas = _context.Cinemas;
            ViewData["Cinemas"] = Cinemas.AsEnumerable();
            #endregion

            #region Pagination
            // Pagination
            ViewBag.TotalPages = Math.Ceiling(Movies.Count() / 8.0);
            ViewBag.CurrentPage = page;
            Movies = Movies.Skip((page - 1) * 8).Take(8); // 0 .. 8 
            #endregion

            return View(Movies.AsEnumerable());
        }
        [HttpGet]
        public IActionResult Create()
        {
            // Categories
            var categories = _context.Categories;
            // Cinemas
            var Cinemas = _context.Cinemas;

            return View(new MovieVM
            {
                Categories = categories.AsEnumerable(),
                Cinemas = Cinemas.AsEnumerable(),
            });
        }

        [HttpPost]
        public IActionResult Create(Movie Movie, IFormFile img, List<IFormFile>? subImgs)
        {
            var transaction = _context.Database.BeginTransaction();

            try
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
                    Movie.MainImg = fileName;
                }

                // Save Movie in db
                var MovieCreated = _context.Movies.Add(Movie);
                _context.SaveChanges();

                if (subImgs is not null && subImgs.Count > 0)
                {
                    foreach (var item in subImgs)
                    {
                        // Save Img in wwwroot
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName); // 30291jsfd4-210klsdf32-4vsfksgs.png
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\Movie_images", fileName);

                        using (var stream = System.IO.File.Create(filePath))
                        {
                            img.CopyTo(stream);
                        }

                        _context.MovieSubimages.Add(new()
                        {
                            Img = fileName,
                            MovieId = MovieCreated.Entity.Id
                        });
                    }

                    _context.SaveChanges();
                }

               

                
            }
            catch (Exception ex)
            {
                // Logging
                TempData["error-notification"] = "Error While Saving the Movie";

                transaction.Rollback();

                // Validation
            }

            //return View(nameof(Index));
            return RedirectToAction(nameof(Index));
        }
    }
}
