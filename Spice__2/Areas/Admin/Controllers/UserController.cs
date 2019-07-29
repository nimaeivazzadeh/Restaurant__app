using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spice__2.Data;
using Spice__2.Utility;

namespace Spice__2.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.ManegerUser)]
    [Area("Admin")]
    public class UserController : Controller
    {
        
        private readonly ApplicationDbContext _db;

        public UserController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var claimIdentity = (ClaimsIdentity)this.User.Identity;  //--> We want to show all users except those that has logged in.
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            
            return View(await _db.ApplicationUser.Where(u=>u.Id != claim.Value).ToListAsync()); 
        }
        //------------------------------------------------------------------------------------//
        public async Task<IActionResult> Lock(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _db.ApplicationUser.FirstOrDefaultAsync(m => m.Id == id);

            applicationUser.LockoutEnd = DateTime.Now.AddYears(1000);

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        //------------------------------------------------------------------------------------//
        public async Task<IActionResult> UnLock(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _db.ApplicationUser.FirstOrDefaultAsync(m => m.Id == id);

            applicationUser.LockoutEnd = DateTime.Now;

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        //------------------------------------------------------------------------------------//
    }
}