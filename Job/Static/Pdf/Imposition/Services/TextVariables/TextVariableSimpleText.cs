using JobSpace.Static.Pdf.Imposition.Models.Marks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Services.TextVariables
{
    public class TextVariableSimpleText : TextVariableAbstract
    {
        public TextVariableSimpleText(TextVariableAbstract command):base(command)
        {
            
        }

        public override List<TextToken> HandleKeyword(TextMark mark, string keyword, TextVariablesService textVariablesService)
        {
            return new List<TextToken>(){TextToken.Create(mark,keyword, textVariablesService) };
        }

        protected override List<TextToken> GetTextTokens(TextMark mark, TextVariablesService textVariablesService)
        {
            return new List<TextToken>();
        }
    }
}
