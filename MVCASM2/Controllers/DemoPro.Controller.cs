using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCASM2.Models;
using MVCASM2.Data;
using Microsoft.AspNetCore.Authorization;
using MVCASM2.Services;

namespace MVCASM2.Controllers
{
    public class DemoProController : Controller
    {
        private readonly ApplicationDbContext _pro;

        public DemoProController(ApplicationDbContext pro)
        {
            _pro = pro;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult GetAll()
        {
            return View();
        }

        // GET: Demo
        public async Task<IActionResult> Index()
        {
            return View(await _pro.Products.ToListAsync());
        }

        // GET: Demo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _pro.Products == null)
            {
                return NotFound();
            }

            var product = await _pro.Products
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Demo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Demo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                
                _pro.Add(product);
                await _pro.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Demo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _pro.Products == null)
            {
                return NotFound();
            }

            var product = await _pro.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Demo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,Description,Price,Cat_Id")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _pro.Update(product);
                    await _pro.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            return View(product);
        }

        // GET: Demo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _pro.Products == null)
            {
                return NotFound();
            }

            var product = await _pro.Products
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Demo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_pro.Products == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Products'  is null.");
            }
            var product = await _pro.Products.FindAsync(id);
            if (product != null)
            {
                _pro.Products.Remove(product);
            }

            await _pro.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _pro.Products.Any(e => e.ProductId == id);
        }
    }
}

