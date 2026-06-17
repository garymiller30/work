using System;
using System.Collections.Generic;
using System.Linq;
using JobSpace.Static.Pdf.Imposition.Models.Marks;

namespace JobSpace.Static.Pdf.Imposition.Services.TextVariables;

// Використовуємо Primary Constructor для C# 12
public class StringToken(TextMark mark, TextVariablesService textVariablesService)
{
    private readonly TextMark _mark = mark ?? throw new ArgumentNullException(nameof(mark));

    public List<TextToken> Tokens { get; } = ParseString(mark.Text, mark, textVariablesService);

    // Статичний метод парсингу полегшує тестування та запобігає витоку "this" під час ініціалізації
    private static List<TextToken> ParseString(string str, TextMark mark, TextVariablesService textVariablesService)
    {
        var tokens = new List<TextToken>();
        if (string.IsNullOrEmpty(str)) return tokens;

        int length = str.Length;
        int start = 0;

        for (int i = 0; i < length; i++)
        {
            // Безпечна перевірка на початок токена $[
            if (i < length - 1 && str[i] == '$' && str[i + 1] == '[')
            {
                // Якщо перед токеном був звичайний текст, обробляємо його
                if (i > start)
                {
                    tokens.AddRange(ProcessChunk(str.AsSpan(start, i - start), mark, textVariablesService));
                }

                start = i;
                // Шукаємо закриваючу дужку з безпечною межею циклу
                while (i < length && str[i] != ']')
                {
                    i++;
                }

                // Включаємо саму дужку ']' в токен, якщо знайшли її
                if (i < length) i++;

                tokens.AddRange(ProcessChunk(str.AsSpan(start, i - start), mark, textVariablesService));
                start = i;
                i--; // Компенсуємо інкремент циклу for
            }
        }

        // Обробляємо залишок рядка
        if (start < length)
        {
            tokens.AddRange(ProcessChunk(str.AsSpan(start, length - start), mark, textVariablesService));
        }

        return tokens;
    }

    // Використовуємо ReadOnlySpan<char> замість StringBuilder, щоб уникнути зайвих алокацій
    private static IEnumerable<TextToken> ProcessChunk(ReadOnlySpan<char> chunk, TextMark mark, TextVariablesService service)
    {
        return service.TextVariableCommand.HandleKeyword(mark, chunk.ToString(), service);
    }

    // Сучасний та швидкий спосіб зшити рядки через LINQ/String.Concat
    public string GetRawString() => string.Concat(Tokens.Select(t => t.Text));
}