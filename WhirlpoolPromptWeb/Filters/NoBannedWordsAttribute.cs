using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WhirlpoolPromptWeb.Filters
{
    public class NoBannedWordsAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (value == null) return ValidationResult.Success;

            var text = NormalizeText(value.ToString());
            var words = text.Split(new char[] { ' ', '\n', '\r', '\t', '.', ',', '!', '?' }, 
                                   StringSplitOptions.RemoveEmptyEntries);

            var bannedWords = LoadBannedWords();

            foreach (var word in words)
            {
                if (bannedWords.Contains(word))
                    return new ValidationResult(ErrorMessage ?? "El texto contiene lenguaje no permitido.");
            }

            return ValidationResult.Success;
        }

        private HashSet<string> LoadBannedWords()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Data", "banned-words.txt");

            if (!File.Exists(path)) return new HashSet<string>();

            return File.ReadAllLines(path)
                       .Select(w => w.Trim().ToLowerInvariant())
                       .Where(w => !string.IsNullOrEmpty(w))
                       .ToHashSet();
        }

        private string NormalizeText(string text)
        {
            var normalized = text.ToLowerInvariant().Normalize(NormalizationForm.FormD);
            var result = new StringBuilder();

            foreach (var c in normalized)
            {
                if (System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c)
                    != System.Globalization.UnicodeCategory.NonSpacingMark)
                    result.Append(c);
            }

            return result.ToString();
        }
    }
}