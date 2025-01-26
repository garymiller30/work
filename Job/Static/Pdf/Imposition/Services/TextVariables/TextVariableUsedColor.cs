using JobSpace.Static.Pdf.Imposition.Drawers;
using JobSpace.Static.Pdf.Imposition.Models.Marks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Services.TextVariables
{
    public class TextVariableUsedColor : TextVariableAbstract
    {
        public TextVariableUsedColor(TextVariableAbstract command) : base(command)
        {
            Keyword = ValueList.UsedColor;
        }

        protected override List<TextToken> GetTextTokens(TextMark mark)
        {
            List<TextToken> list = new List<TextToken>();

            // якимось магічним способом отримати список кольорів
            if (DrawerStatic.CurProductPart != null)
            {
                foreach (var item in DrawerStatic.CurProductPart.UsedColors.Colors.Where(x => DrawerStatic.CurSide == DrawerSideEnum.Front ? x.IsFront == true : x.IsBack == true))
                {
                    var token = new TextToken(item.Name)
                    {
                        Color = item.MarkColor
                    };
                    list.Add(token);
                }
            }

            return list;
        }
    }
}
