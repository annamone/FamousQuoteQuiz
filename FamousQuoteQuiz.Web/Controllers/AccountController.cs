using FamousQuoteQuiz.Web.Constants;
using FamousQuoteQuiz.Web.Models;
using FamousQuoteQuiz.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FamousQuoteQuiz.Web.Controllers
{
    [AllowAnonymous]
    public class AccountController(
        IAccountService accountService,
        IUserClaimsService userClaimsService,
        ICurrentUserService currentUserService) : Controller
    {
        private readonly IAccountService _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
        private readonly IUserClaimsService _userClaimsService = userClaimsService ?? throw new ArgumentNullException(nameof(userClaimsService));
        private readonly ICurrentUserService _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));

        #region Login

        [HttpGet]
        [Route(AuthConstants.LoginPath)]
        public IActionResult Login(string? returnUrl = null)
        {
            if (_currentUserService.IsAuthenticated())
            {
                return RedirectToAction(RouteConstants.Actions.Index, RouteConstants.Controllers.Home);
            }

            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [Route(AuthConstants.LoginPath)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _accountService.AuthenticateAsync(model.Email, model.Password);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, ErrorMessages.InvalidCredentials);
                return View(model);
            }

            await SignInUserAsync(user.Id, user.UserName, user.IsAdmin);

            return RedirectToReturnUrlOrHome(model.ReturnUrl);
        }

        #endregion

        #region Register

        [HttpGet]
        [Route(AuthConstants.RegisterPath)]
        public IActionResult Register(string? returnUrl = null)
        {
            if (_currentUserService.IsAuthenticated())
            {
                return RedirectToAction(RouteConstants.Actions.Index, RouteConstants.Controllers.Home);
            }

            return View(new RegisterViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [Route(AuthConstants.RegisterPath)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (await _accountService.EmailExistsAsync(model.Email))
            {
                ModelState.AddModelError(string.Empty, ErrorMessages.EmailAlreadyExists);
                return View(model);
            }

            var user = await _accountService.RegisterAsync(model);

            await SignInUserAsync(user.Id, user.UserName, user.IsAdmin);

            return RedirectToReturnUrlOrHome(model.ReturnUrl);
        }

        #endregion

        #region Logout

        [HttpPost]
        [Route(AuthConstants.LogoutPath)]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(RouteConstants.Actions.Index, RouteConstants.Controllers.Home);
        }

        #endregion

        #region Private Helpers
        private async Task SignInUserAsync(int userId, string userName, bool isAdmin)
        {
            var principal = _userClaimsService.CreatePrincipal(userId, userName, isAdmin);

            var properties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(AuthConstants.SessionExpirationHours)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                properties);
        }
        private IActionResult RedirectToReturnUrlOrHome(string? returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(RouteConstants.Actions.Index, RouteConstants.Controllers.Home);
        }

        #endregion
    }
}
