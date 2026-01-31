using FamousQuoteQuiz.Application.Commands.Quotes;
using FamousQuoteQuiz.Application.Queries.Quotes;
using FamousQuoteQuiz.Domain.Entities;
using FamousQuoteQuiz.Web.Constants;
using FamousQuoteQuiz.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FamousQuoteQuiz.Web.Controllers
{
    [Authorize(Roles = AuthConstants.AdminRole)]
    public class AdminQuotesController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        private const int PageSize = 10;
        private const string DefaultSortColumn = "Id";

        #region List

        public async Task<IActionResult> Index(
            string? search,
            string? sortBy,
            bool sortDesc = false,
            int page = 1)
        {
            var skip = (page - 1) * PageSize;
            var result = await _mediator.Send(new GetQuotesQuery(
                skip,
                PageSize,
                search,
                sortBy ?? DefaultSortColumn,
                sortDesc));

            SetPaginationViewBag(search, sortBy, sortDesc, page, result.TotalCount);

            return View(result.Quotes);
        }

        #endregion

        #region Create

        [HttpGet]
        public IActionResult Create()
        {
            return View(new QuoteViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(QuoteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var quote = MapToEntity(model);
            await _mediator.Send(new CreateQuoteCommand(quote));

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Edit

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var quote = await GetQuoteOrNotFoundAsync(id);
            if (quote == null) return NotFound();

            var model = MapToViewModel(quote);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, QuoteViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var quote = MapToEntity(model);
            await _mediator.Send(new UpdateQuoteCommand(quote));

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Delete

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var quote = await GetQuoteOrNotFoundAsync(id);
            if (quote == null) return NotFound();

            return View(quote);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var quote = await GetQuoteOrNotFoundAsync(id);
            if (quote == null) return NotFound();

            await _mediator.Send(new DeleteQuoteCommand(quote));

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Private Helpers

        private async Task<Quote?> GetQuoteOrNotFoundAsync(int id)
        {
            return await _mediator.Send(new GetQuoteByIdQuery(id));
        }

        private static Quote MapToEntity(QuoteViewModel model)
        {
            return new Quote
            {
                Id = model.Id,
                Text = model.Text.Trim(),
                Author = model.Author.Trim()
            };
        }

        private static QuoteViewModel MapToViewModel(Quote quote)
        {
            return new QuoteViewModel
            {
                Id = quote.Id,
                Text = quote.Text,
                Author = quote.Author
            };
        }

        private void SetPaginationViewBag(
            string? search,
            string? sortBy,
            bool sortDesc,
            int page,
            int totalCount)
        {
            ViewBag.Search = search;
            ViewBag.SortBy = sortBy ?? DefaultSortColumn;
            ViewBag.SortDesc = sortDesc;
            ViewBag.Page = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize);
            ViewBag.TotalCount = totalCount;
        }

        #endregion
    }
}
