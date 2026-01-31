using FamousQuoteQuiz.Application.Commands.Quiz;
using FamousQuoteQuiz.Application.Queries.Quiz;
using FamousQuoteQuiz.Domain.Enums;
using FamousQuoteQuiz.Web.Constants;
using FamousQuoteQuiz.Web.Models;
using FamousQuoteQuiz.Web.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FamousQuoteQuiz.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;

        public HomeController(IMediator mediator, ICurrentUserService currentUserService)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        }

        #region Views

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Settings()
        {
            var mode = GetStoredGameMode();
            ViewBag.GameMode = mode;
            return View();
        }

        [HttpPost]
        public IActionResult Settings(GameMode mode)
        {
            SetGameMode(mode);
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Quiz API

        [HttpGet]
        public IActionResult StartNewGame()
        {
            ClearGameSession();
            return Json(new { ok = true });
        }

        [HttpGet]
        public async Task<IActionResult> GetQuizQuestion()
        {
            var userId = _currentUserService.GetUserId();
            var sessionId = GetGameSessionId();
            var mode = GetStoredGameMode();

            var result = await _mediator.Send(new GetQuizQuestionQuery(mode, sessionId, userId));

            if (result.NewGameSessionId.HasValue)
            {
                SetGameSessionId(result.NewGameSessionId.Value);
            }

            if (!result.Success)
            {
                return Json(new
                {
                    success = false,
                    message = result.Message,
                    isFinished = result.IsFinished,
                    totalQuotes = result.TotalQuotes,
                    answeredCount = result.AnsweredCount
                });
            }

            if (result.IsFinished)
            {
                return Json(new
                {
                    success = true,
                    isFinished = true,
                    message = result.Message,
                    totalQuotes = result.TotalQuotes,
                    answeredCount = result.AnsweredCount
                });
            }

            var question = result.Question!;
            return Json(new
            {
                success = true,
                question = new
                {
                    question.QuoteId,
                    question.QuoteText,
                    question.CorrectAuthor,
                    question.IsBinaryMode,
                    question.BinaryAuthorName,
                    question.BinaryIsCorrect,
                    question.MultipleOptions
                },
                totalQuotes = result.TotalQuotes,
                answeredCount = result.AnsweredCount
            });
        }

        [HttpPost]
        public async Task<IActionResult> CheckAnswer([FromBody] CheckAnswerRequest request)
        {
            if (request == null)
            {
                return Json(new { correct = false, correctAuthor = (string?)null });
            }

            var userId = _currentUserService.GetUserId();
            var sessionId = GetGameSessionId();
            var mode = GetStoredGameMode();

            var result = await _mediator.Send(new CheckAnswerCommand(
                request.QuoteId,
                request.Answer ?? string.Empty,
                request.DisplayedAuthor,
                sessionId,
                userId,
                mode));

            if (result.NewGameSessionId.HasValue)
            {
                SetGameSessionId(result.NewGameSessionId.Value);
            }

            return Json(new
            {
                correct = result.Correct,
                correctAuthor = result.CorrectAuthor
            });
        }

        #endregion

        #region Session Management

        private GameMode GetStoredGameMode()
        {
            var value = HttpContext.Session.GetInt32(SessionKeys.GameMode);
            return value.HasValue && Enum.IsDefined(typeof(GameMode), value.Value)
                ? (GameMode)value.Value
                : GameMode.Binary;
        }

        private void SetGameMode(GameMode mode)
        {
            HttpContext.Session.SetInt32(SessionKeys.GameMode, (int)mode);
        }

        private int? GetGameSessionId()
        {
            return HttpContext.Session.GetInt32(SessionKeys.CurrentGameSessionId);
        }

        private void SetGameSessionId(int sessionId)
        {
            HttpContext.Session.SetInt32(SessionKeys.CurrentGameSessionId, sessionId);
        }

        private void ClearGameSession()
        {
            HttpContext.Session.Remove(SessionKeys.CurrentGameSessionId);
        }

        #endregion
    }
}
