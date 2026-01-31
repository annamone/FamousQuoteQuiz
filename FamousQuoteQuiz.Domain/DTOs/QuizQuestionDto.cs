namespace FamousQuoteQuiz.Domain.DTOs
{
    public record QuizQuestionDto(
        int QuoteId,
        string QuoteText,
        string CorrectAuthor,
        bool IsBinaryMode,
        string? BinaryAuthorName,
        bool? BinaryIsCorrect,    // True if BinaryAuthorName is the real author
        IReadOnlyList<string>? MultipleOptions  // Three options including correct one
    );
}
