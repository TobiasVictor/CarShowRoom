using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CarShowRoom.Models;
using Microsoft.EntityFrameworkCore;
using CarShowRoom.Data;
using CarShowRoom.Models.ShowRoomViewModels;

namespace CarShowRoom.Controllers
{
    public class HomeController : Controller
    {
        private readonly ShowRoomContext _context;
        public HomeController(ShowRoomContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Chat()
        {
            return View();
        }
        public async Task<ActionResult> Statistics()
        {
            IQueryable<CommandGroup> data =
            from order in _context.Commands
            group order by order.CommandDate into dateGroup
            select new CommandGroup()
            {
                CommandDate = dateGroup.Key,
                CarCount = dateGroup.Count()
            };
            return View(await data.AsNoTracking().ToListAsync());
        }
     
    }
}
