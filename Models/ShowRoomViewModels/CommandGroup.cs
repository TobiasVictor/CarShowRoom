using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CarShowRoom.Models.ShowRoomViewModels
{
    public class CommandGroup
    {
        [DataType(DataType.Date)]
        public DateTime? CommandDate { get; set; }
        public int CarCount { get; set; }
    }
}
