using JobSpace.Static.Pdf.Imposition.Models.Marks;
using JobSpace.Static.Pdf.Imposition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Services.TextVariables
{
    public class TextToken
    {
        public bool IsKeyWord { get; set; }
        public string Keyword { get; set; }
        public string Text { get; set; }
        public int Length { get; set; }
        public MarkColor Color { get; set; }

        public TextToken(string txt, TextVariablesService textVariablesService)
        {
            if (txt.StartsWith("$["))
            {
                IsKeyWord = true;
                Keyword = txt;
                Text = textVariablesService.ReplaceToRealValues(txt);
            }
            else
            {
                Text = txt;
            }
            Length = Text.Length;

        }
        public static TextToken Create(string txt, TextVariablesService textVariablesService)
        {
            return new TextToken(txt, textVariablesService);
        }

        public override string ToString()
        {
            return Text;
        }

        public static TextToken Create(TextMark mark, string txt, TextVariablesService textVariablesService)
        {
            TextToken token = Create(txt, textVariablesService);
            token.Color = mark.Color;
            return token;
        }
    }
}
