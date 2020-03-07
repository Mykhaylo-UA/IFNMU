using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace IFNMUSiteCore.Models
{
    public class User : IdentityUser
    {
        public int? ChairId { get; set; }
    }
}
