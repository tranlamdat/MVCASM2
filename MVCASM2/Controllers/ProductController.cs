using Microsoft.AspNetCore.Mvc;
using MVCASM2.Models;
using MVCASM2.Data;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using MVCASM2.Services;

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
            // Hiện thị danh sách sản phẩm, có nút chọn đưa vào giỏ hàng
            var products = _context.Products.ToList();
            return View(products);
        }
        public IActionResult Create()
        {
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
            if (obj == null)
            {
                return RedirectToAction("Index");
            }
            return View(obj);
            //return Json(obj);
        }
        [HttpPost]
        public IActionResult Edit(int id, Product obj)
        {
            if (ModelState.IsValid)
            {
                var result = _fileService.SaveImage(obj.FileImage);
                if (result.Item1 == 1)
                {               
                    var oldImage = obj.ProductImage;
                    obj.ProductImage = result.Item2;
                    var deleteResult = _fileService.DeleteImage(oldImage);
                    obj.ProductId = id;
                    _context.Products.Update(obj);
                    _context.SaveChanges();
                }
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

        /// Thêm sản phẩm vào cart
        [Route("addcart/{productid:int}", Name = "addcart")]
        public IActionResult AddToCart([FromRoute] int productid)
        {

            var product = _context.Products
          .Where(p => p.ProductId == productid)
          .FirstOrDefault();
            if (product == null)
                return NotFound("Không có sản phẩm");

            // Xử lý đưa vào Cart ...
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.product.ProductId == productid);
            if (cartitem != null)
            {
                // Đã tồn tại, tăng thêm 1
                cartitem.quantity++;
            }
            else
            {
                //  Thêm mới
                cart.Add(new CartItem() { quantity = 1, product = product });
            }

            // Lưu cart vào Session
            SaveCartSession(cart);
            // Chuyển đến trang hiện thị Cart
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
                // Đã tồn tại, tăng thêm 1
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
            // Cập nhật Cart thay đổi số lượng quantity ...
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.product.ProductId == productid);
            if (cartitem != null)
            {
                // Đã tồn tại, tăng thêm 1
                cartitem.quantity = quantity;
            }
            SaveCartSession(cart);
            // Trả về mã thành công (không có nội dung gì - chỉ để Ajax gọi)
            return Ok();
        }


        // Hiện thị giỏ hàng
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
            return View("Index");
        }

        // Key lưu chuỗi json của Cart
        public const string CARTKEY = "cart";

        // Lấy cart từ Session (danh sách CartItem)
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

        // Xóa cart khỏi session
        void ClearCart()
        {
            var session = HttpContext.Session;
            session.Remove(CARTKEY);
        }

        // Lưu Cart (Danh sách CartItem) vào session
        void SaveCartSession(List<CartItem> ls)
        {
            var session = HttpContext.Session;
            string jsoncart = JsonConvert.SerializeObject(ls);
            session.SetString(CARTKEY, jsoncart);
        }

    }
}
