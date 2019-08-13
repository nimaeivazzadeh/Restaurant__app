using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Spice__2.Models
{
    public class SubCategory
    {
        [Key]
        public int Id { get; set; }
                       
        [Required]
        [Display(Name = "Sub Category")]
        public string Name { get; set; }


        [Required]              
        [Display(Name="Category")]              //----------------This is a Reference to Category Table-----//
        public int CategoryId { get; set; }


        [ForeignKey("CategoryId")]             //----------------This is a Foregin Key for the Category Table-----//
        public virtual Category Category { get; set; }
    }
}
