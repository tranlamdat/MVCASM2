using Microsoft.AspNetCore.Mvc;
using MVCASM2.Models;
using MVCASM2.Data;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using MVCASM2.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MVCASM2.Controllers
{
	public class ProductController : Controller
	{
		private readonly ILogger<ProductController> _logger;

		private readonly ApplicationDbContext _context;

		private readonly IFileService _fileService;

		public ProductController(ILogger<ProductController> logger, ApplicationDbContext context, IFileService fileService)
		{
			_logger = logger;
			_context = context;
			_fileService = fileService;
		}
		[Authorize(Roles = "Admin")]
		public IActionResult GetAll()
		{
			return View();
		}

		public IActionResult Index()
		{
			IEnumerable<Product> lstPro = _context.Products.ToList();
			return View(lstPro);
		}
		public IActionResult Create()
		{
			List<Category> lstCate = _context.Categories.ToList();
			ViewData["Cate"] = new SelectList(lstCate, "Cat_Id", "Cat_Name");
			return View();
		}
		[HttpPost]
		public IActionResult Create(Product obj)
		{
			if (ModelState.IsValid)
			{
				var result = _fileService.SaveImage(obj.FileImage);
				if (result.Item1 == 1)
				{
					var oldImage = obj.ProductImage;
					obj.ProductImage = result.Item2;
					var deleteResult = _fileService.DeleteImage(oldImage);
				}
				_context.Products.Add(obj);
				_context.SaveChanges();
				return RedirectToAction("Index");
			}
			else
			{
				return View(obj);
			}
		}
		public IActionResult Edit(int id)
		{
			Product obj = _context.Products.Find(id);
			List<Category> lstCate = _context.Categories.ToList();
			ViewData["Cate"] = new SelectList(lstCate, "Cat_Id", "Cat_Name");
			if (obj == null)
			{
				return RedirectToAction("Index");
			}
			return View(obj);
			//return Json(obj);
		}

		private void SaveProduct(Product obj)
		{
			_context.Products.Update(obj);
			_context.SaveChanges();
		}

		[HttpPost]
		public async Task<IActionResult> Edit(int id, Product obj)
		{
			if (ModelState.IsValid)
			{
				Product book = await _context.Products.AsNoTracking().Where(b => b.ProductId == id).FirstOrDefaultAsync();
				obj.ProductId = id;

				if (obj.FileImage != null)
				{
					var result = _fileService.SaveImage(obj.FileImage);
					if (result.Item1 == 1)
					{
						var oldImage = obj.ProductImage;
						obj.ProductImage = result.Item2;
						_fileService.DeleteImage(oldImage);
					}
				}
				else
				{
					obj.ProductImage = book.ProductImage;

				}
				SaveProduct(obj);
				return RedirectToAction("Index");
			}
			return View(obj);
		}
		public IActionResult Delete(int id)
		{
			Product obj = _context.Products.Find(id);
			if (obj != null)
			{
				_context.Products.Remove(obj);
				_context.SaveChanges();
			}
			return RedirectToAction("Index");
		}

		/// Add products to cart
		[Route("addcart/{productid:int}", Name = "addcart")]
		public IActionResult AddToCart([FromRoute] int productid)
		{
			var product = _context.Products
		  .Where(p => p.ProductId == productid)
		  .FirstOrDefault();
			if (product == null)
				return NotFound("Không có sản phẩm");

			// Handling into cart ...
			var cart = GetCartItems();
			var cartitem = cart.Find(p => p.product.ProductId == productid);
			if (cartitem != null)
			{
				// Existed, increased by 1
				cartitem.quantity++;
			}
			else
			{
				//  Add new
				cart.Add(new CartItem() { quantity = 1, product = product });
			}

			// Save cart into session
			SaveCartSession(cart);
			// Switch to the Cart page
			return RedirectToAction(nameof(Cart));
		}

		/// xóa item trong cart
		[Route("/removecart/{productid:int}", Name = "removecart")]
		public IActionResult RemoveCart([FromRoute] int productid)
		{

			var cart = GetCartItems();
			var cartitem = cart.Find(p => p.product.ProductId == productid);
			if (cartitem != null)
			{
				// Existed, increased by 1
				cart.Remove(cartitem);
			}

			SaveCartSession(cart);
			return RedirectToAction(nameof(Cart));
		}

		/// Cập nhật
		[Route("/updatecart", Name = "updatecart")]
		[HttpPost]
		public IActionResult UpdateCart([FromForm] int productid, [FromForm] int quantity)
		{
			// Update Cart change the number of Quantity ...
			var cart = GetCartItems();
			var cartitem = cart.Find(p => p.product.ProductId == productid);
			if (cartitem != null)
			{
				// Existed, increased by 1
				cartitem.quantity = quantity;
			}
			SaveCartSession(cart);
			// Returning the code successfully (no content - just for Ajax to call)
			return Ok();
		}


		// Show shopping cart
		[Route("/cart", Name = "cart")]
		public IActionResult Cart()
		{
			return View(GetCartItems());
		}

		[Route("/checkout")]
		public IActionResult CheckOut()
		{
			return View(GetCartItems());
		}

		[Route("/checkout")]
		[HttpPost]
		public IActionResult CheckOut(string CustName, string Telephone, string Address)
		{
			Order order = new Order()
			{
				Cus_Name = CustName,
				DeliveryLocal = Address,
				Cus_Phone = Telephone,
				OrderDate = DateTime.Now,
			};
			_context.Orders.Add(order);
			_context.SaveChanges();
			int orderId = order.Order_Id;
			foreach (var cartItem in GetCartItems())
			{
				OrderDetail orderDetail = new OrderDetail()
				{
					Order_Id = orderId,
					Pro_Id = cartItem.product.ProductId,
					Quantity = cartItem.quantity,
				};
				_context.OrderDetails.Add(orderDetail);
				_context.SaveChanges();
			}
			ClearCart();
			return RedirectToAction("Cart");
		}

		// Cart's JSON chain storage key
		public const string CARTKEY = "cart";

		// Get Cart from Session (Cartitem list)
		List<CartItem> GetCartItems()
		{

			var session = HttpContext.Session;
			string jsoncart = session.GetString(CARTKEY);
			if (jsoncart != null)
			{
				return JsonConvert.DeserializeObject<List<CartItem>>(jsoncart);
			}
			return new List<CartItem>();
		}

		// Delete Cart from session
		void ClearCart()
		{
			var session = HttpContext.Session;
			session.Remove(CARTKEY);
		}

		// Save Cart (Cartitem list) into session
		void SaveCartSession(List<CartItem> ls)
		{
			var session = HttpContext.Session;
			string jsoncart = JsonConvert.SerializeObject(ls);
			session.SetString(CARTKEY, jsoncart);
		}

	}
}
