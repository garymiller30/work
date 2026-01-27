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
        Control[] _parts;
        NumericUpDown[] _deltas;
        Label[] _labels;
        FileInfo _fsi;
        private Pen _greenLinePen = new Pen(Color.Green, 0.6f);
        private Pen _whiteLinePen = new Pen(Color.White, 1f);
        decimal[] partsDelta;
        List<IScreenPrimitive> _primitives;
        int currentPageIdx = 1;
        decimal page_w = 0;
        decimal page_h = 0;

        public FormVisualFalc(IFileSystemInfoExt fsi)
        {
            _fsi = new FileInfo(fsi.FileInfo.FullName);
            InitializeComponent();
            uc_PreviewBrowserFile1.OnPageChanged += (s, pageIdx) =>
            {
                currentPageIdx = pageIdx;
                Draw();
            };
            nud_width.Value = fsi.Format.Width;
            page_h = fsi.Format.Height;

            _parts = new Control[] { gb_p1, gb_p2, gb_p3, gb_p4, gb_p5, gb_p6, gb_p7, gb_p8, gb_p9, gb_p10 };
            _deltas = new NumericUpDown[] {numericUpDown1, numericUpDown2, numericUpDown3, numericUpDown4, numericUpDown5,
                numericUpDown6, numericUpDown7, numericUpDown8, numericUpDown9, numericUpDown10 };
            _labels = new Label[] { label1, label2, label3, label4, label5,
                label6, label7, label8, label9, label10 };

            cb_cnt_falc.SelectedIndex = 0;

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            for (int i = 1; i < _parts.Length; i++)
            {
                _parts[i].Enabled = (i <= cb_cnt_falc.SelectedIndex + 1);
            }
            Recalc();
        }

        private void Recalc()
        {
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
            Recalc();
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

            if (cb_mirrored_parts.Checked && uc_PreviewBrowserFile1.GetCurrentPageIdx() % 2 != 0)
            {
                for (int i = 0; i < partsDelta.Length - 1; i++)
                {
                    x += (float)((double)partsDelta[i]);

                    _primitives.Add(new ScreenLine(_whiteLinePen, x, 0, x, (float)page_h));
                    _primitives.Add(new ScreenLine(_greenLinePen, x, 0, x, (float)page_h));
                }
            }
            else
            {
                for (int i = partsDelta.Length - 1; i > 0; i--)
                {
                    x += (float)((double)partsDelta[i]);

                    _primitives.Add(new ScreenLine(_whiteLinePen, x, 0, x, (float)page_h));
                    _primitives.Add(new ScreenLine(_greenLinePen, x, 0, x, (float)page_h));
                }
            }

            DrawDimensions();

            uc_PreviewBrowserFile1.SetPrimitives(_primitives);
        }

        private void DrawDimensions()
        {
            float x = 0;
            float y = 5;
            if (cb_mirrored_parts.Checked && uc_PreviewBrowserFile1.GetCurrentPageIdx() % 2 != 0)
            {
                for (int i = 0; i <= partsDelta.Length - 1; i++)
                {
                    var x_ofs = x+ (float)((double)partsDelta[i] / 2);
                    


                    ScreenText screenText = new ScreenText()
                    {
                        Text = (partsDelta[i]).ToString(),
                        Location = new PointF(x_ofs, y)
                    };

                    _primitives.Add(screenText);
                    x += (float)((double)partsDelta[i]);
                }
            }
            else
            {
                for (int i = partsDelta.Length - 1; i >= 0; i--)
                {
                    var x_ofs = x + (float)((double)partsDelta[i] / 2);
                    


                    ScreenText screenText = new ScreenText()
                    {
                        Text = (partsDelta[i]).ToString(),
                        Location = new PointF(x_ofs, y)
                    };

                    _primitives.Add(screenText);
                    x += (float)((double)partsDelta[i]);
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Recalc();
        }

        private void btn_create_schema_Click(object sender, EventArgs e)
        {
            FalcSchemaParams param = new FalcSchemaParams()
            {
                Mirrored = cb_mirrored_parts.Checked,
                PartsWidth = partsDelta
            };

            new FalcSchema(param).Run(_fsi.FullName);
        }

        private void nud_width_ValueChanged(object sender, EventArgs e)
        {
            page_w = nud_width.Value;
        }

        private void btn_mark_file_Click(object sender, EventArgs e)
        {
            FalcSchemaParams param = new FalcSchemaParams()
            {
                IsMarkFile = true,
                Mirrored = cb_mirrored_parts.Checked,
                
            };

            param.PartsWidth = partsDelta;

            new FalcSchema(param).Run(_fsi.FullName);
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
                    Recalc();
                }
            }
        }

        private void btn_save_schema_Click(object sender, EventArgs e)
        {
            using (Ookii.Dialogs.WinForms.VistaSaveFileDialog sfd = new Ookii.Dialogs.WinForms.VistaSaveFileDialog())
            {
                sfd.Filter = "Falc Schema|*.falcschema";
                sfd.AddExtension = true;
                sfd.DefaultExt = "falcschema";
                sfd.RestoreDirectory = false;
                sfd.InitialDirectory = Path.GetDirectoryName(_fsi.FullName);
                sfd.FileName = Path.Combine(Path.GetDirectoryName(_fsi.FullName), Path.GetFileNameWithoutExtension(_fsi.FullName) + ".falcschema");

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    FalcSchemaParams param = new FalcSchemaParams()
                    {
                        Mirrored = cb_mirrored_parts.Checked,
                        
                    };

                    param.PartsWidth = _deltas.Select((d, idx) => d.Value).Take(partsDelta.Count()).ToArray();

                    // зберегти в json файл 
                    var strJson = System.Text.Json.JsonSerializer.Serialize<FalcSchemaParams>(param, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(sfd.FileName, strJson);
                }
            }
        }
    }
}
