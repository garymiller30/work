using Interfaces;
using JobSpace.Models.ScreenPrimitives;
using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Imposition.Models.Marks.ColorControl.Primitives;
using JobSpace.Static.Pdf.Visual.BlocknoteSpiral;
using JobSpace.UC;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Visual.Commons.Screen
{
    public static class DrawSpiral
    {
        public static List<IScreenPrimitive> Draw(Bitmap spiralPreview, PdfPageInfo spiralPageInfo, SpiralPlaceEnum spiralPlace, PdfPageInfo pageInfo, int curPageIdx)
        {
            List<IScreenPrimitive> primitives = new List<IScreenPrimitive>();

            if (spiralPreview == null || pageInfo == null) return primitives;

            switch (spiralPlace)
            {
                case SpiralPlaceEnum.top:
                    primitives.AddRange(DrawSpiralTop(spiralPreview, spiralPageInfo, pageInfo));
                    break;
                case SpiralPlaceEnum.bottom:
                    primitives.AddRange(DrawSpiralBottom(spiralPreview, spiralPageInfo, pageInfo));
                    break;
                case SpiralPlaceEnum.left:
                    primitives.AddRange(DrawSpiralLeft(spiralPreview, spiralPageInfo, pageInfo));
                    break;
                case SpiralPlaceEnum.right:
                    primitives.AddRange(DrawSpiralRight(spiralPreview, spiralPageInfo, pageInfo));
                    break;
                case SpiralPlaceEnum.spread_horizontal:
                    if (curPageIdx % 2 == 0)
                        primitives.AddRange(DrawSpiralLeft(spiralPreview, spiralPageInfo, pageInfo));
                    else
                        primitives.AddRange(DrawSpiralRight(spiralPreview, spiralPageInfo, pageInfo));
                    break;
                case SpiralPlaceEnum.spread_vertical:
                    if (curPageIdx % 2 == 0)
                        primitives.AddRange(DrawSpiralTop(spiralPreview, spiralPageInfo, pageInfo));
                    else
                        primitives.AddRange(DrawSpiralBottom(spiralPreview, spiralPageInfo, pageInfo));
                    break;
                case SpiralPlaceEnum.top_bottom:
                    primitives.AddRange(DrawSpiralTop(spiralPreview, spiralPageInfo, pageInfo));
                    primitives.AddRange(DrawSpiralBottom(spiralPreview, spiralPageInfo, pageInfo));
                    break;

                case SpiralPlaceEnum.left_right:
                    primitives.AddRange(DrawSpiralLeft(spiralPreview, spiralPageInfo, pageInfo));
                    primitives.AddRange(DrawSpiralRight(spiralPreview, spiralPageInfo, pageInfo));
                    break;
                default:
                    break;
            }

            return primitives;
        }

        private static List<IScreenPrimitive> DrawSpiralRight(Bitmap spiralPreview, PdfPageInfo spiralPageInfo, PdfPageInfo pi)
        {
            double spiralWidth = spiralPageInfo.Trimbox.wMM();
            double spiralHeight = spiralPageInfo.Trimbox.hMM();
            int cntHoles = (int)(pi.Trimbox.hMM() / spiralWidth);
            double x = pi.Trimbox.wMM() - spiralHeight;
            double y = (pi.Trimbox.hMM() - (spiralWidth * cntHoles)) / 2;

            // розвернути _spiralPreview на 90 градусів
            Bitmap rotatedSpiral = new Bitmap(spiralPreview);
            rotatedSpiral.RotateFlip(RotateFlipType.Rotate90FlipNone);

            var primitives = new List<IScreenPrimitive>();

            for (int i = 0; i < cntHoles; i++)
            {
                double holeX = x;
                double holeY = y + i * spiralWidth;
                primitives.Add(new ScreenImage(rotatedSpiral, (float)holeX, (float)holeY, (float)spiralHeight, (float)spiralWidth));
            }

            rotatedSpiral.Dispose();
            return primitives;
        }

        private static List<IScreenPrimitive> DrawSpiralLeft(Bitmap spiralPreview, PdfPageInfo spiralPageInfo, PdfPageInfo pi)
        {
            double spiralWidth = spiralPageInfo.Trimbox.wMM();
            double spiralHeight = spiralPageInfo.Trimbox.hMM();
            int cntHoles = (int)(pi.Trimbox.hMM() / spiralWidth);
            double x = 0;
            double y = (pi.Trimbox.hMM() - (spiralWidth * cntHoles)) / 2;

            // розвернути _spiralPreview на -90 градусів
            Bitmap rotatedSpiral = new Bitmap(spiralPreview);
            rotatedSpiral.RotateFlip(RotateFlipType.Rotate270FlipNone);

            var primitives = new List<IScreenPrimitive>();

            for (int i = 0; i < cntHoles; i++)
            {
                double holeX = x;
                double holeY = y + i * spiralWidth;

                primitives.Add(new ScreenImage(rotatedSpiral, (float)holeX, (float)holeY, (float)spiralHeight, (float)spiralWidth));
            }

            rotatedSpiral.Dispose();
            return primitives;
        }

        private static List<IScreenPrimitive> DrawSpiralBottom(Bitmap spiralPreview, PdfPageInfo spiralPageInfo, PdfPageInfo pi)
        {
            double spiralWidth = spiralPageInfo.Trimbox.wMM();
            double spiralHeight = spiralPageInfo.Trimbox.hMM();
            int cntHoles = (int)(pi.Trimbox.wMM() / spiralWidth);
            double x = (pi.Trimbox.wMM() - (spiralWidth * cntHoles)) / 2;
            double y = pi.Trimbox.hMM() - spiralHeight;

            // розвернути _spiralPreview на 180 градусів
            Bitmap rotatedSpiral = new Bitmap(spiralPreview);
            rotatedSpiral.RotateFlip(RotateFlipType.Rotate180FlipNone);

            var primitives = new List<IScreenPrimitive>();

            for (int i = 0; i < cntHoles; i++)
            {
                double holeX = x + i * spiralWidth;
                double holeY = y;

                primitives.Add(new ScreenImage(rotatedSpiral, (float)holeX, (float)holeY, (float)spiralWidth, (float)spiralHeight));
            }

            rotatedSpiral.Dispose();
            return primitives;
        }

        private static List<IScreenPrimitive> DrawSpiralTop(Bitmap spiralPreview, PdfPageInfo spiralPageInfo, PdfPageInfo pi)
        {
            double spiralWidth = spiralPageInfo.Trimbox.wMM();
            double spiralHeight = spiralPageInfo.Trimbox.hMM();
            int cntHoles = (int)(pi.Trimbox.wMM() / spiralWidth);
            double x = (pi.Trimbox.wMM() - (spiralWidth * cntHoles)) / 2;
            double y = 0;

            var primitives = new List<IScreenPrimitive>();

            for (int i = 0; i < cntHoles; i++)
            {
                double holeX = x + i * spiralWidth;
                double holeY = y;

                primitives.Add(new ScreenImage(spiralPreview, (float)holeX, (float)holeY, (float)spiralWidth, (float)spiralHeight));
            }

            return primitives;
        }
    }
}
