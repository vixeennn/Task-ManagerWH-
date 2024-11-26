using BusinessLogic.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.ModelBinding;

public class AccountController : Controller
{
    private readonly IUsersManager _usersManager;

    public AccountController(IUsersManager usersManager)
    {
        _usersManager = usersManager;
    }

    // Відображення сторінки входу
    [AllowAnonymous]
    public IActionResult Login(string returnUrl = "/Product/Index")
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    // Обробка входу
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string username, string password, string returnUrl = "/Product/Index")
    {
        // Валідація даних на стороні сервера
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            ModelState.AddModelError(string.Empty, "Ім'я користувача та пароль не можуть бути порожніми.");
            return View();
        }

        // Перевірка мінімальної довжини паролю (наприклад, 6 символів)
        if (password.Length < 6)
        {
            ModelState.AddModelError(string.Empty, "Пароль повинен містити щонайменше 6 символів.");
            return View();
        }

        // Перевірка введених даних
        var user = _usersManager.GetUserByUsernameAndPassword(username, password);
        if (user != null)
        {
            // Логування для перевірки user і returnUrl
            Console.WriteLine($"User found: {user.Username}");
            Console.WriteLine($"Return URL: {returnUrl}");

            // Припускаємо, що роль завжди Manager
            var role = "Manager";  // Тут можна додати перевірку ролі користувача

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("UserID", user.UserID.ToString()),
                new Claim(ClaimTypes.Role, role)  // Роль користувача
            };

            var identity = new ClaimsIdentity(claims, "Cookies");
            var principal = new ClaimsPrincipal(identity);

            // Виконання входу
            await HttpContext.SignInAsync("Cookies", principal);

            // Перевіряємо, чи returnUrl валідний
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                // Якщо URL не локальний, перенаправляємо на сторінку продуктів
                returnUrl = Url.Action("Index", "Product");
            }

            // Перенаправлення на сторінку
            return Redirect(returnUrl);
        }
        else
        {
            // Якщо користувача не знайдено, додаємо помилку
            ModelState.AddModelError(string.Empty, "Неправильне ім'я користувача або пароль.");
        }

        return View();
    }

    // Вихід користувача
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("Cookies");
        return RedirectToAction("Index", "Product");
    }

    // Обробка "Увійти без логування"
    [AllowAnonymous]
    public async Task<IActionResult> EnterWithoutLogin(string returnUrl = "/Product/Index")
    {
        // Створюємо тимчасового користувача, який не має доступу до адміністративних функцій
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "Guest"),
            new Claim(ClaimTypes.Role, "Guest") // Роль гостя, яка не має доступу до менеджерських функцій
        };

        var identity = new ClaimsIdentity(claims, "Cookies");
        var principal = new ClaimsPrincipal(identity);

        // Виконання входу як "Гість"
        await HttpContext.SignInAsync("Cookies", principal);

        // Перенаправлення на сторінку продуктів
        return RedirectToAction("Index", "Product");
    }
}
