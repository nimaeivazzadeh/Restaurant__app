using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spice__2.Data;
using Spice__2.Models.ViewModels;
using Spice__2.Utility;

namespace Spice__2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MenuItemController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IHostingEnvironment _hostingEivironment;

        [BindProperty]
        public MenuItemViewModel MenuItemVM { get; set; }

        public MenuItemController(ApplicationDbContext db, IHostingEnvironment hostingEnvironment)  //--Constructor
        {
            _db = db;
            _hostingEivironment = hostingEnvironment;
            MenuItemVM = new MenuItemViewModel()
            {
                Category = _db.Category,
                MenuItem = new Models.MenuItem()
            };
        }

        public async Task<IActionResult> Index()
        {
            var menuItem = await _db.MenuItem.Include(m => m.Category).Include(m => m.SubCategory).ToListAsync();
            return View(menuItem);
        }
        //--------------------------------------------------------------------------------//
        //GET -- CREATE
        public IActionResult Create()
        {
            return View(MenuItemVM);
        }
        //--------------------------------------------------------------------------------//
        //POST -- CREATE
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePOST()
        {
            MenuItemVM.MenuItem.SubCategoryId = Convert.ToInt32(Request.Form["SubCategoryId"].ToString());

            if (!ModelState.IsValid)
            {
                MenuItemVM.SubCategory = await _db.SubCategories.Where(s => s.CategoryId == MenuItemVM.MenuItem.CategoryId).ToListAsync();
                return View(MenuItemVM);
            }

            _db.MenuItem.Add(MenuItemVM.MenuItem);
            await _db.SaveChangesAsync();

            // work on the image saving section.

            string webRootPath = _hostingEivironment.WebRootPath;

            var files = HttpContext.Request.Form.Files;  // extract all files that users has uploaded.

            var menuItemFromDb = await _db.MenuItem.FindAsync(MenuItemVM.MenuItem.Id); // extract menuItem from database what ever has been saved.

            if (files.Count()> 0)
            {
                // files has been uploaded.
                var uploads = Path.Combine(webRootPath, "images");
                var extention = Path.GetExtension(files[0].FileName); 

                using (var filesStream = new FileStream(Path.Combine(uploads, MenuItemVM.MenuItem.Id + extention), FileMode.Create))
                {
                    files[0].CopyTo(filesStream);
                }

                menuItemFromDb.Image = @"\images\" + MenuItemVM.MenuItem.Id + extention;
            }
             else
            {
                // No files was uploaded. So, use default.
                var uploads = Path.Combine(webRootPath, @"images\" + SD.DefaultFoodImage);

                System.IO.File.Copy(uploads, webRootPath + @"\images\" + MenuItemVM.MenuItem.Id + ".png"); // It makes a copy of the default image to the wwwroot/images older.
                menuItemFromDb.Image = @"\images\" + MenuItemVM.MenuItem.Id + ".png";
            }

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
         
        }
        //----------------------------------------------------------------------------------//
        //----------------------------------------------------------------------------------//
        //GET -- EDIT
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MenuItemVM.MenuItem = await _db.MenuItem.Include(m=>m.Category).Include(m=>m.SubCategory).SingleOrDefaultAsync(m=>m.Id==id);
            MenuItemVM.SubCategory = await _db.SubCategories.Where(s => s.CategoryId == MenuItemVM.MenuItem.CategoryId).ToListAsync();

            if (MenuItemVM.MenuItem == null)
            {
                return NotFound();
            }

            return View(MenuItemVM);
        }
        //--------------------------------------------------------------------------------//
        //POST -- EDIT
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPOST(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            MenuItemVM.MenuItem.SubCategoryId = Convert.ToInt32(Request.Form["SubCategoryId"].ToString());

            if (!ModelState.IsValid)
            {
                return View(MenuItemVM);
            }

            //_db.MenuItem.Add(MenuItemVM.MenuItem);
            //await _db.SaveChangesAsync();

            // work on the image saving section.

            string webRootPath = _hostingEivironment.WebRootPath;

            var files = HttpContext.Request.Form.Files;  // extract all files that users has uploaded.

            var menuItemFromDb = await _db.MenuItem.FindAsync(MenuItemVM.MenuItem.Id); // extract menuItem from database what ever has been saved.

            if (files.Count() > 0)
            {
                // New Image has been uploaded.
                var uploads = Path.Combine(webRootPath, "images");
                var extention_new = Path.GetExtension(files[0].FileName);


                // Delete the original file.
                var imagePath = Path.Combine(webRootPath, menuItemFromDb.Image.TrimStart('\\'));

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                // We will upload a new file.
                using (var filesStream = new FileStream(Path.Combine(uploads, MenuItemVM.MenuItem.Id + extention_new), FileMode.Create))
                {
                    files[0].CopyTo(filesStream);
                }

                menuItemFromDb.Image = @"\images\" + MenuItemVM.MenuItem.Id + extention_new;
            }

            menuItemFromDb.Name = MenuItemVM.MenuItem.Name;
            menuItemFromDb.Description = MenuItemVM.MenuItem.Description;
            menuItemFromDb.Price = MenuItemVM.MenuItem.Price;
            menuItemFromDb.Spicyness = MenuItemVM.MenuItem.Spicyness;
            menuItemFromDb.CategoryId = MenuItemVM.MenuItem.CategoryId;
            menuItemFromDb.SubCategoryId = MenuItemVM.MenuItem.SubCategoryId;
           
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        //--------------------------------------------------------------------------------//
        // GET -- DELETE
        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var menuItem = await _db.MenuItem.FindAsync(id);

            if (menuItem == null)
            {
                return NotFound();
            }

            return View(menuItem);
        }
        //--------------------------------------------------------------------------------//
        // POST -- DELETE
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePOST(int? id)
        {
            var menuItem = await _db.MenuItem.FindAsync(id);

            if (menuItem == null)
            {
                return View();
            }

            _db.MenuItem.Remove(menuItem);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        //--------------------------------------------------------------------------------//

    }

}