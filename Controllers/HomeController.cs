using AuthWindows.Extensions;
using AuthWindows.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;

namespace AuthWindows.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ProtectedPage()
        {
            return View();
        }

        [ClaimsAuthorize("DrivingLicense", "Truck123")]
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Authenticate(string urlRedirect)
        {
            HttpContext.SignOutAsync();

            var passPortClaims = new List<Claim>()
            {
                new Claim("IsAuthenticated", "True")
            };

            var licenseClaim = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Berend de Jong"),
                new Claim(ClaimTypes.Email, "enail@driverlicense.com"),
                new Claim("DrivingLicense", "Truckk")
            };

            var passportidentity = new ClaimsIdentity(passPortClaims, "MyPassport");
            var licencyidentity = new ClaimsIdentity(licenseClaim, "MyLicense");

            var userPrincipal = new ClaimsPrincipal(new[] { passportidentity, licencyidentity });

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = false,
                // Refreshing the authentication session should be allowed.

                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(1),
                // The time at which the authentication ticket expires. A 
                // value set here overrides the ExpireTimeSpan option of 
                // CookieAuthenticationOptions set with AddCookie.

                IsPersistent = true
                // Whether the authentication session is persisted across 
                // multiple requests. When used with cookies, controls
                // whether the cookie's lifetime is absolute (matching the
                // lifetime of the authentication ticket) or session-based.

                //IssuedUtc = <DateTimeOffset>,
                // The time at which the authentication ticket was issued.

                //RedirectUri = <string>
                // The full path or absolute URI to be used as an http 
                // redirect response value.
            };

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, authProperties);

            //return RedirectToAction("Index");

            if(string.IsNullOrEmpty(urlRedirect)) return RedirectToAction("Index");

            return Redirect(urlRedirect);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
