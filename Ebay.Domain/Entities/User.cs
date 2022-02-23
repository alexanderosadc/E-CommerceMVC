using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Domain.Entities
{
    public class User : IdentityUser
    {
        public User(): base()
        {
        }
        public int? CartId { get; set; }
        public virtual Cart Cart { get; set; }
    }
}
