using Microsoft.AspNetCore.Mvc;
using WebFrontToBack.DAL;
using WebFrontToBack.Models;
using Microsoft.EntityFrameworkCore;

namespace WebFrontToBack.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ServiceController : Controller
	{

		private readonly AppDbContext _context;

		public ServiceController(AppDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			ICollection<Service> serviceies = await _context.Services.ToListAsync();
			return View(serviceies);
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(Service service)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			bool isExists = await _context.Services.AnyAsync(c =>
			c.Name.ToLower().Trim() == service.Name.ToLower().Trim());

			if (isExists)
			{
				ModelState.AddModelError("Name", "Service name already exists");
				return View();
			}
			await _context.Services.AddAsync(service);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}


		public IActionResult Update(int Id)
		{
			Service? service = _context.Services.Find(Id);

			if (service == null)
			{
				return NotFound();
			}

			return View(service);
		}

		[HttpPost]
		public IActionResult Update(Service service)
		{
            Service? editedService = _context.Services.Find(service.Id);
			if (editedService == null)
			{
				return NotFound();
			}
			editedService.Name = service.Name;
			_context.Services.Update(editedService);
			_context.SaveChanges();
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Delete(int Id)
		{
			Service? service = _context.Services.Find(Id);
			if (service == null)
			{
				return NotFound();
			}
			_context.Services.Remove(service);
			_context.SaveChanges();
			return RedirectToAction(nameof(Index));
		}
	}
}
