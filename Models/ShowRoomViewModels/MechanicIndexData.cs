using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarShowRoom.Models.ShowRoomViewModels
{
    public class MechanicIndexData
    {
        public IEnumerable<Mechanic> Mechanics { get; set; }
        public IEnumerable<Car> Cars { get; set; }
        public IEnumerable<Command> Commands { get; set; }
    }
}
