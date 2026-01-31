using System.ComponentModel.DataAnnotations;

namespace FamousQuoteQuiz.Web.Models
{
    public class QuoteViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Quote text is required.")]
        [StringLength(1000, MinimumLength = 5, ErrorMessage = "Quote must be between 5 and 1000 characters.")]
        [Display(Name = "Quote Text")]
        public string Text { get; set; } = string.Empty;

        [Required(ErrorMessage = "Author is required.")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Author name must be between 2 and 200 characters.")]
        [Display(Name = "Author")]
        public string Author { get; set; } = string.Empty;
    }
}
