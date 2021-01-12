using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarShowRoom.Data;
using CarShowRoom.Models;

namespace CarShowRoom.Controllers
{
    public class CarsController : Controller
    {
        private readonly ShowRoomContext _context;

        public CarsController(ShowRoomContext context)
        {
            _context = context;
        }

        // GET: Cars
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var cars = from b in _context.Cars
                       select b;
            if (!String.IsNullOrEmpty(searchString))
            {
                cars = cars.Where(s => s.Brand.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "title_desc":
                    cars = cars.OrderByDescending(b => b.Brand);
                    break;
                case "Price":
                    cars = cars.OrderBy(b => b.Price);
                    break;
                case "price_desc":
                    cars = cars.OrderByDescending(b => b.Price);
                    break;
                default:
                    cars = cars.OrderBy(b => b.Brand);
                    break;
            }
            int pageSize = 2;
            return View(await PaginatedList<Car>.CreateAsync(cars.AsNoTracking(), pageNumber ??
           1, pageSize));
        }
        // GET: Cars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .Include(s => s.Commands)
                .ThenInclude(e => e.Client)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // GET: Cars/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Brand,Model,Price")] Car car)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(car);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException /* ex*/)
            {

                ModelState.AddModelError("", "Unable to save changes. " +
                "Try again, and if the problem persists ");
            }
            return View(car);
        }

        // GET: Cars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Brand,Model,Price")] Car car)
        {
            if (id == null)
            {
                return NotFound();
            }
            var manToUpdate = await _context.Cars.FirstOrDefaultAsync(s => s.ID == id);
            if (await TryUpdateModelAsync<Car>(
            manToUpdate,
            "",
            s => s.Model, s => s.Brand, s => s.Price))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists");
                }
            }
            return View(manToUpdate);

        }

        // GET: Cars/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (car == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                "Delete failed. Try again";
            }

            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.Cars.Remove(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException/* ex */)
            {
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.ID == id);
        }
    }
}
