using JobSpace.Static.Pdf.Imposition.Models.Marks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Services.TextVariables
{
    public class TextVariableFromService : TextVariableAbstract
    {
        List<string> Keywords = new List<string>();

        public TextVariableFromService(TextVariableAbstract command) : base(command)
        {
            Keywords.AddRange(new[]{
                ValueList.SheetSide,
                ValueList.SheetDesc,
                ValueList.SheetIdx,
                ValueList.SheetFormat,
                ValueList.OrderNo,
                ValueList.Customer,
                ValueList.OrderDesc,
                });
        }

        public override List<TextToken> HandleKeyword(TextMark mark, string keyword)
        {
            if (Keywords.Contains(keyword))
            {
                return GetFromTextVariableService(mark,keyword);
            }
            else if (Command != null)
            {
                return Command.HandleKeyword(mark, keyword);
            }
            else
            {
                return new List<TextToken>();
            }
        }

        protected override List<TextToken> GetTextTokens(TextMark mark)
        {
            throw new NotImplementedException();
        }
    }
}
