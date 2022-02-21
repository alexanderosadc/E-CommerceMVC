using Ebay.Domain.Entities.Base;
using Microsoft.AspNetCore.Identity;

namespace Ebay.Domain.Entities
{
    public class UserCart : BaseEntity
    {
        public double TotalSum { get; set; }
        //public int AppUserId { get; set; }
        public virtual IdentityUser AppUser { get; set; }
        public int CartItemId { get; set; }
        public virtual CartItem CartItem { get; set; }
    }
}