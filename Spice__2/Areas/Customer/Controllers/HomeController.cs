using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spice__2.Data;
using Spice__2.Models;
using Spice__2.Models.ViewModels;

namespace Spice__2.Controllers
{   
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db; //---------------> dependency Injection.

        public HomeController(ApplicationDbContext db) 
        {
            _db = db;
        }
        //-----------------------------------------------------------//
        public async Task<IActionResult> Index()
        {
            IndexViewModel IndexVM = new IndexViewModel() //--> We made an Object from related ViewModels. Then we want to fetch three objects such as MenuItem, Category and Coupon.
            {
                MenuItem = await _db.MenuItem.Include(m => m.Category).Include(m => m.SubCategory).ToListAsync(),
                Category = await _db.Category.ToListAsync(),
                Coupon = await _db.Coupon.Where(c => c.IsActive == true).ToListAsync()
            };

            return View(IndexVM);
        }
        //-----------------------------------------------------------//
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
