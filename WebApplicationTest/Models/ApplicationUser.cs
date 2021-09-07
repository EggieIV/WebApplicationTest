﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationTest.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool Subscription { get; set; }
    }
}
