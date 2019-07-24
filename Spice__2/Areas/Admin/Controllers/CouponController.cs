using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spice__2.Data;
using Spice__2.Models;

namespace Spice__2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CouponController : Controller
    {

        private readonly ApplicationDbContext _db;

        public CouponController(ApplicationDbContext db) //--> Constructor
        {
            _db = db;
        }

        //Retrieve from database and pass to the view-----------------//
        public async Task<IActionResult> Index()
        {
            return View(await _db.Coupon.ToListAsync());
        }
        //-----------------------------------------------------------//
        //GET -- CREATE
        public IActionResult Create()
        {
            return View();
        }
        //-----------------------------------------------------------//
        //POST -- CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Coupon coupons)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files; //--> We want to fetch the Image uploaded in the form and store it into the variable files. 
                if (files.Count > 0) //--> if the files uploaded fron front-end thne the action.
                {
                    byte[] p1 = null; 

                    using (var fs1 = files[0].OpenReadStream())  //--> We converted the Image into the byte Array. Because we want to put it into database.
                    {
                        using (var ms1 = new MemoryStream())
                        {
                            fs1.CopyTo(ms1);
                            p1 = ms1.ToArray(); //--> It converts our Image to streams of byte and put it into p1 variable.
                        }
                    }
                    coupons.Picture = p1;
                }

                _db.Coupon.Add(coupons);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(coupons);
        }
        //-----------------------------------------------------------//
        //EDIT -- GET
        public async Task<IActionResult> Edit(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }

            var coupon = await _db.Coupon.SingleOrDefaultAsync(m => m.Id == id);

            if (coupon == null)
            {
                return NotFound();
            }

            return View(coupon);
        }
        //------------------------------------------------------------//
    }
}