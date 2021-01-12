using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarShowRoom.Models
{
    public class AssembledCar
    {
        public int MechanicID { get; set; }
        public int CarID { get; set; }
        public Mechanic Mechanic { get; set; }
        public Car Car { get; set; }
    }
}
