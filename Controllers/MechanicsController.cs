using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarShowRoom.Data;
using CarShowRoom.Models;
using CarShowRoom.Models.ShowRoomViewModels;

namespace CarShowRoom.Controllers
{
    public class MechanicsController : Controller
    {
        private readonly ShowRoomContext _context;

        public MechanicsController(ShowRoomContext context)
        {
            _context = context;
        }

        // GET: Mechanics
        public async Task<IActionResult> Index(int? id, int? carID)
        {
            var viewModel = new MechanicIndexData();
            viewModel.Mechanics = await _context.Mechanics
            .Include(i => i.AssembledCars)
            .ThenInclude(i => i.Car)
            .ThenInclude(i => i.Commands)
            .ThenInclude(i => i.Client)
            .AsNoTracking()
            .OrderBy(i => i.MechanicName)
            .ToListAsync();
            if (id != null)
            {
                ViewData["MechanicID"] = id.Value;
                Mechanic mechanic = viewModel.Mechanics.Where(
                i => i.ID == id.Value).Single();
                viewModel.Cars = mechanic.AssembledCars.Select(s => s.Car);
            }
            if (carID != null)
            {
                ViewData["CarID"] = carID.Value;
                viewModel.Commands = viewModel.Cars.Where(
                x => x.ID == carID).Single().Commands;
            }
            return View(viewModel);
        }

        // GET: Mechanics/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mechanic = await _context.Mechanics
                .FirstOrDefaultAsync(m => m.ID == id);
            if (mechanic == null)
            {
                return NotFound();
            }

            return View(mechanic);
        }

        // GET: Mechanics/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Mechanics/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,MechanicName,Adress")] Mechanic mechanic)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mechanic);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mechanic);
        }

        // GET: Mechanics/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            {
                if (id == null)
                {
                    return NotFound();
                }
                var mechanic = await _context.Mechanics
                .Include(i => i.AssembledCars).ThenInclude(i => i.Car)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
                if (mechanic == null)
                {
                    return NotFound();
                }
                PopulateAssembledCarData(mechanic);
                return View(mechanic);

            }
        }

        private void PopulateAssembledCarData(Mechanic mechanic)
        {
            var allCars = _context.Cars;
            var mechanicCars = new HashSet<int>(mechanic.AssembledCars.Select(c => c.CarID));
            var viewModel = new List<AssembledCarData>();
            foreach (var car in allCars)
            {
                viewModel.Add(new AssembledCarData
                {
                    CarID = car.ID,
                    Brand = car.Brand,
                    IsAssembled = mechanicCars.Contains(car.ID)
                });
            }
            ViewData["Cars"] = viewModel;
        }

        // POST: Mechanics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedCars)
        {
            if (id == null)
            {
                return NotFound();
            }
            var mechanicToUpdate = await _context.Mechanics
            .Include(i => i.AssembledCars)
            .ThenInclude(i => i.Car)
            .FirstOrDefaultAsync(m => m.ID == id);
            if (await TryUpdateModelAsync<Mechanic>(
            mechanicToUpdate,
            "",
            i => i.MechanicName, i => i.Adress))
            {
                UpdateAssembledCars(selectedCars, mechanicToUpdate);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {

                    ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, ");
                }
                return RedirectToAction(nameof(Index));
            }
            UpdateAssembledCars(selectedCars, mechanicToUpdate);
            PopulateAssembledCarData(mechanicToUpdate);
            return View(mechanicToUpdate);
        }
        private void UpdateAssembledCars(string[] selectedCars, Mechanic mechanicToUpdate)
        {
            if (selectedCars == null)
            {
                mechanicToUpdate.AssembledCars = new List<AssembledCar>();
                return;
            }
            var selectedCarsHS = new HashSet<string>(selectedCars);
            var assembledCars = new HashSet<int>
            (mechanicToUpdate.AssembledCars.Select(c => c.Car.ID));
            foreach (var car in _context.Cars)
            {
                if (selectedCarsHS.Contains(car.ID.ToString()))
                {
                    if (!assembledCars.Contains(car.ID))
                    {
                        mechanicToUpdate.AssembledCars.Add(new AssembledCar
                        {
                            MechanicID =
                       mechanicToUpdate.ID,
                            CarID = car.ID
                        });
                    }
                }
                else
                {
                    if (assembledCars.Contains(car.ID))
                    {
                        AssembledCar carToRemove = mechanicToUpdate.AssembledCars.FirstOrDefault(i
                       => i.CarID == car.ID);
                        _context.Remove(carToRemove);
                    }
                }
            }
        }

        // GET: Mechanics/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mechanic = await _context.Mechanics
                .FirstOrDefaultAsync(m => m.ID == id);
            if (mechanic == null)
            {
                return NotFound();
            }

            return View(mechanic);
        }

        // POST: Mechanics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mechanic = await _context.Mechanics.FindAsync(id);
            _context.Mechanics.Remove(mechanic);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MechanicExists(int id)
        {
            return _context.Mechanics.Any(e => e.ID == id);
        }
    }
}
