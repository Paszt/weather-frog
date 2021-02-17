using System;
using System.Globalization;
using System.Text;

namespace weatherfrog.Extensions
{
    public static class CaseConversions
    {
        // Convert the string to PascalCase.
        public static string ToPascalCase(this string text)
        {
            text = text?.MakeValid();
            // If there are 0 or 1 characters, just return the string with the first letter capitalized.
            if (text == null) return text;
            if (text.Length < 2) return text.ToUpper();

            // Split the string into words.
            string[] words = text.Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries);

            // Combine the words.
            string result = "";
            foreach (string word in words)
            {
                result += word.Substring(0, 1).ToUpper() + word[1..];
            }
            return result;
        }

        // Convert the string to camelCase.
        public static string ToCamelCase(this string text)
        {
            text = text?.MakeValid();
            // If there are 0 or 1 characters, just return the string.
            if (text == null || text.Length < 2)
                return text;

            // Split the string into words.
            string[] words = text.Split(
                Array.Empty<char>(),
                StringSplitOptions.RemoveEmptyEntries);

            // Combine the words.
            string result = words[0].ToLower();
            for (int i = 1; i < words.Length; i++)
            {
                result += words[i].Substring(0, 1).ToUpper() + words[i][1..];
            }
            return result;
        }

        // Convert the string to Capitalized-Kebab-Case.
        public static string ToCapitalizedKebabCase(this string text)
        {
            // If there are 0 or 1 characters, just return the string with the first letter capitalized.
            if (text == null) return text;
            if (text.Length < 2) return text.ToUpper();

            // Split the string into words.
            string[] words = text.Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < words.Length; i++)
            {
                words[i] = words[i].MakeValid();
            }

            // Capitalize and combine the words with a dash in between.
            string result = "";
            foreach (string word in words)
            {
                if (!string.IsNullOrEmpty(result)) result += "-";
                result += word.Substring(0, 1).ToUpper() + word[1..].ToLower();
            }
            return result;
        }

        private static string MakeValid(this string identifier)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < identifier.Length; i++)
            {
                char c = identifier[i];
                if (IsValid(c))
                {
                    builder.Append(c);
                }
            }
            return builder.ToString();
        }

        private static bool IsValid(char c)
        {
            UnicodeCategory uc = char.GetUnicodeCategory(c);
            // each char must be Lu, Ll, Lt, Lm, Lo, Nd, Mn, Mc
            // 
            return uc switch
            {
                // Lu
                UnicodeCategory.UppercaseLetter or
                UnicodeCategory.LowercaseLetter or
                UnicodeCategory.TitlecaseLetter or
                UnicodeCategory.ModifierLetter or
                UnicodeCategory.OtherLetter or
                UnicodeCategory.DecimalDigitNumber or
                UnicodeCategory.NonSpacingMark or
                UnicodeCategory.SpacingCombiningMark
                => true,
                UnicodeCategory.ConnectorPunctuation or
                UnicodeCategory.LetterNumber or
                UnicodeCategory.OtherNumber or
                UnicodeCategory.EnclosingMark or
                UnicodeCategory.SpaceSeparator or
                UnicodeCategory.LineSeparator or
                UnicodeCategory.ParagraphSeparator or
                UnicodeCategory.Control or
                UnicodeCategory.Format or
                UnicodeCategory.Surrogate or
                UnicodeCategory.PrivateUse or
                UnicodeCategory.DashPunctuation or
                UnicodeCategory.OpenPunctuation or
                UnicodeCategory.ClosePunctuation or
                UnicodeCategory.InitialQuotePunctuation or
                UnicodeCategory.FinalQuotePunctuation or
                UnicodeCategory.OtherPunctuation or
                UnicodeCategory.MathSymbol or
                UnicodeCategory.CurrencySymbol or
                UnicodeCategory.ModifierSymbol or
                UnicodeCategory.OtherSymbol or
                UnicodeCategory.OtherNotAssigned
                => false,
                _ => false,
            };
        }
    }
}
