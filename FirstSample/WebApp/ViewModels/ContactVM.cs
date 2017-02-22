﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class ContactVM
    {
        [Required]
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
    }
}
