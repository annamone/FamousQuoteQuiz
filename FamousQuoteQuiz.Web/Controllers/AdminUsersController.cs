using FamousQuoteQuiz.Application.Commands.Users;
using FamousQuoteQuiz.Application.Queries.Users;
using FamousQuoteQuiz.Domain.Entities;
using FamousQuoteQuiz.Web.Constants;
using FamousQuoteQuiz.Web.Models;
using FamousQuoteQuiz.Web.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FamousQuoteQuiz.Web.Controllers
{
    [Authorize(Roles = AuthConstants.AdminRole)]
    public class AdminUsersController(IMediator mediator, IPasswordHasher passwordHasher) : Controller
    {
        private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        private readonly IPasswordHasher _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        private const int PageSize = 10;
        private const string DefaultSortColumn = "Id";

        #region List

        public async Task<IActionResult> Index(
            string? search,
            bool? isActive,
            string? sortBy,
            bool sortDesc = false,
            int page = 1)
        {
            var skip = (page - 1) * PageSize;
            var result = await _mediator.Send(new GetUsersQuery(
                skip,
                PageSize,
                search,
                isActive,
                sortBy ?? DefaultSortColumn,
                sortDesc));

            SetPaginationViewBag(search, isActive, sortBy, sortDesc, page, result.TotalCount);

            return View(result.Users);
        }

        #endregion

        #region Create

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateUserViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = MapToEntity(model);
            await _mediator.Send(new CreateUserCommand(user));

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Edit

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await GetUserOrNotFoundAsync(id);
            if (user == null) return NotFound();

            var model = MapToEditViewModel(user);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditUserViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await GetUserOrNotFoundAsync(id);
            if (user == null) return NotFound();

            UpdateUserFromModel(user, model);

            await _mediator.Send(new UpdateUserCommand(user));

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Enable/Disable

        [HttpGet]
        public async Task<IActionResult> Disable(int id)
        {
            await SetUserActiveStatusAsync(id, false);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Enable(int id)
        {
            await SetUserActiveStatusAsync(id, true);
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Delete

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await GetUserOrNotFoundAsync(id);
            if (user == null) return NotFound();

            return View(user);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await GetUserOrNotFoundAsync(id);
            if (user == null) return NotFound();

            await _mediator.Send(new DeleteUserCommand(user));

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Private Helpers

        private async Task<User?> GetUserOrNotFoundAsync(int id)
        {
            return await _mediator.Send(new GetUserByIdQuery(id));
        }

        private async Task SetUserActiveStatusAsync(int id, bool isActive)
        {
            var user = await GetUserOrNotFoundAsync(id);
            if (user != null)
            {
                user.IsActive = isActive;
                await _mediator.Send(new UpdateUserCommand(user));
            }
        }

        private User MapToEntity(CreateUserViewModel model)
        {
            return new User
            {
                UserName = model.UserName.Trim(),
                Email = model.Email.Trim(),
                IsActive = model.IsActive,
                IsAdmin = model.IsAdmin,
                PasswordHash = _passwordHasher.HashPassword(model.Password)
            };
        }

        private static EditUserViewModel MapToEditViewModel(User user)
        {
            return new EditUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                IsActive = user.IsActive,
                IsAdmin = user.IsAdmin
            };
        }

        private void UpdateUserFromModel(User user, EditUserViewModel model)
        {
            user.UserName = model.UserName.Trim();
            user.Email = model.Email.Trim();
            user.IsActive = model.IsActive;
            user.IsAdmin = model.IsAdmin;

            if (!string.IsNullOrWhiteSpace(model.NewPassword))
            {
                user.PasswordHash = _passwordHasher.HashPassword(model.NewPassword);
            }
        }

        private void SetPaginationViewBag(
            string? search,
            bool? isActive,
            string? sortBy,
            bool sortDesc,
            int page,
            int totalCount)
        {
            ViewBag.Search = search;
            ViewBag.IsActive = isActive;
            ViewBag.SortBy = sortBy ?? DefaultSortColumn;
            ViewBag.SortDesc = sortDesc;
            ViewBag.Page = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize);
            ViewBag.TotalCount = totalCount;
        }

        #endregion
    }
}
