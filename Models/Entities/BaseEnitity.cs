﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Entities
{
    public class BaseEnitity:IdentityUser
    {
        
        public DateTime DateTime { get; set; }
    }
}
