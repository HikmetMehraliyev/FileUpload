using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using WebFrontToBack.Areas.Admin.ViewModels;
using WebFrontToBack.DAL;
using WebFrontToBack.Models;
using WebFrontToBack.Utilities.Constants;
using WebFrontToBack.Utilities.Extensions;

namespace WebFrontToBack.Areas.Admin.Controllers
{
    public class RecentWorksController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public RecentWorksController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;

        }
        public async Task<IActionResult> Index()
            {
                ICollection<RecentWorks> work = await _context.RecentWork.ToListAsync();
                return View(work);
            }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RecentWorks work)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            await _context.RecentWork.AddAsync(work);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int Id)
        {
            RecentWorks recentworker = await _context.RecentWork.FindAsync(Id);
            if (recentworker == null) return NotFound();
            string imagePath = Path.Combine(_environment.WebRootPath, "assets", "img", recentworker.ImagePath);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            _context.RecentWork.Remove(recentworker);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

            //RecentWorks? work = _context.RecentWork.Find(Id);
            //if (work == null)
            //{
            //    return NotFound();
            //}
            //_context.RecentWork.Remove(work);
            //_context.SaveChanges();
            //return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Update()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Update(CreateRecentWorksVM worker)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (!worker.Photo.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Photo", $"{worker.Photo.FileName} {Messages.FileTypeMustBeImage}");
                return View();
            }
            if (!worker.Photo.CheckFileSize(200))
            {
                ModelState.AddModelError("Photo", $"{worker.Photo.FileName} {Messages.FileSizeMustBeLess}");
                return View();
            }

            string root = Path.Combine(_environment.WebRootPath, "asstes", "img");

            string filename = await worker.Photo.SaveAsync(root);

            RecentWorks recentWorker = new RecentWorks()
            {
                Title = worker.Title,
                Description = worker.Description,
                ImagePath = filename,
            };

            bool isExists = await _context.RecentWork.AnyAsync(t => t.Id == worker.Id);
            if (!isExists)
            {
                TempData["Exists"] = "Bu work Bazada Yoxdu";
                return RedirectToAction(nameof(Index));
            }
            await _context.RecentWork.AddAsync(recentWorker);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
