using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spice__2.Data;
using Spice__2.Models;
using Spice__2.Models.ViewModels;
using Spice__2.Utility;

namespace Spice__2.Controllers
{   
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db; //---------------> dependency Injection.

        public HomeController(ApplicationDbContext db) //-----------> Constructor
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

            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                var cnt = _db.ShoppingCart.Where(u => u.ApplicationUserId == claim.Value).ToList().Count();
                HttpContext.Session.SetInt32(SD.ssShoppingCartCount, cnt);
            }

            return View(IndexVM);
        }
        //------------------------Details button functionality in the landing page-----------------------------------//
        //GET
        public async Task<IActionResult> Details(int id)
        {
            var menuItemFromDb = await _db.MenuItem.Include(m => m.Category).Include(m => m.SubCategory).Where(m => m.Id == id).FirstOrDefaultAsync();

            ShoppingCart cartObj = new ShoppingCart  //-- We made an Object from ShoppingCart
            {
                MenuItem = menuItemFromDb,
                MenuItemId = menuItemFromDb.Id
            };

            return View(cartObj);
        }
        //------------------------This part add cart to the database------------------------------------------------//
        //POST
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(ShoppingCart CartObject)
        {
            CartObject.Id = 0 ;

            if (ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity) this.User.Identity;       
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);   ///--> We check if the menu item has already exists.

                CartObject.ApplicationUserId = claim.Value;

                ShoppingCart cartFromDb = await _db.ShoppingCart.Where(c => c.ApplicationUserId == CartObject.ApplicationUserId && c.MenuItemId == CartObject.MenuItemId).FirstOrDefaultAsync();

                if (cartFromDb == null) //-> if user have not picked any card before. 
                {
                    await _db.ShoppingCart.AddAsync(CartObject);
                }
                else 
                {
                    cartFromDb.Count = cartFromDb.Count + CartObject.Count;  //-> we update the count if the record has already exist.
                }

                await _db.SaveChangesAsync();

                var count = _db.ShoppingCart.Where(c => c.ApplicationUserId == CartObject.ApplicationUserId).ToList().Count(); //->Retrieve how many MenuItems user has.
                HttpContext.Session.SetInt32(SD.ssShoppingCartCount, count); //-> store it into a session.

                return RedirectToAction("Index");

            }
            else
            {
                var menuItemFromDb = await _db.MenuItem.Include(m => m.Category).Include(m => m.SubCategory).Where(m => m.Id == CartObject.MenuItemId).FirstOrDefaultAsync();

                ShoppingCart cartObj = new ShoppingCart  
                {
                    MenuItem = menuItemFromDb,
                    MenuItemId = menuItemFromDb.Id
                };

                return View(cartObj);
            }
        }
        //-------------------------------------------------------------------------------------------//
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        //-----------------------------------------------------------//
    }
}
