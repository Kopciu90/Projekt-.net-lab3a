using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Komis.Models; // Zmień na odpowiednią przestrzeń nazw
using Microsoft.AspNetCore.Mvc.Rendering;
using Komis.Models;
using Microsoft.AspNetCore.Authorization;

namespace YourNamespace.Controllers
{
    public class SalesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SalesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Sales
        public async Task<IActionResult> Index()
        {
            var sales = _context.Sales.Include(s => s.Car).Include(s => s.Customer);
            return View(await sales.ToListAsync());
        }

        // GET: Sales/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sale = await _context.Sales
                .Include(s => s.Car)
                .Include(s => s.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sale == null)
            {
                return NotFound();
            }

            return View(sale);
        }
        [Authorize(Roles = "Admin")]

        public IActionResult Create()
        {
            ViewData["CarId"] = new SelectList(_context.Cars.OrderBy(c => c.Make), "Id", "Make");
            ViewData["CustomerId"] = new SelectList(_context.Customers.OrderBy(c => c.FirstName), "Id", "FirstName");
            return View();
        }

        [Authorize(Roles = "Admin")]
        // POST: Sales/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CarId,CustomerId,DateSold")] Sale sale)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(sale);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarId"] = new SelectList(_context.Cars.OrderBy(c => c.Make), "Id", "Make", sale.CarId);
            ViewData["CustomerId"] = new SelectList(_context.Customers.OrderBy(c => c.FirstName), "Id", "FirstName", sale.CustomerId);
            return View(sale);
        }


        [Authorize(Roles = "Admin")]
        // GET: Sales/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sale = await _context.Sales.FindAsync(id);
            if (sale == null)
            {
                return NotFound();
            }
            ViewData["CarId"] = new SelectList(_context.Cars.OrderBy(c => c.Make), "Id", "Make", sale.CarId);
            ViewData["CustomerId"] = new SelectList(_context.Customers.OrderBy(c => c.FirstName), "Id", "FirstName", sale.CustomerId);
            return View(sale);
        }

        [Authorize(Roles = "Admin")]
        // POST: Sales/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CarId,CustomerId,DateSold")] Sale sale)
        {
            if (id != sale.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(sale);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SaleExists(sale.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarId"] = new SelectList(_context.Cars.OrderBy(c => c.Make), "Id", "Make", sale.CarId);
            ViewData["CustomerId"] = new SelectList(_context.Customers.OrderBy(c => c.FirstName), "Id", "FirstName", sale.CustomerId);
            return View(sale);
        }

        [Authorize(Roles = "Admin")]

        // GET: Sales/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sale = await _context.Sales
                .Include(s => s.Car)
                .Include(s => s.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sale == null)
            {
                return NotFound();
            }

            return View(sale);
        }
        [Authorize(Roles = "Admin")]

        // POST: Sales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sale = await _context.Sales.FindAsync(id);
            _context.Sales.Remove(sale);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SaleExists(int id)
        {
            return _context.Sales.Any(e => e.Id == id);
        }
    }
}
