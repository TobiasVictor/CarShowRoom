using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarShowRoom.Models
{
    public class Car
    {

        public int ID { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        [Column(TypeName = "decimal(6, 2)")]
        public decimal Price { get; set; }
        public ICollection<Command> Commands { get; set; }

        public ICollection<AssembledCar> AssembledCars { get; set; }


    }
}
