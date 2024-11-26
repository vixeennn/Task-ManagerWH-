using BusinessLogic.Interface;
using DTO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class ProductController : Controller
{
    private readonly IProductsManager _productsManager;

    public ProductController(IProductsManager productsManager)
    {
        _productsManager = productsManager;
    }

    // Доступ для всіх користувачів
    [AllowAnonymous]
    public IActionResult Index()
    {
        var products = _productsManager.GetAllProducts();
        var productDtos = products.Select(product => new Products
        {
            ProductID = product.ProductID,
            Name = product.Name,
            QuantityInStock = product.QuantityInStock,
            Price = product.Price,
        }).ToList();

        return View(productDtos);
    }

    // Доступ тільки для менеджерів або авторизованих користувачів
    [HttpGet]
    public IActionResult Create()
    {
        if (!User.Identity.IsAuthenticated || User.IsInRole("Guest"))
        {
            // Перенаправлення на сторінку Access Denied
            return RedirectToAction("AccessDenied");
        }

        return View();
    }

    [HttpGet]
    [Authorize(Roles = "Manager")]
    public IActionResult Edit(int id)
    {
        var product = _productsManager.GetAllProducts().FirstOrDefault(p => p.ProductID == id);
        if (product == null)
        {
            return NotFound();
        }

        // Мапінг на DTO для передачі у вигляд
        var productDto = new Products
        {
            ProductID = product.ProductID,
            Name = product.Name,
            QuantityInStock = product.QuantityInStock,
            Price = product.Price
        };

        return View(productDto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Manager")]
    public IActionResult Edit(Products productDto)
    {
        if (ModelState.IsValid)
        {
            // Мапінг DTO назад до сутності
            var product = new Products
            {
                ProductID = productDto.ProductID,
                Name = productDto.Name,
                QuantityInStock = productDto.QuantityInStock,
                Price = productDto.Price
            };

            _productsManager.UpdateProduct(product); // Оновлення в базі даних
            return RedirectToAction(nameof(Index)); // Повернення до списку продуктів
        }

        return View(productDto); // Повернення з помилками валідації
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Manager")]
    public IActionResult Create(Products productDto)
    {
        if (ModelState.IsValid)
        {
            var product = new Products
            {
                ProductID = productDto.ProductID,
                Name = productDto.Name,
                QuantityInStock = productDto.QuantityInStock,
                Price = productDto.Price,
            };
            _productsManager.AddProduct(product);
            return RedirectToAction(nameof(Index));
        }

        return View(productDto);
    }

    // Доступ до видалення
    [HttpGet]
    public IActionResult Delete(int id)
    {
        if (!User.Identity.IsAuthenticated || User.IsInRole("Guest"))
        {
            // Перенаправлення на сторінку Access Denied
            return RedirectToAction("AccessDenied");
        }

        var product = _productsManager.GetAllProducts().FirstOrDefault(p => p.ProductID == id);
        if (product == null)
        {
            return NotFound();
        }

        var productDto = new Products
        {
            ProductID = product.ProductID,
            Name = product.Name,
            QuantityInStock = product.QuantityInStock,
            Price = product.Price
        };

        return View(productDto);
    }

    // Підтвердження видалення продукту
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Manager")]
    public IActionResult DeleteConfirmed(int id)
    {
        _productsManager.DeleteProduct(id);
        return RedirectToAction(nameof(Index));



    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        // Sign the user out of the system
        await HttpContext.SignOutAsync();

        // Redirect the user to the home page or login page
        return RedirectToAction("Index", "Home");
    }


    // Якщо користувач не має доступу до функцій
    public IActionResult AccessDenied()
    {
        ViewBag.Message = "Ви не маєте доступу до цієї функції. Будь ласка, увійдіть в систему.";
        return View();
    }
}
