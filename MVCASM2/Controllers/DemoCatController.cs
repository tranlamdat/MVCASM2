using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCASM2.Models;
using MVCASM2.Data;

namespace MVCASM2.Controllers
{
    public class DemoCatController : Controller
    {
        private readonly ApplicationDbContext _cat;

        public DemoCatController(ApplicationDbContext cat)
        {
            _cat = cat;
        }

        // GET: Demo
        public async Task<IActionResult> Index()
        {
            return View(await _cat.Categories.ToListAsync());
        }

        // GET: Demo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _cat.Categories == null)
            {
                return NotFound();
            }

            var category = await _cat.Categories
                .FirstOrDefaultAsync(m => m.Cat_Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
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
        public async Task<IActionResult> Create([Bind("Cat_Id,Cat_Name,Cat_Des")] Category category)
        {
            if (ModelState.IsValid)
            {
                _cat.Add(category);
                await _cat.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Demo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _cat.Categories == null)
            {
                return NotFound();
            }

            var category = await _cat.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Demo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Cat_Id,Cat_Name,Cat_Des")] Category category)
        {
            if (id != category.Cat_Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _cat.Update(category);
                    await _cat.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Cat_Id))
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
            return View(category);
        }

        // GET: Demo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _cat.Categories == null)
            {
                return NotFound();
            }

            var category = await _cat.Categories
                .FirstOrDefaultAsync(m => m.Cat_Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Demo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_cat.Categories == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Categories'  is null.");
            }
            var category = await _cat.Categories.FindAsync(id);
            if (category != null)
            {
                _cat.Categories.Remove(category);
            }

            await _cat.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _cat.Categories.Any(e => e.Cat_Id == id);
        }
    }
}

