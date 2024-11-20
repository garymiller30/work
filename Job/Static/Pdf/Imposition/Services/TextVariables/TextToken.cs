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

        public TextToken(string txt)
        {
            if (txt.StartsWith("$["))
            {
                IsKeyWord = true;
                Keyword = txt;
                Text = TextVariablesService.ReplaceToRealValues(txt);
            }
            else
            {
                Text = txt;
            }
            Length = Text.Length;

        }
        public static TextToken Create(string txt)
        {
            return new TextToken(txt);
        }

        public override string ToString()
        {
            return Text;
        }

        public static TextToken Create(TextMark mark, string txt)
        {
            TextToken token = Create(txt);
            token.Color = mark.Color;
            return token;
        }
    }
}
