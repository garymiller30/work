using Amazon.Runtime.Internal.Util;
using Interfaces;
using Interfaces.PdfUtils;
using JobSpace.Models.ScreenPrimitives;
using JobSpace.Static;
using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Create.Falc;
using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Visual.SoftCover;
using MongoDB.Bson.IO;
using PDFiumSharp;
using PDFiumSharp.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace JobSpace.UserForms.PDF.Visual
{
    public partial class FormVisualFalc : Form
    {
        public FalcSchemaParams SchemaParams { get; private set; }
        Control[] _parts;
        NumericUpDown[] _deltas;
        Label[] _labels;
        FileInfo _fsi;
        private Pen _greenLinePen = new Pen(Color.Green, 0.6f);
        private Pen _whiteLinePen = new Pen(Color.White, 1f);
        decimal[] partsDelta;
        List<IScreenPrimitive> _primitives = new List<IScreenPrimitive>();
        decimal page_w = 0;
        decimal page_h = 0;

        public FormVisualFalc(IFileSystemInfoExt fsi)
        {
            _fsi = new FileInfo(fsi.FileInfo.FullName);
            InitializeComponent();


            uc_PreviewBrowserFile1.SetFunc_GetScreenPrimitives(GetPrimitives);

            nud_width.Value = fsi.Format.Width;
            page_h = fsi.Format.Height;

            _parts = new Control[] { gb_p1, gb_p2, gb_p3, gb_p4, gb_p5, gb_p6, gb_p7, gb_p8, gb_p9, gb_p10 };
            _deltas = new NumericUpDown[] {numericUpDown1, numericUpDown2, numericUpDown3, numericUpDown4, numericUpDown5,
                numericUpDown6, numericUpDown7, numericUpDown8, numericUpDown9, numericUpDown10 };
            _labels = new Label[] { label1, label2, label3, label4, label5,
                label6, label7, label8, label9, label10 };

            cb_cnt_falc.SelectedIndex = 0;

        }

        private List<IScreenPrimitive> GetPrimitives(int arg)
        {
            _primitives.Clear();
            Recalc();
            return _primitives;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            for (int i = 1; i < _parts.Length; i++)
            {
                _parts[i].Enabled = (i <= cb_cnt_falc.SelectedIndex + 1);
            }
            Redraw();
        }

        void Redraw()
        {
            uc_PreviewBrowserFile1.Redraw();
        }


        private void Recalc()
        {
            if (_deltas == null) return;

            int cntPart = cb_cnt_falc.SelectedIndex + 2;
            decimal width = nud_width.Value;

            partsDelta = new decimal[cntPart];
            decimal delta = 0;
            for (int i = 0; i < cntPart; i++)
            {
                delta += _deltas[i].Value;
                partsDelta[i] = delta;
            }

            delta = partsDelta.Sum();
            decimal partWidth = (width + delta) / cntPart;
            for (int i = 0; i < cntPart; i++)
            {
                partsDelta[i] = Math.Round(partWidth - partsDelta[i], 2);
                _labels[i].Text = partsDelta[i].ToString();
            }

            Draw();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Redraw();
        }

        private void FormVisualFalc_Load(object sender, EventArgs e)
        {
            uc_PreviewBrowserFile1.Show(_fsi.ToFileSystemInfoExt());
        }

        void Draw()
        {
            if (partsDelta == null) return;

            _primitives = new List<IScreenPrimitive>();

            float x = 0;

            decimal[] widths = partsDelta;

            if (cb_mirrored_parts.Checked && uc_PreviewBrowserFile1.GetCurrentPageIdx() % 2 == 0)
            {
                widths = partsDelta.Reverse().ToArray();
            }

            for (int i = 0; i < widths.Length - 1; i++)
            {
                x += (float)(widths[i]);

                _primitives.Add(new ScreenLine(_whiteLinePen, x, 0, x, (float)page_h));
                _primitives.Add(new ScreenLine(_greenLinePen, x, 0, x, (float)page_h));
            }

            DrawDimensions(widths);
        }

        private void DrawDimensions(decimal[] widths)
        {
            float x = 0;
            float y = 5;

            for (int i = 0; i <= widths.Length - 1; i++)
            {
                var x_ofs = x + (float)(widths[i] / 2);

                ScreenText screenText = new ScreenText()
                {
                    Text = (widths[i]).ToString(),
                    Location = new PointF(x_ofs, y)
                };

                _primitives.Add(screenText);
                x += (float)(widths[i]);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Redraw();
        }


        private void nud_width_ValueChanged(object sender, EventArgs e)
        {
            page_w = nud_width.Value;
            Redraw();
        }


        private void btn_load_schema_Click(object sender, EventArgs e)
        {
            using (Ookii.Dialogs.WinForms.VistaOpenFileDialog ofd = new Ookii.Dialogs.WinForms.VistaOpenFileDialog())
            {
                ofd.Filter = "Falc Schema|*.falcschema";
                ofd.RestoreDirectory = false;
                ofd.InitialDirectory = Path.GetDirectoryName(_fsi.FullName);
                ofd.FileName = Path.Combine(Path.GetDirectoryName(_fsi.FullName), Path.GetFileNameWithoutExtension(_fsi.FullName) + ".falcschema");
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    // зчитати json файл
                    var strJson = File.ReadAllText(ofd.FileName);
                    var param = System.Text.Json.JsonSerializer.Deserialize<FalcSchemaParams>(strJson);
                    cb_mirrored_parts.Checked = param.Mirrored;
                    int cntPart = param.PartsWidth.Length;
                    cb_cnt_falc.SelectedIndex = cntPart - 2;

                    for (int i = 0; i < cntPart; i++)
                    {
                        decimal partWidth = param.PartsWidth[i];
                        _deltas[i].Value = partWidth;
                    }
                    Redraw();
                }
            }
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            SchemaParams = new FalcSchemaParams()
            {
                Mirrored = cb_mirrored_parts.Checked,
                PartsWidth = partsDelta,
                CreateFileAndSchema = cb_create_file_and_schema.Checked,
                CreateSchema = cb_create_schema.Checked
            };

            if (cb_save_schema.Checked)
            {
                SaveSchema();
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void SaveSchema()
        {
            string targetfile = Path.Combine(Path.GetDirectoryName(_fsi.FullName), $"{Path.GetFileNameWithoutExtension(_fsi.FullName)}.falcschema");
            var strJson = System.Text.Json.JsonSerializer.Serialize<FalcSchemaParams>(SchemaParams, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(targetfile, strJson);
        }
    }
}
