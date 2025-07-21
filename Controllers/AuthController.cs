using IoTAuth.Models;
using IoTAuth.Supabase;
using Microsoft.AspNetCore.Mvc;


namespace IoTAuth.Controllers
{
    public class AuthController : Controller
    {
        private readonly ISupabaseService _supabase;

        public AuthController(ISupabaseService supabase)
        {
            _supabase = supabase;
        }

        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("AccessToken") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            return View();
        }

        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signup(SignupViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var accessToken = await _supabase.SignUpAsync(model.Email, model.Password);
            if (accessToken != null)
            {
                var username = model.Email.Split('@')[0];
                var profileSuccess = await _supabase.AddUserAsync(accessToken, username, model.FullName);

                if (profileSuccess)
                {
                    TempData["Success"] = "ثبت‌نام و ایجاد پروفایل با موفقیت انجام شد!";
                    return RedirectToAction("Login");
                }

                ModelState.AddModelError("", "ثبت‌نام موفق بود ولی ساخت پروفایل انجام نشد.");
                return View(model);
            }

            ModelState.AddModelError("", "ثبت‌نام انجام نشد.");
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var accessToken = await _supabase.SignInAsync(model.Email, model.Password);

            if (accessToken == null)
            {
                ModelState.AddModelError(string.Empty, "ایمیل یا رمز عبور اشتباه است.");
                return View(model);
            }

            HttpContext.Session.SetString("AccessToken", accessToken);

            return RedirectToAction(nameof(Dashboard));
        }

        
    }
}