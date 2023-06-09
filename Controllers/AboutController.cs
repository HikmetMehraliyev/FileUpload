﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFrontToBack.DAL;
using WebFrontToBack.Models;

namespace WebFrontToBack.Controllers
{
    public class AboutController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public AboutController(AppDbContext appDbContext) 
        {
            _appDbContext = appDbContext;
        } 
        public async Task<IActionResult> Index()
        {
            List<TeamMember> members = await _appDbContext.TeamMember.ToListAsync();
            return View(members);
        }
    }
}
