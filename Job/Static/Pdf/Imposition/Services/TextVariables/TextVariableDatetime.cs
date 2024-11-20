using JobSpace.Static.Pdf.Imposition.Models.Marks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Services.TextVariables
{
    public class TextVariableDatetime : TextVariableAbstract
    {
        public TextVariableDatetime(TextVariableAbstract command):base(command)
        {
            Keyword = ValueList.CurDate;
        }

        protected override List<TextToken> GetTextTokens(TextMark mark)
        {
            return new List<TextToken>(){TextToken.Create(mark, DateTime.Now.ToString()) };
        }
    }
}
