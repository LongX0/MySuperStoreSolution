using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySuperStore.Data;
using MySuperStore.Entities.Models;
using MySuperStore.Models.Customers;
using MySuperStore.Models.Orders;
using MySuperStore.Utils;

namespace MySuperStore.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Customers
        public async Task<IActionResult> Index()
        {
            var customers = await _context.Customers
                .Select(c => new CustomerListViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Email = c.Email
                })
                .ToListAsync();

            return View(customers);
        }

        // GET: /Customers/Create
        public IActionResult Create() => View();

        // POST: /Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var customer = new Customer
                {
                    Name = model.Name,
                    Email = model.Email
                };

                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: /Customers/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return NotFound();

            var model = new CustomerCreateViewModel
            {
                Name = customer.Name,
                Email = customer.Email
            };

            ViewBag.CustomerId = customer.Id;
            return View(model);
        }

        // POST: /Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CustomerCreateViewModel model)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return NotFound();

            if (ModelState.IsValid)
            {
                customer.Name = model.Name;
                customer.Email = model.Email;

                _context.Update(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: /Customers/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var customer = await _context.Customers
                .Include(c => c.Orders)
                    .ThenInclude(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null) return NotFound();

            var model = new CustomerDetailsViewModel
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                Orders = customer.Orders.Select(o => new OrderSummaryViewModel
                {
                    OrderId = o.Id,
                    OrderDate = o.OrderDate,
                    Total = o.OrderProducts.Sum(op => op.Product.Price * op.Quantity)
                }).ToList()
            };

            return View(model);
        }

        // POST: /Customers/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _context.Customers
                .Include(c => c.Orders)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null) return NotFound();

            if (customer.Orders.Any())
            {
                TempData["Error"] = "Cannot delete customer with existing orders.";
                return RedirectToAction(nameof(Index));
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Customer deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
