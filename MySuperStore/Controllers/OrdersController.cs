using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySuperStore.Data;
using MySuperStore.Entities.Models;
using MySuperStore.Models.Orders;
using MySuperStore.Models.Products;

namespace MySuperStore.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Orders
        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                .Select(o => new OrderListViewModel
                {
                    OrderId = o.Id,
                    CustomerName = o.Customer.Name,
                    CustomerEmail = o.Customer.Email,
                    OrderDate = o.OrderDate,
                    Products = o.OrderProducts.Select(op => new ProductItemViewModel
                    {
                        Id = op.Product.Id,
                        Name = op.Product.Name,
                        Price = op.Product.Price,
                        ImageUrl = op.Product.ImageUrl,
                        Quantity = op.Quantity
                    }).ToList()
                })
                .ToListAsync();

            return View(orders);
        }

        // GET: /Orders/Create
        public IActionResult Create()
        {
            ViewBag.Customers = _context.Customers.ToList();
            ViewBag.Products = _context.Products.ToList();
            return View();
        }

        // POST: /Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var order = new Order
                {
                    CustomerId = model.CustomerId,
                    OrderDate = DateTime.UtcNow
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                if (model.ProductIds != null && model.ProductIds.Any())
                {
                    foreach (var productId in model.ProductIds)
                    {
                        int quantity = model.Quantities != null && model.Quantities.TryGetValue(productId, out var q)
                            ? q : 1;

                        _context.OrderProducts.Add(new OrderProduct
                        {
                            OrderId = order.Id,
                            ProductId = productId,
                            Quantity = quantity
                        });
                    }

                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Customers = _context.Customers.ToList();
            ViewBag.Products = _context.Products.ToList();
            return View(model);
        }


        // GET: /Orders/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return NotFound();

            var products = order.OrderProducts.Select(op => new ProductItemViewModel
            {
                Id = op.ProductId,
                Name = op.Product.Name,
                Price = op.Product.Price,
                ImageUrl = op.Product.ImageUrl,
                Quantity = op.Quantity
            }).ToList();

            var viewModel = new OrderListViewModel
            {
                OrderId = order.Id,
                CustomerName = order.Customer.Name,
                CustomerEmail = order.Customer.Email,
                OrderDate = order.OrderDate,
                Products = products
            };

            return View(viewModel);
        }

        // GET: /Orders/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderProducts)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return NotFound();

            var model = new OrderCreateViewModel
            {
                CustomerId = order.CustomerId,
                ProductIds = order.OrderProducts.Select(op => op.ProductId).ToList(),
                Quantities = order.OrderProducts.ToDictionary(op => op.ProductId, op => op.Quantity)
            };

            ViewBag.Customers = _context.Customers.ToList();
            ViewBag.Products = _context.Products.ToList();
            ViewBag.OrderId = order.Id;

            return View(model);
        }

        // POST: /Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OrderCreateViewModel model)
        {
            var order = await _context.Orders
                .Include(o => o.OrderProducts)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return NotFound();

            if (ModelState.IsValid)
            {
                order.CustomerId = model.CustomerId;

                _context.OrderProducts.RemoveRange(order.OrderProducts);

                if (model.ProductIds != null)
                {
                    foreach (var productId in model.ProductIds)
                    {
                        var quantity = model.Quantities != null && model.Quantities.TryGetValue(productId, out var q)
                            ? q : 1;

                        _context.OrderProducts.Add(new OrderProduct
                        {
                            OrderId = order.Id,
                            ProductId = productId,
                            Quantity = quantity
                        });
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Customers = _context.Customers.ToList();
            ViewBag.Products = _context.Products.ToList();
            return View(model);
        }

        // POST: /Orders/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderProducts)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return NotFound();

            _context.OrderProducts.RemoveRange(order.OrderProducts);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
