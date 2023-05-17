using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.AccessControl;
using WebFrontToBack.Areas.Admin.ViewModels;
using WebFrontToBack.DAL;
using WebFrontToBack.Models;
using WebFrontToBack.Utilities.Constants;
using WebFrontToBack.Utilities.Extensions;

namespace WebFrontToBack.Areas.Admin.Controllers
{
    public class TeamMemberContoller : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public TeamMemberContoller(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;

        }
        public async Task<IActionResult> Index()
        {
            ICollection<TeamMember> members = await _context.TeamMember.ToListAsync();
            return View(members);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateTeamMemberVM member)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (!member.Photo.CheckContentType("image/"))
            {
                ModelState.AddModelError("Photo",$"{member.Photo.FileName} {Messages.FileTypeMustBeImage}");
                return View();
            }
            if (!member.Photo.CheckFileSize(200))
            {
                ModelState.AddModelError("Photo", $"{member.Photo.FileName} {Messages.FileSizeMustBeLess}");
                return View();
            }

            string root = Path.Combine(_environment.WebRootPath,"asstes","img");

            string filename = await member.Photo.SaveAsync(root);

            TeamMember teammember = new TeamMember()
            {
                FullName = member.FullName,
                Profession = member.Profession,
                ImagePath = filename,
            };

            await _context.TeamMember.AddAsync(teammember);
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
            TeamMember teammember = await _context.TeamMember.FindAsync(Id);
            if (teammember == null) return NotFound();
            string imagePath = Path.Combine(_environment.WebRootPath, "assets", "img",teammember.ImagePath);
            if (System.IO.File.Exists(imagePath)) 
            {
                System.IO.File.Delete(imagePath);
            }

            _context.TeamMember.Remove(teammember);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

            //TeamMember? member =  _context.TeamMember.Find(Id);
            //if (member == null)
            //{
            //    return NotFound();
            //}
            //_context.TeamMember.Remove(member);
            //_context.SaveChanges();
            //return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Update()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Update(TeamMember member)
        {
            if (!ModelState.IsValid) 
            {
                return View();
            }
            bool isExists = await _context.TeamMember.AnyAsync(t=>t.Id==member.Id);
            if (!isExists) 
            {
                TempData["Exists"] = "Bu Member Bazada Yoxdu";
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
