using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JBNG20250326.AppWebMVC.Models;
using Microsoft.AspNetCore.Authorization;

namespace JBNG20250326.AppWebMVC.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly Test20250326DbContext _context;

        public ProductsController(Test20250326DbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(Product product, int topRegistro = 10)
        {
            var query = _context.Products.AsQueryable();
            if (product.Id > 0)
                query = query.Where(s => s.Id == product.Id);
            if (product.Id > 0)
                query = query.Where(s => s.Id == product.Id);
            if (topRegistro > 0)
                query = query.Take(topRegistro);
             
            if (!string.IsNullOrWhiteSpace(product.ProductName))
                query = query.Where(s => s.ProductName.Contains(product.ProductName));



            var test20250319DbContext = _context.Products.Include(p => p.Brand).Include(p => p.Warehouse);


            var brands = _context.Brands.ToList();
            brands.Add(new Brand { BrandName = "SELECCIONAR", Id = 0 });
            var categories = _context.Warehouses.ToList();
            categories.Add(new Warehouse { WarehouseName = "SELECCIONAR", Id = 0 });
           // ViewData["Id"] = new SelectList(categories, "Id", "WarehouseName", 0);
            //ViewData["Id"] = new SelectList(brands, "Id", "BrandName", 0);
            return View(await query.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Warehouse)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "BrandName");
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "Id", "WarehouseName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductName,Description,Price,WarehouseId,BrandId")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Id", product.BrandId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "Id", "Id", product.WarehouseId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "BrandName", product.BrandId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "Id", "WarehouseName", product.WarehouseId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductName,Description,Price,WarehouseId,BrandId")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Id", product.BrandId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "Id", "Id", product.WarehouseId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Warehouse)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
