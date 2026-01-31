using FamousQuoteQuiz.Application.Queries.Achievements;
using FamousQuoteQuiz.Web.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FamousQuoteQuiz.Web.Controllers
{
    [Authorize(Roles = AuthConstants.AdminRole)]
    public class AdminAchievementsController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        private const int PageSize = 10;
        private const string DefaultSortColumn = "StartedAt";

        public async Task<IActionResult> Index(
            string? userSearch,
            string? sortBy,
            bool sortDesc = false,
            int page = 1)
        {
            var skip = (page - 1) * PageSize;
            var result = await _mediator.Send(new GetGameSessionsQuery(
                skip,
                PageSize,
                userSearch,
                sortBy ?? DefaultSortColumn,
                sortDesc));

            SetPaginationViewBag(userSearch, sortBy, sortDesc, page, result.TotalCount);

            return View(result.Sessions);
        }

        #region Private Helpers

        private void SetPaginationViewBag(
            string? userSearch,
            string? sortBy,
            bool sortDesc,
            int page,
            int totalCount)
        {
            ViewBag.UserSearch = userSearch;
            ViewBag.SortBy = sortBy ?? DefaultSortColumn;
            ViewBag.SortDesc = sortDesc;
            ViewBag.Page = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize);
            ViewBag.TotalCount = totalCount;
        }

        #endregion
    }
}
