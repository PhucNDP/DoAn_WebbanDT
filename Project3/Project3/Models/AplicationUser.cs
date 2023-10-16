﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project3.Models
{
    public class AplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public DateTime Birthday { get; set; }
    }
}