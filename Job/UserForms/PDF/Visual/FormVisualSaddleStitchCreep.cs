using Amazon.Runtime.Internal.Transform;
using Interfaces;
using JobSpace.Models.ScreenPrimitives;
using JobSpace.Static;
using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Imposition.Models.Marks.ColorControl.Primitives;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobSpace.UserForms.PDF.Visual
{
    public partial class FormVisualSaddleStitchCreep : Form
    {
        Pen big_pen = new Pen(Color.Red, 0.4f);
        Pen white_pen = new Pen(Color.White, 0.6f);
        IFileSystemInfoExt _fileSystemInfoExt;
        //List<IScreenPrimitive> _primitives = new List<IScreenPrimitive>();
        //int curPageNo = 0;

        Dictionary<string,decimal> thicknesses;

        public FormVisualSaddleStitchCreep(IFileSystemInfoExt file)
        {
            InitializeComponent();
            _fileSystemInfoExt = file;
            uc_PreviewBrowserFile1.SetFunc_GetScreenPrimitives(GetPrimitives);

            //load paper thicknesses from db\paper_thicknesses.json
            thicknesses = SaveAndLoad.LoadPaperThicknesses();

            if (thicknesses.Count == 0)
            {
                comboBox1.Enabled = false;
            }
            else
            {
                comboBox1.Items.AddRange(thicknesses.Keys.ToArray());
                comboBox1.SelectedIndex = 0;
            }

        }

        private List<IScreenPrimitive> GetPrimitives(int pageNo)
        {
            var primitives = new List<IScreenPrimitive>();

            var box = uc_PreviewBrowserFile1.GetCurrentPageInfo();
            if (box != null)
            {
                int totalPages = uc_PreviewBrowserFile1.GetTotalPages();
                double paperThickness = (double)nud_paper_thickness.Value;
                var x = (float)CreepCalculator.GetCreepForPage(pageNo, totalPages, paperThickness);
                float h = (float)box.Trimbox.hMM();

                if (pageNo % 2 != 0) // парна
                {
                    x = (float)box.Trimbox.wMM() - x;
                }
                primitives.Add(new ScreenLine(white_pen, x, 0, x, h));
                primitives.Add(new ScreenLine(big_pen, x, 0, x, h));
            }

            return primitives;
        }


        private void Redraw()
        {
            uc_PreviewBrowserFile1.Redraw();
            //_primitives = new List<IScreenPrimitive>();
            //DrawLines();

            //uc_PreviewBrowserFile1.SetPrimitives(_primitives);
        }

        //private void DrawLines()
        //{
        //    var box = uc_PreviewBrowserFile1.GetCurrentPageInfo();
        //    if (box == null) return;
        //    int totalPages = uc_PreviewBrowserFile1.GetTotalPages();
        //    double paperThickness = (double)nud_paper_thickness.Value;
        //    var x = (float)CreepCalculator.GetCreepForPage(curPageNo, totalPages, paperThickness);
        //    float h = (float)box.Trimbox.hMM();

        //    if (curPageNo % 2 != 0) // парна
        //    {
        //        x = (float)box.Trimbox.wMM() - x;
        //    }
        //    _primitives.Add(new ScreenLine(white_pen, x, 0, x, h));
        //    _primitives.Add(new ScreenLine(big_pen, x, 0, x, h));

        //}

        private void FormVisualSaddleStitchCreep_Load(object sender, EventArgs e)
        {
            if (_fileSystemInfoExt != null)
            {
                uc_PreviewBrowserFile1.Show(_fileSystemInfoExt);
            }
        }

        private void nud_paper_thickness_ValueChanged(object sender, EventArgs e)
        {
            Redraw();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // виставити nud_paper_thickness
            if (comboBox1.SelectedItem is string key)
            {
                nud_paper_thickness.Value = thicknesses[key];
            }
        }
    }
}
