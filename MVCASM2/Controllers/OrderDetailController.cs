using Microsoft.AspNetCore.Mvc;
using MVCASM2.Data;
using MVCASM2.Models;

namespace MVCASM2.Controllers
{
	public class OrderDetailController : Controller
	{
		private readonly ApplicationDbContext _ords;

        public OrderDetailController(ApplicationDbContext ords)
		{
			_ords = ords;
		}

		public IActionResult Index()
		{
		
			IEnumerable<OrderDetail> lsOrds = _ords.OrderDetails.ToList();
			return View(lsOrds);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(OrderDetail obj)
		{
			if (ModelState.IsValid)
			{
				_ords.OrderDetails.Add(obj);
				_ords.SaveChanges();
				return RedirectToAction("Index");
			}
			else
			{
				return View(obj);
			}
		}

		public IActionResult Edit(int id)
		{
			OrderDetail obj = _ords.OrderDetails.Find(id);
			if (obj == null)
			{
				return RedirectToAction("Index");
			}
			return View(obj);
			//return Json(obj);
		}

		[HttpPost]
		public IActionResult Edit(int id, OrderDetail obj)
		{
			if (ModelState.IsValid)
			{
				obj.Order_Id = id;
				_ords.OrderDetails.Update(obj);
				_ords.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(obj);
		}

		public IActionResult Delete(int id)
		{
			OrderDetail obj = _ords.OrderDetails.Find(id);
			if (obj != null)
			{
				_ords.OrderDetails.Remove(obj);
				_ords.SaveChanges();
			}
			return RedirectToAction("Index");
		}
	}
}

