﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Okta.AspNetCore;

namespace MarkdownViewer.App.Controllers
{
    /// <summary>
    /// lots of help from https://github.com/okta/samples-aspnetcore/tree/master/samples-aspnetcore-3x
    /// </summary>
    public class AccountController : Controller
    {
        private const string homePage = "/Index";

        public IActionResult SignIn()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Challenge(OktaDefaults.MvcAuthenticationScheme);
            }

            return RedirectToPage(homePage);
        }

        [HttpPost]
        public IActionResult SignOut()
        {
            return new SignOutResult(
                new[]
                {
                     OktaDefaults.MvcAuthenticationScheme,
                     CookieAuthenticationDefaults.AuthenticationScheme,
                },
                new AuthenticationProperties { RedirectUri = homePage });
        }
    }
}