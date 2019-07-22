using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spice__2.Models.ViewModels
{
    public class SubCategoryAndCategoryViewModels
    {
        public IEnumerable<Category> CategoryList { get; set; }  // This is our categories wish to display it in dropdown.

        public SubCategory subCategory { get; set; }    // This is our SubCategories

        public List<string> subCategoryList { get; set; } // This is our SubCategoryList

        public string StatusMessage { get; set; }  // This is a static message
    }
}
