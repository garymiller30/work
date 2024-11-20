using JobSpace.Static.Pdf.Imposition.Models.Marks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Services.TextVariables
{
    public abstract class TextVariableAbstract
    {
        public TextVariableAbstract Command { get; private set; }
        public string Keyword { get; set; }

        protected TextVariableAbstract(TextVariableAbstract command)
        {
            Command = command;
        }

        public virtual List<TextToken> HandleKeyword(TextMark mark, string keyword)
        {
            if (Keyword.Equals(keyword, StringComparison.InvariantCulture))
            {
                return GetTextTokens(mark);
            }
            else if (Command != null)
            {
                return Command.HandleKeyword(mark, keyword);
            }
            else
            {
                return new List<TextToken>() { TextToken.Create(mark, keyword) };
            }

        }

        protected abstract List<TextToken> GetTextTokens(TextMark mark);

        protected List<TextToken> GetFromTextVariableService(TextMark mark,string keyword)
        {
            return new List<TextToken> { TextToken.Create(mark, TextVariablesService.ReplaceToRealValues(keyword)) };
        }
    }
}
