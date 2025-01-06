using JobSpace.Static.Pdf.Imposition.Models.Marks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Services.TextVariables
{
    public class StringToken
    {
        public List<TextToken> Tokens { get; set; } = new List<TextToken>();
        TextMark Mark { get; set; }



        public StringToken(TextMark mark)
        {
            Mark = mark;
            ParseSting(mark.Text);
        }

        public void ParseSting(string str)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '$' && str[i + 1] == '[')
                {
                    if (sb.Length > 0)
                    {
                        Tokens.AddRange(ProcessStr(sb));
                        sb.Clear();
                    }
                    //початок ключючового слова
                    while (str[i] != ']')
                    {
                        sb.Append(str[i]);
                        i++;
                    }
                    sb.Append(str[i]);

                    Tokens.AddRange(ProcessStr(sb));
                    sb.Clear();
                }
                else
                {
                    sb.Append(str[i]);
                }
            }

            if (sb.Length > 0) Tokens.AddRange(ProcessStr(sb));
        }

        List<TextToken> ProcessStr(StringBuilder sb)
        {
            return TextVariablesService.TextVariableCommand.HandleKeyword(Mark, sb.ToString());
        }

        public string GetRawString()
        {
            StringBuilder rawString = new StringBuilder();
            foreach (var token in Tokens)
            {
                rawString.Append(token.Text);
            }
            return rawString.ToString();
        }
    }
}
