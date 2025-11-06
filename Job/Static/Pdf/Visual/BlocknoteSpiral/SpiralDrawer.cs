using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Imposition.Models;
using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Visual.BlocknoteSpiral
{
    public static class SpiralDrawer
    {
        public static void DrawSpiral(PDFlib p, Boxes box,int pageNo, SpiralSettings spiralSettings)
        {
            int spiralDoc = p.open_pdi_document(spiralSettings.SpiralFile, "");
            spiralSettings.PageHandle = p.open_pdi_page(spiralDoc, 1, "transparencygroup={colorspace=DeviceCMYK isolated=false}");
            spiralSettings.SpiralWidth = p.pcos_get_number(spiralDoc, "pages[0]/width");
            spiralSettings.SpiralHeight = p.pcos_get_number(spiralDoc, "pages[0]/height");

            switch (spiralSettings.SpiralPlace)
            {
                case SpiralPlaceEnum.left:
                    DrawLeftSpiral(p, box,  spiralSettings);
                    break;
                case SpiralPlaceEnum.top:
                    DrawTopSpiral(p, box, spiralSettings);
                    break;
                case SpiralPlaceEnum.right:
                    DrawRightSpiral(p, box,  spiralSettings);
                    break;
                case SpiralPlaceEnum.bottom:
                    DrawBottomSpiral(p, box,  spiralSettings);
                    break;
                case SpiralPlaceEnum.spread_horizontal:
                    if (pageNo % 2 == 1)
                        DrawLeftSpiral(p, box,  spiralSettings);
                    else
                        DrawRightSpiral(p, box,  spiralSettings);
                    break;
                case SpiralPlaceEnum.spread_vertical:
                    if (pageNo % 2 == 1)
                        DrawTopSpiral(p, box, spiralSettings);
                    else
                        DrawBottomSpiral(p, box,  spiralSettings);
                    break;
                case SpiralPlaceEnum.top_bottom:
                    DrawTopSpiral(p, box,  spiralSettings);
                    DrawBottomSpiral(p, box,  spiralSettings);
                    break;
                case SpiralPlaceEnum.left_right:
                    DrawLeftSpiral(p, box,  spiralSettings);
                    DrawRightSpiral(p, box,  spiralSettings);
                    break;
                default:
                    break;
            }

            p.close_pdi_page(spiralSettings.PageHandle);
            p.close_pdi_document(spiralDoc);
        }

        private static void DrawBottomSpiral(PDFlib p, Boxes box, SpiralSettings spiralSettings)
        {
            int cntHoles = (int)(box.Trim.width / spiralSettings.SpiralWidth);
            double x = (box.Media.width - (spiralSettings.SpiralWidth * cntHoles)) / 2;
            double y = box.Trim.bottom;

            for (int i = 0; i < cntHoles; i++)
            {
                double holeX = x + i * spiralSettings.SpiralWidth;
                double holeY = y;

                p.fit_pdi_page(spiralSettings.PageHandle, holeX, holeY, "orientate=south");
            }
        }

        private static void DrawRightSpiral(PDFlib p, Boxes box,  SpiralSettings spiralSettings)
        {
            int cntHoles = (int)(box.Trim.height / spiralSettings.SpiralWidth);

            double x = box.Media.width - box.Trim.left - spiralSettings.SpiralHeight;
            double y = (box.Media.height - (spiralSettings.SpiralWidth * cntHoles)) / 2;
            for (int i = 0; i < cntHoles; i++)
            {
                double holeX = x;
                double holeY = y + i * spiralSettings.SpiralWidth;
                p.fit_pdi_page(spiralSettings.PageHandle, holeX, holeY, "orientate=east");
            }
        }

        private static void DrawLeftSpiral(PDFlib p, Boxes box, SpiralSettings spiralSettings)
        {
            int cntHoles = (int)(box.Trim.height / spiralSettings.SpiralWidth);

            double x = box.Trim.left;
            double y = (box.Media.height - (spiralSettings.SpiralWidth * cntHoles)) / 2;
            for (int i = 0; i < cntHoles; i++)
            {
                double holeX = x;
                double holeY = y + i * spiralSettings.SpiralWidth;
                p.fit_pdi_page(spiralSettings.PageHandle, holeX, holeY, "orientate=west");
            }
        }

        private static void DrawTopSpiral(PDFlib p, Boxes box, SpiralSettings spiralSettings)
        {
            int cntHoles = (int)(box.Trim.width / spiralSettings.SpiralWidth);
            double x = (box.Media.width - (spiralSettings.SpiralWidth * cntHoles)) / 2;
            double y = box.Media.height - spiralSettings.SpiralHeight - box.Trim.top;

            for (int i = 0; i < cntHoles; i++)
            {
                double holeX = x + i * spiralSettings.SpiralWidth;
                double holeY = y;

                p.fit_pdi_page(spiralSettings.PageHandle, holeX, holeY, "");
            }
        }
    }
}
