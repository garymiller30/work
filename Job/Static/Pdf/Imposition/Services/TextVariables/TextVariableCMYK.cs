using JobSpace.Static.Pdf.Imposition.Models.Marks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Services.TextVariables
{
    public class TextVariableCMYK : TextVariableAbstract
    {
        public TextVariableCMYK(TextVariableAbstract command) : base(command)
        {
            Keyword = ValueList.Cmyk;
        }

        protected override List<TextToken> GetTextTokens(TextMark mark)
        {
            List<TextToken> list = new List<TextToken>();

            var c = TextToken.Create("C");
            c.Color = new Models.MarkColor() { C = 100, M = 0, Y = 0, K = 0 };
            list.Add(c);
            var m = TextToken.Create("M");
            m.Color = new Models.MarkColor() { C = 0, M = 100, Y = 0, K = 0 };
            list.Add(m);
            var y = TextToken.Create("Y");
            y.Color = new Models.MarkColor() { C = 0, M = 0, Y = 100, K = 0 };
            list.Add(y);
            var k = TextToken.Create("K");
            k.Color = new Models.MarkColor() { C = 0, M = 0, Y = 0, K = 100 };
            list.Add(k);
            return list;
        }
    }
}
