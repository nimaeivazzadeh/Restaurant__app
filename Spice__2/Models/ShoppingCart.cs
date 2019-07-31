using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Spice__2.Models
{
    public class ShoppingCart
    {

        public ShoppingCart()   //--Constructor. 
        {
            Count = 1;
        }

        public int Id { get; set; }

        public string ApplicationUserId { get; set; }

        [NotMapped]
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }  //--Reference for ApplicationUserId--//

        public int MenuItemId { get; set; }

        [NotMapped]
        [ForeignKey("MenuItemId")]
        public virtual MenuItem MenuItem { get; set; }   //--Reference for MenuItemId--//


        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value greater than or equal {1}")]
        public int Count { get; set; }
    }
}
