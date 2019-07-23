using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spice__2.Data;
using Spice__2.Models;

namespace Spice__2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {

        private readonly ApplicationDbContext _db; //  This is our local object.

        public CategoryController(ApplicationDbContext db) // This is a constructor used for dependency injection. This is retrived from our Container.
        {
            _db = db;
        }

        //Get----> retreive everyting from database and pass it into the view.
        public async Task<IActionResult> Index()
        {
            return View(await _db.Category.ToListAsync());
        }
        //-------------------------------------------------------------------------//
        //Get ---> Create
        public IActionResult Create()
        {
            return View();
        }
        //--------------------------------------------------------------------------//
        //POST --- Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)  // validation happens on Server side to check that users do not leave the empty space in the input place.
            {
                // if is valid
                _db.Category.Add(category);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
        //-----------------------------------------------------------------------//
        //Get -- Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _db.Category.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
        //--------------------------------------------------------------------------//
        //POST ---> Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category)
        {
            // if it is valid
            if (ModelState.IsValid)
            {
                _db.Category.Update(category);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
        //--------------------------------------------------------------------//
        //Get -- Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _db.Category.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        //--------------------------------------------------------------------//
        //POST -- Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int? id)
        {
            var category = await _db.Category.FindAsync(id);

            if (category == null)
            {
                return View();
            }

            _db.Category.Remove(category);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        //---------------------------------------------------------------------//
        //Get -- Details
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var category = await _db.Category.FindAsync(id);

            if(category == null)
            {
                return NotFound();
            }

            return View(category);
        //----------------------------------------------------------------------//    
        }
    }
}