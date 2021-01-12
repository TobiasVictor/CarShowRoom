﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CarShowRoom.Models
{
    public class Mechanic
    {

        public int ID { get; set; }
        [Required]
        [Display(Name = "Mechanic Name")]
        [StringLength(50)]
        public string MechanicName { get; set; }

        [StringLength(70)]
        public string Adress { get; set; }
        public ICollection<AssembledCar> AssembledCars { get; set; }
    }
}
