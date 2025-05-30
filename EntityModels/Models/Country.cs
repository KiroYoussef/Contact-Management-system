﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModels.Models
{
    public class Country
    {
        [Key]
        public string Code { get; set; }
        public int NumberLength { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }


    }
}
