using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Spice__2.Data;
using Spice__2.Models;
using Spice__2.Models.ViewModels;
using Spice__2.Utility;

namespace Spice__2.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.ManegerUser)]
    [Area("Admin")]
    public class SubCategoryController : Controller
    {

        private readonly ApplicationDbContext _db;   //--> Dependency Injection.

        [TempData]
        public string StatusMessage { get; set; }   

        public SubCategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        //--------------------------------------------------------------------------------------------//
        //-- Get --> Index -- retreive everyting from database and pass it into the view.

        public async Task<IActionResult> Index()
        {
            var subCategories = await _db.SubCategories.Include(s => s.Category).ToListAsync(); // We need to retrieve all subcategories cause we want to display them.
            return View(subCategories);
        }

        //--------------------------------------------------------------------------------------------//
        //Get -- CREATE
        public async Task<IActionResult> Create()
        {
            SubCategoryAndCategoryViewModels model = new SubCategoryAndCategoryViewModels()
            {
                CategoryList = await _db.Category.ToListAsync(),
                subCategory = new Models.SubCategory(),
                subCategoryList = await _db.SubCategories.OrderBy(p => p.Name).Select(p => p.Name).Distinct().ToListAsync(),
            };

            return View(model);
        }
        //---------------------------------------------------------------------------------------------//
        //POST -- CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubCategoryAndCategoryViewModels model)
        {
            if (ModelState.IsValid)
            {
                var doesSubCategoryExists = _db.SubCategories.Include(e => e.Category).Where(e => e.Name == model.subCategory.Name && e.Category.Id == model.subCategory.CategoryId);

                if (doesSubCategoryExists.Count() > 0)
                {
                    //Error
                    StatusMessage = "Error: Sub Category exists under " + doesSubCategoryExists.First().Name + " category. Please use another name.";
                }
                else
                {
                    _db.SubCategories.Add(model.subCategory);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

            }

            SubCategoryAndCategoryViewModels ModelVM = new SubCategoryAndCategoryViewModels()
            {
                CategoryList = await _db.Category.ToListAsync(),
                subCategory = model.subCategory,
                subCategoryList = await _db.SubCategories.OrderBy(p => p.Name).Select(p => p.Name).ToListAsync(),
                StatusMessage = StatusMessage
            };

            return View(ModelVM);
        }

        //---------------------------------------------------------------------------------------------//   

        [ActionName("GetSubCategory")]
        public async Task<IActionResult> GetSubCategory(int id)
        {
            List<SubCategory> subCategories = new List<SubCategory>();

            subCategories = await (from subCategory in _db.SubCategories
                                   where subCategory.CategoryId == id
                                   select subCategory).ToListAsync();
            return Json(new SelectList(subCategories, "Id", "Name"));
        }

        //---------------------------------------------------------------------------------------------//
        //Get -- EDIT
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();

            }

            var SubCategory = await _db.SubCategories.SingleOrDefaultAsync(m => m.Id == id);

            if (SubCategory == null)
            {
                return NotFound();
            }

            SubCategoryAndCategoryViewModels model = new SubCategoryAndCategoryViewModels()
            {
                CategoryList = await _db.Category.ToListAsync(),
                subCategory = SubCategory,
                subCategoryList = await _db.SubCategories.OrderBy(p => p.Name).Select(p => p.Name).Distinct().ToListAsync(),
            };

            return View(model);
        }
        //---------------------------------------------------------------------------------------------//
        //POST -- EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SubCategoryAndCategoryViewModels model)
        {
            if (ModelState.IsValid)
            {
                var doesSubCategoryExists = _db.SubCategories.Include(e => e.Category).Where(e => e.Name == model.subCategory.Name && e.Category.Id == model.subCategory.CategoryId);

                if (doesSubCategoryExists.Count() > 0)
                {
                    //Error
                    StatusMessage = "Error: Sub Category exists under " + doesSubCategoryExists.First().Name + " category. Please use another name.";
                }
                else
                {
                    var subCatFromDb = await _db.SubCategories.FindAsync(model.subCategory.Id);
                    subCatFromDb.Name = model.subCategory.Name;

                   // _db.SubCategories.Add(model.subCategory);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

            }

            SubCategoryAndCategoryViewModels ModelVM = new SubCategoryAndCategoryViewModels()
            {
                CategoryList = await _db.Category.ToListAsync(),
                subCategory = model.subCategory,
                subCategoryList = await _db.SubCategories.OrderBy(p => p.Name).Select(p => p.Name).ToListAsync(),
                StatusMessage = StatusMessage
            };
        //      ModelVM.subCategory.Id = id;
            return View(ModelVM);
        }
        //------------------------------------------------------------------------------------------------//
        //GET -- Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subCategory = await _db.SubCategories.FindAsync(id);

            if (subCategory == null)
            {
                return NotFound();
            }

            return View(subCategory);
        }
        //------------------------------------------------------------------------------------------------//
        //Get -- Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subcategory = await _db.SubCategories.FindAsync(id);

            if (subcategory == null)
            {
                return NotFound();
            }
            return View(subcategory);
        }
        //------------------------------------------------------------------------------------------------//
        //POST -- Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int? id)
        {
            var subcategory = await _db.SubCategories.FindAsync(id);

            if (subcategory == null)
            {
                return View();
            }

            _db.SubCategories.Remove(subcategory);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        //------------------------------------------------------------------------------------------------//
    }
}