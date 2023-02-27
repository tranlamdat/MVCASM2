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
    public class DemoOrderController : Controller
    {
        private readonly ApplicationDbContext _ord;

        public DemoOrderController(ApplicationDbContext ord)
        {
            _ord = ord;
        }

        // GET: Demo
        public async Task<IActionResult> Index()
        {
            return View(await _ord.Orders.ToListAsync());
        }

        // GET: Demo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _ord.Orders == null)
            {
                return NotFound();
            }

            var order = await _ord.Orders
                .FirstOrDefaultAsync(m => m.Order_Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
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
        public async Task<IActionResult> Create([Bind("Order_Id,Cus_Name,OrderDate,")] Order order)
        {
            if (ModelState.IsValid)
            {
                _ord.Add(order);
                await _ord.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Demo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _ord.Orders == null)
            {
                return NotFound();
            }

            var order = await _ord.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Demo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Order_Id,Cus_Name,OrderDate,")] Order order)
        {
            if (id != order.Order_Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _ord.Update(order);
                    await _ord.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Order_Id))
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
            return View(order);
        }

        // GET: Demo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _ord.Orders == null)
            {
                return NotFound();
            }

            var order = await _ord.Orders
                .FirstOrDefaultAsync(m => m.Order_Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Demo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_ord.Orders == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Orders'  is null.");
            }
            var order = await _ord.Orders.FindAsync(id);
            if (order != null)
            {
                _ord.Orders.Remove(order);
            }

            await _ord.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _ord.Orders.Any(e => e.Order_Id == id);
        }
    }
}

