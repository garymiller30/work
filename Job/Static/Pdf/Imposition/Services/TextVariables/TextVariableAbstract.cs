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

        public virtual List<TextToken> HandleKeyword(TextMark mark, string keyword, TextVariablesService textVariablesService)
        {
            if (Keyword.Equals(keyword, StringComparison.InvariantCulture))
            {
                return GetTextTokens(mark, textVariablesService);
            }
            else if (Command != null)
            {
                return Command.HandleKeyword(mark, keyword, textVariablesService);
            }
            else
            {
                return new List<TextToken>() { TextToken.Create(mark, keyword, textVariablesService) };
            }

        }

        protected abstract List<TextToken> GetTextTokens(TextMark mark, TextVariablesService textVariablesService);

        protected List<TextToken> GetFromTextVariableService(TextMark mark,string keyword, TextVariablesService textVariablesService)
        {
            return new List<TextToken> { TextToken.Create(mark, textVariablesService.ReplaceToRealValues(keyword), textVariablesService) };
        }
    }
}
