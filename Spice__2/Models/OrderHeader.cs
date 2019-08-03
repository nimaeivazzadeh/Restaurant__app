using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Spice__2.Models
{
    public class OrderHeader
    {
        [key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public double OrderTotalOriginal { get; set; }  //--This is original OrderTotal without Coupon used--//
        
        [Required]
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name ="Order Total")]
        public double OrderTotal { get; set; } //-- Final OrderTotal after the coupon used and discound has been applied.--//

        [Required]
        [Display(Name ="Pickup Time")]
        public DateTime PickUpTime { get; set; }
        
        [Required]
        [NotMapped]
        public DateTime PickUpDate { get; set; }

        [Display(Name ="Coupon Code")]
        public string CouponCode { get; set; }
        public double CouponCodeDiscount { get; set; }
        public string Status { get; set; }
        public string PaymentStatus { get; set; }
        public string Comments  { get; set; }


        [Display(Name ="Pickup Name")]
        public string PickUpName { get; set; }

        [Display(Name ="Phone Number")]
        public string PhoneNumber { get; set; }
        
        public string  TransactionId { get; set; }

    }
}
