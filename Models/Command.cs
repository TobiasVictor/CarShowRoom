using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarShowRoom.Models
{
    public class Command
    {
        public int CommandID { get; set; }
        public int ClientID { get; set; }
        public int CarID { get; set; }

        public DateTime CommandDate { get; set; }

        public Client Client { get; set; }
        public Car Car { get; set; }
    }
}
