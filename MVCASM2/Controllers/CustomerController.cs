using Microsoft.AspNetCore.Mvc;
using MVCASM2.Data;
using MVCASM2.Models;

namespace MVCASM2.Controllers
{
	public class CustomerController : Controller
	{
		private readonly ApplicationDbContext _db;

		public CustomerController(ApplicationDbContext db)
		{
			_db = db;
		}

		public IActionResult Index()
		{
			// Take list customer
			IEnumerable<Customer> lstCus = _db.Customers.ToList();
			return View(lstCus);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(Customer obj)
		{
			if (ModelState.IsValid)
			{
				_db.Customers.Add(obj);
				_db.SaveChanges();
				return RedirectToAction("Index");
			}
			else
			{
				return View(obj);
			}
		}

		public IActionResult Edit(int id)
		{
			Customer obj = _db.Customers.Find(id);
			if (obj == null)
			{
				return RedirectToAction("Index");
			}
			return View(obj);
			//return Json(obj);
		}

		[HttpPost]
		public IActionResult Edit(int id, Customer obj)
		{
			if (ModelState.IsValid)
			{
				obj.Cus_Id = id;
				_db.Customers.Update(obj);
				_db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(obj);
		}

		public IActionResult Delete(int id)
		{
			Customer obj = _db.Customers.Find(id);
			if (obj != null)
			{
				_db.Customers.Remove(obj);
				_db.SaveChanges();
			}
			return RedirectToAction("Index");
		}
	}
}
